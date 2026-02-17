# Prompt & Workflow Log

## Tools/Models
- Primary AI coding tool: GitHub Copilot Chat (VS Code)
- Secondary tools/models (if any): none
- IDE/editor integration: VS Code workspace + integrated terminals
- MCP servers used (if any): `context7`, `microsoftlearn`, `github` (configured in `mcp.json`)

## Workflow progress
- Phase 1: Completed
- Phase 2: Completed
- Phase 3: Completed
- Phase 4: Completed (health, extract, skills CRUD-lite, extraction logs)
- Phase 5: Completed (SQLite persistence + repository pattern + migrations)

## Prompt History

### Step 1 — Architecture and scope
- **Goal:** Confirm app scope (UI + API), route map, and target endpoint set.
- **Context provided:** Assignment constraints, desired stack, and project idea (Skill Extraction Tool).
- **Prompt used:** Define architecture and project skeleton for React + .NET API with admin pages.
- **Result summary:** Baseline structure with `apps/web`, `apps/api`, `packages/shared`, and initial route/endpoint plan.
- **Accepted / changed:** Accepted structure and route plan; refined endpoint list in README.
- **Why:** Keeps implementation incremental and aligned with assignment criteria.

### Step 2 — Frontend skeleton and extraction flow
- **Goal:** Build pages/layouts and implement extraction user flow.
- **Context provided:** Planned routes (`/`, `/extract`, `/results`, `/admin/*`) and expected UX states.
- **Prompt used:** Generate page scaffolding, shared components, routing, and submit flow to `POST /api/extract`.
- **Result summary:** Main/admin layouts, reusable cards/status blocks, extraction form and results view.
- **Accepted / changed:** Accepted most generated code; kept UI intentionally minimal for MVP.
- **Why:** Prioritized end-to-end flow over visual complexity.

### Step 3 — Initial API endpoints
- **Goal:** Implement `GET /api/health` and `POST /api/extract` with mock extraction logic.
- **Context provided:** Backend stack (.NET 10), endpoint definitions from README.
- **Prompt used:** Add health endpoint and controller-based extraction endpoint with validation.
- **Result summary:** Stable health check + mock keyword-based skill extraction response.
- **Accepted / changed:** Accepted controller approach and mock extraction logic.
- **Why:** Satisfies Phase 4 base endpoints and enables UI integration.

### Step 4 — Phase 4 completion update
- **Goal:** Close all remaining Phase 4 API items.
- **Context provided:** Gap analysis showed missing `/api/skills` and `/api/extractions` endpoints.
- **Prompt used:** Implement missing endpoints and wire extraction request logging.
- **Result summary:** Added:
	- `GET /api/skills`
	- `POST /api/skills`
	- `PATCH /api/skills/:id`
	- `GET /api/extractions`
	- in-memory store + DTO contracts for skills/logs
	- logging from `POST /api/extract` into extraction history
- **Accepted / changed:** Accepted implementation as Phase 4 target; updated Phase 4 checklist in README.
- **Why:** Completes declared API scope before moving to SQLite persistence in Phase 5.

### Step 5 — UI polish in small PR commits
- **Goal:** Keep PR history with small, focused UI improvements.
- **Context provided:** Request to include lightweight UI changes in each PR and split them into separate commits.
- **Prompt used:** Apply incremental UI polish and commit each change separately.
- **Result summary:** Added and committed:
	- improved interactive hover/focus-visible states for actions (`ui: refine interactive states on home actions`)
	- improved Results empty-state CTA styling/text (`ui: improve results empty state call-to-action`)
- **Accepted / changed:** Accepted both changes as minimal UI-only refinements.
- **Why:** Improves UX consistency while keeping commit history readable.

### Step 6 — Backend unit tests
- **Goal:** Add unit tests for backend controller logic.
- **Context provided:** Existing API endpoints in Phase 4 and no existing test project.
- **Prompt used:** Create xUnit test project, add controller tests, and run `dotnet test`.
- **Result summary:** Added test project under `apps/api/tests` with tests for `ExtractionController` and `SkillsController`; updated solution and scripts to run tests from solution.
- **Accepted / changed:** Accepted test coverage for core controller behaviors (validation, success, conflict/not-found).
- **Why:** Establishes baseline backend quality gates before Phase 5 persistence work.

