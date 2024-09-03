using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Item
{
    public string Itemid { get; set; } = null!;

    public string? Itemname { get; set; }

    public string? Description { get; set; }

    public decimal? Startingprice { get; set; }

    public DateTime? Postdate { get; set; }

    public byte[]? Image { get; set; }

    public virtual ICollection<Auction> Auctions { get; set; } = new List<Auction>();

    public virtual ICollection<Feedbackpublish> Feedbackpublishes { get; set; } = new List<Feedbackpublish>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
