using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Payment
{
    public string Paymentid { get; set; } = null!;

    public string? Transactionid { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? Paymenttime { get; set; }

    public string? Paymentmethod { get; set; }

    public virtual Transaction? Transaction { get; set; }
}
