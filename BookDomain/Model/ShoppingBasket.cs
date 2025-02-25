using BookDomain.Model;
using System.ComponentModel.DataAnnotations.Schema;

public partial class ShoppingBasket

{
    
    public int Id { get; set; }
    // Email клієнта
    public string CustomerEmail { get; set; } = null!;


    // Зв'язок із клієнтом через ForeignKey
    [ForeignKey("CustomerEmail")]
    public virtual Customer CustomerEmailNavigation { get; set; } = null!;

    // Зв'язок з книгами в кошику
    public ICollection<ShoppingBasketBook> ShoppingBasketBooks { get; set; } = new List<ShoppingBasketBook>();
}
