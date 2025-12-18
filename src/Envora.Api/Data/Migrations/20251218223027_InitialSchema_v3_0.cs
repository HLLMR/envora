using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Envora.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema_v3_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "dbo",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                    table.CheckConstraint("CK_Companies_Type", "[Type] IN ('Customer','Vendor','Engineer','Contractor')");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AzureAdId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.CheckConstraint("CK_Users_Role", "[Role] IN ('Admin','ProjectManager','Estimator','DesignEngineer','Technician','ServiceCoordinator','ContractorSuper','Client')");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                schema: "dbo",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EngineeringFirmId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EstimatedCompletion = table.Column<DateOnly>(type: "date", nullable: true),
                    ActualCompletion = table.Column<DateOnly>(type: "date", nullable: true),
                    BudgetAmount = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    ProjectManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DesignEngineer1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DesignEngineer2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BuildingType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SquareFootage = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                    table.CheckConstraint("CK_Projects_Status", "[Status] IN ('Conceptual','Design','Bidding','Awarded','Procurement','Installation','Startup','Complete')");
                    table.ForeignKey(
                        name: "FK_Projects_Companies_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_Projects_Companies_EngineeringFirmId",
                        column: x => x.EngineeringFirmId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_Projects_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Projects_Users_DesignEngineer1Id",
                        column: x => x.DesignEngineer1Id,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Projects_Users_DesignEngineer2Id",
                        column: x => x.DesignEngineer2Id,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Projects_Users_ProjectManagerId",
                        column: x => x.ProjectManagerId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                schema: "dbo",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Controllers",
                schema: "dbo",
                columns: table => new
                {
                    ControllerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ControllerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ControllerType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FirmwareVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AnalogInputCount = table.Column<int>(type: "int", nullable: true),
                    AnalogOutputCount = table.Column<int>(type: "int", nullable: true),
                    DigitalInputCount = table.Column<int>(type: "int", nullable: true),
                    DigitalOutputCount = table.Column<int>(type: "int", nullable: true),
                    CommissioningDate = table.Column<DateOnly>(type: "date", nullable: true),
                    WarrantyExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controllers", x => x.ControllerId);
                    table.CheckConstraint("CK_Controllers_ControllerType", "[ControllerType] IN ('VAVController','BMS','PLC','RTU','Thermostat','VFD','Gateway','Other')");
                    table.ForeignKey(
                        name: "FK_Controllers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_Controllers_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "DamperSchedule",
                schema: "dbo",
                columns: table => new
                {
                    DamperScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DamperTag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Pressure = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActuatorType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamperSchedule", x => x.DamperScheduleId);
                    table.ForeignKey(
                        name: "FK_DamperSchedule_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                });

            migrationBuilder.CreateTable(
                name: "Deliverables",
                schema: "dbo",
                columns: table => new
                {
                    DeliverableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliverableType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    SubmittedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ApprovedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ApprovedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliverables", x => x.DeliverableId);
                    table.CheckConstraint("CK_Deliverables_DeliverableType", "[DeliverableType] IN ('Submittal','IOM','AsBuilt','CommissioningReport')");
                    table.CheckConstraint("CK_Deliverables_Status", "[Status] IS NULL OR [Status] IN ('NotStarted','InProgress','Ready','Submitted','ApprovedWithChanges','Approved','Rejected','Archived')");
                    table.ForeignKey(
                        name: "FK_Deliverables_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_Deliverables_Users_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Deliverables_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                schema: "dbo",
                columns: table => new
                {
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentTag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EquipmentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Capacity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CapacityUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecSheetUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InstallationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    WarrantyExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    MaintenanceFrequency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.EquipmentId);
                    table.CheckConstraint("CK_Equipment_EquipmentType", "[EquipmentType] IN ('RTU','AHU','VAV','Pump','Fan','Chiller','Boiler','Damper','Valve','Sensor','Other')");
                    table.ForeignKey(
                        name: "FK_Equipment_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_Equipment_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                schema: "dbo",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Queued"),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    MaxRetries = table.Column<int>(type: "int", nullable: true, defaultValue: 3),
                    RequestedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.JobId);
                    table.CheckConstraint("CK_Jobs_JobType", "[JobType] IN ('GenerateSubmittal','VisioExport','PDFGeneration','EquipmentSchedule','BOMGeneration','Other')");
                    table.CheckConstraint("CK_Jobs_Status", "[Status] IN ('Queued','Processing','Completed','Failed','Cancelled')");
                    table.ForeignKey(
                        name: "FK_Jobs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_Jobs_Users_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                schema: "dbo",
                columns: table => new
                {
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discipline = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ParentNoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MentionedUserIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsResolved = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NoteId);
                    table.CheckConstraint("CK_Notes_Discipline", "[Discipline] IS NULL OR [Discipline] IN ('Overview','Financial','Schedule','Design','Service')");
                    table.ForeignKey(
                        name: "FK_Notes_Notes_ParentNoteId",
                        column: x => x.ParentNoteId,
                        principalSchema: "dbo",
                        principalTable: "Notes",
                        principalColumn: "NoteId");
                    table.ForeignKey(
                        name: "FK_Notes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_Notes_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Notes_Users_ResolvedByUserId",
                        column: x => x.ResolvedByUserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ProjectDocuments",
                schema: "dbo",
                columns: table => new
                {
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlobStorageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UploadedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Version = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDocuments", x => x.DocumentId);
                    table.CheckConstraint("CK_ProjectDocuments_DocumentType", "[DocumentType] IS NULL OR [DocumentType] IN ('Submittal','Drawing','Specification','Manual','Report','Other')");
                    table.ForeignKey(
                        name: "FK_ProjectDocuments_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_ProjectDocuments_Users_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "RFIs",
                schema: "dbo",
                columns: table => new
                {
                    RFIId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RFINumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedToUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ResponseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFIs", x => x.RFIId);
                    table.CheckConstraint("CK_RFIs_Status", "[Status] IS NULL OR [Status] IN ('Open','Acknowledged','InProgress','Responded','Closed')");
                    table.ForeignKey(
                        name: "FK_RFIs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_RFIs_Users_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_RFIs_Users_IssuedByUserId",
                        column: x => x.IssuedByUserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ValveSchedule",
                schema: "dbo",
                columns: table => new
                {
                    ValveScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValveTag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Pressure = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Temperature = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Material = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActuatorType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValveSchedule", x => x.ValveScheduleId);
                    table.ForeignKey(
                        name: "FK_ValveSchedule_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                schema: "dbo",
                columns: table => new
                {
                    NodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ControllerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NodeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NodeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Protocol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NetworkAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaxDevices = table.Column<int>(type: "int", nullable: true),
                    BusAssociation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CommissioningDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.NodeId);
                    table.CheckConstraint("CK_Nodes_NodeType", "[NodeType] IN ('BACnetMSTP','BACnetIP','ModbusTCP','ModbusRTU','TCPIPGateway','EthernetInterface','SerialInterface','Other')");
                    table.ForeignKey(
                        name: "FK_Nodes_Controllers_ControllerId",
                        column: x => x.ControllerId,
                        principalSchema: "dbo",
                        principalTable: "Controllers",
                        principalColumn: "ControllerId");
                    table.ForeignKey(
                        name: "FK_Nodes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_Nodes_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                schema: "dbo",
                columns: table => new
                {
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhysicalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatasheetUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IOMUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    WiringDiagramUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MountedOnEquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    InstallationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ProjectSpecificSpecs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommissioningDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CommissioningStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.DeviceId);
                    table.CheckConstraint("CK_Devices_CommissioningStatus", "[CommissioningStatus] IS NULL OR [CommissioningStatus] IN ('NotStarted','InProgress','Commissioned','Verified','Failed')");
                    table.CheckConstraint("CK_Devices_DeviceType", "[DeviceType] IN ('Relay','Enclosure','Terminal','Transformer','Wiring','Thermistor','RTD','Sensor','Actuator','Transducer','Valve','Damper','FlowMeter','UtilityMeter','AirflowStation','Other')");
                    table.ForeignKey(
                        name: "FK_Devices_Equipment_MountedOnEquipmentId",
                        column: x => x.MountedOnEquipmentId,
                        principalSchema: "dbo",
                        principalTable: "Equipment",
                        principalColumn: "EquipmentId");
                    table.ForeignKey(
                        name: "FK_Devices_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_Devices_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "NoteReactions",
                schema: "dbo",
                columns: table => new
                {
                    ReactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Emoji = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteReactions", x => x.ReactionId);
                    table.ForeignKey(
                        name: "FK_NoteReactions_Notes_NoteId",
                        column: x => x.NoteId,
                        principalSchema: "dbo",
                        principalTable: "Notes",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteReactions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ControllerIOSlots",
                schema: "dbo",
                columns: table => new
                {
                    IOSlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ControllerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SlotName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IOType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SlotNumber = table.Column<int>(type: "int", nullable: true),
                    DataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MinValue = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    MaxValue = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    AssignedPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControllerIOSlots", x => x.IOSlotId);
                    table.CheckConstraint("CK_ControllerIOSlots_IOType", "[IOType] IN ('AI','AO','DI','DO')");
                    table.ForeignKey(
                        name: "FK_ControllerIOSlots_Controllers_ControllerId",
                        column: x => x.ControllerId,
                        principalSchema: "dbo",
                        principalTable: "Controllers",
                        principalColumn: "ControllerId");
                });

            migrationBuilder.CreateTable(
                name: "Points",
                schema: "dbo",
                columns: table => new
                {
                    PointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointTag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PointDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ControllerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ControllerIOSlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PointType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MinValue = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    MaxValue = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    DefaultValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MinPhysical = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    MaxPhysical = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    MinRaw = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    MaxRaw = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    ScalingFactor = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    Offset = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    BACnetObjectName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BACnetObjectInstance = table.Column<int>(type: "int", nullable: true),
                    IsMonitored = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    IsLogged = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    Quality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ControlPriority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsMultiTermination = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    ParentDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.PointId);
                    table.CheckConstraint("CK_Points_ControlPriority", "[ControlPriority] IS NULL OR [ControlPriority] IN ('Manual','Auto','Locked','Override')");
                    table.CheckConstraint("CK_Points_DataType", "[DataType] IN ('AnalogInput','AnalogOutput','DigitalInput','DigitalOutput','Integer','Real','String','Enumeration')");
                    table.CheckConstraint("CK_Points_PointType", "[PointType] IN ('Input','Output','Variable','Parameter')");
                    table.CheckConstraint("CK_Points_SourceType", "[SourceType] IN ('SoftPoint','HardPoint')");
                    table.ForeignKey(
                        name: "FK_Points_ControllerIOSlots_ControllerIOSlotId",
                        column: x => x.ControllerIOSlotId,
                        principalSchema: "dbo",
                        principalTable: "ControllerIOSlots",
                        principalColumn: "IOSlotId");
                    table.ForeignKey(
                        name: "FK_Points_Controllers_ControllerId",
                        column: x => x.ControllerId,
                        principalSchema: "dbo",
                        principalTable: "Controllers",
                        principalColumn: "ControllerId");
                    table.ForeignKey(
                        name: "FK_Points_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "dbo",
                        principalTable: "Devices",
                        principalColumn: "DeviceId");
                    table.ForeignKey(
                        name: "FK_Points_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalSchema: "dbo",
                        principalTable: "Equipment",
                        principalColumn: "EquipmentId");
                    table.ForeignKey(
                        name: "FK_Points_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                    table.ForeignKey(
                        name: "FK_Points_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "PointDistribution",
                schema: "dbo",
                columns: table => new
                {
                    DistributionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoftPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsumingControllerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocalPointName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointDistribution", x => x.DistributionId);
                    table.ForeignKey(
                        name: "FK_PointDistribution_Controllers_ConsumingControllerId",
                        column: x => x.ConsumingControllerId,
                        principalSchema: "dbo",
                        principalTable: "Controllers",
                        principalColumn: "ControllerId");
                    table.ForeignKey(
                        name: "FK_PointDistribution_Nodes_NodeId",
                        column: x => x.NodeId,
                        principalSchema: "dbo",
                        principalTable: "Nodes",
                        principalColumn: "NodeId");
                    table.ForeignKey(
                        name: "FK_PointDistribution_Points_SoftPointId",
                        column: x => x.SoftPointId,
                        principalSchema: "dbo",
                        principalTable: "Points",
                        principalColumn: "PointId");
                    table.ForeignKey(
                        name: "FK_PointDistribution_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "dbo",
                        principalTable: "Projects",
                        principalColumn: "ProjectId");
                });

            migrationBuilder.CreateIndex(
                name: "idx_activitylogs_project",
                schema: "dbo",
                table: "ActivityLogs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_UserId",
                schema: "dbo",
                table: "ActivityLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Name_Type",
                schema: "dbo",
                table: "Companies",
                columns: new[] { "Name", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_ioslots_controller",
                schema: "dbo",
                table: "ControllerIOSlots",
                column: "ControllerId");

            migrationBuilder.CreateIndex(
                name: "idx_ioslots_used",
                schema: "dbo",
                table: "ControllerIOSlots",
                column: "IsUsed");

            migrationBuilder.CreateIndex(
                name: "IX_ControllerIOSlots_AssignedPointId",
                schema: "dbo",
                table: "ControllerIOSlots",
                column: "AssignedPointId");

            migrationBuilder.CreateIndex(
                name: "IX_ControllerIOSlots_ControllerId_SlotName",
                schema: "dbo",
                table: "ControllerIOSlots",
                columns: new[] { "ControllerId", "SlotName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_controllers_project",
                schema: "dbo",
                table: "Controllers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Controllers_CreatedByUserId",
                schema: "dbo",
                table: "Controllers",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Controllers_ProjectId_ControllerName",
                schema: "dbo",
                table: "Controllers",
                columns: new[] { "ProjectId", "ControllerName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DamperSchedule_ProjectId_DamperTag",
                schema: "dbo",
                table: "DamperSchedule",
                columns: new[] { "ProjectId", "DamperTag" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_deliverables_project",
                schema: "dbo",
                table: "Deliverables",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliverables_ApprovedByUserId",
                schema: "dbo",
                table: "Deliverables",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliverables_CreatedByUserId",
                schema: "dbo",
                table: "Deliverables",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "idx_devices_equipment",
                schema: "dbo",
                table: "Devices",
                column: "MountedOnEquipmentId");

            migrationBuilder.CreateIndex(
                name: "idx_devices_project",
                schema: "dbo",
                table: "Devices",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "idx_devices_type",
                schema: "dbo",
                table: "Devices",
                column: "DeviceType");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CreatedByUserId",
                schema: "dbo",
                table: "Devices",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ProjectId_DeviceName",
                schema: "dbo",
                table: "Devices",
                columns: new[] { "ProjectId", "DeviceName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_equipment_project",
                schema: "dbo",
                table: "Equipment",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "idx_equipment_type",
                schema: "dbo",
                table: "Equipment",
                column: "EquipmentType");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_CreatedByUserId",
                schema: "dbo",
                table: "Equipment",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_ProjectId_EquipmentTag",
                schema: "dbo",
                table: "Equipment",
                columns: new[] { "ProjectId", "EquipmentTag" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_jobs_project_status",
                schema: "dbo",
                table: "Jobs",
                columns: new[] { "ProjectId", "Status" });

            migrationBuilder.CreateIndex(
                name: "idx_jobs_type",
                schema: "dbo",
                table: "Jobs",
                column: "JobType");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_RequestedByUserId",
                schema: "dbo",
                table: "Jobs",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "idx_nodes_controller",
                schema: "dbo",
                table: "Nodes",
                column: "ControllerId");

            migrationBuilder.CreateIndex(
                name: "idx_nodes_project",
                schema: "dbo",
                table: "Nodes",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "idx_nodes_protocol",
                schema: "dbo",
                table: "Nodes",
                column: "Protocol");

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_ControllerId_NodeName",
                schema: "dbo",
                table: "Nodes",
                columns: new[] { "ControllerId", "NodeName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_CreatedByUserId",
                schema: "dbo",
                table: "Nodes",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteReactions_NoteId_UserId_Emoji",
                schema: "dbo",
                table: "NoteReactions",
                columns: new[] { "NoteId", "UserId", "Emoji" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NoteReactions_UserId",
                schema: "dbo",
                table: "NoteReactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "idx_notes_project_discipline",
                schema: "dbo",
                table: "Notes",
                columns: new[] { "ProjectId", "Discipline" });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_AuthorId",
                schema: "dbo",
                table: "Notes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ParentNoteId",
                schema: "dbo",
                table: "Notes",
                column: "ParentNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ResolvedByUserId",
                schema: "dbo",
                table: "Notes",
                column: "ResolvedByUserId");

            migrationBuilder.CreateIndex(
                name: "idx_distribution_consumer",
                schema: "dbo",
                table: "PointDistribution",
                column: "ConsumingControllerId");

            migrationBuilder.CreateIndex(
                name: "idx_distribution_soft",
                schema: "dbo",
                table: "PointDistribution",
                column: "SoftPointId");

            migrationBuilder.CreateIndex(
                name: "IX_PointDistribution_NodeId",
                schema: "dbo",
                table: "PointDistribution",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_PointDistribution_ProjectId",
                schema: "dbo",
                table: "PointDistribution",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PointDistribution_SoftPointId_ConsumingControllerId",
                schema: "dbo",
                table: "PointDistribution",
                columns: new[] { "SoftPointId", "ConsumingControllerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_points_controller",
                schema: "dbo",
                table: "Points",
                column: "ControllerId");

            migrationBuilder.CreateIndex(
                name: "idx_points_device",
                schema: "dbo",
                table: "Points",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "idx_points_equipment",
                schema: "dbo",
                table: "Points",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "idx_points_multiterm",
                schema: "dbo",
                table: "Points",
                column: "IsMultiTermination");

            migrationBuilder.CreateIndex(
                name: "idx_points_project",
                schema: "dbo",
                table: "Points",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "idx_points_source",
                schema: "dbo",
                table: "Points",
                column: "SourceType");

            migrationBuilder.CreateIndex(
                name: "IX_Points_ControllerIOSlotId",
                schema: "dbo",
                table: "Points",
                column: "ControllerIOSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_CreatedByUserId",
                schema: "dbo",
                table: "Points",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_ProjectId_PointTag",
                schema: "dbo",
                table: "Points",
                columns: new[] { "ProjectId", "PointTag" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_documents_project",
                schema: "dbo",
                table: "ProjectDocuments",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDocuments_ProjectId_DocumentName_Version",
                schema: "dbo",
                table: "ProjectDocuments",
                columns: new[] { "ProjectId", "DocumentName", "Version" },
                unique: true,
                filter: "[Version] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDocuments_UploadedByUserId",
                schema: "dbo",
                table: "ProjectDocuments",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "idx_projects_pm",
                schema: "dbo",
                table: "Projects",
                column: "ProjectManagerId");

            migrationBuilder.CreateIndex(
                name: "idx_projects_status",
                schema: "dbo",
                table: "Projects",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatedByUserId",
                schema: "dbo",
                table: "Projects",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CustomerId",
                schema: "dbo",
                table: "Projects",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DesignEngineer1Id",
                schema: "dbo",
                table: "Projects",
                column: "DesignEngineer1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DesignEngineer2Id",
                schema: "dbo",
                table: "Projects",
                column: "DesignEngineer2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_EngineeringFirmId",
                schema: "dbo",
                table: "Projects",
                column: "EngineeringFirmId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectNumber",
                schema: "dbo",
                table: "Projects",
                column: "ProjectNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RFIs_AssignedToUserId",
                schema: "dbo",
                table: "RFIs",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RFIs_IssuedByUserId",
                schema: "dbo",
                table: "RFIs",
                column: "IssuedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RFIs_ProjectId",
                schema: "dbo",
                table: "RFIs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RFIs_RFINumber",
                schema: "dbo",
                table: "RFIs",
                column: "RFINumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AzureAdId",
                schema: "dbo",
                table: "Users",
                column: "AzureAdId",
                unique: true,
                filter: "[AzureAdId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "dbo",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ValveSchedule_ProjectId_ValveTag",
                schema: "dbo",
                table: "ValveSchedule",
                columns: new[] { "ProjectId", "ValveTag" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ControllerIOSlots_Points_AssignedPointId",
                schema: "dbo",
                table: "ControllerIOSlots",
                column: "AssignedPointId",
                principalSchema: "dbo",
                principalTable: "Points",
                principalColumn: "PointId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Controllers_Projects_ProjectId",
                schema: "dbo",
                table: "Controllers");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Projects_ProjectId",
                schema: "dbo",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_Projects_ProjectId",
                schema: "dbo",
                table: "Equipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Projects_ProjectId",
                schema: "dbo",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Controllers_Users_CreatedByUserId",
                schema: "dbo",
                table: "Controllers");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Users_CreatedByUserId",
                schema: "dbo",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_Users_CreatedByUserId",
                schema: "dbo",
                table: "Equipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Users_CreatedByUserId",
                schema: "dbo",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_ControllerIOSlots_Controllers_ControllerId",
                schema: "dbo",
                table: "ControllerIOSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Controllers_ControllerId",
                schema: "dbo",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_ControllerIOSlots_Points_AssignedPointId",
                schema: "dbo",
                table: "ControllerIOSlots");

            migrationBuilder.DropTable(
                name: "ActivityLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DamperSchedule",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Deliverables",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Jobs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NoteReactions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PointDistribution",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProjectDocuments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RFIs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ValveSchedule",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Notes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Nodes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Projects",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Controllers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Points",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ControllerIOSlots",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Devices",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Equipment",
                schema: "dbo");
        }
    }
}
