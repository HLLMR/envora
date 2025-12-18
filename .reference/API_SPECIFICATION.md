# Envora Platform - REST API Specification

**Version:** 1.0  
**Date:** December 2025  
**Status:** Active Development  
**Audience:** Backend Engineers, Frontend Engineers, QA  

---

## 1. Overview

This document defines all REST API endpoints for the Envora Platform v1.0 (Phase 1–5). The API follows RESTful principles with JSON request/response bodies.

**Base URL**: `https://api.envora.com/api/v1` (production) or `http://localhost:5000/api/v1` (local)

**Authentication**: Bearer token (JWT from Azure AD or local auth)

**Rate Limiting**: 100 requests/minute per user (429 Too Many Requests)

**Content-Type**: `application/json`

---

## 2. Authentication & Authorization

### 2.1 Login Endpoint

```
POST /auth/login
Content-Type: application/json

Request Body:
{
  "username": "jill@enviromatic.com",
  "password": "SecurePassword123"
}

Response (200 OK):
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "userId": 1,
    "username": "jill@enviromatic.com",
    "firstName": "Jill",
    "lastName": "Martinez",
    "email": "jill@enviromatic.com",
    "role": "ApplicationsEngineer",
    "department": "Engineering"
  },
  "expiresIn": 3600
}

Errors:
401 Unauthorized: Invalid credentials
400 Bad Request: Missing username or password
```

### 2.2 Azure AD Integration (Alternative)

```
POST /auth/azure-login
Content-Type: application/json

Request Body:
{
  "idToken": "[Azure AD ID token from MSAL]"
}

Response (200 OK):
{
  "accessToken": "...",
  "user": { ... }
}

Errors:
401 Unauthorized: Invalid or expired token
400 Bad Request: Missing idToken
```

### 2.3 Token Refresh

```
POST /auth/refresh
Authorization: Bearer [refreshToken]

Response (200 OK):
{
  "accessToken": "...",
  "expiresIn": 3600
}

Errors:
401 Unauthorized: Invalid or expired refresh token
```

### 2.4 Logout

```
POST /auth/logout
Authorization: Bearer [accessToken]

Response (200 OK):
{ "message": "Logged out successfully" }
```

---

## 3. Projects Endpoints

### 3.1 List All Projects

```
GET /projects?skip=0&take=20&status=InProgress&customerId=5
Authorization: Bearer [accessToken]

Query Parameters:
- skip: int (default 0) – pagination offset
- take: int (default 20, max 100) – page size
- status: string (optional) – filter by status (Draft, InProgress, Design Review, Approved, Delivered, Archived)
- customerId: int (optional) – filter by customer company
- searchTerm: string (optional) – search by project number or job name
- sortBy: string (optional) – field to sort by (createdDate, projectNumber, jobName, status)
- sortOrder: string (optional) – asc or desc

Response (200 OK):
{
  "data": [
    {
      "projectId": 1,
      "projectNumber": "FW25-101",
      "jobName": "Downtown Medical Center HVAC",
      "customerId": 5,
      "customerName": "Downtown Medical Center",
      "status": "InProgress",
      "contractAmount": 150000.00,
      "startDate": "2025-12-01T00:00:00Z",
      "targetCompletionDate": "2026-02-28T00:00:00Z",
      "drawingsPercentage": 75,
      "programmingPercentage": 40,
      "installPercentage": 0,
      "graphicsPercentage": 60,
      "applicationEngineer": "Jill Martinez",
      "projectManager": "Marcus Thompson",
      "createdDate": "2025-12-01T10:30:00Z"
    },
    ...
  ],
  "totalCount": 48,
  "skip": 0,
  "take": 20
}

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
```

### 3.2 Get Project by ID

