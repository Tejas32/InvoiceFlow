using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InvoiceFlow.DAL.Models;

public partial class Invoice
{
    [Key]
    public int InvoiceId { get; set; }

    public int? UserId { get; set; }

    public int? ClientId { get; set; }

    [StringLength(50)]
    public string? InvoiceNumber { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? TotalAmount { get; set; }

    [StringLength(20)]
    public string? Status { get; set; }

    [StringLength(50)]
    public string? PaymentType { get; set; }

    [StringLength(255)]
    public string? PaymentLink { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DueDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    public bool? IsDeleted { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("Invoices")]
    public virtual Client? Client { get; set; }

    [InverseProperty("Invoice")]
    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    [ForeignKey("UserId")]
    [InverseProperty("Invoices")]
    public virtual User? User { get; set; }
}
