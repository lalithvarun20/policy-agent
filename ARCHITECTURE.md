# Architecture Note — L&W Policy Agent

## What I Built

A policy Q&A agent that answers questions grounded strictly in four L&W policy documents. The system has two parts:

**Backend (.NET 10):**
A web server that loads all four policy documents into memory at startup. When a question comes in, it searches the documents using keyword matching to find relevant paragraphs. It then sends those paragraphs plus the question to the AI model (gpt-4o-mini via GitHub Models). The AI is instructed to answer ONLY from the provided paragraphs and to always cite the source section. If no relevant paragraphs are found, the AI is instructed to say "I do not have grounded evidence for this."

**Frontend (React):**
A simple chat interface where users type questions and see streaming answers. Citations appear below each answer showing which document and section the answer came from.

**Eval Suite:**
Three automated tests — behavioral (does it cite correctly?), regression (does it keep getting a known answer right?), and adversarial (does it refuse to hallucinate?).

## Grounding Strategy

I used keyword-based paragraph retrieval. The question is split into words, and paragraphs containing 2 or more matching words are selected. Top 5 paragraphs by score are sent to the model as context. This is simple, fast, explainable, and sufficient for structured policy documents.

## Tradeoff I Made Consciously

I chose keyword matching over vector embeddings for retrieval. Embeddings would find semantically similar paragraphs even without exact word matches, making retrieval more robust. However, keyword matching is simpler to explain, has no additional dependencies, and works well for structured policy documents with consistent terminology.

## What I Would Build Differently With More Time

I would add vector embeddings for semantic search, which would handle questions phrased differently from the policy text. I would also add a memory layer to handle multi-turn conversations where follow-up questions refer to earlier context.

## Eval I Expect Could Fail

EVAL-002 (regression) is the most fragile. It requires the agent to reason across multiple policy documents simultaneously. If the keyword retrieval does not surface all relevant paragraphs, the agent may miss part of the answer. This is a known limitation of keyword-based retrieval versus semantic search.