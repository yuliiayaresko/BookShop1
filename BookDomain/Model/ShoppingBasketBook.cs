using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookDomain.Model
{
    public partial class ShoppingBasketBook

    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 

        public int Id { get; set; }
        public int ShoppingBasketId { get; set; }
        public int BookId { get; set; }  // Додаємо явний зовнішній ключ

        public int Count { get; set; }

     

        public virtual Book Book { get; set; } = null!;
        [ForeignKey("ShoppingBasketId")]
        public virtual ShoppingBasket ShoppingBasket { get; set; } = null!;
    }
}
