using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Bidrecord
{
    public string Bidid { get; set; } = null!;

    public string? Auctionid { get; set; }

    public string? Userid { get; set; }

    public decimal? Bidamount { get; set; }

    public DateTime? Bidtime { get; set; }

    public string? Itemid { get; set; }

    public virtual Auction? Auction { get; set; }

    public virtual Item? Item { get; set; }

    public virtual User? User { get; set; }
}