```
GET /projects/{projectId}
Authorization: Bearer [accessToken]

Path Parameters:
- projectId: int – primary key

Response (200 OK):
{
  "projectId": 1,
  "projectNumber": "FW25-101",
  "branchId": 1,
  "customerId": 5,
  "customerName": "Downtown Medical Center",
  "jobName": "Downtown Medical Center HVAC Controls",
  "jobsiteAddressId": 10,
  "jobsiteAddress": {
    "addressId": 10,
    "street": "123 Main St",
    "city": "Dallas",
    "state": "TX",
    "zipCode": "75201"
  },
  "primaryContactId": 7,
  "primaryContact": {
    "contactId": 7,
    "firstName": "John",
    "lastName": "Smith",
    "title": "Director of Operations",
    "email": "john.smith@downtown.com"
  },
  "salesPersonId": 2,
  "applicationEngineer": { "userId": 1, "firstName": "Jill", "lastName": "Martinez" },
  "projectManager": { "userId": 3, "firstName": "Marcus", "lastName": "Thompson" },
  "programmer": { "userId": 4, "firstName": "David", "lastName": "Chen" },
  "graphicsPerson": { "userId": 5, "firstName": "Jennifer", "lastName": "Kim" },
  "mepEngineer": { "companyId": 20, "companyName": "Smith Engineering" },
  "installer": { "companyId": 15, "companyName": "Acme HVAC" },
  "contractAmount": 150000.00,
  "contractAmountTaxable": false,
  "installAmount": 50000.00,
  "installAmountTaxable": false,
  "materialAmount": 65000.00,
  "materialAmountTaxable": false,
  "laborAmount": 35000.00,
  "laborAmountTaxable": false,
  "totalChangeOrderAmount": 5000.00,
  "finalContractAmount": 155000.00,
  "retainagePercentage": 10.0,
  "taxRate": 8.25,
  "taxAmount": 12487.50,
  "bondRequired": false,
  "warrantyPercentage": 5.0,
  "overheadPercentage": 15.0,
  "freightRate": 2.5,
  "drawingsPercentage": 75,
  "drawingsReviewed": 8,
  "installPercentage": 0,
  "programmingPercentage": 40,
  "graphicsPercentage": 60,
  "engineeringHours": 120.5,
  "managementHours": 40.0,
  "graphicsHours": 60.0,
  "programmingHours": 80.0,
  "commissioningHours": null,
  "trainingHours": null,
  "installationHours": null,
  "startDate": "2025-12-01T00:00:00Z",
  "completionDate": "2026-02-28T00:00:00Z",
  "bookedDate": "2025-11-15T00:00:00Z",
  "warrantyStartDate": "2026-03-01T00:00:00Z",
  "warrantyEndDate": "2027-03-01T00:00:00Z",
  "contractDate": "2025-12-01T00:00:00Z",
  "noticeToProceedDate": "2025-12-05T00:00:00Z",
  "status": "InProgress",
  "installSubmitted": null,
  "asbuiltsDone": false,
  "createdDate": "2025-12-01T10:30:00Z",
  "modifiedDate": "2025-12-15T14:22:00Z",
  "createdBy": "system",
  "modifiedBy": "jill@enviromatic.com"
}

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access to this project
404 Not Found: Project doesn't exist
```

### 3.3 Create Project

```
POST /projects
Authorization: Bearer [accessToken]
Content-Type: application/json

Request Body:
{
  "projectNumber": "FW25-102",
  "jobName": "Tech Campus Retrofit",
  "customerId": 6,
  "jobsiteAddressId": 11,
  "primaryContactId": 8,
  "contractAmount": 200000.00,
  "contractAmountTaxable": false,
  "startDate": "2026-01-15T00:00:00Z",
  "targetCompletionDate": "2026-04-30T00:00:00Z",
  "contractDate": "2025-12-10T00:00:00Z",
  "applicationEngineerId": 2,
  "projectManagerId": 3,
  "programmerId": 4,
  "graphicsPersonId": 5,
  "mepEngineerId": 20,
  "installerId": 15,
  "salesPersonId": 2
}

Response (201 Created):
{
  "projectId": 2,
  "projectNumber": "FW25-102",
  "jobName": "Tech Campus Retrofit",
  ... (full project object)
}

Location Header: /api/v1/projects/2

Errors:
400 Bad Request: Missing required fields or invalid data
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have create permission
409 Conflict: Project number already exists
```

### 3.4 Update Project

```
PATCH /projects/{projectId}
Authorization: Bearer [accessToken]
Content-Type: application/json

Path Parameters:
- projectId: int

Request Body (partial update):
{
  "drawingsPercentage": 80,
  "programmingPercentage": 45,
  "status": "Design Review"
}

Response (200 OK):
{
  "projectId": 1,
  ... (updated full project object)
}

Errors:
400 Bad Request: Invalid field or data type
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have update permission
404 Not Found: Project doesn't exist
```

### 3.5 Delete Project

```
DELETE /projects/{projectId}
Authorization: Bearer [accessToken]

Path Parameters:
- projectId: int

Response (204 No Content): (empty body)

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have delete permission (admin only)
404 Not Found: Project doesn't exist
409 Conflict: Project has associated equipment/documents and can't be deleted
```

---

## 4. Equipment Endpoints

### 4.1 List Equipment by Project

