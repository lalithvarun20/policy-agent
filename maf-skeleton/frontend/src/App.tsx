// App.tsx
// Skeleton chat surface. CopilotKit v2 talking to your MAF agent over AG-UI.
// The "I do not have grounded evidence for this" path must be visible here,
// not buried in console output.

import { useMemo } from "react";
import {
  CopilotKitProvider,
  CopilotChatConfigurationProvider,
  CopilotChat,
} from "@copilotkit/react-core/v2";
import { HttpAgent } from "@ag-ui/client";
import "@copilotkit/react-core/v2/styles.css";

// Point this at your MAF agent's AG-UI endpoint (e.g. the .NET `MapAGUI` route).
// Across origins in dev, either enable CORS on the agent or proxy this through
// vite.config's `server.proxy` so the browser call stays same-origin.
const AGENT_URL = import.meta.env.VITE_AGENT_URL ?? "http://localhost:5000/";

export default function App() {
  // Register the agent client-side. v2 otherwise probes `runtimeUrl` for an
  // agent-discovery handshake that a raw MAF AG-UI endpoint does not implement.
  const agents = useMemo(
    () => ({ policy_agent: new HttpAgent({ url: AGENT_URL }) }),
    [],
  );

  return (
    <div style={{ height: "100vh", display: "flex", flexDirection: "column" }}>
      <header style={{ padding: "16px", borderBottom: "1px solid #eee" }}>
        <h1 style={{ margin: 0, fontSize: "18px" }}>L&amp;W Policy Assistant</h1>
        <p style={{ margin: "4px 0 0 0", color: "#666", fontSize: "13px" }}>
          Ask a question about L&amp;W policies. Answers cite the source paragraph.
        </p>
      </header>

      <main style={{ flex: 1, minHeight: 0 }}>
        <CopilotKitProvider runtimeUrl={AGENT_URL} agents__unsafe_dev_only={agents}>
          <CopilotChatConfigurationProvider agentId="policy_agent">
            <CopilotChat />
          </CopilotChatConfigurationProvider>
        </CopilotKitProvider>
      </main>
    </div>
  );
}
