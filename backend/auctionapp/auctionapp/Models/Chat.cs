using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Chat
{
    public decimal Chatid { get; set; }

    public decimal? Userid { get; set; }

    public DateTime? Chattime { get; set; }

    public string? Content { get; set; }

    public virtual User? User { get; set; }
}
