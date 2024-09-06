using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Admin
{
    public decimal Adminid { get; set; }

    public decimal? Permissionlevel { get; set; }

    public virtual ICollection<Regulation> Regulations { get; set; } = new List<Regulation>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
