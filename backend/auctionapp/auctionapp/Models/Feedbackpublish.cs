using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Feedbackpublish
{
    public string Feedbackid { get; set; } = null!;

    public string Userid { get; set; } = null!;

    public string Itemid { get; set; } = null!;

    public virtual Feedback Feedback { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
