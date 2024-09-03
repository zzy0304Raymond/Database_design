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
        => optionsBuilder.UseOracle(
            "Name=ConnectionStrings:DefaultConnection",
            oracleOptions => oracleOptions.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion21)
            );


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("AUCTION_USER")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Adminid).HasName("SYS_C008224");

            entity.ToTable("ADMINS");

            entity.Property(e => e.Adminid)
                .HasMaxLength(20)
                .IsUnicode(false)
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
                        .HasConstraintName("SYS_C008255"),
                    l => l.HasOne<Admin>().WithMany()
                        .HasForeignKey("Adminid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C008254"),
                    j =>
                    {
                        j.HasKey("Adminid", "Userid").HasName("SYS_C008253");
                        j.ToTable("ADMINMANAGEMENT");
                        j.IndexerProperty<string>("Adminid")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("ADMINID");
                        j.IndexerProperty<string>("Userid")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("USERID");
                    });
        });

        modelBuilder.Entity<Auction>(entity =>
        {
            entity.HasKey(e => e.Auctionid).HasName("SYS_C008226");

            entity.ToTable("AUCTION");

            entity.Property(e => e.Auctionid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("AUCTIONID");
            entity.Property(e => e.Currenthighestbid)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("CURRENTHIGHESTBID");
            entity.Property(e => e.Currenthighestbiduserid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CURRENTHIGHESTBIDUSERID");
            entity.Property(e => e.Endtime)
                .HasPrecision(6)
                .HasColumnName("ENDTIME");
            entity.Property(e => e.Itemid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ITEMID");
            entity.Property(e => e.Starttime)
                .HasPrecision(6)
                .HasColumnName("STARTTIME");

            entity.HasOne(d => d.Currenthighestbiduser).WithMany(p => p.Auctions)
                .HasForeignKey(d => d.Currenthighestbiduserid)
                .HasConstraintName("SYS_C008228");

            entity.HasOne(d => d.Item).WithMany(p => p.Auctions)
                .HasForeignKey(d => d.Itemid)
                .HasConstraintName("SYS_C008227");
        });

        modelBuilder.Entity<Bidrecord>(entity =>
        {
            entity.HasKey(e => e.Bidid).HasName("SYS_C008229");

            entity.ToTable("BIDRECORD");

            entity.Property(e => e.Bidid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("BIDID");
            entity.Property(e => e.Auctionid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("AUCTIONID");
            entity.Property(e => e.Bidamount)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("BIDAMOUNT");
            entity.Property(e => e.Bidtime)
                .HasPrecision(6)
                .HasColumnName("BIDTIME");
            entity.Property(e => e.Userid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USERID");

            entity.HasOne(d => d.Auction).WithMany(p => p.Bidrecords)
                .HasForeignKey(d => d.Auctionid)
                .HasConstraintName("SYS_C008230");

            entity.HasOne(d => d.User).WithMany(p => p.Bidrecords)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008231");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Feedbackid).HasName("SYS_C008243");

            entity.ToTable("FEEDBACK");

            entity.Property(e => e.Feedbackid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("FEEDBACKID");
            entity.Property(e => e.Content)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.Feedbacktime)
                .HasPrecision(6)
                .HasColumnName("FEEDBACKTIME");
            entity.Property(e => e.Rating)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("RATING");
        });

        modelBuilder.Entity<Feedbackpublish>(entity =>
        {
            entity.HasKey(e => new { e.Feedbackid, e.Userid, e.Itemid }).HasName("SYS_C008244");

            entity.ToTable("FEEDBACKPUBLISH");

            entity.Property(e => e.Feedbackid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("FEEDBACKID");
            entity.Property(e => e.Userid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USERID");
            entity.Property(e => e.Itemid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ITEMID");

            entity.HasOne(d => d.Feedback).WithMany(p => p.Feedbackpublishes)
                .HasForeignKey(d => d.Feedbackid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008245");

            entity.HasOne(d => d.Item).WithMany(p => p.Feedbackpublishes)
                .HasForeignKey(d => d.Itemid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008247");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbackpublishes)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008246");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Itemid).HasName("SYS_C008225");

            entity.ToTable("ITEM");

            entity.Property(e => e.Itemid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ITEMID");
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
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("STARTINGPRICE");

            entity.HasMany(d => d.Users).WithMany(p => p.Items)
                .UsingEntity<Dictionary<string, object>>(
                    "Itempublish",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("Userid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C008259"),
                    l => l.HasOne<Item>().WithMany()
                        .HasForeignKey("Itemid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C008258"),
                    j =>
                    {
                        j.HasKey("Itemid", "Userid").HasName("SYS_C008257");
                        j.ToTable("ITEMPUBLISH");
                        j.IndexerProperty<string>("Itemid")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("ITEMID");
                        j.IndexerProperty<string>("Userid")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("USERID");
                    });
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Notificationid).HasName("SYS_C008248");

            entity.ToTable("NOTIFICATION");

            entity.Property(e => e.Notificationid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NOTIFICATIONID");
            entity.Property(e => e.Content)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.Senttime)
                .HasPrecision(6)
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
                        .HasConstraintName("SYS_C008251"),
                    l => l.HasOne<Notification>().WithMany()
                        .HasForeignKey("Notificationid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C008250"),
                    j =>
                    {
                        j.HasKey("Notificationid", "Userid").HasName("SYS_C008249");
                        j.ToTable("SENDNOTIFICATION");
                        j.IndexerProperty<string>("Notificationid")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("NOTIFICATIONID");
                        j.IndexerProperty<string>("Userid")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("USERID");
                    });
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("SYS_C008239");

            entity.ToTable("PAYMENT");

            entity.Property(e => e.Paymentid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PAYMENTID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Paymentmethod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PAYMENTMETHOD");
            entity.Property(e => e.Paymenttime)
                .HasPrecision(6)
                .HasColumnName("PAYMENTTIME");
            entity.Property(e => e.Transactionid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TRANSACTIONID");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Transactionid)
                .HasConstraintName("SYS_C008240");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.Refundid).HasName("SYS_C008241");

            entity.ToTable("REFUND");

            entity.Property(e => e.Refundid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("REFUNDID");
            entity.Property(e => e.Refundamount)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("REFUNDAMOUNT");
            entity.Property(e => e.Refundreason)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("REFUNDREASON");
            entity.Property(e => e.Refundtime)
                .HasPrecision(6)
                .HasColumnName("REFUNDTIME");
            entity.Property(e => e.Transactionid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TRANSACTIONID");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.Transactionid)
                .HasConstraintName("SYS_C008242");
        });

        modelBuilder.Entity<Regulation>(entity =>
        {
            entity.HasKey(e => e.Regulationid).HasName("SYS_C008236");

            entity.ToTable("REGULATION");

            entity.Property(e => e.Regulationid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("REGULATIONID");
            entity.Property(e => e.Adminid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ADMINID");
            entity.Property(e => e.Content)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.Regulationtime)
                .HasPrecision(6)
                .HasColumnName("REGULATIONTIME");
            entity.Property(e => e.Transactionid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TRANSACTIONID");

            entity.HasOne(d => d.Admin).WithMany(p => p.Regulations)
                .HasForeignKey(d => d.Adminid)
                .HasConstraintName("SYS_C008238");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Regulations)
                .HasForeignKey(d => d.Transactionid)
                .HasConstraintName("SYS_C008237");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Transactionid).HasName("SYS_C008232");

            entity.ToTable("TRANSACTION");

            entity.Property(e => e.Transactionid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TRANSACTIONID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Auctionid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("AUCTIONID");
            entity.Property(e => e.Buyeruserid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("BUYERUSERID");
            entity.Property(e => e.Selleruserid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SELLERUSERID");
            entity.Property(e => e.Status)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Transactiontime)
                .HasPrecision(6)
                .HasColumnName("TRANSACTIONTIME");

            entity.HasOne(d => d.Auction).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.Auctionid)
                .HasConstraintName("SYS_C008233");

            entity.HasOne(d => d.Buyeruser).WithMany(p => p.TransactionBuyerusers)
                .HasForeignKey(d => d.Buyeruserid)
                .HasConstraintName("SYS_C008234");

            entity.HasOne(d => d.Selleruser).WithMany(p => p.TransactionSellerusers)
                .HasForeignKey(d => d.Selleruserid)
                .HasConstraintName("SYS_C008235");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008223");

            entity.ToTable("USERS");

            entity.Property(e => e.Userid)
                .HasMaxLength(20)
                .IsUnicode(false)
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
                .HasPrecision(6)
                .HasColumnName("REGISTRATIONDATE");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
            entity.Property(e => e.Usertype)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("USERTYPE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
