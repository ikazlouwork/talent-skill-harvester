import { NavLink } from "react-router-dom";

type NavItem = {
  to: string;
  label: string;
  end?: boolean;
};

const navItems: NavItem[] = [
  { to: "/", label: "Home", end: true },
  { to: "/extract", label: "Extract" },
  { to: "/results", label: "Results" },
  { to: "/admin", label: "Admin" },
  { to: "/admin/skills", label: "Admin Skills" },
  { to: "/admin/logs", label: "Admin Logs" }
];

export function Navigation() {
  return (
    <nav aria-label="Global navigation" className="nav">
      <ul className="nav-list">
        {navItems.map((item) => (
          <li key={item.to}>
            <NavLink
              to={item.to}
              end={item.end}
              className={({ isActive }) =>
                isActive ? "nav-link nav-link-active" : "nav-link"
              }
            >
              {item.label}
            </NavLink>
          </li>
        ))}
      </ul>
    </nav>
  );
}