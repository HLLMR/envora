# Backlog (Phase 1 Focus)

This is an implementation checklist derived from the reference bundle. Treat this as the “next actions” list.

## Repo + foundations

- [ ] Align repo structure to `.reference/DEVELOPMENT_WORKFLOW.md` (add `src/Envora.Api`, `src/Envora.Domain`, `tests/*`)
- [ ] Add PR template and basic issue templates
- [ ] Add separate GitHub Actions workflows for build + test (and deploy later)

## Database + EF Core

- [ ] Implement EF Core entity model for v3.0 schema (22 tables)
- [ ] Add migrations and document local migration workflow
- [ ] Seed minimal data for local dev (optional)

## API (Envora.Api)

- [ ] Auth endpoints (local + Azure AD alternative)
- [ ] CRUD: Projects
- [ ] CRUD: Equipment
- [ ] CRUD: Devices
- [ ] CRUD: Controllers / Nodes / IO slots
- [ ] CRUD: Points + soft point distribution
- [ ] Notes endpoints + reactions
- [ ] SignalR hub for notes + job status

## Web (Envora.Web)

- [ ] Implement layout (header + sidebar) aligned to UX plan
- [ ] Dashboard page
- [ ] Projects list + project detail shell
- [ ] Discipline navigation (Overview/Financial/Schedule/Design/Service)
- [ ] Persistent notes panel wiring (SignalR)


