﻿using System.Security.Cryptography;
using a;
using LunchSplitter.Domain.Entity;
using LunchSplitter.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;


namespace LunchSplitter.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupUser> GroupUsers { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<GroupUserPermissions> GroupUserPermissions { get; set; }
    
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionShare> TransactionShares { get; set; }
    
    public DbSet<GroupInvite> GroupInvites { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //create unique constraint on name 
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Name)
            .IsUnique();

        modelBuilder.Entity<GroupUser>()
            .HasIndex(gu => new { gu.GroupId, gu.UserId })
            .IsUnique();

        modelBuilder.Entity<TransactionShare>()
            .HasIndex(ts => new { ts.TransactionId, ts.UserId })
            .IsUnique();
        
        
    }
}