# Talent Skill Harvester

## Overview
Talent Skill Harvester is a planned Skill Extraction Tool assignment project with a UI + API architecture.
The final app will include multiple pages, reusable components, routing, and different layouts (for example, main and admin areas).

## Current stack
- Frontend: React + Vite + TypeScript
- Backend: .NET 10 Minimal API
- Shared package: TypeScript types/utilities (`packages/shared`)

## Scope for v1
- Build UI pages for core user flows (upload/input, review, admin, and settings).
- Implement routing across several pages and layout groups.
- Add at least two different layouts (main layout and admin layout).
- Build API endpoints for extraction, skill management, and health checks.
- Use SQLite for lightweight persistence in local development.
- Start with a mock extractor service, then allow replacement with real AI extraction logic.

## Architecture draft
### Planned pages
- `/` Home / landing
- `/extract` CV + IFU input and extraction trigger
- `/results` Structured skills review
- `/admin` Admin dashboard shell
- `/admin/skills` Skills catalog management
- `/admin/logs` Extraction/request logs view

### Planned API endpoints
- `GET /api/health`
- `POST /api/extract`
- `GET /api/skills`
- `POST /api/skills`
- `PATCH /api/skills/:id`
- `GET /api/extractions`

## Workflow (90% AI-generated code with Copilot)
1. Define architecture and folder structure prompts first.
2. Generate implementation file-by-file using focused prompts (one feature at a time).
3. Ask Copilot for tests and edge cases after each feature block.
4. Run and validate locally; keep manual edits minimal and review-focused.
5. Log every major prompt, output summary, and acceptance/refactor rationale in docs.
6. Track generated vs manually edited areas to keep AI-generated code share at or above 90%.

## MCP setup
- MCP configuration is defined in `mcp.json` with three server entries:
	- `context7`
	- `microsoftlearn`
	- `github`
- The config uses environment variables for secure setup:
	- `MCP_CONTEXT7_URL`
	- `MCP_CONTEXT7_API_KEY`
	- `MCP_MICROSOFTLEARN_URL`
	- `MCP_GITHUB_URL`
	- `GITHUB_TOKEN`
- This keeps secrets out of source control and allows switching endpoints per environment.

## Local start
### Prerequisites
- Node.js 20+ and npm
- .NET SDK 10+

### Quick start (PowerShell)
1. (Optional) Create local env file for frontend overrides:
	- `Copy-Item .env.example .env`
2. Install dependencies:
	- `npm install`
3. Start API + Web in parallel:
	- `npm run dev`

### Simplified start (from any folder)
- If your current working directory is not the repository root, run:
	- `npm --prefix c:\work\talent-skill-harvester run dev`
- This avoids `ENOENT: Could not read package.json` errors when launching from `C:\work`.

### Useful scripts
- `npm run dev` — starts API and Web together (fast API start via `dotnet run --no-build`)
- `npm run dev:api` — starts only API (.NET, fast start via `--no-build`)
- `npm run dev:api:watch` — starts only API with auto-rebuild/restart on changes
- `npm run dev:web` — starts only Web
- `npm run typecheck` — runs TypeScript checks for Web + Shared
- `npm run build` — builds API (.NET) + Web + Shared
- `npm run test` — runs API tests (.NET) + placeholder tests for Web + Shared

### Local endpoints
- Web app: `http://localhost:5173`
- API health: `http://localhost:4000/api/health`

### Smoke check
- Open `http://localhost:5173`
- Home page calls backend healthcheck automatically
- Expected result on page: healthcheck status `ok`

### Backend launch profile
- API dev profile is defined in `apps/api/Properties/launchSettings.json`
- Uses fixed URL: `http://localhost:4000`
- Uses environment: `ASPNETCORE_ENVIRONMENT=Development`

### SQLite persistence
- API uses SQLite with EF Core migrations.
- Default DB file path: `apps/api/App_Data/talent-skill-harvester.db`
- Override DB file location with env var: `SQLITE_DB_PATH`
- Apply/create migrations manually (optional):
	- `dotnet ef migrations add <MigrationName> --project apps/api/TalentSkillHarvester.Api.csproj --startup-project apps/api/TalentSkillHarvester.Api.csproj --output-dir src/Persistence/Migrations`
	- `dotnet ef database update --project apps/api/TalentSkillHarvester.Api.csproj --startup-project apps/api/TalentSkillHarvester.Api.csproj`

## Plan (checklist)

### Phase 1: Project bootstrap
- [x] Choose stack and initialize app structure (UI + API + shared config).
- [x] Add base scripts for dev/build/test.
- [x] Configure environment variables and update `mcp.json`/runtime config.
- [x] Add initial README run instructions for local start.

### Phase 2: Frontend skeleton and routing
- [x] Create app shell and global navigation.
- [x] Add routes for `/`, `/extract`, `/results`, `/admin`, `/admin/skills`, `/admin/logs`.
- [x] Implement two layouts: main layout and admin layout.
- [x] Create reusable UI components (forms, tables/lists, cards, status blocks).
- [x] Added client routing with `react-router-dom` and full route map for main + admin sections.
- [x] Implemented `MainLayout` and `AdminLayout` with shared global navigation.
- [x] Added reusable UI primitives: `Card` and `StatusBlock`.
- [x] Created placeholder pages for all planned routes and kept home API healthcheck visible.

### Phase 3: Extraction flow (core user scenario)
- [x] Build `/extract` page with CV + IFU input form.
- [x] Implement submit flow to `POST /api/extract`.
- [x] Build `/results` page to show structured extracted skills.
- [x] Add loading/error/empty states for extraction UX.
- [x] UI polish: improved Home page visual dashboard and quick actions.
- [x] UX polish: healthcheck status colors updated (success = green, loading = yellow, error = red).

### Phase 4: API implementation
- [x] Implement `GET /api/health`.
- [x] Implement `POST /api/extract` with mock extractor service.
- [x] Implement skills endpoints: `GET /api/skills`, `POST /api/skills`, `PATCH /api/skills/:id`.
- [x] Implement extraction logs endpoint: `GET /api/extractions`.
- [x] UI polish: updated Workflow progress status, improved action/focus states, and refined Results empty-state CTA.

### Phase 5: Database and persistence (SQLite)
- [x] Define SQLite schema for skills and extraction logs.
- [x] Add migrations and optional seed data.
- [x] Connect API endpoints to persistence layer.
- [x] Verify basic CRUD and extraction history retention.
- [x] Added SQLite persistence with EF Core (`AppDbContext`, entities, and initial migration).
- [x] Implemented repository-based data access for skills and extraction logs.
- [x] Replaced in-memory API storage with persistence-backed store/service wiring.
- [x] Added DB seeding for initial skills and validated extraction log retention after API restart.

### Phase 6: Admin area
- [x] Build `/admin` dashboard shell with navigation to admin sections.
- [x] Build `/admin/skills` page for catalog management (list/create/edit).
- [x] Build `/admin/logs` page for extraction/request logs review.
- [x] Add simple validation and error handling in admin workflows.
- [x] Implemented Admin dashboard quick links and Phase 6 status summary.
- [x] Implemented admin skills workflows with API integration (`GET/POST/PATCH`), form validation, and status feedback.
- [x] Implemented admin extraction logs review with API integration (`GET /api/extractions`) and loading/empty/error states.

### Phase 7: Quality and delivery
- [ ] Add tests for core API and critical UI flow.
- [ ] Run local validation: lint/build/tests.
- [ ] Finalize documentation for run, DB setup, and architecture decisions.
- [ ] Confirm AI-generated code share target (>= 90%) and log key prompts in `docs/prompt-log.md`.
