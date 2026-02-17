import { Link } from "react-router-dom";
import { Card } from "../components/Card";

export function AdminPage() {
  return (
    <div className="page-grid admin-grid">
      <Card title="Admin Dashboard">
        <p>Admin area is ready for operational workflows and quick access.</p>
        <p>Use sections below to manage skills and review extraction logs.</p>
        <div className="admin-actions">
          <Link className="inline-action-link" to="/admin/skills">
            Open skills catalog
          </Link>
          <Link className="inline-action-link" to="/admin/logs">
            Open extraction logs
          </Link>
        </div>
      </Card>

      <Card title="Phase 6 checklist">
        <ul className="plain-list">
          <li>Dashboard shell and navigation are available.</li>
          <li>Skills catalog supports list, create, and edit operations.</li>
          <li>Logs page supports extraction request history review.</li>
          <li>Admin workflows include validation and error handling.</li>
        </ul>
      </Card>
    </div>
  );
}