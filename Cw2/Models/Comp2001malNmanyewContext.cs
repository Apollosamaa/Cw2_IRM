using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cw2.Models;

public partial class Comp2001malNmanyewContext : DbContext
{
    public Comp2001malNmanyewContext()
    {
    }

    public Comp2001malNmanyewContext(DbContextOptions<Comp2001malNmanyewContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HikingActivity> HikingActivities { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Postfeed> Postfeeds { get; set; }

    public virtual DbSet<Privacy> Privacies { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<ProfilePreference> ProfilePreferences { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001MAL_NManyew;User Id=NManyew;Password=YreI936*;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HikingActivity>(entity =>
        {
            entity.HasKey(e => e.HikingId).HasName("PK__HikingAc__E1FC7A014CA11DB2");

            entity.ToTable("HikingActivity", "CW1");

            entity.Property(e => e.HikingId).HasColumnName("hiking_id");
            entity.Property(e => e.HikingDistance).HasColumnName("hiking_distance");
            entity.Property(e => e.HikingElevationGain).HasColumnName("hiking_elevation_gain");
            entity.Property(e => e.HikingTimestamp)
                .HasColumnType("datetime")
                .HasColumnName("hiking_timestamp");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");

            entity.HasOne(d => d.Location).WithMany(p => p.HikingActivities)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__HikingAct__locat__0C85DE4D");

            entity.HasOne(d => d.Profile).WithMany(p => p.HikingActivities)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("FK__HikingAct__profi__0B91BA14");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Location__771831EA76317A91");

            entity.ToTable("Locations", "CW1");

            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.LocationName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("location_name");
        });

        modelBuilder.Entity<Postfeed>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Postfeed__3ED787661FA2D2D2");

            entity.ToTable("Postfeed", "CW1", tb => tb.HasTrigger("DeleteLikesAndPost"));

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.PostAttachment)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("post_attachment");
            entity.Property(e => e.PostCreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("post_created_at");
            entity.Property(e => e.PostLike)
                .HasDefaultValue(0)
                .HasColumnName("post_like");
            entity.Property(e => e.PostText)
                .HasColumnType("text")
                .HasColumnName("post_text");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");

            entity.HasOne(d => d.Profile).WithMany(p => p.Postfeeds)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("FK__Postfeed__profil__7C4F7684");
        });

        modelBuilder.Entity<Privacy>(entity =>
        {
            entity.HasKey(e => e.PrivacyId).HasName("PK__Privacy__09F919FED396964C");

            entity.ToTable("Privacy", "CW1");

            entity.Property(e => e.PrivacyId)
                .ValueGeneratedNever()
                .HasColumnName("privacy_id");
            entity.Property(e => e.PrivacyLevel)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("privacy_level");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK__Profiles__AEBB701F4A65B77F");

            entity.ToTable("Profiles", "CW1", tb =>
                {
                    tb.HasTrigger("DeletePostsAfterProfileArchival");
                    tb.HasTrigger("PreventSingleProfileArchival");
                    tb.HasTrigger("RemoveFollowersAfterProfileArchival");
                });

            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.ProfileArchive)
                .HasDefaultValue(false)
                .HasColumnName("profile_archive");
            entity.Property(e => e.ProfileBio)
                .HasColumnType("text")
                .HasColumnName("profile_bio");
            entity.Property(e => e.ProfileCreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("profile_created_at");
            entity.Property(e => e.ProfileDob).HasColumnName("profile_dob");
            entity.Property(e => e.ProfileFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("profile_fname");
            entity.Property(e => e.ProfileHeight)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("profile_height");
            entity.Property(e => e.ProfileLname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("profile_lname");
            entity.Property(e => e.ProfileLocation).HasColumnName("profile_location");
            entity.Property(e => e.ProfileWeight)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("profile_weight");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.ProfileLocationNavigation).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.ProfileLocation)
                .HasConstraintName("FK__Profiles__profil__787EE5A0");

            entity.HasOne(d => d.User).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Profiles__user_i__778AC167");

            entity.HasMany(d => d.Followers).WithMany(p => p.Followings)
                .UsingEntity<Dictionary<string, object>>(
                    "Follower",
                    r => r.HasOne<Profile>().WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Followers__follo__07C12930"),
                    l => l.HasOne<Profile>().WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Followers__follo__08B54D69"),
                    j =>
                    {
                        j.HasKey("FollowerId", "FollowingId").HasName("PK__Follower__CAC186A7046F77E7");
                        j.ToTable("Followers", "CW1");
                        j.IndexerProperty<int>("FollowerId").HasColumnName("follower_id");
                        j.IndexerProperty<int>("FollowingId").HasColumnName("following_id");
                    });

            entity.HasMany(d => d.Followings).WithMany(p => p.Followers)
                .UsingEntity<Dictionary<string, object>>(
                    "Follower",
                    r => r.HasOne<Profile>().WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Followers__follo__08B54D69"),
                    l => l.HasOne<Profile>().WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Followers__follo__07C12930"),
                    j =>
                    {
                        j.HasKey("FollowerId", "FollowingId").HasName("PK__Follower__CAC186A7046F77E7");
                        j.ToTable("Followers", "CW1");
                        j.IndexerProperty<int>("FollowerId").HasColumnName("follower_id");
                        j.IndexerProperty<int>("FollowingId").HasColumnName("following_id");
                    });

            entity.HasMany(d => d.Posts).WithMany(p => p.Profiles)
                .UsingEntity<Dictionary<string, object>>(
                    "PostLike",
                    r => r.HasOne<Postfeed>().WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PostLikes__post___1F98B2C1"),
                    l => l.HasOne<Profile>().WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PostLikes__profi__1EA48E88"),
                    j =>
                    {
                        j.HasKey("ProfileId", "PostId").HasName("PK__PostLike__DD560869A825F4E7");
                        j.ToTable("PostLikes", "CW1");
                        j.IndexerProperty<int>("ProfileId").HasColumnName("profile_id");
                        j.IndexerProperty<int>("PostId").HasColumnName("post_id");
                    });
        });

        modelBuilder.Entity<ProfilePreference>(entity =>
        {
            entity.HasKey(e => e.PreferencesId).HasName("PK__ProfileP__AB785541B1E63210");

            entity.ToTable("ProfilePreferences", "CW1");

            entity.Property(e => e.PreferencesId).HasColumnName("preferences_id");
            entity.Property(e => e.PreferenceEmailNotification)
                .HasDefaultValue(false)
                .HasColumnName("preference_email_notification");
            entity.Property(e => e.PreferenceNotification)
                .HasDefaultValue(false)
                .HasColumnName("preference_notification");
            entity.Property(e => e.PrivacyId)
                .HasDefaultValue(1)
                .HasColumnName("privacy_id");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");

            entity.HasOne(d => d.Privacy).WithMany(p => p.ProfilePreferences)
                .HasForeignKey(d => d.PrivacyId)
                .HasConstraintName("FK__ProfilePr__priva__04E4BC85");

            entity.HasOne(d => d.Profile).WithMany(p => p.ProfilePreferences)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("FK__ProfilePr__profi__03F0984C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370FC233349B");

            entity.ToTable("Users", "CW1");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E616455BB86F4").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.UserRole)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("user_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
