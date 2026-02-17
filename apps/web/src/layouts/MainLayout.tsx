import { Outlet } from "react-router-dom";
import { Navigation } from "../components/Navigation";

export function MainLayout() {
  return (
    <main className="app-shell">
      <header className="workspace-header">
        <div>
          <h1 className="workspace-title">Talent Skill Harvester</h1>
          <p className="workspace-subtitle">Main workspace</p>
        </div>
      </header>
      <Navigation />
      <section className="content">
        <Outlet />
      </section>
    </main>
  );
}