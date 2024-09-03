﻿using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Feedback
{
    public string Feedbackid { get; set; } = null!;

    public string? Content { get; set; }

    public string? Rating { get; set; }

    public DateTime? Feedbacktime { get; set; }

    public virtual ICollection<Feedbackpublish> Feedbackpublishes { get; set; } = new List<Feedbackpublish>();
}
