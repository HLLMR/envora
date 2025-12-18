# Envora Development Course (Ground Truth)

This repository is being implemented against the reference specification bundle in `.reference/`.

If something is unclear, **refer to the spec first**. If it is still ambiguous, create an ADR under `docs/adr/` and get a human decision.

## 1) Ground truth documents (in priority order)

When documents disagree, use this priority:

1. **Data model + DDL (authoritative)**:
   - `.reference/ENVORA_TDS_COMPLETE_v3.0.md` (schema section contains 19 `CREATE TABLE` blocks; corrected controllers/nodes/devices constraints)
   - `.reference/DATA_MODEL_v3.0_READY_FOR_CURSOR.md` (summary + Phase 1 prompt)
2. **Development standards**:
   - `.reference/DEVELOPMENT_WORKFLOW.md` (repo structure, naming, testing, CI/CD)
   - `.reference/BLAZOR_ARCHITECTURE.md` (Blazor patterns, Pages/Components/Services/Models)
3. **API contracts**:
   - `.reference/API_SPECIFICATION.md` (endpoints + schemas)
4. **Product + UX**:
   - `.reference/ENVORA_PRD.md`
   - `.reference/Envora UI UX Design Plan - v2.pdf`

Note: some “summary” docs mention 13/18 tables. We are using v3.0; the **DDL blocks currently define 19 tables**.

## 2) Non-negotiable platform decisions (locked by spec)

- **Frontend**: Blazor Server + Bootstrap 5.3
- **Backend**: ASP.NET Core API (REST) + SignalR hubs
- **Data access**: Entity Framework Core + SQL Server (Azure SQL in prod)
- **Real-time**: SignalR (notes + job status), with Service Bus backplane in Azure
- **Auth**: JWT + Azure AD (Microsoft Entra ID) option; local auth may exist for dev
- **Storage**: Azure Blob Storage for documents (PDFs, Visio exports, uploads)
- **Workflow**: GitHub Flow + standards in `.reference/DEVELOPMENT_WORKFLOW.md`

## 3) Product scope guardrails

- Follow Phase scope explicitly; if not in spec → treat as out of scope for MVP.
- Persistent **Notes Panel on every tab** is a core UX requirement.
- Navigation model is the 5-discipline grouping:
  - Overview, Financial, Schedule, Design, Service

## 4) Phase plan (what we build first)

### Phase 1 (Weeks 1–2 in the bundle prompt / PRD “Core Platform”)

- Establish repo structure per `.reference/DEVELOPMENT_WORKFLOW.md`
- Implement database schema + EF model layer (v3.0 DDL blocks; 19 tables)
- Implement core API CRUD:
  - Projects, Equipment, Devices, Controllers, Nodes, Points
- Implement baseline Blazor shell:
  - Layout (header + sidebar)
  - Dashboard
  - Project list + project detail shell with discipline navigation
- Wire real-time notes scaffolding (SignalR contract), then iterate to full notes UX

## 5) Engineering principles

- Prefer **small PRs** that land continuously.
- Add tests as you add business logic (target: >80% for services).
- No “inventing” endpoints, tables, or roles: mirror the spec.


