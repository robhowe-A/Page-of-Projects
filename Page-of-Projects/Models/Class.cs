using System.ComponentModel.DataAnnotations;

namespace ProjectsPage.Models
{
    public class Class
    {
        [Key]
        public int id { get; set; }
        public string site { get; set; }

        public string document { get; set; }
    }
}
