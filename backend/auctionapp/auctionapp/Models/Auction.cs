using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Auction
{
    public decimal Auctionid { get; set; }

    public decimal? Itemid { get; set; }

    public DateTime? Starttime { get; set; }

    public DateTime? Endtime { get; set; }

    public decimal? Currenthighestbid { get; set; }

    public decimal? Currenthighestbiduserid { get; set; }

    public virtual ICollection<Bidrecord> Bidrecords { get; set; } = new List<Bidrecord>();

    public virtual User? Currenthighestbiduser { get; set; }

    public virtual Item? Item { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
