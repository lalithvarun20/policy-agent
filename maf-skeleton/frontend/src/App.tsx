import React, { useState, useRef } from "react";

const AGENT_URL = "http://localhost:5000";

interface Message {
  role: "user" | "assistant";
  text: string;
  citations?: string;
}

export default function App() {
  const [messages, setMessages] = useState<Message[]>([]);
  const [question, setQuestion] = useState("");
  const [loading, setLoading] = useState(false);
  const bottomRef = useRef<HTMLDivElement>(null);

  const ask = async () => {
    if (!question.trim() || loading) return;

    const userMessage: Message = { role: "user", text: question };
    setMessages((prev) => [...prev, userMessage]);
    setQuestion("");
    setLoading(true);

    try {
      const response = await fetch(`${AGENT_URL}/chat`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ question }),
      });

      const citations = response.headers.get("X-Citations") || "";
      const reader = response.body!.getReader();
      const decoder = new TextDecoder();
      let fullText = "";

      setMessages((prev) => [
        ...prev,
        { role: "assistant", text: "", citations },
      ]);

      while (true) {
        const { done, value } = await reader.read();
        if (done) break;
        fullText += decoder.decode(value);
        setMessages((prev) => {
          const updated = [...prev];
          updated[updated.length - 1] = {
            role: "assistant",
            text: fullText,
            citations,
          };
          return updated;
        });
        bottomRef.current?.scrollIntoView({ behavior: "smooth" });
      }
    } catch {
      setMessages((prev) => [
        ...prev,
        {
          role: "assistant",
          text: "Error connecting to the policy agent. Is the backend running?",
        },
      ]);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ height: "100vh", display: "flex", flexDirection: "column", fontFamily: "sans-serif", maxWidth: "800px", margin: "0 auto" }}>
      <header style={{ padding: "16px", borderBottom: "1px solid #ddd", background: "#1a1a2e", color: "white" }}>
        <h1 style={{ margin: 0, fontSize: "20px" }}>L&W Policy Assistant</h1>
        <p style={{ margin: "4px 0 0 0", fontSize: "13px", opacity: 0.8 }}>
          Ask any question about L&W policies. Answers are grounded in source documents.
        </p>
      </header>

      <main style={{ flex: 1, overflowY: "auto", padding: "16px", background: "#f9f9f9" }}>
        {messages.length === 0 && (
          <div style={{ textAlign: "center", color: "#888", marginTop: "60px" }}>
            <p>Ask a question about L&W policies to get started.</p>
            <p style={{ fontSize: "13px" }}>Examples:</p>
            <p style={{ fontSize: "13px", fontStyle: "italic" }}>"What is the cap for client entertainment per attendee?"</p>
            <p style={{ fontSize: "13px", fontStyle: "italic" }}>"Can I share customer data with a competitor?"</p>
          </div>
        )}

        {messages.map((msg, i) => (
          <div key={i} style={{ marginBottom: "16px" }}>
            <div style={{
              background: msg.role === "user" ? "#1a1a2e" : "white",
              color: msg.role === "user" ? "white" : "#333",
              padding: "12px 16px",
              borderRadius: "8px",
              maxWidth: "85%",
              marginLeft: msg.role === "user" ? "auto" : "0",
              boxShadow: "0 1px 3px rgba(0,0,0,0.1)"
            }}>
              <p style={{ margin: 0, whiteSpace: "pre-wrap" }}>{msg.text}</p>
            </div>
            {msg.citations && (
              <div style={{ fontSize: "12px", color: "#666", marginTop: "4px", paddingLeft: "4px" }}>
                📄 Sources: {msg.citations}
              </div>
            )}
          </div>
        ))}

        {loading && (
          <div style={{ color: "#888", fontStyle: "italic", fontSize: "14px" }}>
            Searching policy documents...
          </div>
        )}
        <div ref={bottomRef} />
      </main>

      <footer style={{ padding: "16px", borderTop: "1px solid #ddd", background: "white" }}>
        <div style={{ display: "flex", gap: "8px" }}>
          <input
            value={question}
            onChange={(e) => setQuestion(e.target.value)}
            onKeyDown={(e) => e.key === "Enter" && ask()}
            placeholder="Ask a policy question..."
            style={{ flex: 1, padding: "10px", borderRadius: "6px", border: "1px solid #ddd", fontSize: "14px" }}
            disabled={loading}
          />
          <button
            onClick={ask}
            disabled={loading}
            style={{ padding: "10px 20px", background: "#1a1a2e", color: "white", border: "none", borderRadius: "6px", cursor: "pointer", fontSize: "14px" }}
          >
            Ask
          </button>
        </div>
      </footer>
    </div>
  );
}