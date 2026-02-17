import { useEffect, useState } from "react";
import { Card } from "../components/Card";
import { StatusBlock } from "../components/StatusBlock";
import { webConfig } from "../config";

type HealthResponse = {
  status: "ok";
  service: string;
  env: "development" | "test" | "production";
};

export function HomePage() {
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

        setError("Healthcheck error: Cannot reach API healthcheck endpoint");
      }
    }

    loadHealthcheck();

    return () => {
      controller.abort();
    };
  }, []);

  const successText = health
    ? `Healthcheck: ${health.status} (${health.service}, env: ${health.env})`
    : null;

  return (
    <>
      <Card title="Project status">
        <p>Phase 2 frontend skeleton is active.</p>
        <p>Configured API base URL: {webConfig.apiBaseUrl}</p>
      </Card>
      <Card title="API healthcheck">
        <StatusBlock
          loadingText="Healthcheck: loading..."
          errorText={error}
          successText={successText}
        />
      </Card>
    </>
  );
}