// --Copyright (c) 2026 Robert A. Howell

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectsPage.Domain;

[Table("anonymous_user")]
[Index(nameof(UserAgentId))]
[Index(nameof(IpAddrHash), IsUnique = true)]
public sealed class RwbAnonymousUser
{
    [Key]
    [Column("AnonymousUserId", TypeName = "int")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AnonymousUserId { get; set; }

    [Required]
    [Column("UserAgentId", TypeName = "int")]
    public int UserAgentId { get; set; }

    [Required]
    [Column("IpAddrHash", TypeName = "varchar(255)")]
    [StringLength(255)]
    public string IpAddrHash { get; set; } = string.Empty;

    [Column("CreatedDate", TypeName = "date")]
    public DateTime? CreatedDate { get; set; }

    [Column("LastAccessed", TypeName = "datetime")]
    public DateTime? LastAccessed { get; set; }
};
