# Backlog (Phase 1 Focus)

This is an implementation checklist derived from the reference bundle. Treat this as the “next actions” list.

## Repo + foundations

- [ ] Align repo structure to `.reference/DEVELOPMENT_WORKFLOW.md` (add `src/Envora.Api`, `src/Envora.Domain`, `tests/*`)
- [ ] Add PR template and basic issue templates
- [ ] Add separate GitHub Actions workflows for build + test (and deploy later)

## Database + EF Core

- [x] Implement EF Core entity model for v3.0 schema (22 tables)
- [x] Add migrations and document local migration workflow
- [ ] Seed minimal data for local dev (optional)

## API (Envora.Api)

- [ ] Auth endpoints (local + Azure AD alternative)
- [x] CRUD: Projects
- [x] CRUD: Equipment
- [x] CRUD: Devices
- [x] CRUD: Controllers / Nodes / IO slots
- [x] CRUD: Points + soft point distribution
- [x] Notes endpoints + reactions
- [x] SignalR hub for notes + job status
- [x] Dashboard statistics endpoint

## Web (Envora.Web)

- [x] Implement layout (header + sidebar) aligned to UX plan
- [x] Dashboard page with KPIs and recent projects
- [x] Projects list + project detail shell
- [x] Project overview page with team assignments
- [x] Discipline navigation (Overview/Financial/Schedule/Design/Service)
- [x] Persistent notes panel wiring (SignalR)
- [x] Equipment list with edit/delete
- [x] Points list with edit/delete
- [x] Project editing functionality
- [x] SignalR client-side integration for real-time updates


