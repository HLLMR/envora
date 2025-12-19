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

- Equipment ↔ Devices ↔ Points ✅ **Implemented**
- Controllers ↔ Nodes ↔ IO slots ✅ **Implemented**
- Soft point distribution (one-to-many distribution) ✅ **Schema ready**
- Collaboration (Notes + Reactions) ✅ **Implemented**
- Operational traceability (Jobs + ActivityLogs) - **Schema ready**
 
Note: `.reference/ENVORA_TDS_COMPLETE_v3.0.md` currently contains **19 `CREATE TABLE` blocks** in the schema section.

**Current Implementation Status:**
- ✅ All 22 entities implemented in EF Core
- ✅ Initial migration applied
- ✅ Full CRUD operations for Projects, Equipment, Points, Devices, Controllers, Nodes, IO slots
- ✅ Notes with reactions and real-time updates

## Real-time

SignalR is used for:

- Notes updates (create/update/delete/reactions) ✅ **Implemented**
- Job status notifications (submittal generation pipeline) - **Phase 2**

**Current Implementation:**
- SignalR hub at `/hubs/project` with project group subscriptions
- Client-side `HubConnectionService` with automatic reconnection
- Real-time note updates across all connected clients
- Event handlers: `NoteAdded`, `NoteUpdated`, `NoteDeleted`, `ReactionAdded`

## Cross-cutting concerns

- **Auth**: JWT + Azure AD (Entra) option
- **RBAC**: enforce permissions server-side; UI should hide/disable but never trust client
- **Audit**: structured audit trail for changes (per spec)
- **Performance**: target API p95 < 100ms (spec target); use indexing/caching as needed


