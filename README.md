# Envora (vNext)

This is a **fresh** Envora revision scaffolded from scratch.

## What's included

- **Docker Compose**: `web` + `api` + `sqlserver`
- **NGINX Proxy Manager (NPM)** friendly defaults:
  - Web container name: `envora-web`
  - API container name: `envora-api`
  - Internal upstream: `envora-web:80`, `envora-api:80`
  - Compose network name: `envora_envora-network` (stable)

## Phase 1 Status

✅ **Core Platform Complete**

**Implemented Features:**
- Projects CRUD with team assignments
- Equipment CRUD with edit/delete
- Points CRUD with edit/delete
- Devices, Controllers, Nodes, IO slots CRUD
- Notes with real-time collaboration (SignalR)
- Dashboard with KPIs and project statistics
- Project detail pages with discipline navigation
- SignalR client-side integration for real-time updates
- Full editing capabilities for all entities

## Quick start (local)

1. Create an env file:
   - Copy `env.example` to `.env`
   - Set `SA_PASSWORD` to a strong password (SQL Server rules apply)
2. Start:
   - `docker compose up -d --build`
3. Visit:
   - Web UI: `http://localhost:8080/`
   - API: `http://localhost:5000/` (health: `/api/v1/health`, swagger: `/swagger`)

## Repo notes

- **License**: proprietary / private (see `LICENSE.md`)
- **Solution**: `Envora.sln` (ASP.NET Core `src/Envora.Web`)

## NGINX Proxy Manager (NPM)

See `docs/nginx-proxy-manager.md`.

## Course / roadmap (spec-aligned)

- `docs/COURSE.md`
- `docs/ROADMAP.md`
- `docs/ARCHITECTURE.md`
- `docs/BACKLOG.md`


