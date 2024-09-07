using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace auctionapp.Models;

public partial class AuctionDbContext : DbContext
{
    public AuctionDbContext()
    {
    }

    public AuctionDbContext(DbContextOptions<AuctionDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Auction> Auctions { get; set; }

    public virtual DbSet<Bidrecord> Bidrecords { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Feedbackpublish> Feedbackpublishes { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    public virtual DbSet<Regulation> Regulations { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseOracle("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("AUCTION_USER")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Adminid).HasName("SYS_C008265");

            entity.ToTable("ADMINS");

            entity.Property(e => e.Adminid)
                .HasColumnType("NUMBER")
                .HasColumnName("ADMINID");
            entity.Property(e => e.Permissionlevel)
                .HasColumnType("NUMBER")
                .HasColumnName("PERMISSIONLEVEL");

            entity.HasMany(d => d.Users).WithMany(p => p.Admins)
                .UsingEntity<Dictionary<string, object>>(
                    "Adminmanagement",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("Userid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C008295"),
                    l => l.HasOne<Admin>().WithMany()
                        .HasForeignKey("Adminid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C008294"),
                    j =>
                    {
                        j.HasKey("Adminid", "Userid").HasName("SYS_C008293");
                        j.ToTable("ADMINMANAGEMENT");
                        j.IndexerProperty<decimal>("Adminid")
                            .HasColumnType("NUMBER")
                            .HasColumnName("ADMINID");
                        j.IndexerProperty<decimal>("Userid")
                            .HasColumnType("NUMBER")
                            .HasColumnName("USERID");
                    });
        });

        modelBuilder.Entity<Auction>(entity =>
        {
            entity.HasKey(e => e.Auctionid).HasName("SYS_C008267");

            entity.ToTable("AUCTION");

            entity.Property(e => e.Auctionid)
                .HasColumnType("NUMBER")
                .HasColumnName("AUCTIONID");
            entity.Property(e => e.Currenthighestbid)
                .HasColumnType("NUMBER(20,2)")
                .HasColumnName("CURRENTHIGHESTBID");
            entity.Property(e => e.Currenthighestbiduserid)
                .HasColumnType("NUMBER")
                .HasColumnName("CURRENTHIGHESTBIDUSERID");
            entity.Property(e => e.Endtime)
                .HasColumnType("DATE")
                .HasColumnName("ENDTIME");
            entity.Property(e => e.Itemid)
                .HasColumnType("NUMBER")
                .HasColumnName("ITEMID");
            entity.Property(e => e.Starttime)
                .HasColumnType("DATE")
                .HasColumnName("STARTTIME");

            entity.HasOne(d => d.Currenthighestbiduser).WithMany(p => p.Auctions)
                .HasForeignKey(d => d.Currenthighestbiduserid)
                .HasConstraintName("SYS_C008269");

            entity.HasOne(d => d.Item).WithMany(p => p.Auctions)
                .HasForeignKey(d => d.Itemid)
                .HasConstraintName("SYS_C008268");
        });

        modelBuilder.Entity<Bidrecord>(entity =>
        {
            entity.HasKey(e => e.Bidid).HasName("SYS_C008270");

            entity.ToTable("BIDRECORD");

            entity.Property(e => e.Bidid)
                .HasColumnType("NUMBER")
                .HasColumnName("BIDID");
            entity.Property(e => e.Auctionid)
                .HasColumnType("NUMBER")
                .HasColumnName("AUCTIONID");
            entity.Property(e => e.Bidamount)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("BIDAMOUNT");
            entity.Property(e => e.Bidtime)
                .HasColumnType("DATE")
                .HasColumnName("BIDTIME");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Auction).WithMany(p => p.Bidrecords)
                .HasForeignKey(d => d.Auctionid)
                .HasConstraintName("SYS_C008271");

            entity.HasOne(d => d.User).WithMany(p => p.Bidrecords)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008272");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Chatid).HasName("SYS_C008304");

            entity.ToTable("CHAT");

            entity.Property(e => e.Chatid)
                .HasColumnType("NUMBER")
                .HasColumnName("CHATID");
            entity.Property(e => e.Chattime)
                .HasColumnType("DATE")
                .HasColumnName("CHATTIME");
            entity.Property(e => e.Content)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Chats)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008305");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Feedbackid).HasName("SYS_C008284");

            entity.ToTable("FEEDBACK");

            entity.Property(e => e.Feedbackid)
                .HasColumnType("NUMBER")
                .HasColumnName("FEEDBACKID");
            entity.Property(e => e.Content)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.Feedbacktime)
                .HasColumnType("DATE")
                .HasColumnName("FEEDBACKTIME");
            entity.Property(e => e.Rating)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("RATING");
        });

