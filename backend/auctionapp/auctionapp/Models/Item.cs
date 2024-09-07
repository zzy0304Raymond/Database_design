using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Item
{
    public decimal Itemid { get; set; }

    public string? Itemname { get; set; }

    public string? Description { get; set; }

    public decimal? Startingprice { get; set; }

    public DateTime? Postdate { get; set; }

    public byte[]? Image { get; set; }

    public string? Category { get; set; }

    public bool? Valid { get; set; }

    public virtual ICollection<Auction> Auctions { get; set; } = new List<Auction>();

    public virtual ICollection<Feedbackpublish> Feedbackpublishes { get; set; } = new List<Feedbackpublish>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<User> UsersNavigation { get; set; } = new List<User>();
}
