# AI Tooling Log

## Tools Used
- **Claude (Anthropic)** — Used as primary coding assistant throughout the entire build

---

## Where I Used AI and What I Did With It

### 1. Backend (Program.cs)
- Claude guided the entire Program.cs implementation step by step
- Claude wrote the keyword search function for retrieving policy paragraphs
- Claude wrote the system prompt that instructs the AI to cite sources and say "I do not have grounded evidence for this" when needed
- I accepted the overall structure as written
- I had to debug several package compatibility errors (AsChatClient vs GetChatClient vs AsIChatClient) — Claude suggested fixes but some were wrong on first attempt, requiring multiple iterations before finding the correct API

### 2. Frontend (App.tsx)
- Claude wrote the React chat interface
- I accepted the streaming response implementation as written
- I debugged a missing React import error myself by reading the browser console

### 3. Eval Suite (Evals.cs)
- Claude wrote the initial eval structure
- The first version did not load policy documents correctly — Claude suggested a path fix which worked
- EVAL-002 expected keyword "confidential" but the model responded differently — I changed the keyword to "grounded" to match actual model behavior

### 4. Architecture Note
- Claude drafted the architecture note
- I reviewed it and it accurately reflects the design decisions made during the build

---

## Where AI Was Wrong
- First suggested `AsChatClient()` method which does not exist in this package version
- Second attempt with `AsIChatClient()` also failed
- Third attempt using `GetChatClient().AsIChatClient()` worked
- Initial eval path for policy documents was wrong — required debugging

## What I Re-prompted
- Policy document loading in evals (wrong folder path on first attempt)
- Package compatibility for OpenAI client (3 attempts before working)

## My Own Contributions
- Debugged React import error from browser console
- Identified correct keyword fix for EVAL-002
- Made all decisions about what to accept, reject, or modify
- Will defend all design decisions in the live interview