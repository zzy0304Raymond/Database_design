using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Notification
{
    public string Notificationid { get; set; } = null!;

    public string? Content { get; set; }

    public DateTime? Senttime { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
