using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PogGames.Model
{
    public partial class Character
    {
        public int CharId { get; set; }
        public int GameId { get; set; }
        [Required]
        [StringLength(255)]
        public string CharName { get; set; }
        [Required]
        [StringLength(255)]
        public string CharCountry { get; set; }
        [Required]
        [StringLength(255)]
        public string CharDescription { get; set; }
        [StringLength(255)]
        public string CharGender { get; set; }
        [Required]
        [Column("CharImageURL")]
        [StringLength(255)]
        public string CharImageUrl { get; set; }

        [ForeignKey("GameId")]
        [InverseProperty("Character")]
        public virtual Game Game { get; set; }
    }
}
