# L&W Policy Agent — Submission

## What This Is
A policy Q&A agent that answers questions grounded strictly in four L&W policy documents. It cites the exact source paragraph for every answer and says "I do not have grounded evidence for this" when the answer is not in the documents.

## How To Run It

### Prerequisites
- .NET 10 SDK
- Node.js 18+
- A GitHub personal access token (free, from github.com/settings/tokens)

### Step 1 — Set your GitHub token
Open a terminal and run:
cd maf-skeleton/src/PolicyAgent

dotnet user-secrets set "GitHub:Token" "your_github_token_here"

### Step 2 — Start the backend
cd maf-skeleton/src/PolicyAgent

dotnet run
Backend runs on http://localhost:5000

### Step 3 — Start the frontend
Open a new terminal:
cd maf-skeleton/frontend

npm install

npm run dev
Frontend runs on http://localhost:5173

### Step 4 — Open the chat
Go to http://localhost:5173 in your browser.

### Step 5 — Run the evals
Open a new terminal:
cd maf-skeleton/src/PolicyAgent

dotnet run --project . -- --eval

## Example Questions To Try
- "What is the cap for client entertainment per attendee?"
- "Can I share customer data with a competitor?"
- "What is the policy on working from home?"
- "What is the company policy on playing video games?" — This should say "I do not have grounded evidence"

## Project Structure
take-home/

├── policies/                    ← 4 L&W policy documents

├── maf-skeleton/

│   ├── src/PolicyAgent/

│   │   ├── Program.cs           ← Main agent + search logic

│   │   └── Evals.cs             ← Eval suite (3 cases)

│   └── frontend/

│       └── src/App.tsx          ← React chat interface

├── ARCHITECTURE.md              ← Design decisions

├── AI-TOOLING-LOG.md            ← AI usage log

└── README.md                    ← This file

## Eval Results
All 3 evals pass:
- EVAL-001 Behavioral — cites correct source and dollar figure
- EVAL-002 Regression — handles multi-policy reasoning
- EVAL-003 Adversarial — refuses to hallucinate