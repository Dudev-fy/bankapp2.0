using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using System;

namespace Bank
{
    public class ApplicationDbContext : DbContext
    {   
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Statement> Statements {get; set;}
        public DbSet<User> Users {get; set;}
        public DbSet<Account> Accounts {get; set;}
        public DbSet<Salt> Salts {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;port=3306;database=bank_system;uid=root;password=1234", new MySqlServerVersion(new Version(8, 0 , 31)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<Statement>()
                .ToTable("STATEMENT")
                .HasKey(s => s.IDSTATEMENT);
            
            modelBuilder.Entity<Statement>()
                .HasOne(s => s.UserSourceName)  // Statement has one associated User
                .WithMany(u => u.STATEMENTS)    // User has many associated Statements
                .HasForeignKey(s => s.ID_SOURCE)   // Define foreign key property
                .IsRequired(); //every statement must have an associated user

            modelBuilder.Entity<Statement>()
                .HasOne(s => s.UserDestinyName)
                .WithMany(u => u.STATEMENTD)
                .HasForeignKey(s => s.ID_DESTINY)
                .IsRequired();

            modelBuilder.Entity<User>()
                .ToTable("USER")
                .HasKey(u => u.IDUSER);

            modelBuilder.Entity<Account>()
                .ToTable("ACCOUNT")
                .HasKey(a => a.IDACCOUNT);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.SALT)    // Account has one associated Salt
                .WithOne(salt => salt.ACCOUNT)  // Salt has one associated Account (inverse navigation)
                .HasForeignKey<Salt>(salt => salt.ID_ACCOUNT) // Define the foreign key property
                .IsRequired(); // every account must have an associated salt and vice-versa  

            modelBuilder.Entity<Salt>()
                .ToTable("SALT")
                .HasKey(sa => sa.IDSALT);
        }
    }
}
