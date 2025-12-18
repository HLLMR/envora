# ADR 0003: Add Phase 1 CRUD endpoints for Devices / Controllers / Nodes / IO slots

## Status

Accepted

## Context

The Phase 1 prompt in `.reference/DATA_MODEL_v3.0_READY_FOR_CURSOR.md` explicitly calls for API CRUD for:

- Projects, Equipment, **Devices, Controllers, Nodes, Points**

However, `.reference/API_SPECIFICATION.md` currently defines routes for Projects, Equipment, and Points, but does not enumerate endpoint routes for Devices / Controllers / Nodes / IO slots.

## Decision

We will implement Phase 1 CRUD endpoints for the missing resources under `/api/v1`, using consistent nested routing:

- Devices: `/api/v1/projects/{projectId}/devices`
- Controllers: `/api/v1/projects/{projectId}/controllers`
- Nodes: `/api/v1/projects/{projectId}/controllers/{controllerId}/nodes`
- IO slots: `/api/v1/projects/{projectId}/controllers/{controllerId}/io-slots`

If/when `API_SPECIFICATION.md` is updated, we will reconcile route shapes and update this ADR.

## Consequences

- Phase 1 development can proceed without waiting on spec expansion.
- Route choices are documented and reversible (API versioning already assumed in spec).


