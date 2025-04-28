using System;
using System.Collections.Generic;
using DEPI_Donation.Models;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Data;

public partial class AppDbcontext : DbContext
{
    public AppDbcontext()
    {
    }

    public AppDbcontext(DbContextOptions<AppDbcontext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Donation> Donations { get; set; }

    public virtual DbSet<DonorNotification> DonorNotifications { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<User> Users { get; set; }


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

            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.SentAt).HasDefaultValueSql("(getdate())");

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
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C53F7E6B7");
            entity.Property(e => e.UserId).ValueGeneratedOnAdd(); // تغيير من Never إلى OnAdd
            entity.Property(e => e.UserType).HasDefaultValue("Donor");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
