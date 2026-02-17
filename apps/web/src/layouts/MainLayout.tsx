import { Outlet } from "react-router-dom";
import { Navigation } from "../components/Navigation";

export function MainLayout() {
  return (
    <main>
      <header>
        <h1>Talent Skill Harvester</h1>
        <p>Main workspace</p>
      </header>
      <Navigation />
      <section>
        <Outlet />
      </section>
    </main>
  );
}