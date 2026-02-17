import { useEffect, useState } from "react";
import { webConfig } from "./config";

type HealthResponse = {
  status: "ok";
  service: string;
  env: "development" | "test" | "production";
};

export function App() {
  const [health, setHealth] = useState<HealthResponse | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const controller = new AbortController();

    async function loadHealthcheck() {
      try {
        const response = await fetch(`${webConfig.apiBaseUrl}/api/health`, {
          signal: controller.signal
        });

        if (!response.ok) {
          setError(`Healthcheck failed with status ${response.status}`);
          return;
        }

        const payload = (await response.json()) as HealthResponse;
        setHealth(payload);
      } catch (requestError) {
        if (requestError instanceof DOMException && requestError.name === "AbortError") {
          return;
        }

        setError("Cannot reach API healthcheck endpoint");
      }
    }

    loadHealthcheck();

    return () => {
      controller.abort();
    };
  }, []);

  return (
    <main>
      <h1>Talent Skill Harvester</h1>
      <p>Bootstrap complete: UI shell initialized.</p>
      <p>Configured API base URL: {webConfig.apiBaseUrl}</p>
      {health ? (
        <p>
          Healthcheck: {health.status} ({health.service}, env: {health.env})
        </p>
      ) : error ? (
        <p>Healthcheck error: {error}</p>
      ) : (
        <p>Healthcheck: loading...</p>
      )}
    </main>
  );
}
