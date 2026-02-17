import { Outlet } from "react-router-dom";
import { Navigation } from "../components/Navigation";

export function AdminLayout() {
  return (
    <main className="app-shell">
      <header className="workspace-header">
        <div>
          <h1 className="workspace-title">Talent Skill Harvester</h1>
          <p className="workspace-subtitle">Admin workspace</p>
        </div>
        <p className="workspace-tag">Admin area</p>
      </header>
      <Navigation />
      <section className="content">
        <Outlet />
      </section>
    </main>
  );
}