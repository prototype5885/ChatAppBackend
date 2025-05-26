using System.ComponentModel.DataAnnotations;

namespace ChatAppBackend.Models;

public class LoginViewModel
{
    [Required] [EmailAddress] public required string Email { get; init; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(4)]
    [MaxLength(64)]
    public required string Password { get; init; }
}