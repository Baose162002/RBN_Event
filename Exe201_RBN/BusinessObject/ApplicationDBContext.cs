using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BusinessObject
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<EventImg> EventImgs { get; set; }
        public DbSet<PromotionFee> PromotionFees { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<SubscriptionPackage> SubscriptionPackage { get; set; }
        public DbSet<Response> Responses { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public ApplicationDBContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Local"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring One-to-Many relationship for User and Company
            modelBuilder.Entity<User>()
                .HasMany(u => u.Companies)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuring One-to-Many relationship for User and Chat (User as Participant)
            modelBuilder.Entity<Chat>()
                .HasOne(u => u.User)
                .WithMany(c => c.Chats)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuring One-to-Many relationship for Chat and Messages
            modelBuilder.Entity<Chat>()
                .HasMany(ch => ch.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(m => m.ChatID)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuring One-to-Many relationship for Event and Booking
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Bookings)
                .WithOne(b => b.Event)
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuring One-to-Many relationship for Company and Event
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Events)
                .WithOne(e => e.Company)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuring One-to-Many relationship for User and Feedback
            modelBuilder.Entity<FeedBack>()
                .HasOne(u => u.User)
                .WithMany(f => f.FeedBacks)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction); 

            // Configuring One-to-Many relationship for Feedback and Response
            modelBuilder.Entity<FeedBack>()
                .HasMany(f => f.Responses)
                .WithOne(r => r.FeedBack)
                .HasForeignKey(r => r.FeedBackId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SubscriptionPackage>()
                .HasMany(sp => sp.Companies)
                .WithOne(c => c.SubscriptionPackage)
                .HasForeignKey(c => c.SubscriptionPackageId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Company>()
                .HasOne(c => c.SubscriptionPackage) 
                .WithMany(sp => sp.Companies) 
                .HasForeignKey(c => c.SubscriptionPackageId);

            // Configuring One-to-Many relationship for Company and Feedback
            modelBuilder.Entity<Company>()
                .HasMany(c => c.FeedBacks)
                .WithOne(f => f.Company)
                .HasForeignKey(f => f.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.PromotionFees)
                .WithOne(p => p.Event)
                .HasForeignKey(p => p.EventId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuring One-to-Many relationship for Company and PromotionFee
            modelBuilder.Entity<Company>()
                .HasMany(c => c.PromotionFees)
                .WithOne(p => p.Company)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Promotion>()
                .HasMany(p => p.PromotionFees)
                .WithOne(pf => pf.Promotion)
                .HasForeignKey(pf => pf.PromotionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
                .Property(b => b.Price)
                .HasPrecision(18, 2);
        }
    }
}