### Step 7 — Phase 5 implementation (SQLite + repository pattern)
- **Goal:** Complete persistence phase with SQLite while keeping API contracts stable.
- **Context provided:** Phase 5 checklist in README and requirement to use repository pattern.
- **Prompt used:** Implement EF Core SQLite persistence, repositories, migrations, service abstraction, and wire controllers through DI.
- **Result summary:** Added:
	- `AppDbContext`, entities, migrations, seed logic
	- repository interfaces + SQLite repository implementations
	- shared `IApiStore`/service abstraction for controllers
	- async controller/action updates and test adjustments
	- retention smoke check (logs persisted across API restart)
- **Accepted / changed:** Accepted full persistence refactor; replaced in-memory runtime path while preserving endpoint DTOs.
- **Why:** Closed all declared Phase 5 outcomes without expanding API scope.

### Step 8 — Home page UX updates (workflow section)
- **Goal:** Improve dashboard clarity on Home page without adding extra pages.
- **Context provided:** Requests to remove workspace tag, align cards, split columns, and visualize progress.
- **Prompt used:** Refactor home layout into independent columns and add circular workflow progress ring.
- **Result summary:** Implemented:
	- removed `workspace-tag` from main/admin layouts
	- independent Home columns (`Talent Skill Harvester` + `API healthcheck` in one column, `Workflow progress` in the other)
	- progress ring above workflow title showing completed phases out of 10
- **Accepted / changed:** Accepted all requested layout and visualization adjustments; kept styles within existing design system variables.
- **Why:** Better visual progress tracking and cleaner content grouping.

### Step 9 — Delivery ops (branch, commits, push, PR)
- **Goal:** Prepare and publish incremental work safely.
- **Context provided:** Request for branch creation, focused commits, push, and PR creation.
- **Prompt used:** Create feature branch, commit in logical chunks, push branch, open PR to `main`.
- **Result summary:** Published branch `feature/phase5-sqlite-repository` and created PR: `https://github.com/ikazlouwork/talent-skill-harvester/pull/6`.
- **Accepted / changed:** Kept commit history split by concern (API persistence, UI layout, workflow ring, startup reliability).
- **Why:** Improves reviewability and traceability.

### Step 10 — Dev startup reliability
- **Goal:** Reduce friction when starting app locally.
- **Context provided:** Frequent `npm run dev` failures from non-repo cwd and `dotnet watch` sensitivity to test artifacts.
- **Prompt used:** Stabilize API csproj watch inputs and document cwd-independent startup command.
- **Result summary:**
	- excluded `apps/api/tests/**` from watched/content items in API csproj
	- documented `npm --prefix c:\work\talent-skill-harvester run dev` in README
- **Accepted / changed:** Accepted as minimal operational fix and docs update.
- **Why:** Enables predictable one-command startup from any folder.

## Accepted/Changed and Why
- What was accepted directly:
	- Generated API/controller scaffolding, route wiring, and baseline validation patterns.
	- Small UI state polish changes with separate `ui:` commits.
	- xUnit project scaffold and controller-focused unit test structure.
- What was accepted directly (new):
	- Full Phase 5 persistence implementation with EF Core + SQLite + repository pattern.
	- Home layout refinements and workflow progress ring visualization.
	- Branch/push/PR delivery workflow for incremental review.
- What was refactored manually:
	- Kept implementation in-memory only for Phase 4 to avoid mixing with Phase 5 DB scope.
	- Excluded `apps/api/tests/**` from API project compile items to prevent duplicate/invalid compilation during solution test run.
- What was refactored manually (new):
	- Converted controller/store interactions to async service abstraction to preserve API contracts during persistence swap.
	- Refined Home grid into independent columns so card heights do not affect adjacent column layout.
- What was rejected and why:
	- Extra UI/admin features beyond scope were deferred to Phase 6.

## Insights
- Prompts that worked well:
	- “Implement one phase completely with explicit endpoint list and constraints.”
- Prompts that did not work well:
	- Broad prompts that mixed API, DB, and UI in one request.
- Prompting patterns that produced best results:
	- First define architecture/checklist, then generate feature-by-feature, then run smoke checks.
	- Keep one prompt = one narrow change + one focused commit message.
- Recommendations for future assignments:
	- Keep each prompt scoped to one phase and include acceptance criteria in bullet form.
	- Maintain at least one small `ui:` commit in each PR for steady UX improvements.
	- When using monorepo scripts, document `--prefix` startup fallback early to avoid cwd-related run issues.
