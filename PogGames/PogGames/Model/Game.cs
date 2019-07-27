using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PogGames.Model
{
    public partial class Game
    {
        public Game()
        {
            Character = new HashSet<Character>();
        }

        public int GameId { get; set; }
        [Required]
        [StringLength(255)]
        public string GameName { get; set; }
        [Required]
        [StringLength(255)]
        public string GameRelease { get; set; }
        [Required]
        [StringLength(255)]
        public string GameCompany { get; set; }
        [Required]
        [Column("CoverImageURL")]
        [StringLength(255)]
        public string CoverImageUrl { get; set; }
        [Required]
        [StringLength(255)]
        public string Genre { get; set; }
        [Required]
        [StringLength(2000)]
        public string GameSummary { get; set; }
        [Required]
        [StringLength(255)]
        public string Rating { get; set; }
        public int? RatingCount { get; set; }
        [Column("isFavourite")]
        public bool IsFavourite { get; set; }

        [InverseProperty("Game")]
        public virtual ICollection<Character> Character { get; set; }
    }
}
