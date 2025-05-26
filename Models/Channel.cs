using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackend.Models;

[PrimaryKey(nameof(Id))]
public class Channel
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required long Id { get; init; }

    [Required] public required long ServerId { get; init; }

    [Required][MaxLength(16)] public required string Name { get; init; }
}

public class AddChannel
{
    [Required][MaxLength(16)] public required string Name { get; init; }
    [Required] public required long ServerId { get; init; }
}