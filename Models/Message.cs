using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackend.Models;

[PrimaryKey(nameof(Id))]
public class Message
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    public required long Id { get; init; }

    [Required] public required long UserId { get; init; }
    [Required] public required long ChannelId { get; init; }
    [Required] [MaxLength(8192)] public required string Msg { get; init; }
}

public class AddMessage
{
    [Required] [MaxLength(8192)] public required string Msg { get; init; }
    [Required] public required long ChannelId { get; init; }
}