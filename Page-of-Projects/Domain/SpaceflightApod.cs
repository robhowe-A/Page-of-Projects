// --Copyright (c) 2026 Robert A. Howell

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectsPage.Domain;

[Table("APOD")]
public class SpaceflightApod
{
    [Key]
    [Column("id")]
    public uint Id { get; set; }

    [Column("apodTitle")]
    public string? ApodTitle { get; set; }

    [Column("date")]
    public string? Date { get; set; }

    [Column("hdurl")]
    public string? HdUrl { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("media")]
    public string? Media { get; set; }
}