```
GET /projects/{projectId}/equipment
Authorization: Bearer [accessToken]

Query Parameters:
- equipmentType: string (optional) – filter by type (RTU, AHU, VAV, Pump, etc.)
- searchTerm: string (optional) – search by tag or location

Response (200 OK):
{
  "data": [
    {
      "equipmentId": 1,
      "projectId": 1,
      "equipmentTag": "RTU-1",
      "equipmentType": "RTU",
      "condition": "New",
      "manufacturer": "Carrier",
      "modelNumber": "50PCHT60X",
      "building": "Main",
      "level": "L0",
      "areaZone": "Zone A",
      "location": "Roof",
      "areaServed": "1st Floor",
      "comments": "Main supply unit for zone A",
      "controllerTag": "CTRL-1",
      "drawingNumber": "M-5.1",
      "onEquipmentSubmittal": true,
      "onMechPlans": true,
      "onFloorLayout": false,
      "onCommLayout": false,
      "pointCount": 6,
      "specs": {
        "CFM": 5000,
        "Tons": 10,
        "PSIG": 120,
        "EnergyCode": "90.1-2019"
      },
      "createdDate": "2025-12-01T11:00:00Z",
      "modifiedDate": "2025-12-10T14:30:00Z"
    },
    ...
  ],
  "totalCount": 8
}

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Project doesn't exist
```

### 4.2 Get Equipment by ID

```
GET /projects/{projectId}/equipment/{equipmentId}
Authorization: Bearer [accessToken]

Response (200 OK):
{
  "equipmentId": 1,
  "projectId": 1,
  "equipmentTag": "RTU-1",
  ... (full equipment object)
  "points": [
    {
      "pointId": 1,
      "pointTag": "RT",
      "dataType": "Temperature",
      "unit": "°F",
      "description": "Return Air Temperature"
    },
    ...
  ]
}

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Equipment or project doesn't exist
```

### 4.3 Create Equipment

```
POST /projects/{projectId}/equipment
Authorization: Bearer [accessToken]
Content-Type: application/json

Request Body:
{
  "equipmentTag": "RTU-1",
  "equipmentType": "RTU",
  "condition": "New",
  "manufacturer": "Carrier",
  "modelNumber": "50PCHT60X",
  "building": "Main",
  "level": "L0",
  "areaZone": "Zone A",
  "location": "Roof",
  "areaServed": "1st Floor",
  "comments": "Main supply unit",
  "controllerTag": "CTRL-1",
  "specs": {
    "CFM": 5000,
    "Tons": 10,
    "PSIG": 120
  }
}

Response (201 Created):
{
  "equipmentId": 1,
  "projectId": 1,
  "equipmentTag": "RTU-1",
  ... (full equipment object),
  "points": [
    // Auto-created from RTU template
    { "pointId": 1, "pointTag": "RT", "dataType": "Temperature" },
    { "pointId": 2, "pointTag": "SAT", "dataType": "Temperature" },
    ...
  ]
}

Location Header: /api/v1/projects/1/equipment/1

Errors:
400 Bad Request: Missing required fields or invalid type
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have create permission
404 Not Found: Project doesn't exist
409 Conflict: Equipment tag already exists in this project
```

### 4.4 Update Equipment

```
PATCH /projects/{projectId}/equipment/{equipmentId}
Authorization: Bearer [accessToken]
Content-Type: application/json

Request Body (partial):
{
  "location": "Roof - New Position",
  "specs": {
    "CFM": 5200,
    "Tons": 10
  }
}

Response (200 OK):
{
  "equipmentId": 1,
  ... (updated equipment object)
}

Errors:
400 Bad Request: Invalid field or data
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have update permission
404 Not Found: Equipment or project doesn't exist
409 Conflict: Equipment tag conflicts with another
```

### 4.5 Delete Equipment

```
DELETE /projects/{projectId}/equipment/{equipmentId}
Authorization: Bearer [accessToken]

Response (204 No Content)

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have delete permission
404 Not Found: Equipment doesn't exist
409 Conflict: Equipment has associated points/valves/dampers
```

### 4.6 Bulk Import Equipment

```
POST /projects/{projectId}/equipment/import
Authorization: Bearer [accessToken]
Content-Type: application/json

Request Body:
{
  "equipmentList": [
    {
      "equipmentTag": "RTU-1",
      "equipmentType": "RTU",
      "manufacturer": "Carrier",
      "modelNumber": "50PCHT60X",
      "location": "Roof",
      "specs": { "CFM": 5000, "Tons": 10 }
    },
    {
      "equipmentTag": "VAV-1",
      "equipmentType": "VAV",
      "manufacturer": "Honeywell",
      "modelNumber": "W7100",
      "location": "Room 101",
      "specs": { "CFM": 1500 }
    }
  ]
}

Response (201 Created):
{
  "successful": 2,
  "failed": 0,
  "results": [
    { "equipmentTag": "RTU-1", "equipmentId": 1, "status": "Created", "pointsCreated": 6 },
    { "equipmentTag": "VAV-1", "equipmentId": 2, "status": "Created", "pointsCreated": 4 }
  ]
}

Errors:
400 Bad Request: Invalid format or data
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have create permission
404 Not Found: Project doesn't exist
```

