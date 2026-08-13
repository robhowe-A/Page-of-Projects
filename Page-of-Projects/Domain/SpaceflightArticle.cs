// --Copyright (c) 2026 Robert A. Howell

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectsPage.Domain;

[Table("Article")]
public class SpaceflightArticle
{
    [Key]
    [DisplayName("Article Number")]
    [Column("articleNum")]
    public uint ArticleNum { get; set; }

    [Column("title")]
    public string? Title { get; set; }

    [Column("url")]
    public string? Url { get; set; }

    [DisplayName("Text URL")]
    [Column("textURL")]
    public string? TextUrl { get; set; }

    [Column("summary")]
    public string? Summary { get; set; }

    [Column("date")]
    public DateTime? Date { get; set; }

    [Column("featured")]
    public bool? Featured { get; set; }

    [Column("id")]
    public uint? Id { get; set; }

    [DisplayName("News Site")]
    [Column("newsSite")]
    public string? NewsSite { get; set; }
};
