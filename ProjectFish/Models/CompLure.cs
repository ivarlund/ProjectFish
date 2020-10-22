using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectFish.Models
{
    public partial class CompLure
    {
        [Required]
        [DisplayName("Composition")]
        public int CompositionId { get; set; }

        [Required]
        [DisplayName("Lure")]
        public int LureId { get; set; }
        public int CompLureId { get; set; }

        public virtual Composition Composition { get; set; }
        public virtual Lure Lure { get; set; }
    }
}
