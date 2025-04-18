using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookDomain.Model;


public partial class Order
{
    
    public int OrderId { get; set; } 
    public string? CustomerEmail { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? DeliveryAddress { get; set; }

    public string? OrderStatus { get; set; }

    public int? ShoppingBasketId { get; set; }
    public string? FullName { get; set; }
    public string? DeliveryCity { get; set; }       // Місто
    public string? DeliveryStreet { get; set; }     // Вулиця
    public string? DeliveryHouseNumber { get; set; } // Номер будинку
    public string? DeliveryPostalCode { get; set; } // Поштовий індекс
    public string? DeliveryInstructions { get; set; } // Додаткові інструкції (необов’язково)


    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();


    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
