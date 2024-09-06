using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Bidrecord
{
    public decimal Bidid { get; set; }

    public decimal? Auctionid { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Bidamount { get; set; }

    public DateTime? Bidtime { get; set; }

    public virtual Auction? Auction { get; set; }

    public virtual User? User { get; set; }
}
