using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookDomain.Model;

public partial class Category:Entity
{

    [Display(Name = "Жанр")]
    public string CategoryName { get; set; } = null!;
    [Display(Name = "Опис жанру")]
    public string Description { get; set; } = null!;
    
    public int? BookId { get; set; }
    
    public virtual Book? Book { get; set; }
    

}
