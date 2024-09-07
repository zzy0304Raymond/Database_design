using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class User
{
    public decimal Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime? Registrationdate { get; set; }

    public virtual ICollection<Auction> Auctions { get; set; } = new List<Auction>();

    public virtual ICollection<Bidrecord> Bidrecords { get; set; } = new List<Bidrecord>();

    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public virtual ICollection<Feedbackpublish> Feedbackpublishes { get; set; } = new List<Feedbackpublish>();

    public virtual ICollection<Transaction> TransactionBuyerusers { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionSellerusers { get; set; } = new List<Transaction>();

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual ICollection<Item> ItemsNavigation { get; set; } = new List<Item>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
