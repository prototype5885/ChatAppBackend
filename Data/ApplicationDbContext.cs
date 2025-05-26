using ChatAppBackend.Models;
using IdGen;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ChatAppBackend.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<RegisteredUser, IdentityRole<long>, long>(options)
{
    public required DbSet<Server> Servers { get; set; }
    public required DbSet<Channel> Channels { get; set; }
    public required DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RegisteredUser>(b =>
        {
            b.Property(u => u.Id).ValueGeneratedNever().HasValueGenerator<SnowflakeDb>();
            b.Property(u => u.UserName).HasMaxLength(32);
            b.Property(u => u.NormalizedUserName).HasMaxLength(32);
            b.Property(u => u.Email).HasMaxLength(128);
            b.Property(u => u.NormalizedEmail).HasMaxLength(128);
        });
    }
}

