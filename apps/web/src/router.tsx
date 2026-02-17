import { createBrowserRouter } from "react-router-dom";
import { AdminLayout } from "./layouts/AdminLayout";
import { MainLayout } from "./layouts/MainLayout";
import { AdminLogsPage } from "./pages/AdminLogsPage";
import { AdminPage } from "./pages/AdminPage";
import { AdminSkillsPage } from "./pages/AdminSkillsPage";
import { ExtractPage } from "./pages/ExtractPage";
import { HomePage } from "./pages/HomePage";
import { ResultsPage } from "./pages/ResultsPage";

export const appRouter = createBrowserRouter([
  {
    element: <MainLayout />,
    children: [
      {
        path: "/",
        element: <HomePage />
      },
      {
        path: "/extract",
        element: <ExtractPage />
      },
      {
        path: "/results",
        element: <ResultsPage />
      }
    ]
  },
  {
    path: "/admin",
    element: <AdminLayout />,
    children: [
      {
        index: true,
        element: <AdminPage />
      },
      {
        path: "skills",
        element: <AdminSkillsPage />
      },
      {
        path: "logs",
        element: <AdminLogsPage />
      }
    ]
  }
]);