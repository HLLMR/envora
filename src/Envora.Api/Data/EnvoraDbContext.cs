using Envora.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Envora.Api.Data;

public sealed class EnvoraDbContext(DbContextOptions<EnvoraDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Controller> Controllers => Set<Controller>();
    public DbSet<Node> Nodes => Set<Node>();
    public DbSet<ControllerIoSlot> ControllerIoSlots => Set<ControllerIoSlot>();
    public DbSet<Point> Points => Set<Point>();
    public DbSet<PointDistribution> PointDistributions => Set<PointDistribution>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<NoteReaction> NoteReactions => Set<NoteReaction>();
    public DbSet<ProjectDocument> ProjectDocuments => Set<ProjectDocument>();
    public DbSet<Deliverable> Deliverables => Set<Deliverable>();
    public DbSet<ValveScheduleItem> ValveSchedule => Set<ValveScheduleItem>();
    public DbSet<DamperScheduleItem> DamperSchedule => Set<DamperScheduleItem>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<Rfi> Rfis => Set<Rfi>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EnvoraDbContext).Assembly);
    }
}


