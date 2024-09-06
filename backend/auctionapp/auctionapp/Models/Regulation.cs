using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Regulation
{
    public decimal Regulationid { get; set; }

    public decimal? Transactionid { get; set; }

    public decimal? Adminid { get; set; }

    public DateTime? Regulationtime { get; set; }

    public string? Content { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Transaction? Transaction { get; set; }
}
