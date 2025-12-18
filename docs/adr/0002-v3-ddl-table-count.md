# ADR 0002: Use the v3.0 DDL blocks as canonical schema (currently 19 tables)

## Status

Accepted

## Context

The v3.0 technical spec references “22 tables”, but the actual schema section in:

- `.reference/ENVORA_TDS_COMPLETE_v3.0.md`

contains **19 explicit `CREATE TABLE` blocks**.

## Decision

We treat the **explicit DDL blocks** as the canonical database schema. Until the reference bundle is updated, we implement and migrate based on those 19 tables.

## Consequences

- EF Core model/migrations match the authoritative DDL that exists in the repo today.
- If/when the remaining tables are added to the spec, we will add them via a follow-up migration and update this ADR.


