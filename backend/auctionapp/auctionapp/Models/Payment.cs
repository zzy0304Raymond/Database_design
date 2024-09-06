using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Payment
{
    public decimal Paymentid { get; set; }

    public decimal? Transactionid { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? Paymenttime { get; set; }

    public string? Paymentmethod { get; set; }

    public virtual Transaction? Transaction { get; set; }
}
