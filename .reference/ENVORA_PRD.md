# Envora Platform - Product Requirements Document (PRD)

**Version:** 1.0  
**Date:** December 2025  
**Author:** [Your Organization]  
**Status:** Active Development  

---

## Executive Summary

**Envora** is a cloud-native BAS (Building Automation System) project management and design automation platform built to replace fragmented Excel/Access/VBA workflows at Enviromatic Systems with a modern, integrated, web-first solution.

**Key Value Proposition:**
- **Single source of truth** for projects, equipment, points, and design data
- **Automated submittal generation** (complete PDF packages in 1 minute vs. 1+ day)
- **Native Visio integration** via cloud-orchestrated desktop bridge (no VBA)
- **Engineering-first design** that scales to company-wide adoption
- **Competitive advantage** – replaces $200+/user/year Smartware licensing with one-time investment

---

## 1. Vision & Goals

### Vision Statement
Empower Enviromatic Systems to design, manage, and deliver building automation systems faster, with higher quality, and complete visibility—from sales through installation, warranty, and service.

### Strategic Goals
1. **Eliminate manual workflows** – Replace Excel-based design tools, PM tracking, and BOM management with automated data-driven processes
2. **Improve project visibility** – Real-time status across ops, engineering, programming, and management
3. **Reduce submittal cycle time** – From 1–2 days (manual assembly) to 5 minutes (automated)
4. **Retire technical debt** – Replace VBA macros, Access DB, and file-based versioning with a modern stack
5. **Enable scalability** – Support 10+ engineers, 200+ concurrent projects, unlimited historical data
6. **Build competitive edge** – Develop proprietary platform usable long-term internally; potential future product for industry

### Success Metrics
- **Adoption**: 100% of engineering team actively using platform within 3 months
- **Time to submittal**: <10 minutes (from "Generate" button to download)
- **Data accuracy**: Zero manual data re-entry for standard workflows
- **Uptime**: 99.5% platform availability during business hours
- **User satisfaction**: >4/5 average rating from engineering team after 6 weeks

---

## 2. Problem Statement

### Current State (Pain Points)

**A. Project Management (Ops)**
- PM List is a flat Excel file (282 columns, 882 rows)
- No workflow automation (manual status updates)
- No real-time visibility (email/Slack updates only)
- Scattered financial tracking (multiple sheets, no audit trail)
- No integration with other systems (CRM, accounting, scheduling)

**B. Engineering Design**
- Equipment specifications exist in 3+ places (PM List, Excel design tool, Visio)
- Manual point list creation (error-prone, time-consuming)
- VBA macros for automation (fragile, version-control nightmare, single-point failure)
- Visio shapes are manually positioned/named (no link to actual data)
- Submittal packages assembled manually (cover → specs → drawings → BOM → signatures)
- No version control for designs (file names like `FW25-001_v3_FINAL_actual_final.vsdx`)

**C. Data Management**
- Equipment data duplicated across Excel sheets
- BOM calculations in Excel formulas (hard to audit, easy to break)
- Valve/damper sizing done manually, calculations hidden in spreadsheet
- No single point of truth (leads to inconsistencies, rework)

