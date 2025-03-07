﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookDomain.Model;


public partial class Order
{
    
    public int OrderId { get; set; } 
    public string? CustomerEmail { get; set; }

    public DateOnly? OrderDate { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? DeliveryAddress { get; set; }

    public string? OrderStatus { get; set; }

    public int? ShoppingBasketId { get; set; }

    public decimal? Price { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();


    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
