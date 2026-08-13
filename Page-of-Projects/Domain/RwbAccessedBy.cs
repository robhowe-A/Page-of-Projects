// --Copyright (c) 2026 Robert A. Howell

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectsPage.Domain;

[Table("accessed_by")]
[PrimaryKey(nameof(AnonymousUserId), nameof(UserAgentId))]
[Index(nameof(UserAgentId))]
public sealed class RwbAccessedBy
{
    [Required]
    [Column("AnonymousUserId", TypeName = "int")] public int AnonymousUserId { get; set; }

    [Required]
    [Column("UserAgentId", TypeName = "int")] public int UserAgentId { get; set; }

    [Required]
    [Column("LastAccessed", TypeName = "date")] public DateTime LastAccessed { get; set; }

    [Column("Site", TypeName = "varchar(512)")]
    [StringLength(512)] public string? Site { get; set; }

    [Column("Uri", TypeName = "varchar(512)")]
    [StringLength(512)] public string? Uri { get; set; }
};
