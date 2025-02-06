﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class UserEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; } = null!;

    [Column(TypeName = "nvarchar(200)")]
    public string DisplayName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [Column(TypeName = "varchar(200)")]
    public string EmailAddress { get; set; } = null!;

    [Required]
    public bool AdminRole { get; set; }

    [Required]
    public bool ProjectManagerRole { get; set; }




    public ICollection<ProjectEntity> Projects { get; set; } = [];
}
