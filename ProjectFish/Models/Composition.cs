using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectFish.Models
{
    public partial class Composition
    {
        public Composition()
        {
            CompFish = new HashSet<CompFish>();
            CompLure = new HashSet<CompLure>();
            CompPlace = new HashSet<CompPlace>();
        }

        public int CompositionId { get; set; }
        public int AccountId { get; set; }

        [Required]
        [DisplayName("Rod")]
        public int RodId { get; set; }

        [Required]
        [DisplayName("Reel")]
        public int ReelId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Account Account { get; set; }
        public virtual Reel Reel { get; set; }
        public virtual Rod Rod { get; set; }
        public virtual ICollection<CompFish> CompFish { get; set; }
        public virtual ICollection<CompLure> CompLure { get; set; }
        public virtual ICollection<CompPlace> CompPlace { get; set; }
    }
}
