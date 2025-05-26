using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackend.Models;

[PrimaryKey(nameof(Id))]
public class Server
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required long Id { get; init; }

    [Required] public required long OwnerId { get; init; }

    [Required][MaxLength(128)] public required string Name { get; init; }

    [Required][MaxLength(128)] public required string Picture { get; init; }
}

public class AddServer
{
    [Required][MaxLength(64)] public required string Name { get; init; }
}