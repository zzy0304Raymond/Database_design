using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Notification
{
    public decimal Notificationid { get; set; }

    public string? Content { get; set; }

    public DateTime? Senttime { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
