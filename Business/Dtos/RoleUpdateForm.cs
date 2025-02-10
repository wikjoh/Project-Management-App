﻿using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class RoleUpdateForm
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "Role name must be at least 2 characters long.")]
    [MaxLength(100, ErrorMessage = "Role name cannot exceed 100 characters.")]
    public string Role { get; set; } = null!;
}
