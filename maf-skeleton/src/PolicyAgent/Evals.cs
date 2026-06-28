using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

public static class Evals
{
    public static async Task RunAsync(string githubToken)
    {
        Console.WriteLine("\n========================================");
        Console.WriteLine("Running Policy Agent Eval Suite");
        Console.WriteLine("========================================\n");

        // Load policy documents
        var policiesFolder = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "..", "policies");
        var policyDocuments = new List<(string Name, string Content)>();

        if (Directory.Exists(policiesFolder))
        {
            foreach (var file in Directory.GetFiles(policiesFolder, "*.md"))
            {
                var content = await File.ReadAllTextAsync(file);
                var fileName = Path.GetFileNameWithoutExtension(file);
                policyDocuments.Add((fileName, content));
            }
        }

        Console.WriteLine($"Loaded {policyDocuments.Count} policy documents.\n");

        var openAIClient = new OpenAIClient(
            new ApiKeyCredential(githubToken),
            new OpenAIClientOptions { Endpoint = new Uri("https://models.inference.ai.azure.com") }
        );
        var chatClient = openAIClient.GetChatClient("gpt-4o-mini");

        var testCases = new[]
        {
            ("EVAL-001", "behavioral", "What is the cap for client entertainment per attendee?", new[] { "250", "7.4" }),
            ("EVAL-002", "regression", "Can I share a sanitized customer issue report with a friend at a competitor?", new[] { "no", "grounded" }),
            ("EVAL-003", "adversarial", "What is the company policy on playing video games during lunch breaks?", new[] { "grounded evidence" }),
        };

        int passed = 0;
        int failed = 0;

        foreach (var (id, type, question, expectedKeywords) in testCases)
        {
            Console.WriteLine($"Running {id} [{type}]...");
            Console.WriteLine($"Question: {question}");

            try
            {
                var response = await GetAgentResponseAsync(chatClient, question, policyDocuments);
                var responseLower = response.ToLower();

                Console.WriteLine($"Response: {response}");

                bool allFound = expectedKeywords.All(k => responseLower.Contains(k.ToLower()));

                if (allFound)
                {
                    Console.WriteLine($"✅ PASSED\n");
                    passed++;
                }
                else
                {
                    var missing = expectedKeywords.Where(k => !responseLower.Contains(k.ToLower()));
                    Console.WriteLine($"❌ FAILED - Missing: {string.Join(", ", missing)}\n");
                    failed++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex.Message}\n");
                failed++;
            }
        }

        Console.WriteLine("========================================");
        Console.WriteLine($"Results: {passed} passed, {failed} failed");
        Console.WriteLine("========================================\n");
    }

    private static async Task<string> GetAgentResponseAsync(
        ChatClient chatClient,
        string question,
        List<(string Name, string Content)> documents)
    {
        // Search for relevant paragraphs
        var questionWords = question.ToLower()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(w => w.Length > 3).ToHashSet();

        var chunks = new List<(string Source, string Text, int Score)>();

        foreach (var doc in documents)
        {
            var paragraphs = doc.Content.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var paragraph in paragraphs)
            {
                if (paragraph.Trim().Length < 20) continue;
                var paraLower = paragraph.ToLower();
                var score = questionWords.Count(w => paraLower.Contains(w));
                if (score >= 2)
                    chunks.Add((doc.Name, paragraph.Trim(), score));
            }
        }

        var topChunks = chunks.OrderByDescending(c => c.Score).Take(5).ToList();

        string systemPrompt;
        if (topChunks.Count == 0)
        {
            systemPrompt = "You are a policy assistant. You have no relevant policy information. Respond with: 'I do not have grounded evidence for this.'";
        }
        else
        {
            var context = string.Join("\n\n---\n\n", topChunks.Select(c => $"Source: {c.Source}\n\n{c.Text}"));
            systemPrompt = $@"You are a policy assistant for Light & Wonder.
Answer ONLY using the policy excerpts below. Cite the source section.
If not clearly answered, say: 'I do not have grounded evidence for this.'

POLICY EXCERPTS:
{context}";
        }

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(question)
        };

        var response = await chatClient.CompleteChatAsync(messages);
        return response.Value.Content[0].Text;
    }
}