**D. Tooling & Technical Debt**
- VBA macros breaking with Office updates
- Access DB (boss's side project) not integrated with workflows
- File sharing via email/SharePoint (version conflicts, no audit trail)
- No cross-platform access (tied to Windows + local install)
- No mobile access for ops/field teams

### Business Impact
- **Engineering time wasted**: ~8 hrs/week per engineer on manual data entry, file management
- **Submittal delays**: 1–2 day cycle time (customer approval waiting)
- **Data errors**: Specifications mismatched between drawings and schedules (rework)
- **Scalability blocked**: Adding new engineers requires them to learn fragile macro system
- **Competitive risk**: Using 1998 architecture while competitors modernize

---

## 3. Solution Overview

### High-Level Architecture

```
┌────────────────────────────────────────────────────────┐
│         ENVORA PLATFORM (Cloud)                        │
│  ASP.NET Core + Blazor Server + Azure SQL + Blob      │
├────────────────────────────────────────────────────────┤
│                                                        │
│  • Projects (PM List replacement)                      │
│  • Equipment + Points (design foundation)              │
│  • BOM + Schedules (auto-calculated)                   │
│  • Documents + Versioning                              │
│  • Submittal Builder (orchestration engine)            │
│  • Team Dashboard (visibility layer)                   │
│  • Audit Trail (compliance)                            │
│                                                        │
└────────────────────┬─────────────────────────────────┘
                     │ REST API + WebSocket
         ┌───────────┴───────────┐
         │                       │
    ┌────▼──────┐           ┌────▼──────┐
    │  Desktop  │           │  Desktop  │
    │  Bridge   │           │  Bridge   │
    │  Service  │           │  Service  │
    │           │           │           │
    │ • Visio   │           │ • Visio   │
    │ • COM     │           │ • COM     │
    │ • Export  │           │ • Export  │
    └────┬──────┘           └────┬──────┘
         │                       │
    ┌────▼──────┐           ┌────▼──────┐
    │ Visio 2024│           │ Visio 2024│
    │ (Local)   │           │ (Local)   │
    └───────────┘           └───────────┘
```

**Key Components:**

1. **Web Platform** (cloud, accessible anywhere)
   - Projects (ops hub)
   - Equipment + Points (design data model)
   - Schedules & calculations (BOM, valve, damper)
   - Visio integration coordination
   - Submittal assembly

2. **Desktop Bridge** (local Windows Service, one per engineer)
   - Polls cloud for Visio export jobs
   - Manages local Visio COM automation
   - Exports diagrams to PDF
   - Uploads results back to cloud

3. **Data Store** (Azure SQL + Blob)
   - Relational data (projects, equipment, points)
   - Document storage (Visio, specs, PDFs)
   - Version history (audit trail)

---

## 4. Detailed Requirements

### 4.1 Scope – What's In (Phase 1 v1.0)

**A. Project Management Module**
- [x] Project CRUD (create, read, update, delete)
- [x] Project search & filtering
- [x] Team assignments (AE, PM, PROG, installer, MEP, graphics)
- [x] Status tracking (design%, prog%, install%)
- [x] Key dates (start, target completion, warranty)
- [x] Financial tracking (contract $, labor $, material $)
- [x] Notes & change orders

**B. Equipment & Points Module**
- [x] Equipment CRUD
- [x] Equipment template binding (RTU, AHU, VAV, pump, chiller → auto-points)
- [x] Point list management (auto-created from template, editable)
- [x] Bulk equipment import (copy/paste, CSV)
- [x] Equipment specs storage (JSON blob for flexibility)

**C. Reports & Schedules**
- [x] Equipment schedules (PDF export)
- [x] Point lists (PDF export)
- [x] Bill of Materials (BOM) – aggregated from equipment
- [x] Valve schedule (calculated from specs)
- [x] Damper schedule (calculated from specs)

**D. Visio Integration**
- [x] Desktop bridge (Windows Service) installed on engineer PCs
- [x] Visio export job queue
- [x] RTU/AHU control panel diagram export
- [x] Floorplan diagram export
- [x] PDF generation from Visio

**E. Submittal Builder**
- [x] One-click submittal generation
- [x] Automatic assembly: cover → equipment schedules → diagrams → BOM → sequences → signature
- [x] PDF bookmarking & TOC
- [x] Hyperlinks (BOM to suppliers, points to diagrams)

**F. Document Management**
- [x] Upload/store Visio, specs, calc sheets
- [x] Version history
- [x] Link documents to projects/equipment

**G. Dashboard & UI**
- [x] Project list (with status, team, key dates)
- [x] Project detail pages (overview, equipment, points, documents, status)
- [x] Team workload (who's assigned, capacity)
- [x] Quick filters (by customer, status, date range)

**H. Data Persistence & Audit**
- [x] All changes logged (who, what, when)
- [x] Deliverable versioning (Rev A, B, C)
- [x] Document version history

---

### 4.2 Scope – What's Out (Future Phases)

**Phase 2 (v1.1+)**
- [ ] Advanced estimating module (labor costing, service rates)
- [ ] Equipment template editor UI (vs. code-based)
- [ ] CRM integration (Salesforce / Dynamics)
- [ ] Accounting integration (QuickBooks / NetSuite)
- [ ] Mobile apps (ops dashboard on phone)
- [ ] Field team tools (site commissioning data collection)

**Phase 3 (v2.0+)**
- [ ] Real-time collaboration (multi-engineer on same project)
- [ ] Advanced workflows (design review gates, approval chains)
- [ ] Customizable report templates
- [ ] Equipment library management (internal catalog)
- [ ] Supplier integrations (pricing, lead time)

---

## 5. Data Model (Core Entities)

### 5.1 Core Project Data

```
Project
├─ ProjectId (PK)
├─ ProjectNumber (unique, e.g., "FW25-001")
├─ JobName, Customer (FK), Contact (FK)
├─ Dates: StartDate, CompletionDate, WarrantyEndDate
├─ Financial: ContractAmount, InstallAmount, MaterialAmount, LaborAmount
├─ Status: Design%, Programming%, Install%
├─ Team: ApplicationsEngineer (FK), ProjectManager (FK), Programmer (FK), MEPEngineer (FK), GraphicsPerson (FK)
├─ Metadata: CreatedBy, ModifiedBy, CreatedDate, ModifiedDate
└─ Relations: Equipment[], Documents[], Deliverables[], Notes[], ChangeOrders[]
```

### 5.2 Engineering Data

```
Equipment
├─ EquipmentId (PK)
├─ ProjectId (FK)
├─ EquipmentTag (e.g., "RTU-1"), EquipmentType ("RTU", "VAV", etc.)
├─ Manufacturer, ModelNumber, Specs (JSON)
├─ Location, Building, Building Level, AreaZone
├─ VisioPageName, VisioShapeId (for diagram sync)
└─ Relations: Points[], Valves[], Dampers[]

Point
├─ PointId (PK)
├─ EquipmentId (FK)
├─ PointTag (e.g., "RT-1"), PointType ("Primary", "Secondary", "Tertiary")
├─ SignalType ("Analog", "Binary", "Network"), DataType ("Temperature", "Pressure", etc.)
├─ InputOutput ("Input", "Output", "Bidirectional")
├─ MinValue, MaxValue, DefaultValue
├─ ControllerTag, TerminalBlockNumber, TerminalNumber
└─ VisioShapeId, LinkedToWiringDiagram

EquipmentTemplate
├─ TemplateId (PK)
├─ EquipmentType (e.g., "RTU")
├─ StandardPoints[] (JSON – what points to auto-create)
├─ StandardTasks[] (labor, commissioning hours)
├─ StandardSpecs (default values)
└─ BindingRules (if CFM > X, select this controller, etc.)
```

### 5.3 BOM & Schedules

```
Valve
├─ ValveId (PK)
├─ ProjectId (FK)
├─ ValveTag, System, Service, FlowPattern
├─ WaterFlowGPM, DesignDeltaP, CalculatedCV
├─ PipeSize, Manufacturer, Model, Actuator
├─ Quantity

Damper
├─ DamperId (PK)
├─ ProjectId (FK)
├─ DamperTag, SystemServed, Position
├─ Width, Height, BladeType, Manufacturer, Model
├─ TorqueRequired, ActuatorPartNumber
├─ Quantity
```

### 5.4 Documents & Deliverables

```
ProjectDocument
├─ DocumentId (PK)
├─ ProjectId (FK)
├─ FileName, FilePath (Azure Blob), ContentType
├─ DocumentType (Visio, PDF, Excel, Word, etc.)
├─ DocumentCategory (Specification, Drawing, Submittal, etc.)
├─ Version, DocumentDate, IsParsed
├─ CreatedBy, ModifiedBy

Deliverable (Submittal)
├─ DeliverableId (PK)
├─ ProjectId (FK)
├─ DeliverableType ("Design Package", "Submittal", "As-Built")
├─ Status ("Draft", "InternalReview", "ExternalReview", "Approved")
├─ GeneratedPdfUrl, Revision (Rev A, B, C)
├─ Contents[] (JSON array of sections)
├─ InternalReviewedBy, CustomerApprovedBy
├─ CreatedDate, LastModifiedDate
```

### 5.5 Supporting Models

```
Company (CRM)
├─ CompanyId (PK)
├─ CompanyName, Category (Customer, Vendor, Contractor, MEPEngineer)
├─ AddressId (FK), WebSite, TaxId
├─ ParentCompanyId (for subsidiaries)

Contact (CRM)
├─ ContactId (PK)
├─ FirstName, LastName, Title, Email
├─ PhoneNumbers[] (polymorphic – can belong to Company or Contact)

ChangeOrder
├─ ChangeOrderId (PK)
├─ ProjectId (FK)
├─ ChangeOrderNumber (CO-1, CO-2, etc.)
├─ Description, Amount, DateIssued, DateApproved, Status
├─ Notes

User (Employee)
├─ UserId (PK)
├─ Username, Email, FirstName, LastName
├─ Role (SalesPerson, ApplicationsEngineer, ProjectManager, Programmer, GraphicsPerson)
├─ Department, EmployeeId, IsActive

Note (Generic)
├─ NoteId (PK)
├─ Content, Category (General, Issue, ChangeRequest, Technical, etc.)
├─ ProjectId|CompanyId|ContactId (polymorphic FK)
├─ CreatedBy, CreatedDate
```

---

## 6. User Personas & Use Cases

### 6.1 Primary Users

**A. Applications Engineer (Jill)**
- **Goal**: Design controls systems, manage equipment specs, generate submittals
- **Daily workflow**:
  1. Opens Envora, creates new project
  2. Adds equipment (RTU-1, VAV-1, VAV-2, pump, chiller)
  3. Platform auto-creates standard points for each equipment type
  4. Reviews/edits points as needed
  5. Clicks "Generate Submittal"
  6. Gets PDF in 1 minute, reviews, sends to customer
- **Key pain now**: Manually creating point lists, assembling submittals
- **Success**: "I never touch Excel again for this project"

**B. Project Manager (Marcus)**
- **Goal**: Track project status, team capacity, financials, deadlines
- **Daily workflow**:
  1. Logs in to Envora dashboard
  2. Sees all 20 projects at a glance (design%, prog%, install%, key dates)
  3. Identifies bottlenecks ("FW25-008 design is 2 weeks late")
  4. Adjusts team assignments
  5. Generates monthly status report
- **Key pain now**: Manually scrolling PM List, emailing for updates
- **Success**: "I see everything I need in one screen"

**C. Programmer (David)**
- **Goal**: Access equipment specs and point lists to write control logic
- **Daily workflow**:
  1. Logs in, clicks on project (FW25-010)
  2. Sees equipment list and all points (auto-organized by type)
  3. Exports point list (if needed) or works directly in platform
  4. Sees when points linked to diagrams (page numbers hyperlinked)
- **Key pain now**: Manually cross-referencing Excel sheets and Visio
- **Success**: "Everything I need is here, no hunting through files"

**D. Graphics Person (Jennifer)**
- **Goal**: Manage Visio diagrams, update floorplans, export for print
- **Daily workflow**:
  1. Platform notifies her: "RTU equipment added to FW25-003, needs diagram update"
  2. Clicks "Generate Diagram"
  3. Desktop bridge exports Visio with auto-populated equipment data
  4. She downloads, opens locally in Visio, refines (moves shapes, adds connectors)
  5. Exports as PDF
  6. Uploads back to platform
- **Key pain now**: Manually recreating shapes in Visio, no data sync
- **Success**: "Equipment data is already in the drawing, I just refine layout"

**E. Ops Lead (Patricia)**
- **Goal**: Monitor project pipeline, schedule, financials across all branches
- **Monthly workflow**:
  1. Views platform dashboard
  2. Exports project report (list, status, $, people, timeline)
  3. Identifies under-utilized resources
  4. Plans next quarter
- **Key pain now**: Manual consolidation of PM List + branch sheets
- **Success**: "All projects visible in one place, no compilation needed"

---

### 6.2 Use Cases

**UC1: Create New Project (Ops/Engineering)**
1. User clicks "New Project"
2. Enters: ProjectNumber (FW25-101), Customer, Contact, Contract$, Dates
3. Assigns: AE, PM, PROG, Installer, MEP, Graphics
4. Platform creates record, sends notifications
5. User can immediately start adding equipment

**UC2: Add Equipment & Auto-Generate Points (Engineering)**
1. Engineer clicks "Add Equipment"
2. Selects type (RTU)
3. Enters: Tag (RTU-1), Location, Specs (CFM, tons, etc.)
4. Platform:
   - Creates Equipment record
   - Auto-creates 6 standard Points (from RTU template)
   - Assigns controller (based on specs + rules)
   - Updates BOM
5. Engineer can manually add/edit points as needed

**UC3: Generate Equipment Schedule Report (Any)**
1. User navigates to Project → Reports
2. Clicks "Equipment Schedule"
3. Platform queries all Equipment for project
4. Renders as interactive table + PDF export
5. User downloads PDF for submittal

**UC4: Generate Complete Submittal Package (Engineering)**
1. Engineer confirms project is ready: design% = 100%
2. Clicks "Generate Submittal"
3. Platform:
   - Queues Visio export jobs (control panels, floorplan, network)
   - Waits for completion (with timeout)
   - Generates all sections (cover, equipment schedules, point lists, BOM, valve/damper schedules)
   - Assembles into single PDF with TOC + hyperlinks
4. Engineer downloads: `FW25-101_Design_Submittal_RevA_20251218.pdf`
5. Sends to customer

**UC5: Update Equipment Specs → Visio Auto-Updates (Engineering)**
1. Engineer edits RTU specs (changes CFM, equipment tag)
2. Saves
3. Platform queues "export RTU diagram" job
4. Desktop bridge invokes Visio COM:
   - Loads template
   - Updates RTU shape properties
   - Exports PDF
5. Engineer is notified: "Diagram updated"

**UC6: Review Project Status (PM)**
1. PM logs in to dashboard
2. Sees KPIs: total projects, avg design%, avg prog%, resource utilization
3. Filters by "In Flight" projects
4. Sees table: Project | Status | AE | PM | PROG | Key Dates | Issues
5. Clicks on FW25-008 → sees detail page
6. Sees design is 85% (spec says 100% target) → sends message to AE

**UC7: Migrate Existing Project Data (Ops)**
1. Ops uploads existing Excel design tool
2. Platform parses equipment, points, valves, dampers
3. Auto-maps to database
4. User reviews mappings, corrects any mismatches
5. Imports data into project
6. Legacy Excel can be archived

---

## 7. Technical Requirements

### 7.1 Technology Stack

**Backend**
- **Runtime**: .NET 8.0
- **Framework**: ASP.NET Core 8.0
- **UI**: Blazor Server (real-time, server-side rendering)
- **ORM**: Entity Framework Core 8.0
- **Database**: Azure SQL Database (managed, serverless pricing optional)
- **Storage**: Azure Blob Storage (documents, Visio files, PDFs)
- **Messaging**: Azure Service Bus (async jobs, notifications)
- **Search**: Azure Cognitive Search (optional, for large deployments)

**Desktop Bridge (Local Windows Service)**
- **Language**: C# .NET
- **Runtime**: .NET 8.0 Runtime
- **Visio Interop**: Microsoft.Office.Interop.Visio COM
- **Azure SDK**: Azure.Identity, Azure.Storage.Blobs
- **HTTP**: HttpClient + System.Net.Http

**PDF Generation**
- **Library**: Aspose.PDF or iTextSharp (server-side PDF assembly)
- **Visio Export**: Visio's native ExportAsFixedFormat (PDF)

**Frontend**
- **Framework**: Blazor Server (C# in browser via WebAssembly-like experience, but server-rendered)
- **UI Library**: Bootstrap 5 (responsive, accessible)
- **Charts**: Chart.js (project dashboards, status visualizations)
- **Notifications**: SignalR (real-time updates to UI)

**Infrastructure**
- **Cloud**: Azure (App Service, SQL Database, Blob Storage, Service Bus)
- **CI/CD**: Azure DevOps or GitHub Actions
- **Monitoring**: Application Insights
- **Container**: Docker (optional, for future self-hosted deployments)

---

### 7.2 Performance Requirements

| Metric | Target |
|--------|--------|
| **Page load time** | <2 sec (P95) |
| **Project list query** | <500ms (100+ projects) |
| **Equipment CRUD** | <300ms |
| **PDF generation** | <30 sec (standard submittal) |
| **Visio export** | <15 sec (diagram export + upload) |
| **Concurrent users** | 50+ simultaneous |
| **Data volume** | 1000+ projects, 100K+ points, 5+ years history |
| **API response time** | <100ms (P95) |
| **Uptime** | 99.5% during business hours |

---

### 7.3 Security & Compliance

- **Authentication**: Azure AD (Microsoft Entra ID) or local account
- **Authorization**: Role-based access control (RBAC)
  - SalesPerson: View projects, create new
  - ApplicationsEngineer: Full equipment/point editing, submittal generation
  - ProjectManager: View all, approve/review submittals
  - Programmer: View equipment/points/specs
  - GraphicsPerson: Edit Visio metadata, upload diagrams
  - Executive/Admin: Full access + user management
- **Data encryption**: HTTPS (TLS 1.3) for all traffic
- **Database encryption**: Azure SQL TDE (Transparent Data Encryption)
- **Audit logging**: All changes logged (user, timestamp, old value, new value)
- **Compliance**: GDPR-ready (data export, deletion), SOC 2 ready (for future SaaS offerings)

---

### 7.4 Scalability

- **Horizontal scaling**: Stateless app service (auto-scale 1–10 instances based on load)
- **Database**: Azure SQL Serverless (auto-scale compute) or standard with read replicas
- **Caching**: Redis (optional, for future performance optimization)
- **Search**: Azure Cognitive Search (for large deployments with 1000+ projects)
- **File storage**: Blob storage (unlimited, cheap, accessible globally)

---

## 8. Phasing & Timeline

### Phase 0: Discovery & Design (1–2 weeks)
**Deliverable**: Finalized requirements, architecture review, kickoff with team

### Phase 1: Core Platform (3–4 weeks)
**Deliverable**: Projects, Equipment, Points, Templates
- Projects CRUD + team assignments
- Equipment templates (RTU, AHU, VAV, pump, chiller)
- Equipment CRUD with auto-point generation
- Dashboard (KPIs, project list, team view)

### Phase 2: Reports & Data Export (2–3 weeks)
**Deliverable**: All non-Visio schedules, BOM generator, PDF assembly
- Equipment schedules (interactive + PDF)
- Point lists (sorted, formatted, PDF export)
- BOM generator (equipment × parts × costs)
- Valve/damper schedule calculators
- PDF assembly engine (cover + sections + TOC)

### Phase 3: Visio Integration (3–4 weeks)
**Deliverable**: Desktop bridge + Visio export engine + submittal generation
- Desktop bridge (Windows Service) scaffold
- Visio COM interop (shape property mapping)
- Diagram export jobs (RTU, AHU, floorplan)
- PDF generation from Visio
- Job queue + status tracking

### Phase 4: Submittal Builder (2–3 weeks)
**Deliverable**: One-button submittal generation + review workflow
- Orchestrate all sections into single PDF
- Add hyperlinks, bookmarks, TOC
- Deliverable versioning (Rev A, B, C)
- Approval workflow (internal → external)

### Phase 5: Testing & Hardening (2–3 weeks)
**Deliverable**: Production-ready, scaled, documented
- Performance testing (1000+ projects)
- Stress testing (concurrent users)
- UAT with real engineering team
- Documentation (user guides, admin guides, API docs)

### Phase 6: Migration & Rollout (1–2 weeks)
**Deliverable**: Live in production, team trained
- Data migration (PM List → Projects)
- Design tool migration (Excel → Equipment + Points)
- Team training
- Cutover (pilot → full adoption)

**Total Timeline**: 14–18 weeks (full v1.0)

---

## 9. Definition of Done

A feature is "Done" when:

- [ ] **Code written** and passes lint/style checks
- [ ] **Unit tests** written (>80% coverage for business logic)
- [ ] **Integration tests** pass (data flows correctly)
- [ ] **Database migrations** applied successfully
- [ ] **API endpoints** documented (Swagger/OpenAPI)
- [ ] **UI reviewed** by PM or stakeholder
- [ ] **Performance acceptable** (meets targets in section 7.2)
- [ ] **Security reviewed** (no SQL injection, XSS, CSRF vulnerabilities)
- [ ] **Deployed to staging** and verified
- [ ] **Documentation updated** (user guide, if needed)
- [ ] **Ready for UAT** or production release

---

## 10. Success Criteria

### v1.0 Release (MVP)

- [ ] **Projects module works**: 10+ projects successfully created, tracked, updated
- [ ] **Equipment & points**: RTU, AHU, VAV, pump, chiller templates functional; auto-point generation works
- [ ] **Submittals generated**: Complete PDF (cover + schedules + BOM + diagrams) in <2 minutes
- [ ] **Engineering team using it**: 3+ engineers actively logging in daily
- [ ] **Zero data loss**: All changes tracked, audit trail complete
- [ ] **Uptime**: 99%+ uptime for 2 consecutive weeks
- [ ] **Performance**: Page loads <2 sec, PDF generation <30 sec

### v1.1 (First Iteration Improvements)

- [ ] **Mobile responsive**: Works on tablet/phone for quick project lookup
- [ ] **Bulk import**: Import existing Excel design files (equipment + points)
- [ ] **Estimating**: Labor hours + cost tracking
- [ ] **CRM integration**: Link to Salesforce or internal CRM

---

## 11. Risk Management

| Risk | Probability | Impact | Mitigation |
|------|-------------|--------|-----------|
| **Visio COM interop complexity** | Med | High | Early prototype (Phase 1), dedicated research spike |
| **PDF assembly failures** | Low | High | Comprehensive testing, fallback to manual if needed |
| **Database migration data loss** | Low | Critical | Multiple backup strategies, dry-run migrations |
| **Team adoption resistance** | Med | Med | Early stakeholder involvement, phased rollout, training |
| **Performance degradation at scale** | Med | Med | Load testing Phase 4, database indexing, caching strategy |
| **Azure cost overruns** | Low | Med | Budget alerts, resource monitoring, cost optimization Phase 3 |

---

## 12. Assumptions & Constraints

**Assumptions**
- All engineers use Windows + Visio 2024 or later
- Azure subscription available (or can be provisioned)
- Internet connectivity always available (platform is cloud-only, no offline mode v1)
- Team size stays <50 engineers (scalability concerns for larger teams)

**Constraints**
- **Licensing**: Visio must be licensed per engineer (not by platform)
- **Desktop bridge**: Must be installed on each engineer's PC (manual install or MSI)
- **Visio version**: Support only Visio 2022 or later (older versions have compatibility issues)
- **Browser**: Edge, Chrome, or Firefox required (IE not supported)
- **Budget**: ~$100K development, ~$10K/year infrastructure (Azure)

---

## 13. Appendix: Glossary

| Term | Definition |
|------|-----------|
| **Equipment** | A mechanical device (RTU, AHU, pump, valve, etc.) that is controlled by the BAS |
| **Point** | A single data point (sensor or actuator) associated with equipment (e.g., Return Air Temperature, Damper Position) |
| **Primary/Secondary/Tertiary** | Classification of points; Primary = main equipment control, Secondary = supporting systems, Tertiary = general monitoring |
| **BOM** | Bill of Materials; list of all equipment, parts, labor needed for a project |
| **Submittal** | Formal documentation package (drawings, specs, schedules) submitted to customer for approval |
| **Deliverable** | Output artifact (PDF, Visio file, etc.) linked to a project |
| **Equipment Template** | Predefined set of standard points for a particular equipment type (e.g., RTU always has RT, SAT, MAT, DAM1, DAM2, FDP) |
| **Desktop Bridge** | Local Windows Service running on engineer's PC that handles Visio COM automation and syncs with cloud platform |
| **Visio Metadata** | Database record linking Visio shapes to actual equipment data (shape ID ↔ equipment ID) |

---

## 14. Sign-Off

| Role | Name | Date | Signature |
|------|------|------|-----------|
| **Product Owner** | [Your Name] | [Date] | _____ |
| **Engineering Lead** | [Name] | [Date] | _____ |
| **Executive Sponsor** | [Boss Name] | [Date] | _____ |
| **QA Lead** | [Name] | [Date] | _____ |

---

**Document Version History**

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | Dec 18, 2025 | [Author] | Initial PRD |

---

**Next Steps**

1. **Share & Review**: Distribute PRD to stakeholders, collect feedback (48-72 hours)
2. **Refine**: Address feedback, finalize scope
3. **Kickoff**: Engineering team review, architecture deep-dive
4. **Phase 0 Completion**: Finalized requirements, design docs, project setup

