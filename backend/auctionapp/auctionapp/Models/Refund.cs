using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Refund
{
    public decimal Refundid { get; set; }

    public decimal? Transactionid { get; set; }

    public decimal? Refundamount { get; set; }

    public DateTime? Refundtime { get; set; }

    public string? Refundreason { get; set; }

    public virtual Transaction? Transaction { get; set; }
}
