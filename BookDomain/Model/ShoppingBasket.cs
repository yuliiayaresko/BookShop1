using BookDomain.Model;
using System.ComponentModel.DataAnnotations.Schema;

public partial class ShoppingBasket
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }  // Це ваш первинний ключ
    public string CustomerId { get; set; } = null!;

    

    // Зв'язок із клієнтом через ForeignKey
    public virtual Customer Customer { get; set; } = null!;  // Зв'язок з таблицею Customer

   
    public ICollection<ShoppingBasketBook> ShoppingBasketBooks { get; set; } = new List<ShoppingBasketBook>();
    public decimal TotalPrice => ShoppingBasketBooks?.Sum(i => i.Count * i.Book.Price) ?? 0;
}
