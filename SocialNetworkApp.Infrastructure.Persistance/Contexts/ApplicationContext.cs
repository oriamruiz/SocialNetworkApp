using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Core.Domain.Entities;


namespace SocialNetworkApp.Infrastructure.Persistance.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    
        public DbSet<User> Users { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Tables
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Friendship>().ToTable("Friendships");
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Reply>().ToTable("Replies");
            

            #endregion

            #region PrimaryKeys
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Friendship>().HasKey(f => f.Id);
            modelBuilder.Entity<Post>().HasKey(p => p.Id);
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Reply>().HasKey(r => r.Id);
            

            #endregion

            #region Relationships
            modelBuilder.Entity<User>()
                .HasMany<Friendship>(c => c.Friends)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);
           
            modelBuilder.Entity<Friendship>()
                .HasOne(a => a.Friend)
                .WithMany()
                .HasForeignKey(a => a.FriendId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
               .HasMany<Post>(c => c.Posts)
               .WithOne(a => a.User)
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
               .HasMany<Comment>(p => p.Comments)
               .WithOne(c => c.Post)
               .HasForeignKey(p => p.PostId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
               .HasMany<Reply>(c => c.Replies)
               .WithOne(r => r.Comment)
               .HasForeignKey(c => c.CommentId)
               .OnDelete(DeleteBehavior.NoAction);


            #endregion

            #region Property Confirguration

            #region User
            modelBuilder.Entity<User>()
                .Property(s => s.Name)
                .HasMaxLength(150);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();


            #endregion

            #region Post
            modelBuilder.Entity<Post>()
                .Property(p => p.Text)
                .HasMaxLength(int.MaxValue);
            #endregion

            #region Reply
            modelBuilder.Entity<Reply>()
                .Property(r => r.Text)
                .HasMaxLength(int.MaxValue);


            #endregion

            #region Comment
            modelBuilder.Entity<Comment>()
                .Property(c => c.Text)
                .HasMaxLength(int.MaxValue);
            #endregion


            #region Friendship
            modelBuilder.Entity<Friendship>()
                .HasIndex(f => new{ f.UserId, f.FriendId})
                .IsUnique();
            #endregion

            #endregion

        }
    }
}
