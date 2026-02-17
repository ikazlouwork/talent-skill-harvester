import { Outlet } from "react-router-dom";
import { Navigation } from "../components/Navigation";

export function AdminLayout() {
  return (
    <main>
      <header>
        <h1>Talent Skill Harvester</h1>
        <p>Admin workspace</p>
      </header>
      <Navigation />
      <section>
        <Outlet />
      </section>
    </main>
  );
}