---

## 5. Points Endpoints

### 5.1 List Points by Equipment

```
GET /projects/{projectId}/equipment/{equipmentId}/points
Authorization: Bearer [accessToken]

Query Parameters:
- signalType: string (optional) – Analog, Binary, Network
- dataType: string (optional) – Temperature, Pressure, etc.

Response (200 OK):
{
  "data": [
    {
      "pointId": 1,
      "equipmentId": 1,
      "pointTag": "RT",
      "pointType": "Primary",
      "signalType": "Analog",
      "dataType": "Temperature",
      "inputOutput": "Input",
      "description": "Return Air Temperature",
      "unit": "°F",
      "minValue": 50,
      "maxValue": 95,
      "defaultValue": 72,
      "controllerTag": "CTRL-1",
      "terminalBlockNumber": "TB-1",
      "terminalNumber": 1,
      "linkedToWiringDiagram": true,
      "diagramPageNumber": 5
    },
    ...
  ],
  "totalCount": 6
}

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Equipment or project doesn't exist
```

### 5.2 Get Point by ID

```
GET /projects/{projectId}/equipment/{equipmentId}/points/{pointId}
Authorization: Bearer [accessToken]

Response (200 OK):
{
  "pointId": 1,
  ... (full point object)
}

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Point, equipment, or project doesn't exist
```

### 5.3 Create Point

```
POST /projects/{projectId}/equipment/{equipmentId}/points
Authorization: Bearer [accessToken]
Content-Type: application/json

Request Body:
{
  "pointTag": "RT",
  "pointType": "Primary",
  "signalType": "Analog",
  "dataType": "Temperature",
  "inputOutput": "Input",
  "description": "Return Air Temperature",
  "unit": "°F",
  "minValue": 50,
  "maxValue": 95,
  "defaultValue": 72,
  "controllerTag": "CTRL-1",
  "terminalBlockNumber": "TB-1",
  "terminalNumber": 1
}

Response (201 Created):
{
  "pointId": 1,
  ... (full point object)
}

Location Header: /api/v1/projects/1/equipment/1/points/1

Errors:
400 Bad Request: Missing required fields or invalid data
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have create permission
404 Not Found: Equipment or project doesn't exist
409 Conflict: Point tag already exists on this equipment
```

### 5.4 Update Point

```
PATCH /projects/{projectId}/equipment/{equipmentId}/points/{pointId}
Authorization: Bearer [accessToken]
Content-Type: application/json

Request Body (partial):
{
  "minValue": 48,
  "maxValue": 98,
  "terminalNumber": 2
}

Response (200 OK):
{
  "pointId": 1,
  ... (updated point object)
}

Errors:
400 Bad Request: Invalid field
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have update permission
404 Not Found: Point doesn't exist
```

### 5.5 Delete Point

```
DELETE /projects/{projectId}/equipment/{equipmentId}/points/{pointId}
Authorization: Bearer [accessToken]

Response (204 No Content)

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have delete permission
404 Not Found: Point doesn't exist
```

---

## 6. Schedules Endpoints (Auto-Calculated)

### 6.1 Get Equipment Schedule (PDF Export)

```
GET /projects/{projectId}/schedules/equipment?format=pdf
Authorization: Bearer [accessToken]

Query Parameters:
- format: string (pdf, excel) – output format

Response (200 OK):
Binary PDF file (Content-Type: application/pdf)
Content-Disposition: attachment; filename="FW25-101_EquipmentSchedule.pdf"

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Project doesn't exist or has no equipment
```

### 6.2 Get BOM (Bill of Materials)

```
GET /projects/{projectId}/schedules/bom?format=excel
Authorization: Bearer [accessToken]

Query Parameters:
- format: string (pdf, excel) – output format

Response (200 OK):
{
  "data": [
    {
      "itemNumber": 1,
      "description": "RTU - Carrier 50PCHT60X",
      "quantity": 1,
      "unitCost": 8500.00,
      "totalCost": 8500.00,
      "manufacturer": "Carrier",
      "leadTime": "6 weeks"
    },
    {
      "itemNumber": 2,
      "description": "VAV - Honeywell W7100",
      "quantity": 3,
      "unitCost": 2200.00,
      "totalCost": 6600.00,
      "manufacturer": "Honeywell",
      "leadTime": "4 weeks"
    },
    ...
  ],
  "subtotal": 47500.00,
  "tax": 3918.75,
  "total": 51418.75
}

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Project doesn't exist or has no equipment
```

