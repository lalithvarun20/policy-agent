// PolicyAgent entry point
//
// Skeleton. Your agent registration goes here. The csproj references MAF +
// Microsoft.Extensions.AI.Evaluation; add your model client (any
// Microsoft.Extensions.AI `IChatClient`) and its packages. We are
// provider-agnostic. See "Model access" in the skeleton README for free,
// zero-setup options. Add what you need; remove what you do not.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// Configuration
// ============================================================================
// Wire any Microsoft.Extensions.AI `IChatClient`. Bring whatever provider you
// already use; if you want a free, zero-setup option, GitHub Models works with
// the GitHub account you'll submit from (OpenAI-compatible endpoint + a PAT).
// Note that its free tier is daily-rate-limited (see the README), so a local
// model or a smaller catalog model is easier for heavy iteration. Any capable
// model is fine. Configure endpoint / key / model however you like, e.g.:
//   Model:Endpoint   (OpenAI-compatible base URL)
//   Model:ApiKey     (from user-secrets / env, do not commit)
//   Model:Name       (e.g., gpt-4o-mini)

// ============================================================================
// Services
// ============================================================================
// Register your MAF agent, tools, memory provider, eval runner here.

// builder.Services.AddSingleton<...>();

// ============================================================================
// HTTP surface
// ============================================================================

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

// app.MapPost("/chat", async (HttpContext ctx, ...) => { ... });

app.Run();
