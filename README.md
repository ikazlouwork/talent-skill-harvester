# Talent Skill Harvester

## Overview
Talent Skill Harvester is a planned Skill Extraction Tool assignment project with a UI + API architecture.
The final app will include multiple pages, reusable components, routing, and different layouts (for example, main and admin areas).

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

## Run plan
- TODO: Choose final stack bootstrap command and initialize app runtime.
- TODO: Add frontend run command documentation.
- TODO: Add backend/API run command documentation.
- TODO: Add database migration/seed instructions.
- TODO: Add CI checks and local validation flow.