### 6.3 Get Valve Schedule

```
GET /projects/{projectId}/schedules/valves?format=pdf
Authorization: Bearer [accessToken]

Response (200 OK):
Binary PDF or JSON with valve data

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Project doesn't exist or has no valves
```

### 6.4 Get Damper Schedule

```
GET /projects/{projectId}/schedules/dampers?format=pdf
Authorization: Bearer [accessToken]

Response (200 OK):
Binary PDF or JSON with damper data

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Project doesn't exist or has no dampers
```

---

## 7. Submittal Generation Endpoints

### 7.1 Generate Submittal

```
POST /projects/{projectId}/submittal/generate
Authorization: Bearer [accessToken]
Content-Type: application/json

Request Body:
{
  "revision": "Rev A",
  "includeSections": [
    "CoverPage",
    "EquipmentSchedule",
    "PointList",
    "BillOfMaterials",
    "ValveSchedule",
    "DamperSchedule",
    "ControlDiagrams",
    "FloorPlan",
    "SequencesOfOperation"
  ],
  "notes": "Customer submission"
}

Response (202 Accepted):
{
  "deliverableId": 15,
  "projectId": 1,
  "deliverableType": "Design Package",
  "status": "Processing",
  "revision": "Rev A",
  "estimatedTimeSeconds": 45,
  "message": "Submittal generation started. You'll receive a notification when complete."
}

Location Header: /api/v1/projects/1/submittal/15

Errors:
400 Bad Request: Missing sections or invalid data
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have create permission
404 Not Found: Project doesn't exist
409 Conflict: Another submittal is already generating
```

### 7.2 Get Submittal Status (WebSocket or Polling)

```
GET /projects/{projectId}/submittal/{deliverableId}
Authorization: Bearer [accessToken]

Response (200 OK):
{
  "deliverableId": 15,
  "projectId": 1,
  "status": "Completed",
  "revision": "Rev A",
  "generatedPdfUrl": "https://envora.blob.core.windows.net/deliverables/FW25-101_Rev-A.pdf",
  "pageCount": 42,
  "fileSizeMB": 8.5,
  "completedAt": "2025-12-18T15:32:10Z",
  "contents": [
    { "section": "CoverPage", "status": "Completed", "pages": "1-1" },
    { "section": "EquipmentSchedule", "status": "Completed", "pages": "2-3" },
    { "section": "PointList", "status": "Completed", "pages": "4-12" },
    ...
  ]
}

If status is "Processing":
{
  "deliverableId": 15,
  "status": "Processing",
  "progress": 60,
  "currentTask": "Assembling RTU control panel diagram",
  "remainingTimeSeconds": 25
}

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Deliverable doesn't exist
```

### 7.3 Download Submittal PDF

```
GET /projects/{projectId}/submittal/{deliverableId}/download
Authorization: Bearer [accessToken]

Response (200 OK):
Binary PDF file
Content-Type: application/pdf
Content-Disposition: attachment; filename="FW25-101_DesignSubmittal_RevA.pdf"

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Deliverable doesn't exist or PDF not ready
```

### 7.4 List Submittal History

```
GET /projects/{projectId}/submittal/history
Authorization: Bearer [accessToken]

Query Parameters:
- skip: int (default 0)
- take: int (default 20)

Response (200 OK):
{
  "data": [
    {
      "deliverableId": 15,
      "deliverableType": "Design Package",
      "revision": "Rev A",
      "status": "Completed",
      "generatedAt": "2025-12-18T15:32:10Z",
      "generatedBy": "jill@enviromatic.com",
      "pageCount": 42,
      "pdfUrl": "https://..."
    },
    {
      "deliverableId": 14,
      "deliverableType": "Design Package",
      "revision": "-",
      "status": "Completed",
      "generatedAt": "2025-12-17T10:15:00Z",
      "generatedBy": "jill@enviromatic.com",
      "pageCount": 38,
      "pdfUrl": "https://..."
    }
  ],
  "totalCount": 5
}

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Project doesn't exist
```

---

## 8. Documents Endpoints

### 8.1 Upload Document

