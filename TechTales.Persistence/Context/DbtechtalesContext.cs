using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using TechTales.Domain.entities;

namespace TechTales.Persistence.Context;

public partial class DbtechtalesContext : DbContext
{
    public DbtechtalesContext()
    {
    }

    public DbtechtalesContext(DbContextOptions<DbtechtalesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comments> Comments { get; set; }

    public virtual DbSet<Stories> Stories { get; set; }

    public virtual DbSet<Challenges> Challenges { get; set; }

    public virtual DbSet<ChallengesCompleted> ChallengesCompleted { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    public virtual DbSet<VotesStories> VotesStories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Comments>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("comments");

            entity.HasIndex(e => e.IdStorie, "IdStorie");

            entity.HasIndex(e => e.IdUser, "IdUser");

            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.DateComment)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.IdStoriesNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdStorie)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comments_ibfk_2");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("comments_ibfk_1");
        });

        modelBuilder.Entity<Stories>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("stories");

            entity.HasIndex(e => e.IdUser, "IdUser");

            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.DatePublication)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Stories)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("stories_ibfk_1");
        });

        modelBuilder.Entity<Challenges>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("challenges");

            entity.HasIndex(e => e.IdAdmin, "IdAdmin");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.DatePublication)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.Type)
       .HasDefaultValueSql("'basic'")
       .HasColumnType("enum('basic','intermediate','advanced')");
            entity.Property(e => e.Hint).HasColumnType("text");

            entity.HasOne(d => d.IdAdminNavigation).WithMany(p => p.Challenges)
                .HasForeignKey(d => d.IdAdmin)
                .HasConstraintName("challenges_ibfk_1");
        });

        modelBuilder.Entity<ChallengesCompleted>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("challengescompleted");

            entity.HasIndex(e => e.IdChallenges, "IdChallenges");

            entity.HasIndex(e => new { e.IdUser, e.IdChallenges }, "IdUser").IsUnique();

            entity.Property(e => e.DateCompleted)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.IdChallengesNavigation).WithMany(p => p.ChallengesCompleted)
                .HasForeignKey(d => d.IdChallenges)
                .HasConstraintName("challengescompleted_ibfk_2");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.ChallengesCompleted
            
            )
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("challengescompleted_ibfk_1");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "Email");

            entity.Property(e => e.ImgProfile).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.DateRegister)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Role)
                .HasDefaultValueSql("'user'")
                .HasColumnType("enum('user','admin')");
        });

        modelBuilder.Entity<VotesStories>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("votesstories");

            entity.HasIndex(e => e.IdStories, "IdStories");

            entity.HasIndex(e => new { e.IdUser, e.IdStories }, "IdUser").IsUnique();

            entity.Property(e => e.DateVote)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.Property(e => e.Vote)
                .IsRequired()
                .HasColumnType("tinyint(1)");

            entity.HasOne(d => d.IdStoriesNavigation).WithMany(p => p.VotesStories)
                .HasForeignKey(d => d.IdStories)
                .HasConstraintName("votesstories_ibfk_2");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.VotesStories)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("votesstories_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
