using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookDomain.Model;

public partial class Book: Entity
{
    [Display(Name = "Назва")]


    public string Name { get; set; } = null!;
    [Display(Name = "Автор")]


    public string AuthorName { get; set; } = null!;
    [Display(Name = "Жанр")]
    [Column(TypeName = "nvarchar(255)")]
    public string Genre { get; set; } = null!;
    [Display(Name = "Рік видання")]
    public int Year { get; set; }
    [Display(Name = "Опис")]
    public string Description { get; set; } = null!;
    [Display(Name = "Ціна")]
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    //public int CategoryId { get; set; }


    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ShoppingBasketBook> ShoppingBasketBooks { get; set; } = new List<ShoppingBasketBook>();

    public virtual ICollection<WarehouseBook> WarehouseBooks { get; set; } = new List<WarehouseBook>();
}