```
POST /projects/{projectId}/documents
Authorization: Bearer [accessToken]
Content-Type: multipart/form-data

Form Data:
- file: binary (Visio, PDF, Excel, Word, etc.)
- documentType: string (Visio, PDF, Excel, Word, Image, Other)
- documentCategory: string (Specification, Drawing, Submittal, RFI, Sequence, Other)
- description: string (optional)

Response (201 Created):
{
  "documentId": 42,
  "projectId": 1,
  "fileName": "Design_Spec_v2.pdf",
  "fileSize": 2048576,
  "contentType": "application/pdf",
  "documentType": "PDF",
  "documentCategory": "Specification",
  "blobUrl": "https://envora.blob.core.windows.net/documents/FW25-101/...",
  "uploadedBy": "jill@enviromatic.com",
  "uploadedAt": "2025-12-18T14:20:30Z"
}

Location Header: /api/v1/projects/1/documents/42

Errors:
400 Bad Request: Missing file or invalid data
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have upload permission
404 Not Found: Project doesn't exist
413 Payload Too Large: File exceeds 100MB limit
```

### 8.2 List Documents by Project

```
GET /projects/{projectId}/documents?category=Specification&type=PDF
Authorization: Bearer [accessToken]

Query Parameters:
- category: string (optional) – Specification, Drawing, Submittal, etc.
- type: string (optional) – Visio, PDF, Excel, etc.
- searchTerm: string (optional) – search by file name

Response (200 OK):
{
  "data": [
    {
      "documentId": 42,
      "fileName": "Design_Spec_v2.pdf",
      "fileSize": 2048576,
      "documentType": "PDF",
      "documentCategory": "Specification",
      "blobUrl": "https://...",
      "uploadedBy": "jill@enviromatic.com",
      "uploadedAt": "2025-12-18T14:20:30Z",
      "versions": 1
    }
  ],
  "totalCount": 5
}

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Project doesn't exist
```

### 8.3 Download Document

```
GET /projects/{projectId}/documents/{documentId}/download
Authorization: Bearer [accessToken]

Response (200 OK):
Binary file (original file type)
Content-Disposition: attachment; filename="Design_Spec_v2.pdf"

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Document doesn't exist
```

### 8.4 Delete Document

```
DELETE /projects/{projectId}/documents/{documentId}
Authorization: Bearer [accessToken]

Response (204 No Content)

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have delete permission
404 Not Found: Document doesn't exist
```

---

## 9. Notes Endpoints (Persistent Comments)

### 9.1 List Notes by Project

```
GET /projects/{projectId}/notes?discipline=Design&disciplineTab=Equipment
Authorization: Bearer [accessToken]

Query Parameters:
- discipline: string (optional) – Overview, Financial, Schedule, Design, Service
- disciplineTab: string (optional) – specific tab name
- skip: int (default 0)
- take: int (default 50)

Response (200 OK):
{
  "data": [
    {
      "noteId": 101,
      "projectId": 1,
      "contentContext": "DESIGN:Equipment",
      "content": "RTU-1 specs finalized. Ready for point definition.",
      "author": "jill@enviromatic.com",
      "authorName": "Jill Martinez",
      "createdAt": "2025-12-18T10:30:00Z",
      "editedAt": null,
      "parentNoteId": null,
      "replies": 1,
      "reactions": {
        "👍": 2,
        "❤️": 1
      }
    },
    {
      "noteId": 102,
      "projectId": 1,
      "contentContext": "DESIGN:Equipment",
      "content": "@Jill Can you confirm CFM spec for VAV-1?",
      "author": "david@enviromatic.com",
      "authorName": "David Chen",
      "createdAt": "2025-12-18T11:00:00Z",
      "editedAt": null,
      "parentNoteId": 101,
      "replies": 0,
      "reactions": {}
    }
  ],
  "totalCount": 25
}

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Project doesn't exist
```

### 9.2 Create Note

```
POST /projects/{projectId}/notes
Authorization: Bearer [accessToken]
Content-Type: application/json

Request Body:
{
  "content": "RTU-1 specs finalized. Ready for point definition.",
  "contentContext": "DESIGN:Equipment",
  "parentNoteId": null,
  "mentions": ["david@enviromatic.com"]
}

Response (201 Created):
{
  "noteId": 101,
  "projectId": 1,
  "content": "RTU-1 specs finalized. Ready for point definition.",
  "contentContext": "DESIGN:Equipment",
  "author": "jill@enviromatic.com",
  "authorName": "Jill Martinez",
  "createdAt": "2025-12-18T10:30:00Z",
  "parentNoteId": null,
  "replies": 0,
  "reactions": {}
}

Location Header: /api/v1/projects/1/notes/101

Errors:
400 Bad Request: Missing content or invalid context
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have create permission
404 Not Found: Project doesn't exist
429 Too Many Requests: Rate limited (max 10 notes/minute)
```

