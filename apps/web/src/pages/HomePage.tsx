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
  const totalPhases = 10;
  const completedPhases = 5;
  const progressPercent = Math.round((completedPhases / totalPhases) * 100);
  const ringRadius = 46;
  const ringCircumference = 2 * Math.PI * ringRadius;
  const ringOffset = ringCircumference * (1 - progressPercent / 100);

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
      <div className="home-main-column">
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

        <Card title="API healthcheck">
          <StatusBlock
            loadingText="Healthcheck: loading..."
            errorText={error}
            successText={successText}
          />
        </Card>
      </div>

      <div className="home-side-column">
        <Card
          title="Workflow progress"
          topContent={
            <div className="workflow-progress-ring" aria-label={`Completed ${completedPhases} of ${totalPhases} phases`}>
              <svg className="workflow-progress-ring-svg" viewBox="0 0 120 120" role="img" aria-hidden="true">
                <circle className="workflow-progress-ring-track" cx="60" cy="60" r={ringRadius} />
                <circle
                  className="workflow-progress-ring-value"
                  cx="60"
                  cy="60"
                  r={ringRadius}
                  strokeDasharray={`${ringCircumference} ${ringCircumference}`}
                  strokeDashoffset={ringOffset}
                />
              </svg>
              <div className="workflow-progress-ring-label">
                <strong>{progressPercent}%</strong>
                <span>
                  {completedPhases}/{totalPhases} phases
                </span>
              </div>
            </div>
          }
        >
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
              <span className="home-progress-badge home-progress-badge-done">Done</span>
              <span>Phase 4: API implementation</span>
            </li>
            <li>
              <span className="home-progress-badge home-progress-badge-done">Done</span>
              <span>Phase 5: Database and persistence (SQLite)</span>
            </li>
            <li>
              <span className="home-progress-badge">Next</span>
              <span>Phase 6: Admin area</span>
            </li>
          </ul>
        </Card>
      </div>
    </div>
  );
}