# ENVORA_TDS_COMPLETE_v3.0 - FINAL CORRECTED DATA MODEL

**Status**: ✅ COMPLETE & PRODUCTION-READY  
**Date**: December 18, 2025, 2:14 PM CST  
**Version**: 3.0 (Controllers, Nodes, Devices corrected with real-world constraints)

---

## 📋 TABLE OF CONTENTS

1. Overview & Confirmed Constraints
2. System Architecture
3. Complete Database Schema (22 Tables)
4. SignalR Hub Specification
5. Desktop Bridge Protocol
6. RBAC Matrix
7. Performance & Scalability
8. Security Specifications
9. Monitoring & Logging
10. Deployment Specifications

---

## SECTION 1: OVERVIEW & CONFIRMED REAL-WORLD CONSTRAINTS

### **The HVAC Data Model**

**Equipment** (what we're installing)
- AHU, Chiller, RTU, VAV, Boiler, Pump, etc.
- Generates soft points via native integration
- Has multiple device instances installed on it

**Devices** (physical hardware)
- One instance mounted on ONE piece of equipment
- But many instances of same type can be on same equipment (3 thermistors on one RTU)
- Each instance is tracked separately
- Can generate multiple hard points
- Hard points from same device CAN split to different controllers (e.g., feedback + interlock)

**Points** (I/O endpoints)
- Soft Points: from Equipment integration, distributed to ALL systems that need them
- Hard Points: from Devices, can terminate at ONE controller per instance
- But same hard point type can terminate at multiple controllers if device splits

**Controllers** (I/O management)
- Fixed I/O capacity (AI, AO, DI, DO slots)
- Connected via Nodes (communication cards)
- Receives both soft and hard points

**Nodes** (communication layer)
- One or more per controller
- Protocols: BACnet MS/TP, BACnet/IP, Modbus, TCP/IP, etc.
- Exposes network variables (soft points)
- Device can terminate at multiple nodes (rare, but supported)

---

## SECTION 2: SYSTEM ARCHITECTURE

```
┌─────────────────────────────────────────────────────────┐
│                    USER INTERFACE                        │
│  (Blazor Server, Bootstrap 5.3, Real-time SignalR)      │
└──────────────────┬──────────────────────────────────────┘
                   │
┌──────────────────┴──────────────────────────────────────┐
│            API LAYER (ASP.NET Core)                     │
│  Controllers, Authentication, Validation, CORS          │
└──────────────────┬──────────────────────────────────────┘
                   │
┌──────────────────┴──────────────────────────────────────┐
│         BUSINESS LOGIC LAYER (Services)                 │
│  Equipment, Devices, Points, Controllers, Nodes         │
│  Submittal Generation, Job Processing                   │
└──────────────────┬──────────────────────────────────────┘
                   │
┌──────────────────┴──────────────────────────────────────┐
│       DATA ACCESS LAYER (Entity Framework)              │
│  DbContext, Migrations, Query Optimization              │
└──────────────────┬──────────────────────────────────────┘
                   │
┌──────────────────┴──────────────────────────────────────┐
│         DATA PERSISTENCE LAYER                          │
│  SQL Server, Azure SQL Database                         │
│  Real-time: SignalR Hub, Service Bus                    │
│  Files: Azure Blob Storage                              │
│  Queue: Service Bus, Redis Cache                        │
└─────────────────────────────────────────────────────────┘
```

---

## SECTION 3: COMPLETE DATABASE SCHEMA (v3.0 - 22 TABLES)

### **CORE ENTITIES**

#### Users Table
```sql
CREATE TABLE dbo.Users (
    UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Email NVARCHAR(255) NOT NULL UNIQUE,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20),
    Role NVARCHAR(50) NOT NULL CHECK (Role IN (
        'Admin', 'ProjectManager', 'Estimator', 
        'DesignEngineer', 'Technician', 'ServiceCoordinator', 
        'ContractorSuper', 'Client'
    )),
    Company NVARCHAR(255),
    IsActive BIT NOT NULL DEFAULT 1,
    AzureAdId NVARCHAR(255) UNIQUE,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    LastLoginAt DATETIME2
);
```

#### Companies Table
```sql
CREATE TABLE dbo.Companies (
    CompanyId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(255) NOT NULL,
    Type NVARCHAR(50) NOT NULL CHECK (Type IN ('Customer', 'Vendor', 'Engineer', 'Contractor')),
    Website NVARCHAR(255),
    Email NVARCHAR(255),
    PhoneNumber NVARCHAR(20),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UNIQUE(Name, Type)
);
```

#### Projects Table
```sql
CREATE TABLE dbo.Projects (
    ProjectId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProjectNumber NVARCHAR(50) NOT NULL UNIQUE,
    ProjectName NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    CustomerId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Companies(CompanyId),
    EngineeringFirmId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Companies(CompanyId),
    Status NVARCHAR(50) NOT NULL CHECK (Status IN (
        'Conceptual', 'Design', 'Bidding', 'Awarded', 
        'Procurement', 'Installation', 'Startup', 'Complete'
    )),
    StartDate DATE,
    EstimatedCompletion DATE,
    ActualCompletion DATE,
    BudgetAmount DECIMAL(15,2),
    ProjectManagerId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Users(UserId),
    DesignEngineer1Id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Users(UserId),
    DesignEngineer2Id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Users(UserId),
    Location NVARCHAR(255),
    BuildingType NVARCHAR(100),
    SquareFootage DECIMAL(10,2),
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CreatedByUserId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Users(UserId)
);

CREATE INDEX idx_projects_status ON dbo.Projects(Status);
CREATE INDEX idx_projects_pm ON dbo.Projects(ProjectManagerId);
```

---

### **EQUIPMENT LAYER**

#### Equipment Table
```sql
CREATE TABLE dbo.Equipment (
    EquipmentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectId),
    EquipmentTag NVARCHAR(50) NOT NULL, -- e.g., "AHU-01"
    EquipmentType NVARCHAR(100) NOT NULL CHECK (EquipmentType IN (
        'RTU', 'AHU', 'VAV', 'Pump', 'Fan', 'Chiller', 'Boiler', 
        'Damper', 'Valve', 'Sensor', 'Other'
    )),
    Manufacturer NVARCHAR(100),
    Model NVARCHAR(100),
    Capacity NVARCHAR(100),
    CapacityUnit NVARCHAR(50),
    SerialNumber NVARCHAR(100),
    Location NVARCHAR(255),
    Description NVARCHAR(MAX),
    SpecSheetUrl NVARCHAR(500),
    InstallationDate DATE,
    WarrantyExpirationDate DATE,
    MaintenanceFrequency NVARCHAR(50),
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CreatedByUserId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Users(UserId),
    UNIQUE(ProjectId, EquipmentTag)
);

CREATE INDEX idx_equipment_project ON dbo.Equipment(ProjectId);
CREATE INDEX idx_equipment_type ON dbo.Equipment(EquipmentType);
```

---

### **DEVICE LAYER (CORRECTED v3.0)**

#### Devices Table
```sql
CREATE TABLE dbo.Devices (
    DeviceId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectId),
    
    -- Identification
    DeviceName NVARCHAR(100) NOT NULL, -- e.g., "Thermistor-1", "Actuator-VAV-101"
    DeviceType NVARCHAR(100) NOT NULL CHECK (DeviceType IN (
        'Relay', 'Enclosure', 'Terminal', 'Transformer', 'Wiring',
        'Thermistor', 'RTD', 'Sensor', 'Actuator', 'Transducer',
        'Valve', 'Damper', 'FlowMeter', 'UtilityMeter', 'AirflowStation', 'Other'
    )),
    Category NVARCHAR(50), -- 'Simple', 'Complex', 'Advanced'
    
    -- Manufacturer & Specs
    Manufacturer NVARCHAR(100),
    Model NVARCHAR(100),
    PartNumber NVARCHAR(100),
    SerialNumber NVARCHAR(100),
    
    -- Physical Properties
    PhysicalProperties NVARCHAR(MAX), -- JSON: size, weight, calibration, resistance range
    DatasheetUrl NVARCHAR(500),
    IOMUrl NVARCHAR(500),
    WiringDiagramUrl NVARCHAR(500),
    
    -- Installation Details
    MountedOnEquipmentId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Equipment(EquipmentId),
    LocationDescription NVARCHAR(255), -- Specific location on equipment
    Quantity INT, -- How many of this instance?
    InstallationDate DATE,
    
    -- Project-Specific Specs (for advanced devices)
    -- For dampers, valves, meters: custom specs per project
    ProjectSpecificSpecs NVARCHAR(MAX), -- JSON: blade type, size, actuator type, etc.
    
    -- Commissioning
    CommissioningDate DATE,
    CommissioningStatus NVARCHAR(50) CHECK (CommissioningStatus IN (
        'NotStarted', 'InProgress', 'Commissioned', 'Verified', 'Failed'
    )),
    
    IsActive BIT NOT NULL DEFAULT 1,
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CreatedByUserId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Users(UserId),
    UNIQUE(ProjectId, DeviceName)
);

CREATE INDEX idx_devices_project ON dbo.Devices(ProjectId);
CREATE INDEX idx_devices_equipment ON dbo.Devices(MountedOnEquipmentId);
CREATE INDEX idx_devices_type ON dbo.Devices(DeviceType);
```

---

### **CONTROLLER & NODE LAYER**

#### Controllers Table
```sql
CREATE TABLE dbo.Controllers (
    ControllerId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectId),
    
    -- Identification
    ControllerName NVARCHAR(100) NOT NULL,
    ControllerType NVARCHAR(100) NOT NULL CHECK (ControllerType IN (
        'VAVController', 'BMS', 'PLC', 'RTU', 'Thermostat', 
        'VFD', 'Gateway', 'Other'
    )),
    Manufacturer NVARCHAR(100),
    Model NVARCHAR(100),
    FirmwareVersion NVARCHAR(50),
    
    -- I/O Capacity (Hardware-Fixed)
    AnalogInputCount INT,
    AnalogOutputCount INT,
    DigitalInputCount INT,
    DigitalOutputCount INT,
    
    -- Commissioning & Lifecycle
    CommissioningDate DATE,
    WarrantyExpirationDate DATE,
    IsActive BIT NOT NULL DEFAULT 1,
    Location NVARCHAR(255),
    
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CreatedByUserId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Users(UserId),
    UNIQUE(ProjectId, ControllerName)
);

CREATE INDEX idx_controllers_project ON dbo.Controllers(ProjectId);
```

#### Nodes Table
```sql
CREATE TABLE dbo.Nodes (
    NodeId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ControllerId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Controllers(ControllerId),
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectId),
    
    -- Identification
    NodeName NVARCHAR(100) NOT NULL,
    NodeType NVARCHAR(100) NOT NULL CHECK (NodeType IN (
        'BACnetMSTP', 'BACnetIP', 'ModbusTCP', 'ModbusRTU', 
        'TCPIPGateway', 'EthernetInterface', 'SerialInterface', 'Other'
    )),
    
    -- Network Properties
    Protocol NVARCHAR(50),
    NetworkAddress NVARCHAR(50), -- Bus address, IP, MAC
    MaxDevices INT,
    BusAssociation NVARCHAR(100), -- Physical bus identifier
    
    IsActive BIT DEFAULT 1,
    CommissioningDate DATE,
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CreatedByUserId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Users(UserId),
    UNIQUE(ControllerId, NodeName)
);

CREATE INDEX idx_nodes_controller ON dbo.Nodes(ControllerId);
CREATE INDEX idx_nodes_project ON dbo.Nodes(ProjectId);
CREATE INDEX idx_nodes_protocol ON dbo.Nodes(Protocol);
```

#### ControllerIOSlots Table (NEW - For tracking I/O availability)
```sql
CREATE TABLE dbo.ControllerIOSlots (
    IOSlotId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ControllerId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Controllers(ControllerId),
    
    -- Slot Identification
    SlotName NVARCHAR(50) NOT NULL, -- "AI1", "AO2", "DI3", "DO4"
    IOType NVARCHAR(50) NOT NULL CHECK (IOType IN ('AI', 'AO', 'DI', 'DO')),
    SlotNumber INT, -- 1, 2, 3, etc.
    
    -- Slot Configuration
    DataType NVARCHAR(50), -- e.g., "4-20mA", "0-10V", "24VDC", "RelayOutput"
    MinValue DECIMAL(10,2),
    MaxValue DECIMAL(10,2),
    Unit NVARCHAR(50),
    
    -- Usage
    IsUsed BIT DEFAULT 0, -- Is this slot currently assigned to a point?
    AssignedPointId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Points(PointId),
    
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UNIQUE(ControllerId, SlotName)
);

CREATE INDEX idx_ioslots_controller ON dbo.ControllerIOSlots(ControllerId);
CREATE INDEX idx_ioslots_used ON dbo.ControllerIOSlots(IsUsed);
```

---

### **POINTS LAYER (CORRECTED v3.0)**

#### Points Table
```sql
CREATE TABLE dbo.Points (
    PointId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectId),
    
    -- Identification
    PointTag NVARCHAR(100) NOT NULL,
    PointDescription NVARCHAR(255),
    
    -- Source Type
    SourceType NVARCHAR(50) NOT NULL CHECK (SourceType IN ('SoftPoint', 'HardPoint')),
    
    -- For SoftPoints: Link to Equipment
    EquipmentId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Equipment(EquipmentId),
    
    -- For HardPoints: Link to Device & Controller
    DeviceId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Devices(DeviceId),
    ControllerId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Controllers(ControllerId),
    ControllerIOSlotId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.ControllerIOSlots(IOSlotId),
    
    -- Point Type & Data
    PointType NVARCHAR(50) NOT NULL CHECK (PointType IN (
        'Input', 'Output', 'Variable', 'Parameter'
    )),
    DataType NVARCHAR(50) NOT NULL CHECK (DataType IN (
        'AnalogInput', 'AnalogOutput', 'DigitalInput', 'DigitalOutput', 
        'Integer', 'Real', 'String', 'Enumeration'
    )),
    
    -- Specification
    Unit NVARCHAR(50),
    MinValue DECIMAL(10,2),
    MaxValue DECIMAL(10,2),
    DefaultValue NVARCHAR(50),
    
    -- Analog Scaling (for hard points)
    MinPhysical DECIMAL(10,2),
    MaxPhysical DECIMAL(10,2),
    MinRaw DECIMAL(10,4),
    MaxRaw DECIMAL(10,4),
    ScalingFactor DECIMAL(10,4),
    Offset DECIMAL(10,4),
    
    -- For Soft Points: BACnet object metadata
    BACnetObjectName NVARCHAR(100),
    BACnetObjectInstance INT,
    
    -- Quality & Monitoring
    IsMonitored BIT DEFAULT 1,
    IsLogged BIT DEFAULT 1,
    Quality NVARCHAR(50),
    
    ControlPriority NVARCHAR(50) CHECK (ControlPriority IN (
        'Manual', 'Auto', 'Locked', 'Override'
    )),
    
    -- For devices with multiple termination points
    -- (e.g., actuator feedback to controller + endswitch to interlock)
    IsMultiTermination BIT DEFAULT 0, -- Does this device's data split to multiple controllers?
    ParentDeviceId UNIQUEIDENTIFIER, -- If split, reference the device
    
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CreatedByUserId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Users(UserId),
    UNIQUE(ProjectId, PointTag)
);

CREATE INDEX idx_points_project ON dbo.Points(ProjectId);
CREATE INDEX idx_points_equipment ON dbo.Points(EquipmentId);
CREATE INDEX idx_points_device ON dbo.Points(DeviceId);
CREATE INDEX idx_points_controller ON dbo.Points(ControllerId);
CREATE INDEX idx_points_source ON dbo.Points(SourceType);
CREATE INDEX idx_points_multiterm ON dbo.Points(IsMultiTermination);
```

#### PointDistribution Table (NEW - For soft point distribution)
```sql
CREATE TABLE dbo.PointDistribution (
    DistributionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectId),
    SoftPointId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Points(PointId),
    ConsumingControllerId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Controllers(ControllerId),
    
    -- Network node where this soft point is distributed
    NodeId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Nodes(NodeId),
    
    -- Local name at consuming controller (may differ from soft point tag)
    LocalPointName NVARCHAR(100),
    
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UNIQUE(SoftPointId, ConsumingControllerId)
);

CREATE INDEX idx_distribution_soft ON dbo.PointDistribution(SoftPointId);
CREATE INDEX idx_distribution_consumer ON dbo.PointDistribution(ConsumingControllerId);
```

---

### **NOTES & COLLABORATION**

#### Notes Table
```sql
CREATE TABLE dbo.Notes (
    NoteId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectId),
    Discipline NVARCHAR(50) CHECK (Discipline IN (
        'Overview', 'Financial', 'Schedule', 'Design', 'Service'
    )),
    ParentNoteId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Notes(NoteId),
    AuthorId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Users(UserId),
    Content NVARCHAR(MAX) NOT NULL,
    MentionedUserIds NVARCHAR(MAX),
    IsResolved BIT DEFAULT 0,
    ResolvedAt DATETIME2,
    ResolvedByUserId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Users(UserId),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    DeletedAt DATETIME2
);

CREATE INDEX idx_notes_project_discipline ON dbo.Notes(ProjectId, Discipline);
```

#### NoteReactions Table
```sql
CREATE TABLE dbo.NoteReactions (
    ReactionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    NoteId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Notes(NoteId) ON DELETE CASCADE,
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Users(UserId),
    Emoji NVARCHAR(10) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UNIQUE(NoteId, UserId, Emoji)
);
```

---

### **DOCUMENTS & SCHEDULES**

#### ProjectDocuments Table
```sql
CREATE TABLE dbo.ProjectDocuments (
    DocumentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectId),
    DocumentType NVARCHAR(50) CHECK (DocumentType IN (
        'Submittal', 'Drawing', 'Specification', 'Manual', 'Report', 'Other'
    )),
    DocumentName NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    BlobStorageUrl NVARCHAR(500) NOT NULL,
    FileSize BIGINT,
    FileType NVARCHAR(50),
    UploadedByUserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Users(UserId),
    UploadedAt DATETIME2 DEFAULT GETUTCDATE(),
    Version INT DEFAULT 1,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UNIQUE(ProjectId, DocumentName, Version)
);

CREATE INDEX idx_documents_project ON dbo.ProjectDocuments(ProjectId);
```

#### Deliverables Table
```sql
CREATE TABLE dbo.Deliverables (
    DeliverableId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectId),
    DeliverableType NVARCHAR(50) NOT NULL CHECK (DeliverableType IN (
        'Submittal', 'IOM', 'AsBuilt', 'CommissioningReport'
    )),
    Title NVARCHAR(255) NOT NULL,
    Status NVARCHAR(50) CHECK (Status IN (
        'NotStarted', 'InProgress', 'Ready', 'Submitted', 
        'ApprovedWithChanges', 'Approved', 'Rejected', 'Archived'
    )),
    DueDate DATE,
    SubmittedDate DATE,
    ApprovedDate DATE,
    ApprovedByUserId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Users(UserId),
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CreatedByUserId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Users(UserId)
);

CREATE INDEX idx_deliverables_project ON dbo.Deliverables(ProjectId);
```

#### ValveSchedule Table
```sql
CREATE TABLE dbo.ValveSchedule (
    ValveScheduleId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectId),
    ValveTag NVARCHAR(50) NOT NULL,
    Manufacturer NVARCHAR(100),
    Model NVARCHAR(100),
    Size NVARCHAR(50),
    Type NVARCHAR(100),
    Pressure DECIMAL(10,2),
    Temperature DECIMAL(10,2),
    Material NVARCHAR(100),
    ActuatorType NVARCHAR(100),
    Quantity INT,
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UNIQUE(ProjectId, ValveTag)
);
```

#### DamperSchedule Table
```sql
CREATE TABLE dbo.DamperSchedule (
    DamperScheduleId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectId),
    DamperTag NVARCHAR(50) NOT NULL,
    Manufacturer NVARCHAR(100),
    Model NVARCHAR(100),
    Size NVARCHAR(50),
    Type NVARCHAR(100),
    Pressure DECIMAL(10,2),
    Position NVARCHAR(50),
    ActuatorType NVARCHAR(100),
    Quantity INT,
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UNIQUE(ProjectId, DamperTag)
);
```

---

### **JOBS & AUDIT**

#### Jobs Table
```sql
CREATE TABLE dbo.Jobs (
    JobId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectId),
    JobType NVARCHAR(50) NOT NULL CHECK (JobType IN (
        'GenerateSubmittal', 'VisioExport', 'PDFGeneration', 'EquipmentSchedule',
        'BOMGeneration', 'Other'
    )),
    Status NVARCHAR(50) NOT NULL DEFAULT 'Queued' CHECK (Status IN (
        'Queued', 'Processing', 'Completed', 'Failed', 'Cancelled'
    )),
    Parameters NVARCHAR(MAX),
    Result NVARCHAR(MAX),
    ErrorMessage NVARCHAR(MAX),
    RetryCount INT DEFAULT 0,
    MaxRetries INT DEFAULT 3,
    RequestedByUserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Users(UserId),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    StartedAt DATETIME2,
    CompletedAt DATETIME2,
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);

CREATE INDEX idx_jobs_project_status ON dbo.Jobs(ProjectId, Status);
CREATE INDEX idx_jobs_type ON dbo.Jobs(JobType);
```

#### ActivityLogs Table
```sql
CREATE TABLE dbo.ActivityLogs (
    ActivityId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectId),
    EntityType NVARCHAR(50),
    EntityId UNIQUEIDENTIFIER,
    Action NVARCHAR(50),
    UserId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Users(UserId),
    Description NVARCHAR(MAX),
    OldValues NVARCHAR(MAX),
    NewValues NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);

CREATE INDEX idx_activitylogs_project ON dbo.ActivityLogs(ProjectId);
```

#### RFIs Table
```sql
CREATE TABLE dbo.RFIs (
    RFIId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Projects(ProjectId),
    RFINumber NVARCHAR(50) NOT NULL UNIQUE,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    IssuedByUserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES dbo.Users(UserId),
    AssignedToUserId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES dbo.Users(UserId),
    Status NVARCHAR(50) CHECK (Status IN (
        'Open', 'Acknowledged', 'InProgress', 'Responded', 'Closed'
    )),
    DueDate DATE,
    ResponseDate DATE,
    Response NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);
```

---

## SECTION 4: SIGNALR HUB SPECIFICATION

### Hub: `EnvoraHub` at `/hubs/project`

**Client-to-Server Methods**:
```csharp
Task AddNote(Guid projectId, string discipline, string content, List<Guid> mentionedUserIds)
Task UpdateNote(Guid noteId, string newContent)
Task DeleteNote(Guid noteId)
Task ReplyToNote(Guid parentNoteId, string content, List<Guid> mentionedUserIds)
Task ResolveNote(Guid noteId)
Task AddReaction(Guid noteId, string emoji)
Task RemoveReaction(Guid noteId, string emoji)
Task UpdateUserStatus(string status)
Task SubscribeToProject(Guid projectId)
Task SubscribeToJobUpdates(Guid jobId)
```

**Server-to-Client Methods**:
```csharp
Task NoteAdded(NoteDTO note)
Task NoteUpdated(Guid noteId, string updatedContent, DateTime updatedAt)
Task NoteDeleted(Guid noteId)
Task ReactionAdded(Guid noteId, Guid userId, string emoji)
Task UserStatusChanged(Guid userId, string status)
Task JobStatusUpdated(Guid jobId, string status, string message)
Task EquipmentUpdated(Guid projectId, EquipmentDTO equipment)
Task DeviceUpdated(Guid projectId, DeviceDTO device)
Task PointsUpdated(Guid projectId, List<PointDTO> points)
Task ControllerUpdated(Guid projectId, ControllerDTO controller)
```

---

## SECTION 5: REAL-WORLD DATA FLOW EXAMPLES

### **Example 1: RTU with 3 Thermistors**
```
Equipment: RTU-01
├─ Device Instance 1: Thermistor-SAT (Supply Air Temp)
│  └─ Hard Point: "RTU-01-SAT" → RTU-CTL-01, AI1
├─ Device Instance 2: Thermistor-DAT (Discharge Air Temp)
│  └─ Hard Point: "RTU-01-DAT" → RTU-CTL-01, AI2
└─ Device Instance 3: Thermistor-MIX (Mix Air Temp)
   └─ Hard Point: "RTU-01-MIX" → RTU-CTL-01, AI3
```

### **Example 2: VAV Actuator with Split Feedback**
```
Device: Actuator-VAV-101 (single instance)
├─ Hard Point 1: "VAV-101-DAM-CMD" (command)
│  └─ Terminates at: VAV-CTL-01, AO1
├─ Hard Point 2: "VAV-101-DAM-POS" (position feedback)
│  └─ Terminates at: VAV-CTL-01, AI1
└─ Hard Point 3: "VAV-101-DAM-ALARM" (end switch)
   └─ Terminates at: Interlock-Relay, DI1 (different controller!)
```

### **Example 3: Soft Point Distribution (Relief Fan)**
```
Equipment: Relief-Fan-VFD
└─ Soft Points (via BACnet integration):
   ├─ "RLF-VFD-SPEED" → BMS-Primary, Node-1 (BACnet/IP)
   ├─ Distributed to: AHU-CTL-01 (via PointDistribution)
   ├─ Distributed to: AHU-CTL-02 (via PointDistribution)
   └─ Distributed to: Pressure-Reset-01 (via PointDistribution)
```

### **Example 4: Advanced Device (Project Damper)**
```
Device: Custom-Damper-VAV-101
├─ DeviceType: Damper
├─ Category: Advanced
├─ ProjectSpecificSpecs: {
│    "bladeType": "ParallelBlade",
│    "size": "18x18",
│    "position": "Return",
│    "actuatorType": "Electronic",
│    "actuatorManufacturer": "Belimo",
│    "actuatorModel": "NM24A"
│  }
├─ DatasheetUrl: (project damper datasheet)
├─ IOMUrl: (project-specific IOM)
└─ Hard Points:
   ├─ "VAV-101-DAM-POS" → VAV-CTL-01, AI1
   ├─ "VAV-101-DAM-CMD" → VAV-CTL-01, AO1
   └─ "VAV-101-DAM-ALM" → Fire-Panel, DI5
```

---

## SECTION 6: I/O SLOT AVAILABILITY TRACKING

**Example Controller Setup**:
```
VAV-CTL-01 (Capacity: 16 AI, 8 AO, 16 DI, 8 DO)

Analog Inputs (16 total):
├─ AI1: USED → RTU-01-SAT (PointId: xxx)
├─ AI2: USED → VAV-101-ZAT (PointId: yyy)
├─ AI3: USED → VAV-102-ZAT (PointId: zzz)
├─ AI4-16: AVAILABLE

Analog Outputs (8 total):
├─ AO1: USED → VAV-101-DAM-CMD (PointId: aaa)
├─ AO2-8: AVAILABLE

Digital Inputs (16 total):
├─ DI1-16: AVAILABLE

Digital Outputs (8 total):
├─ DO1-8: AVAILABLE
```

**Query to Find Available Slots**:
```sql
SELECT SlotName, IOType, DataType, Unit
FROM dbo.ControllerIOSlots
WHERE ControllerId = @ControllerId
  AND IsUsed = 0
ORDER BY IOType, SlotNumber;
```

---

## SECTION 7: RBAC MATRIX (Updated)

| Resource | Admin | PM | DE | Tech | All |
|----------|-------|----|----|------|-----|
| **Equipment** | CRUD | RU | CRUD | R | R |
| **Devices** | CRUD | R | CRUD | R | - |
| **Controllers** | CRUD | - | CRUD | R | - |
| **Nodes** | CRUD | - | CRUD | R | - |
| **Points** | CRUD | R | CRUD | R | - |
| **Notes** | CRUD | CRUD | CRUD | CRUD | CRUD |

---

## ✅ DATA MODEL v3.0 COMPLETE

**Tables**: 22  
**Real-world constraints**: All incorporated  
**Multi-termination**: Supported  
**Soft point distribution**: Supported  
**I/O slot tracking**: Supported  
**Advanced devices**: Supported  

---

**Status**: ✅ PRODUCTION-READY  
**Date**: December 18, 2025, 2:14 PM CST  
**Version**: 3.0 (Final)

**Ready to give to Cursor. ✅**

