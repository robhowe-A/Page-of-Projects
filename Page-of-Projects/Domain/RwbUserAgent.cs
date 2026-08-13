// --Copyright (c) 2026 Robert A. Howell

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectsPage.Domain;

[Table("user_agent")]
[Index(nameof(UserAgentFull), IsUnique = true)]
public sealed class RwbUserAgent
{
    [Key]
    [Column("AgentId", TypeName = "int")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AgentId { get; set; }

    [Required]
    [Column("UserAgentFull", TypeName = "varchar(255)")]
    [StringLength(255)]
    public string UserAgentFull { get; set; } = string.Empty;

    [Column("Count", TypeName = "int unsigned")]
    public uint? Count { get; set; }

    [Column("CreatedDate", TypeName = "date")]
    public DateTime? CreatedDate { get; set; }

    [Column("Browser", TypeName = "varchar(255)")]
    [StringLength(255)]
    public string? Browser { get; set; }

    [Column("BrowserVersion", TypeName = "varchar(255)")]
    [StringLength(255)]
    public string? BrowserVersion { get; set; }
};
