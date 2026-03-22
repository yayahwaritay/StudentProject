using System.ComponentModel.DataAnnotations;

namespace StudentAPI.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Email { get; set; }

    [Required]
    public string Role { get; set; } = "User";
}
