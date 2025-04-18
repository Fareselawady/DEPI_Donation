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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source =DESKTOP-1M8QOF6;Initial Catalog=Donation1;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK__Activiti__45F4A7F1550916E8");

            entity.Property(e => e.ActivityId).ValueGeneratedNever();

            // تحديث العلاقة لتطبيق Cascade Delete
            entity.HasOne(d => d.Report)
                .WithMany(p => p.Activities)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.Cascade); // استخدام Cascade Delete
        });

        modelBuilder.Entity<Donation>(entity =>
        {
            entity.HasKey(e => e.DonationId).HasName("PK__Donation__C5082EDB7E81B6EF");

            entity.Property(e => e.DonationId).ValueGeneratedNever();

            // تحديث العلاقة لتطبيق Cascade Delete
            entity.HasOne(d => d.Activity)
                .WithMany(p => p.Donations)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.Cascade); // استخدام Cascade Delete

            entity.HasOne(d => d.Donor)
                .WithMany(p => p.Donations)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.Cascade); // استخدام Cascade Delete

            entity.HasOne(d => d.Payment)
                .WithMany(p => p.Donations)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.Cascade); // استخدام Cascade Delete
        });

        modelBuilder.Entity<DonorNotification>(entity =>
        {
            entity.HasKey(e => new { e.DonorId, e.NotificationId }).HasName("PK__DonorNot__3722CD7B73A8810A");

            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.SentAt).HasDefaultValueSql("(getdate())");

            // تعديل العلاقات هنا إذا كنت تريد حذف الـ Notifications بشكل تلقائي عند حذف الـ Donor
            entity.HasOne(d => d.Donor)
                .WithMany(p => p.DonorNotifications)
                .OnDelete(DeleteBehavior.Cascade) // استخدام Cascade Delete
                .HasConstraintName("FK__DonorNoti__Donor__403A8C7D");

            // حذف Notification عند حذف DonorNotification
            entity.HasOne(d => d.Notification)
                .WithMany(p => p.DonorNotifications)
                .OnDelete(DeleteBehavior.Cascade) // استخدام Cascade Delete
                .HasConstraintName("FK__DonorNoti__Notif__412EB0B6");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E12171BDDDB");

            entity.Property(e => e.NotificationId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A58925DCF70");

            entity.Property(e => e.PaymentId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Reports__D5BD48E52A1E7B15");

            entity.Property(e => e.ReportId).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C53F7E6B7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
