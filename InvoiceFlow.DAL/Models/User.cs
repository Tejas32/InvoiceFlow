using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InvoiceFlow.DAL.Models;

[Index("Email", Name = "UQ__Users__A9D105349AD4436F", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(150)]
    public string? Email { get; set; }

    [StringLength(200)]
    public string? Password { get; set; }

    [StringLength(150)]
    public string? BusinessName { get; set; }

    [StringLength(255)]
    public string? BusinessAddress { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(100)]
    public string? Industry { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    public bool? IsDeleted { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    [InverseProperty("User")]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    [InverseProperty("User")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