### 9.3 Update Note

```
PATCH /projects/{projectId}/notes/{noteId}
Authorization: Bearer [accessToken]
Content-Type: application/json

Request Body:
{
  "content": "RTU-1 specs finalized and approved. Ready for point definition."
}

Response (200 OK):
{
  "noteId": 101,
  "content": "RTU-1 specs finalized and approved. Ready for point definition.",
  "editedAt": "2025-12-18T11:15:00Z",
  ...
}

Errors:
400 Bad Request: Invalid content
401 Unauthorized: Invalid or missing token
403 Forbidden: User can only edit own notes
404 Not Found: Note doesn't exist
```

### 9.4 Delete Note

```
DELETE /projects/{projectId}/notes/{noteId}
Authorization: Bearer [accessToken]

Response (204 No Content)

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User can only delete own notes or admin
404 Not Found: Note doesn't exist
```

### 9.5 Add Reaction to Note

```
POST /projects/{projectId}/notes/{noteId}/reactions
Authorization: Bearer [accessToken]
Content-Type: application/json

Request Body:
{
  "emoji": "👍"
}

Response (201 Created):
{
  "noteId": 101,
  "reactions": {
    "👍": 2,
    "❤️": 1
  }
}

Errors:
400 Bad Request: Invalid emoji
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have permission
404 Not Found: Note doesn't exist
```

---

## 10. Desktop Bridge Coordination Endpoints

### 10.1 Bridge Polling for Visio Jobs

```
GET /bridge/jobs/pending?bridgeId=bridge-001
Authorization: Bearer [bridgeServiceToken]

Response (200 OK):
{
  "jobs": [
    {
      "jobId": 501,
      "jobType": "VisioExport",
      "projectId": 1,
      "projectNumber": "FW25-101",
      "diagramType": "RTU",
      "templateUrl": "https://envora.blob.core.windows.net/templates/RTU_Template.vsdx",
      "equipmentData": [
        {
          "equipmentId": 1,
          "equipmentTag": "RTU-1",
          "manufacturer": "Carrier",
          "modelNumber": "50PCHT60X",
          "specs": { "CFM": 5000, "Tons": 10 }
        }
      ],
      "outputFileName": "FW25-101_RTU_Diagram.pdf",
      "deliverableId": 15,
      "createdAt": "2025-12-18T15:00:00Z"
    }
  ]
}

Errors:
401 Unauthorized: Invalid bridge token
503 Service Unavailable: Cloud service temporarily down
```

### 10.2 Report Job Completion

```
POST /bridge/jobs/{jobId}/complete
Authorization: Bearer [bridgeServiceToken]
Content-Type: multipart/form-data

Form Data:
- pdfFile: binary (generated PDF)
- metadata: JSON {
    "pageCount": 3,
    "generationTimeSeconds": 12,
    "bridgeVersion": "1.0.0"
  }

Response (200 OK):
{
  "jobId": 501,
  "status": "Completed",
  "pdfUrl": "https://envora.blob.core.windows.net/deliverables/FW25-101_RTU_Diagram.pdf",
  "message": "PDF received and processed successfully"
}

Errors:
400 Bad Request: Missing PDF or metadata
401 Unauthorized: Invalid bridge token
404 Not Found: Job doesn't exist
409 Conflict: Job already completed
```

### 10.3 Report Job Failure

```
POST /bridge/jobs/{jobId}/fail
Authorization: Bearer [bridgeServiceToken]
Content-Type: application/json

Request Body:
{
  "errorMessage": "Visio COM error: Object doesn't exist",
  "errorCode": "COM_ERROR_500",
  "stackTrace": "..."
}

Response (200 OK):
{
  "jobId": 501,
  "status": "Failed",
  "retryable": true,
  "message": "Job failure recorded. Will retry in 5 minutes."
}

Errors:
401 Unauthorized: Invalid bridge token
404 Not Found: Job doesn't exist
```

---

## 11. Team & User Endpoints

### 11.1 List Project Team

