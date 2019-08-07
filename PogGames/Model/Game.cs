using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PogGames.Model
{
    public partial class Game
    {
        public Game()
        {
            Character = new HashSet<Character>();
        }

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
        [StringLength(255)]
        public string GameId { get; set; }

        [InverseProperty("Game")]
        public virtual ICollection<Character> Character { get; set; }
    }

    [DataContract]
    public class GameDTO
    {
        [DataMember]
        public string GameName { get; set; }

        [DataMember]
        public string GameRelease { get; set; }

        [DataMember]
        public string GameCompany { get; set; }

        [DataMember]
        public string CoverImageUrl { get; set; }

        [DataMember]
        public string Genre { get; set; }

        [DataMember]
        public string GameSummary { get; set; }

        [DataMember]
        public string Rating { get; set; }

        [DataMember]
        public int? RatingCount { get; set; }

        [DataMember]
        public bool IsFavourite { get; set; }

        [DataMember]
        public string GameId { get; set; }

    }
}
