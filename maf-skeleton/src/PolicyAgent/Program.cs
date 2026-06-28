using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var policiesFolder = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "policies");
var policyDocuments = new List<PolicyDocument>();

if (Directory.Exists(policiesFolder))
{
    foreach (var file in Directory.GetFiles(policiesFolder, "*.md"))
    {
        var content = await File.ReadAllTextAsync(file);
        var fileName = Path.GetFileNameWithoutExtension(file);
        policyDocuments.Add(new PolicyDocument(fileName, content));
    }
}

var githubToken = builder.Configuration["GitHub:Token"]
    ?? Environment.GetEnvironmentVariable("GITHUB_TOKEN")
    ?? "";

var openAIClient = new OpenAIClient(
    new ApiKeyCredential(githubToken),
    new OpenAIClientOptions { Endpoint = new Uri("https://models.inference.ai.azure.com") }
);
var chatClient = openAIClient.GetChatClient("gpt-4o-mini");

var app = builder.Build();
app.UseCors();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/chat", async (HttpContext ctx) =>
{
    var request = await JsonSerializer.DeserializeAsync<ChatRequest>(
        ctx.Request.Body,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
    );

    if (request == null || string.IsNullOrWhiteSpace(request.Question))
    {
        ctx.Response.StatusCode = 400;
        return;
    }

    var relevantChunks = SearchPolicies(policyDocuments, request.Question);

    string systemPrompt;
    if (relevantChunks.Count == 0)
    {
        systemPrompt = @"You are a policy assistant for Light & Wonder (L&W).
You only answer questions based on company policy documents.
You have no relevant policy information for this question.
You MUST respond with exactly: 'I do not have grounded evidence for this.'
Do not make up any answer.";
    }
    else
    {
        var context = string.Join("\n\n---\n\n", relevantChunks.Select(c =>
            $"Source: {c.Source}\n\n{c.Text}"));

        systemPrompt = $@"You are a policy assistant for Light & Wonder (L&W).
Answer questions ONLY using the policy excerpts provided below.
Always cite the exact source (document name and section) for your answer.
If the answer is not clearly stated in the excerpts, respond with:
'I do not have grounded evidence for this.'
Do not make up information.

POLICY EXCERPTS:
{context}";
    }

    var messages = new List<ChatMessage>
    {
        new SystemChatMessage(systemPrompt),
        new UserChatMessage(request.Question)
    };

    ctx.Response.ContentType = "text/plain";
    ctx.Response.Headers["X-Citations"] = string.Join(", ", relevantChunks.Select(c => c.Source));

    await foreach (var update in chatClient.CompleteChatStreamingAsync(messages))
    {
        foreach (var part in update.ContentUpdate)
        {
            await ctx.Response.WriteAsync(part.Text);
            await ctx.Response.Body.FlushAsync();
        }
    }
});

// Run evals if --eval flag is passed
if (args.Contains("--eval"))
{
    await Evals.RunAsync(githubToken);
    return;
}

app.Run();

static List<PolicyChunk> SearchPolicies(List<PolicyDocument> documents, string question)
{
    var results = new List<PolicyChunk>();
    var questionWords = question.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Where(w => w.Length > 3).ToHashSet();

    foreach (var doc in documents)
    {
        var paragraphs = doc.Content.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        foreach (var paragraph in paragraphs)
        {
            if (paragraph.Trim().Length < 20) continue;

            var paraLower = paragraph.ToLower();
            var matchCount = questionWords.Count(w => paraLower.Contains(w));

            if (matchCount >= 2)
            {
                results.Add(new PolicyChunk(doc.Name, paragraph.Trim(), matchCount));
            }
        }
    }

    return results
        .OrderByDescending(r => r.Score)
        .Take(5)
        .ToList();
}

record PolicyDocument(string Name, string Content);
record PolicyChunk(string Source, string Text, int Score);
record ChatRequest(string Question);