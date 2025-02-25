using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookDomain.Model;

public partial class Customer

{
    [Key]  // Позначаємо як первинний ключ
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Автоінкремент
    public int Id { get; set; }
    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Phone { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ShoppingBasket> ShoppingBaskets { get; set; } = null!;
}