        modelBuilder.Entity<Feedbackpublish>(entity =>
        {
            entity.HasKey(e => new { e.Feedbackid, e.Userid, e.Itemid }).HasName("SYS_C008285");

            entity.ToTable("FEEDBACKPUBLISH");

            entity.Property(e => e.Feedbackid)
                .HasColumnType("NUMBER")
                .HasColumnName("FEEDBACKID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Itemid)
                .HasColumnType("NUMBER")
                .HasColumnName("ITEMID");

            entity.HasOne(d => d.Feedback).WithMany(p => p.Feedbackpublishes)
                .HasForeignKey(d => d.Feedbackid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008286");

            entity.HasOne(d => d.Item).WithMany(p => p.Feedbackpublishes)
                .HasForeignKey(d => d.Itemid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008288");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbackpublishes)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008287");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Itemid).HasName("SYS_C008266");

            entity.ToTable("ITEM");

            entity.Property(e => e.Itemid)
                .HasColumnType("NUMBER")
                .HasColumnName("ITEMID");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CATEGORY");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Image)
                .HasColumnType("BLOB")
                .HasColumnName("IMAGE");
            entity.Property(e => e.Itemname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ITEMNAME");
            entity.Property(e => e.Postdate)
                .HasColumnType("DATE")
                .HasColumnName("POSTDATE");
            entity.Property(e => e.Startingprice)
                .HasColumnType("NUMBER(20,2)")
                .HasColumnName("STARTINGPRICE");
            entity.Property(e => e.Valid)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("VALID");

            entity.HasMany(d => d.Users).WithMany(p => p.Items)
                .UsingEntity<Dictionary<string, object>>(
                    "Itempublish",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("Userid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C008298"),
                    l => l.HasOne<Item>().WithMany()
                        .HasForeignKey("Itemid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C008297"),
                    j =>
                    {
                        j.HasKey("Itemid", "Userid").HasName("SYS_C008296");
                        j.ToTable("ITEMPUBLISH");
                        j.IndexerProperty<decimal>("Itemid")
                            .HasColumnType("NUMBER")
                            .HasColumnName("ITEMID");
                        j.IndexerProperty<decimal>("Userid")
                            .HasColumnType("NUMBER")
                            .HasColumnName("USERID");
                    });

            entity.HasMany(d => d.UsersNavigation).WithMany(p => p.ItemsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "Shoppingcart",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("Userid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C008301"),
                    l => l.HasOne<Item>().WithMany()
                        .HasForeignKey("Itemid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C008300"),
                    j =>
                    {
                        j.HasKey("Itemid", "Userid").HasName("SYS_C008299");
                        j.ToTable("SHOPPINGCART");
                        j.IndexerProperty<decimal>("Itemid")
                            .HasColumnType("NUMBER")
                            .HasColumnName("ITEMID");
                        j.IndexerProperty<decimal>("Userid")
                            .HasColumnType("NUMBER")
                            .HasColumnName("USERID");
                    });
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Notificationid).HasName("SYS_C008261");

            entity.ToTable("NOTIFICATION");

            entity.Property(e => e.Notificationid)
                .HasColumnType("NUMBER")
                .HasColumnName("NOTIFICATIONID");
            entity.Property(e => e.Content)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.Senttime)
                .HasColumnType("DATE")
                .HasColumnName("SENTTIME");
            entity.Property(e => e.Status)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("STATUS");

            entity.HasMany(d => d.Users).WithMany(p => p.Notifications)
                .UsingEntity<Dictionary<string, object>>(
                    "Sendnotification",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("Userid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C008291"),
                    l => l.HasOne<Notification>().WithMany()
                        .HasForeignKey("Notificationid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C008290"),
                    j =>
                    {
                        j.HasKey("Notificationid", "Userid").HasName("SYS_C008289");
                        j.ToTable("SENDNOTIFICATION");
                        j.IndexerProperty<decimal>("Notificationid")
                            .HasColumnType("NUMBER")
                            .HasColumnName("NOTIFICATIONID");
                        j.IndexerProperty<decimal>("Userid")
                            .HasColumnType("NUMBER")
                            .HasColumnName("USERID");
                    });
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("SYS_C008280");

            entity.ToTable("PAYMENT");

            entity.Property(e => e.Paymentid)
                .HasColumnType("NUMBER")
                .HasColumnName("PAYMENTID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(20,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Paymentmethod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PAYMENTMETHOD");
            entity.Property(e => e.Paymenttime)
                .HasColumnType("DATE")
                .HasColumnName("PAYMENTTIME");
            entity.Property(e => e.Transactionid)
                .HasColumnType("NUMBER")
                .HasColumnName("TRANSACTIONID");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Transactionid)
                .HasConstraintName("SYS_C008281");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.Refundid).HasName("SYS_C008282");

