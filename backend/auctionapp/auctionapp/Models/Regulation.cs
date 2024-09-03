using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Regulation
{
    public string Regulationid { get; set; } = null!;

    public string? Transactionid { get; set; }

    public string? Adminid { get; set; }

    public DateTime? Regulationtime { get; set; }

    public string? Content { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Transaction? Transaction { get; set; }
}
