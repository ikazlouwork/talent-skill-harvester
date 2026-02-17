import { Link, useLocation } from "react-router-dom";
import { Card } from "../components/Card";
import { StatusBlock } from "../components/StatusBlock";
import type { ResultsLocationState } from "../types/extraction";

export function ResultsPage() {
  const location = useLocation();
  const state = location.state as ResultsLocationState | null;
  const result = state?.result;

  if (!result) {
    return (
      <div className="page-grid">
        <Card title="Results">
          <StatusBlock
            loadingText="No extraction result available."
            errorText={null}
            successText={null}
          />
          <p>Run extraction first to view structured skills and confidence levels.</p>
          <p>
            <Link className="inline-action-link" to="/extract">
              Go to extraction form
            </Link>
          </p>
        </Card>
      </div>
    );
  }

  return (
    <div className="page-grid">
      <Card title="Extraction summary">
        <p>{result.summary}</p>
      </Card>

      <Card title="Structured skills">
        {result.skills.length === 0 ? (
          <p>No skills were extracted from the provided input.</p>
        ) : (
          <ul className="skills-list">
            {result.skills.map((skill) => (
              <li key={skill.name} className="skills-item">
                <p>
                  <strong>{skill.name}</strong> ({skill.category})
                </p>
                <p>Confidence: {skill.confidence}%</p>
                <p>{skill.evidence}</p>
              </li>
            ))}
          </ul>
        )}
      </Card>

      <Card title="Warnings">
        {result.warnings.length === 0 ? (
          <p>No warnings for this extraction.</p>
        ) : (
          <ul className="warnings-list">
            {result.warnings.map((warning) => (
              <li key={warning}>{warning}</li>
            ))}
          </ul>
        )}
      </Card>
    </div>
  );
}