            entity.ToTable("REFUND");

            entity.Property(e => e.Refundid)
                .HasColumnType("NUMBER")
                .HasColumnName("REFUNDID");
            entity.Property(e => e.Refundamount)
                .HasColumnType("NUMBER(20,2)")
                .HasColumnName("REFUNDAMOUNT");
            entity.Property(e => e.Refundreason)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("REFUNDREASON");
            entity.Property(e => e.Refundtime)
                .HasColumnType("DATE")
                .HasColumnName("REFUNDTIME");
            entity.Property(e => e.Transactionid)
                .HasColumnType("NUMBER")
                .HasColumnName("TRANSACTIONID");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.Transactionid)
                .HasConstraintName("SYS_C008283");
        });

        modelBuilder.Entity<Regulation>(entity =>
        {
            entity.HasKey(e => e.Regulationid).HasName("SYS_C008277");

            entity.ToTable("REGULATION");

            entity.Property(e => e.Regulationid)
                .HasColumnType("NUMBER")
                .HasColumnName("REGULATIONID");
            entity.Property(e => e.Adminid)
                .HasColumnType("NUMBER")
                .HasColumnName("ADMINID");
            entity.Property(e => e.Content)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.Regulationtime)
                .HasColumnType("DATE")
                .HasColumnName("REGULATIONTIME");
            entity.Property(e => e.Transactionid)
                .HasColumnType("NUMBER")
                .HasColumnName("TRANSACTIONID");

            entity.HasOne(d => d.Admin).WithMany(p => p.Regulations)
                .HasForeignKey(d => d.Adminid)
                .HasConstraintName("SYS_C008279");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Regulations)
                .HasForeignKey(d => d.Transactionid)
                .HasConstraintName("SYS_C008278");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Transactionid).HasName("SYS_C008273");

            entity.ToTable("TRANSACTION");

            entity.Property(e => e.Transactionid)
                .HasColumnType("NUMBER")
                .HasColumnName("TRANSACTIONID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(20,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Auctionid)
                .HasColumnType("NUMBER")
                .HasColumnName("AUCTIONID");
            entity.Property(e => e.Buyeruserid)
                .HasColumnType("NUMBER")
                .HasColumnName("BUYERUSERID");
            entity.Property(e => e.Selleruserid)
                .HasColumnType("NUMBER")
                .HasColumnName("SELLERUSERID");
            entity.Property(e => e.Status)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Transactiontime)
                .HasColumnType("DATE")
                .HasColumnName("TRANSACTIONTIME");

            entity.HasOne(d => d.Auction).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.Auctionid)
                .HasConstraintName("SYS_C008274");

            entity.HasOne(d => d.Buyeruser).WithMany(p => p.TransactionBuyerusers)
                .HasForeignKey(d => d.Buyeruserid)
                .HasConstraintName("SYS_C008275");

            entity.HasOne(d => d.Selleruser).WithMany(p => p.TransactionSellerusers)
                .HasForeignKey(d => d.Selleruserid)
                .HasConstraintName("SYS_C008276");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008264");

            entity.ToTable("USERS");

            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PHONE");
            entity.Property(e => e.Registrationdate)
                .HasColumnType("DATE")
                .HasColumnName("REGISTRATIONDATE");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
