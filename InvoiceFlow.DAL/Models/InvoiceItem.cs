using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InvoiceFlow.DAL.Models;

public partial class InvoiceItem
{
    [Key]
    public int ItemId { get; set; }

    public int? InvoiceId { get; set; }

    public int? ProductId { get; set; }

    [StringLength(150)]
    public string? ItemName { get; set; }

    public int? Quantity { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(21, 2)")]
    public decimal? Total { get; set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("InvoiceItems")]
    public virtual Invoice? Invoice { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("InvoiceItems")]
    public virtual Product? Product { get; set; }
}
