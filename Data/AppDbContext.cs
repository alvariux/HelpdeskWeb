using HelpDeskWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskWeb.Data
{
    public class AppDbContext: IdentityDbContext<User>
    {
        public DbSet<TicketStatus> TicketStatus { get; set; }
        public DbSet<TicketPriority> TicketPriority { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");  // SQL Server

            builder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");  // SQL Server

            builder.Entity<TicketStatus>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Entity<TicketStatus>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Entity<TicketPriority>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Entity<TicketPriority>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Entity<TicketStatus>()
                .HasIndex(ts => ts.Name)
                .IsUnique();

            builder.Entity<Ticket>()
                .ToTable("Tickets");

            builder.Entity<Ticket>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Entity<Ticket>()
                .HasIndex(t => t.TicketNumber)
                .IsUnique();

            builder.Entity<Ticket>()
                .HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ticket>()
                .HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ticket>()
                .HasOne(t => t.ResolvedBy)
                .WithMany()
                .HasForeignKey(t => t.ResolvedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ticket>()
                .HasOne(t => t.ClosedBy)
                .WithMany()
                .HasForeignKey(t => t.ClosedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Ticket>()
                .HasOne(t => t.Status)
                .WithMany()
                .HasForeignKey(t => t.StatusId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ticket>()
                .HasOne(t => t.Priority)
                .WithMany()
                .HasForeignKey(t => t.PriorityId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
