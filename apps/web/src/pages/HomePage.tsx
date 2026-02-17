import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
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
    <div className="page-grid home-grid">
      <Card title="Talent Skill Harvester">
        <div className="home-hero">
          <p className="home-lead">
            Extract skills from CV + IFU and review results in a structured format.
          </p>
          <p className="home-muted">Configured API base URL: {webConfig.apiBaseUrl}</p>
        </div>

        <div className="home-action-row">
          <Link className="home-action-link" to="/extract">
            Start extraction
          </Link>
          <Link className="home-action-link home-action-link-secondary" to="/results">
            Open latest results
          </Link>
        </div>
      </Card>

      <Card title="Workflow progress">
        <ul className="home-progress-list">
          <li>
            <span className="home-progress-badge home-progress-badge-done">Done</span>
            <span>Phase 1: Project bootstrap</span>
          </li>
          <li>
            <span className="home-progress-badge home-progress-badge-done">Done</span>
            <span>Phase 2: Frontend skeleton and routing</span>
          </li>
          <li>
            <span className="home-progress-badge home-progress-badge-done">Done</span>
            <span>Phase 3: Extraction flow (core user scenario)</span>
          </li>
          <li>
            <span className="home-progress-badge">Next</span>
            <span>Phase 4+: Skills endpoints, logs, and persistence</span>
          </li>
        </ul>
      </Card>

      <Card title="API healthcheck">
        <StatusBlock
          loadingText="Healthcheck: loading..."
          errorText={error}
          successText={successText}
        />
      </Card>
    </div>
  );
}