using FastDateNotificatorAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastDateNotificatorAPI.Models;

public class DataContext : DbContext
{
    public DbSet<RememberedDate> RememberedDates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RememberedDate>(m =>
        {
            m.HasKey(r => r.Id);
            m.HasIndex(r => r.TelegramId);
        });
    }
}