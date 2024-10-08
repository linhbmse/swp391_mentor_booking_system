using System;
using System.Collections.Generic;
using MentorScheduleCustom.Models;
using Microsoft.EntityFrameworkCore;

namespace MentorScheduleCustom.Data;

public partial class SwpFall24Context : DbContext
{
    public SwpFall24Context()
    {
    }

    public SwpFall24Context(DbContextOptions<SwpFall24Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminDetail> AdminDetails { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<MentorDetail> MentorDetails { get; set; }

    public virtual DbSet<MentorSchedule> MentorSchedules { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Response> Responses { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<StudentDetail> StudentDetails { get; set; }

    public virtual DbSet<StudentGroup> StudentGroups { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserActivity> UserActivities { get; set; }

    public virtual DbSet<UserActivityType> UserActivityTypes { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public virtual DbSet<WalletTransaction> WalletTransactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];

        return strConn;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminDetail>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__AdminDet__CB9A1CFFC77A2247");

            entity.ToTable("AdminDetail");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("userId");

            entity.HasOne(d => d.User).WithOne(p => p.AdminDetail)
                .HasForeignKey<AdminDetail>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AdminDeta__userI__6754599E");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3213E83F3D55E1FD");

            entity.ToTable("Booking");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LeaderId).HasColumnName("leaderId");
            entity.Property(e => e.MentorScheduleId).HasColumnName("mentorScheduleId");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Leader).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.LeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__leaderI__72C60C4A");

            entity.HasOne(d => d.MentorSchedule).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.MentorScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__mentorS__73BA3083");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3213E83FE49DBE94");

            entity.ToTable("Feedback");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.GivenBy).HasColumnName("givenBy");
            entity.Property(e => e.GivenTo).HasColumnName("givenTo");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Booking).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__bookin__787EE5A0");

            entity.HasOne(d => d.GivenByNavigation).WithMany(p => p.FeedbackGivenByNavigations)
                .HasForeignKey(d => d.GivenBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__givenB__797309D9");

            entity.HasOne(d => d.GivenToNavigation).WithMany(p => p.FeedbackGivenToNavigations)
                .HasForeignKey(d => d.GivenTo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__givenT__7A672E12");
        });

        modelBuilder.Entity<MentorDetail>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__MentorDe__CB9A1CFF9B00F9FB");

            entity.ToTable("MentorDetail");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("userId");
            entity.Property(e => e.AdditionalContactInfo)
                .HasMaxLength(255)
                .HasColumnName("additionalContactInfo");
            entity.Property(e => e.AltProgrammingLanguage)
                .HasMaxLength(255)
                .HasColumnName("altProgrammingLanguage");
            entity.Property(e => e.BookingScore)
                .HasDefaultValue(0)
                .HasColumnName("bookingScore");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Education)
                .HasMaxLength(255)
                .HasColumnName("education");
            entity.Property(e => e.Framework)
                .HasMaxLength(50)
                .HasColumnName("framework");
            entity.Property(e => e.MainProgrammingLanguage)
                .HasMaxLength(50)
                .HasColumnName("mainProgrammingLanguage");

            entity.HasOne(d => d.User).WithOne(p => p.MentorDetail)
                .HasForeignKey<MentorDetail>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MentorDet__userI__3E52440B");

            entity.HasMany(d => d.Skills).WithMany(p => p.MentorDetails)
                .UsingEntity<Dictionary<string, object>>(
                    "MentorSkill",
                    r => r.HasOne<Skill>().WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MentorSki__skill__07C12930"),
                    l => l.HasOne<MentorDetail>().WithMany()
                        .HasForeignKey("MentorDetailId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MentorSki__mento__06CD04F7"),
                    j =>
                    {
                        j.HasKey("MentorDetailId", "SkillId").HasName("PK__MentorSk__F216C2BCEB9CA1B2");
                        j.ToTable("MentorSkill");
                        j.IndexerProperty<int>("MentorDetailId").HasColumnName("mentorDetailId");
                        j.IndexerProperty<int>("SkillId").HasColumnName("skillId");
                    });

            entity.HasMany(d => d.Specs).WithMany(p => p.MentorDetails)
                .UsingEntity<Dictionary<string, object>>(
                    "MentorSpecialization",
                    r => r.HasOne<Specialization>().WithMany()
                        .HasForeignKey("SpecId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MentorSpe__specI__5441852A"),
                    l => l.HasOne<MentorDetail>().WithMany()
                        .HasForeignKey("MentorDetailId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MentorSpe__mento__534D60F1"),
                    j =>
                    {
                        j.HasKey("MentorDetailId", "SpecId").HasName("PK__MentorSp__3FDDA9D878CF7743");
                        j.ToTable("MentorSpecialization");
                        j.IndexerProperty<int>("MentorDetailId").HasColumnName("mentorDetailId");
                        j.IndexerProperty<int>("SpecId").HasColumnName("specId");
                    });
        });

        modelBuilder.Entity<MentorSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MentorSc__3213E83F31FD699D");

            entity.ToTable("MentorSchedule");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.MentorDetailId).HasColumnName("mentorDetailId");
            entity.Property(e => e.SlotId).HasColumnName("slotId");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("available")
                .HasColumnName("status");

            entity.HasOne(d => d.MentorDetail).WithMany(p => p.MentorSchedules)
                .HasForeignKey(d => d.MentorDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MentorSch__mento__6E01572D");

            entity.HasOne(d => d.Slot).WithMany(p => p.MentorSchedules)
                .HasForeignKey(d => d.SlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MentorSch__slotI__6EF57B66");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Request__3213E83F2D665342");

            entity.ToTable("Request");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.LeaderId).HasColumnName("leaderId");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("pending")
                .HasColumnName("status");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Leader).WithMany(p => p.Requests)
                .HasForeignKey(d => d.LeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Request__leaderI__00200768");
        });

        modelBuilder.Entity<Response>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Response__3213E83F7E762023");

            entity.ToTable("Response");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.RequestId).HasColumnName("requestId");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Request).WithMany(p => p.Responses)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Response__reques__03F0984C");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Skill__3213E83F3E0BFFD8");

            entity.ToTable("Skill");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Slot__3213E83F1B17D9CB");

            entity.ToTable("Slot");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndTime).HasColumnName("endTime");
            entity.Property(e => e.StartTime).HasColumnName("startTime");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Speciali__3213E83FB329C35F");

            entity.ToTable("Specialization");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<StudentDetail>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__StudentD__CB9A1CFFCCCEF088");

            entity.ToTable("StudentDetail");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("userId");
            entity.Property(e => e.GroupId).HasColumnName("groupId");
            entity.Property(e => e.IsLeader).HasColumnName("isLeader");
            entity.Property(e => e.StudentCode)
                .HasMaxLength(8)
                .HasColumnName("studentCode");

            entity.HasOne(d => d.Group).WithMany(p => p.StudentDetails)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK__StudentDe__group__5DCAEF64");

            entity.HasOne(d => d.User).WithOne(p => p.StudentDetail)
                .HasForeignKey<StudentDetail>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentDe__userI__4222D4EF");
        });

        modelBuilder.Entity<StudentGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StudentG__3213E83F14C3AB95");

            entity.ToTable("StudentGroup");

            entity.HasIndex(e => e.WalletId, "UQ__StudentG__3785C87154012065").IsUnique();

            entity.HasIndex(e => e.GroupName, "UQ__StudentG__9011AC827582765A").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GroupName)
                .HasMaxLength(100)
                .HasColumnName("groupName");
            entity.Property(e => e.TopicId).HasColumnName("topicId");
            entity.Property(e => e.WalletId).HasColumnName("walletId");

            entity.HasOne(d => d.Topic).WithMany(p => p.StudentGroups)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentGr__topic__5BE2A6F2");

            entity.HasOne(d => d.Wallet).WithOne(p => p.StudentGroup)
                .HasForeignKey<StudentGroup>(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentGr__walle__5CD6CB2B");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Topic__3213E83F7A898CC1");

            entity.ToTable("Topic");

            entity.HasIndex(e => e.Name, "UQ__Topic__72E12F1B50BC7596").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83FF92A7092");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E61643F766798").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("fullName");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsFirstLogin)
                .HasDefaultValue(true)
                .HasColumnName("isFirstLogin");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .HasColumnName("phone");
            entity.Property(e => e.ProfilePhoto)
                .HasMaxLength(255)
                .HasColumnName("profilePhoto");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasDefaultValue("default")
                .HasColumnName("role");
        });

        modelBuilder.Entity<UserActivity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserActi__3213E83F4BE151F6");

            entity.ToTable("UserActivity");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActivityTypeId).HasColumnName("activityTypeId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.OperatorId).HasColumnName("operatorId");

            entity.HasOne(d => d.ActivityType).WithMany(p => p.UserActivities)
                .HasForeignKey(d => d.ActivityTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserActiv__activ__4AB81AF0");

            entity.HasOne(d => d.Operator).WithMany(p => p.UserActivities)
                .HasForeignKey(d => d.OperatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserActiv__opera__4BAC3F29");
        });

        modelBuilder.Entity<UserActivityType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserActi__3213E83F7E3B1339");

            entity.ToTable("UserActivityType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserSess__3213E83FF19C52EB");

            entity.ToTable("UserSession");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExpireTime)
                .HasColumnType("datetime")
                .HasColumnName("expireTime");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.UserSessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserSessi__userI__44FF419A");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3213E83F6F27F2FD");

            entity.ToTable("Wallet");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Balance).HasColumnName("balance");
        });

        modelBuilder.Entity<WalletTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WalletTr__3213E83FD1EBC541");

            entity.ToTable("WalletTransaction");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");
            entity.Property(e => e.WalletId).HasColumnName("walletId");

            entity.HasOne(d => d.Wallet).WithMany(p => p.WalletTransactions)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WalletTra__walle__6477ECF3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
