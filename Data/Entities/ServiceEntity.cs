﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;

namespace Data.Entities;

[Index(nameof(Name), IsUnique = true)]
public class ServiceEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; } = null!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public int UnitId { get; set; }



    public ServiceUnitEntity Unit { get; set; } = null!;
    public ICollection<ProjectEntity> Projects { get; set; } = [];
}
