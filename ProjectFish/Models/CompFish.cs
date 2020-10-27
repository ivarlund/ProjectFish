using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectFish.Models
{
    public partial class CompFish
    {
        [Required]
        [DisplayName("Composition")]
        public int CompositionId { get; set; }

        [Required]
        [DisplayName("Fish")]
        public int FishId { get; set; }
        public int CompFishId { get; set; }

        public virtual Composition Composition { get; set; }
        public virtual Fish Fish { get; set; }
    }
}
