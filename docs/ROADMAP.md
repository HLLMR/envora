# Envora Roadmap (Spec-Aligned)

This roadmap is derived from `.reference/ENVORA_PRD.md` and the Phase 1 prompt in `.reference/DATA_MODEL_v3.0_READY_FOR_CURSOR.md`.

## Phase 0: Discovery & Design (1–2 weeks)

- Confirm Phase 1 scope is locked
- Confirm RBAC roles and permissions expectations (spec-defined matrix)
- Confirm “5 disciplines” navigation + persistent notes UX

## Phase 1: Core Platform (3–4 weeks) ✅ **COMPLETE**

Deliverables:

- ✅ Projects CRUD + team assignments
- ✅ Equipment CRUD (templates - Phase 2)
- ✅ Points CRUD (auto-point generation - Phase 2)
- ✅ Dashboard (KPIs, project list, team view)
- ✅ Baseline Notes (real-time scaffolding with SignalR)
- ✅ Full editing capabilities for Projects, Equipment, Points
- ✅ SignalR client-side integration
- ✅ All discipline pages (Overview, Financial, Schedule, Design, Service)

## Phase 2: Reports & Data Export (2–3 weeks)

- Schedules (interactive + PDF/Excel exports)
- BOM generator
- Valve/damper schedule calculators
- PDF assembly engine (non-Visio)

## Phase 3: Visio Integration (3–4 weeks)

- Desktop bridge scaffold + job polling protocol
- Visio COM automation spike(s)
- Diagram export → PDF upload pipeline

## Phase 4: Submittal Builder (2–3 weeks)

- One-button submittal generation orchestration
- Versioning and approval workflow

## Phase 5: Testing & Hardening (2–3 weeks)

- Load/perf testing
- UAT with engineering users
- Documentation and rollout readiness

## Phase 6: Migration & Rollout (1–2 weeks)

- Migration from PM list / Excel tools
- Training + cutover


