using System;
using System.Collections.Generic;
using DEPI_Donation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DEPI_Donation.Data;

public class AppDbcontext : IdentityDbContext<
    User,
    IdentityRole<int>,
    int,
    IdentityUserClaim<int>,
    IdentityUserRole<int>,
    IdentityUserLogin<int>,
    IdentityRoleClaim<int>,
    IdentityUserToken<int>>
{
    public AppDbcontext()
    {
    }

    public AppDbcontext(DbContextOptions<AppDbcontext> options)
        : base(options)
    {
    }

    public DbSet<Activity> Activities { get; set; }

    public DbSet<Donation> Donations { get; set; }

    public DbSet<DonorNotification> DonorNotifications { get; set; }

    public DbSet<Notification> Notifications { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<Report> Reports { get; set; }

    //public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK__Activiti__45F4A7F1550916E8");
            entity.Property(e => e.ActivityId).ValueGeneratedOnAdd(); // تغيير من Never إلى OnAdd
            entity.Property(e => e.StartDate).HasDefaultValueSql("(getdate())");


            entity.HasMany(a => a.Reports)
                  .WithOne(r => r.Activity)
                  .HasForeignKey(r => r.ActivityId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Donation>(entity =>
        {
            entity.HasKey(e => e.DonationId).HasName("PK__Donation__C5082EDB7E81B6EF");
            entity.Property(e => e.DonationId).ValueGeneratedOnAdd(); // تغيير من Never إلى OnAdd
            entity.Property(e => e.Status).HasDefaultValue(DonationStatusType.Pending);
            entity.Property(e => e.DonationDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Activity)
                .WithMany(p => p.Donations)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Donor)
                .WithMany(p => p.Donations)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Payment)
                .WithMany(p => p.Donations)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DonorNotification>(entity =>
        {
            entity.HasKey(e => new { e.DonorId, e.NotificationId }).HasName("PK__DonorNot__3722CD7B73A8810A");

            entity.HasOne(d => d.Donor)
                .WithMany(p => p.DonorNotifications)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DonorNoti__Donor__403A8C7D");

            entity.HasOne(d => d.Notification)
                .WithMany(p => p.DonorNotifications)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DonorNoti__Notif__412EB0B6");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E12171BDDDB");
            entity.Property(e => e.NotificationId).ValueGeneratedOnAdd(); // تغيير من Never إلى OnAdd
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A58925DCF70");
            entity.Property(e => e.PaymentId).ValueGeneratedOnAdd(); // تغيير من Never إلى OnAdd
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId);
            entity.Property(e => e.ReportId).ValueGeneratedOnAdd(); // تغيير من Never إلى OnAdd
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); // تغيير من Never إلى OnAdd
        });

        modelBuilder.Entity<IdentityUserLogin<int>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });

        modelBuilder.Entity<IdentityUserToken<int>>()
            .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

        modelBuilder.Entity<IdentityUserRole<int>>()
            .HasKey(r => new { r.UserId, r.RoleId });

    }
}
