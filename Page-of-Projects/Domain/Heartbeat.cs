// --Copyright (c) 2026 Robert A. Howell

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectsPage.Domain;

public class HeartbeatData
{
    public long Id { get; set; } = default;
    public DateTime Timestamp { get; set; }
    public string Agent { get; set; } = default!;
    public string Project { get; set; } = default!;
    public string Domain { get; set; } = default!;
    public string RelativeUrl { get; set; } = default!;
    public string ResponseCode { get; set; } = default!;
}

[Table("Heartbeat")]
internal sealed class Heartbeat
{
    [Column("id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }

    [Column("Record_Timestamp")]
    public DateTime RecordTimestamp { get; init; }

    [Column("Agent")]
    [MinLength(1)]
    [MaxLength(255)]
    public required string Agent { get; init; }

    [Column("Project_Name")]
    [MinLength(1)]
    [MaxLength(255)]
    public required string ProjectName { get; init; }

    [Column("Domain")]
    [MinLength(1)]
    [MaxLength(255)]
    public required string Domain { get; init; }

    [Column("Url_Rel")]
    [MinLength(1)]
    [MaxLength(512)]
    public required string UrlRel { get; init; }

    [Column("Response_Status")]
    [MinLength(1)]
    [MaxLength(32)]
    public required string ResponseStatus { get; init; }
};