```
GET /projects/{projectId}/team
Authorization: Bearer [accessToken]

Response (200 OK):
{
  "data": [
    {
      "userId": 1,
      "firstName": "Jill",
      "lastName": "Martinez",
      "email": "jill@enviromatic.com",
      "role": "ApplicationsEngineer",
      "department": "Engineering",
      "status": "Online",
      "lastSeenAt": "2025-12-18T15:30:00Z"
    },
    {
      "userId": 3,
      "firstName": "Marcus",
      "lastName": "Thompson",
      "email": "marcus@enviromatic.com",
      "role": "ProjectManager",
      "department": "Operations",
      "status": "Away",
      "lastSeenAt": "2025-12-18T12:00:00Z"
    }
  ]
}

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Project doesn't exist
```

### 11.2 Update Project Team Assignment

```
PATCH /projects/{projectId}/team
Authorization: Bearer [accessToken]
Content-Type: application/json

Request Body:
{
  "applicationEngineerId": 1,
  "projectManagerId": 3,
  "programmerId": 4,
  "graphicsPersonId": 5,
  "mepEngineerId": 20,
  "installerId": 15
}

Response (200 OK):
{
  "projectId": 1,
  "applicationEngineer": { "userId": 1, "firstName": "Jill", "lastName": "Martinez" },
  "projectManager": { "userId": 3, "firstName": "Marcus", "lastName": "Thompson" },
  ...
}

Errors:
400 Bad Request: Invalid user ID or role
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have update permission (admin/PM only)
404 Not Found: Project or user doesn't exist
```

---

## 12. Audit Log Endpoints

### 12.1 Get Project Audit Trail

```
GET /projects/{projectId}/audit?skip=0&take=50&entity=Equipment
Authorization: Bearer [accessToken]

Query Parameters:
- skip: int (default 0)
- take: int (default 50)
- entity: string (optional) – Project, Equipment, Point, Document, Note
- action: string (optional) – Created, Updated, Deleted
- userId: int (optional) – filter by user

Response (200 OK):
{
  "data": [
    {
      "auditLogId": 1001,
      "projectId": 1,
      "entity": "Equipment",
      "entityId": 1,
      "action": "Created",
      "userId": 1,
      "userName": "Jill Martinez",
      "changes": {
        "equipmentTag": { "oldValue": null, "newValue": "RTU-1" },
        "manufacturer": { "oldValue": null, "newValue": "Carrier" }
      },
      "timestamp": "2025-12-01T11:00:00Z"
    },
    {
      "auditLogId": 1002,
      "projectId": 1,
      "entity": "Equipment",
      "entityId": 1,
      "action": "Updated",
      "userId": 1,
      "userName": "Jill Martinez",
      "changes": {
        "location": { "oldValue": "Roof", "newValue": "Roof - North Side" }
      },
      "timestamp": "2025-12-10T14:30:00Z"
    }
  ],
  "totalCount": 127
}

Errors:
401 Unauthorized: Invalid or missing token
403 Forbidden: User doesn't have read access
404 Not Found: Project doesn't exist
```

---

## 13. Error Response Format

All errors follow this standard format:

```json
{
  "error": {
    "code": "RESOURCE_NOT_FOUND",
    "message": "The requested project does not exist.",
    "statusCode": 404,
    "timestamp": "2025-12-18T15:30:00Z",
    "traceId": "0HMVB7MIVQTFE:00000001",
    "details": [
      {
        "field": "projectId",
        "message": "Project with ID 999 not found"
      }
    ]
  }
}
```

**Common Error Codes**:
- `INVALID_REQUEST` (400)
- `UNAUTHORIZED` (401)
- `FORBIDDEN` (403)
- `RESOURCE_NOT_FOUND` (404)
- `CONFLICT` (409)
- `RATE_LIMITED` (429)
- `INTERNAL_ERROR` (500)

---

## 14. Rate Limiting & Throttling

**Limits** (per user, per minute):
- GET requests: 100/min
- POST/PATCH/DELETE requests: 50/min
- File uploads: 10/min
- Large exports (PDF, Excel): 5/min

**Response Headers**:
```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 94
X-RateLimit-Reset: 1734023100
```

**429 Response**:
```json
{
  "error": {
    "code": "RATE_LIMITED",
    "message": "Too many requests. Please wait 45 seconds before retrying.",
    "statusCode": 429,
    "retryAfter": 45
  }
}
```

---

## 15. API Versioning Strategy

**Current**: v1 (`/api/v1/...`)

**Future**: v2 would be at `/api/v2/...` (backward compatibility maintained for 2 versions)

**Deprecation**: Endpoints marked as deprecated will include header:
```
Deprecation: true
Sunset: 2026-12-31T00:00:00Z
Link: <https://docs.envora.com/migration-v1-to-v2>; rel="deprecation"
```

---

## 16. Document Version History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | Dec 18, 2025 | Architecture Team | Initial API Specification |

