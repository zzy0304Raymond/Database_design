using System;
using System.Collections.Generic;

namespace auctionapp.Models;

public partial class Transaction
{
    public decimal Transactionid { get; set; }

    public decimal? Auctionid { get; set; }

    public decimal? Buyeruserid { get; set; }

    public decimal? Selleruserid { get; set; }

    public DateTime? Transactiontime { get; set; }

    public decimal? Amount { get; set; }

    public string? Status { get; set; }

    public virtual Auction? Auction { get; set; }

    public virtual User? Buyeruser { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();

    public virtual ICollection<Regulation> Regulations { get; set; } = new List<Regulation>();

    public virtual User? Selleruser { get; set; }
}
