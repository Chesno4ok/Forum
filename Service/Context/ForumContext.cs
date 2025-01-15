using System;
using System.Collections.Generic;
using Forum.Logic.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Forum.Persistance;

public partial class ForumContext : DbContext
{
    public ForumContext()
    {
    }

    public ForumContext(DbContextOptions<ForumContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=193.124.44.234;Port=5432;Database=Forum2;Username=postgres;Password=df23H3FsgL");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(Entity =>
        {
            Entity.HasKey(e => e.Id).HasName("UserTypes_pkey");

            Entity.Property(e => e.Id).UseIdentityColumn();
        });

        modelBuilder.Entity<User>(Entity =>
        {
            Entity.HasKey(e => e.Id).HasName("Users_pkey");

            Entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Role_User");
        });

        modelBuilder.Entity<Post>(Entity =>
        {
            Entity.HasKey(e => e.Id).HasName("Posts_pkey");

            Entity.HasOne(d => d.UserNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Post_User");
        });

        modelBuilder.Entity<Comment>(Entity =>
        {
            Entity.HasKey(e => e.Id).HasName("Comments_pkey");

            Entity.HasOne(d => d.PostNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Comment_Post");


            Entity.HasOne(d => d.UserNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Comment_User");

            Entity.HasOne(d => d.CommentNavigation).WithMany(p => p.Replies)
                .HasForeignKey(d => d.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Comment_Reply");


        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
