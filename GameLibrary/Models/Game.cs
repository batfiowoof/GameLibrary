using System.ComponentModel.DataAnnotations;

namespace GameLibrary.Models
{
    public class Game
    {
        [Key]

        public int ID { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public string Publisher { get; set; }
        [Required]
        public string Developer { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        [MaxLength(50)]
        public string Genre { get; set; }
    }
}
