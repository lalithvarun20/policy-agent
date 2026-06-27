# Work Sample - Senior Full-stack AI Engineer
## Stage 1 - Take-home

Welcome. This is the take-home portion of the Senior Full-stack AI Engineer assessment. About five to six hours over a weekend. Honor system on the time-box.

## The task

Build a small Microsoft Agent Framework agent on .NET with a thin React + CopilotKit front-end. The agent answers grounded questions about a fixed set of L&W policy documents (provided in `policies/`). It cites the source paragraph for each answer and offers a "this is uncertain" path when grounding is weak.

The deliverables are listed below, followed by what you receive and the conventions we expect you to follow.

## Deliverables

1. **Working repo, as a self-contained archive.** Submit a `git bundle` (run `git bundle create submission.bundle --all`) or a zip that includes the `.git` folder, so your full commit history comes with it. Send it through the recruiting channel you were given. Please do not share a private GitHub repo with an L&W email; our GitHub cannot accept outside collaborators.
2. **MAF orchestration on .NET** with at least one tool, basic memory, and a graceful "I do not have grounded evidence for this" fallback.
3. **Front-end** in React with CopilotKit. Deliberately minimal: functional, not pixel-perfect, streaming where the framework supports it. The "I do not have grounded evidence for this" path must be visible to the user, not buried in console logs.
4. **Eval suite** of at least 3 cases: one behavioral evaluator (does the answer cite a real source?), one regression evaluator (a known-good question and answer the agent must keep getting right), and one adversarial evaluator (a question whose grounded answer is "I do not know"; does the agent hallucinate?). Use `Microsoft.Extensions.AI.Evaluation` for at least one custom evaluator.
5. **A short architecture note** (one page max): what you built, what you would build differently with more time, one tradeoff you made consciously, and one eval you expect to fail in the current build and why (if there is one).
6. **An explicit AI-tooling log:** where you used AI, what you accepted as written, what you rejected, what you re-prompted, and where it was wrong.

You will defend this submission live in the final interview, so build something you can explain end to end.

## What is provided in this package

```
take-home/
├── 00-cover-and-context.md     (this file)
├── policies/                   (the L&W policy documents your agent grounds against)
│   ├── 01-acceptable-use-policy.md
│   ├── 02-vendor-management-policy.md
│   ├── 03-data-classification-standard.md
│   └── 04-travel-and-expense-policy.md
├── eval-starter.csv            (two starter eval cases; you add at least 2 more)
└── maf-skeleton/               (.NET solution + React frontend scaffolding)
    ├── README.md
    ├── PolicyAgent.slnx
    ├── src/PolicyAgent/
    │   ├── PolicyAgent.csproj
    │   └── Program.cs
    └── frontend/
        ├── package.json
        └── src/App.tsx
```

## Conventions we expect

**Stack.** .NET 10, Microsoft Agent Framework, React 19 with CopilotKit v2 / AG-UI, Microsoft.Extensions.AI.Evaluation for evals. The MAF skeleton in `maf-skeleton/` is a starting point; extend or replace as you see fit.

**Model access.** Provider-agnostic; wire any `Microsoft.Extensions.AI` `IChatClient`. No Azure or paid account required. GitHub Models (free, uses your GitHub account) is the zero-setup default but daily-rate-limited; a local model (Ollama / Foundry Local) is unlimited and good for heavy iteration; any provider you already use is fine. Any capable model works. We grade design, not model choice. If you use GitHub Models, note the free tier is capped at roughly 50 requests per day for frontier models (more for smaller ones), so develop against a smaller or local model and save a frontier model for a final run.

**Front-end scope.** Functional, not pixel-perfect. Streams responses where the framework supports it. The "I do not have grounded evidence for this" path must be visible to the user, not buried in console logs.

**Eval suite.** Start with the two cases in `eval-starter.csv`. Add at least a regression evaluator (a known-good question and answer the agent must keep getting right) and an adversarial evaluator (a question whose grounded answer is "I do not know". Does the agent hallucinate?), so your suite covers all three evaluator types: behavioral, regression, and adversarial. At least one of your additions must be a custom evaluator using `Microsoft.Extensions.AI.Evaluation`.

**Defense.** You will defend this submission live in the final interview. Expect us to challenge your design, grounding strategy, eval choices, and AI-tooling log, and to ask you to show an eval failing and then passing. Build something you can explain end to end.

**AI tooling.** Required and expected. Use whatever you actually use day-to-day. Submit an explicit AI-tooling log as part of your deliverables (deliverable 6 above): where you used AI, what you accepted, what you rejected, what you re-prompted, where it was wrong.

## What we will do with your submission

The hiring manager and one peer read it independently against the published rubric and run it on our side before your final interview. We do not score it in isolation. In the final interview you will defend it live, and that defense is part of the score. We are explicitly NOT assessing production-grade UI polish, exhaustive test coverage, or fully wired CI/CD.

## Practical notes

The policies are real-shape documents sanitized for distribution. Paragraph numbers are stable. Treat the documents as authoritative; do not edit them.

References you will likely reach for:
- [Microsoft Agent Framework Learn docs](https://learn.microsoft.com/en-us/agent-framework/)
- [CopilotKit documentation](https://docs.copilotkit.ai/)
- [AG-UI documentation](https://docs.ag-ui.com/)
- [Microsoft.Extensions.AI.Evaluation API reference](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.ai.evaluation)
- [GitHub Models](https://docs.github.com/en/github-models) - free, OpenAI-compatible model access for this exercise

MAF is recently GA. Depth is uncommon and not required. We are evaluating how you reason about agent design, not how much MAF you have memorized.

Have fun. Be honest about the time. A submission you can clearly defend is worth more to us than another hour of polish.
