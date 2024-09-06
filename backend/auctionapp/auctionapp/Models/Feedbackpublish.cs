using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Feedbackpublish
{
    public decimal Feedbackid { get; set; }

    public decimal Userid { get; set; }

    public decimal Itemid { get; set; }

    public virtual Feedback Feedback { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
