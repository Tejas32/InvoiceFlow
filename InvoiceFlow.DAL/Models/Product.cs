using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InvoiceFlow.DAL.Models;

public partial class Product
{
    [Key]
    public int ProductId { get; set; }

    public int? UserId { get; set; }

    [StringLength(150)]
    public string? Name { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Price { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    public bool? IsActive { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    [ForeignKey("UserId")]
    [InverseProperty("Products")]
    public virtual User? User { get; set; }
}
