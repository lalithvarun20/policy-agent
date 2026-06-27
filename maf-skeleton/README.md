# MAF starter skeleton

This is a minimal .NET solution + React frontend scaffold. It exists so you do not spend the first two hours of your time-box on project setup. Use it, extend it, or replace it.

## What is here

```
maf-skeleton/
├── README.md                       (this file)
├── PolicyAgent.slnx                 (solution file)
├── src/PolicyAgent/
│   ├── PolicyAgent.csproj          (.NET 10, MAF + Microsoft.Extensions.AI.Evaluation referenced)
│   └── Program.cs                  (entry point; agent registration goes here)
└── frontend/
    ├── package.json                (React 19, CopilotKit v2 + AG-UI, Vite)
    └── src/App.tsx                 (minimal chat surface placeholder)
```

## What you need to add

- Your MAF agent definition (single agent, with at least one tool, basic memory, and a graceful "I do not have grounded evidence" fallback).
- Your retrieval / grounding mechanism over the policy documents.
- Your prompt strategy.
- Your eval suite (use `Microsoft.Extensions.AI.Evaluation` for at least one custom evaluator).
- Whatever else you need to make the deliverables list happen.

## Model access

We are provider-agnostic: wire any `Microsoft.Extensions.AI` `IChatClient`. Pick whatever keeps you moving. We grade agent design, grounding, and evals, not which model you used. You do **not** need an Azure subscription or any paid account for this exercise.

- **GitHub Models (free, no card).** Works with the GitHub account you'll submit from; an OpenAI-compatible endpoint authenticated with a PAT. Free access is rate-limited *per day*, and the caps are low enough to plan around: roughly **50 requests/day** for frontier models (gpt-4o class) and **~150/day** for smaller models on a free account, with an 8k-in / 4k-out limit per call. Develop against a **mini-class model** to stay inside the limits and reserve a frontier model for a final eval/demo run. No surprise charges. Once the daily quota is gone, requests are blocked, not billed (unless you opt into paid usage).
- **Local (free, unlimited).** Ollama or Foundry Local via the OpenAI-compatible path. No rate limit, good for heavy iteration. Note a small local model can make the adversarial / hallucination eval less discriminating.
- **Bring your own key.** OpenAI, Anthropic, Azure, etc. If you already have one and don't want to manage rate limits, use it.

Any capable instruction-following model is fine. We grade design, not model choice. Scope your eval suite to fit whatever limits you're working under, and keep keys out of the repo (user-secrets / env).

## What we do not expect

- A full Aspire AppHost. For this exercise, running locally with `dotnet run` and `npm run dev` is fine.
- Production CI/CD wiring. You are not building the platform; the platform exists.
- Comprehensive .NET project layering. One project for the agent and one for the front-end is enough.

## Running

After adding your agent code:

```bash
# Backend
cd src/PolicyAgent
dotnet run

# Frontend (separate terminal)
cd frontend
npm install
npm run dev
```

If you prefer a different runtime setup, change it. We will follow whatever instructions you provide in your repo's README when we attempt to run it on our side.

## What we look at

Your decisions about agent structure, prompt strategy, tool surface, eval coverage, and front-end UX matter more to us than how cleanly the project is laid out.
