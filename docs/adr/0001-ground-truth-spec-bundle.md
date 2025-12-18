# ADR 0001: Reference bundle is the ground truth

## Status

Accepted

## Context

The repository includes a complete specification bundle under `.reference/` (PRD, technical design spec, API spec, workflow standards, Blazor architecture guide).

## Decision

We will treat the reference bundle as **ground truth**:

- Do not invent endpoints, entities, or roles that do not exist in the spec.
- When documents conflict, prefer the most recent/corrected technical spec (v3.0 schema).
- When ambiguity remains, write a new ADR and get a human decision.

## Consequences

- Implementation stays aligned with product intent and avoids rework.
- Specs drive code structure (repo layout, naming conventions, testing requirements).


