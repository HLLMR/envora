# Envora Architecture (Spec Snapshot)

This is a lightweight snapshot of the architecture described in:

- `.reference/ENVORA_TDS_COMPLETE_v3.0.md`
- `.reference/DEVELOPMENT_WORKFLOW.md`
- `.reference/BLAZOR_ARCHITECTURE.md`

## High-level components

- **Envora.Web**: Blazor Server UI (Bootstrap 5.3)
- **Envora.Api**: ASP.NET Core API (REST) + SignalR hubs
- **SQL Server**: primary relational store (Azure SQL in prod)
- **Blob Storage**: documents + generated PDFs + Visio artifacts
- **Service Bus**: async job orchestration + SignalR backplane (Azure)
- **Desktop Bridge**: Windows service that polls for jobs and runs Visio COM automation

## Data model

Use **v3.0** corrected model as source of truth:

- Equipment ↔ Devices ↔ Points
- Controllers ↔ Nodes ↔ IO slots
- Soft point distribution (one-to-many distribution)
- Collaboration (Notes + Reactions)
- Operational traceability (Jobs + ActivityLogs)
 
Note: `.reference/ENVORA_TDS_COMPLETE_v3.0.md` currently contains **19 `CREATE TABLE` blocks** in the schema section.

## Real-time

SignalR is used for:

- Notes updates (create/update/delete/reactions)
- Job status notifications (submittal generation pipeline)

## Cross-cutting concerns

- **Auth**: JWT + Azure AD (Entra) option
- **RBAC**: enforce permissions server-side; UI should hide/disable but never trust client
- **Audit**: structured audit trail for changes (per spec)
- **Performance**: target API p95 < 100ms (spec target); use indexing/caching as needed


