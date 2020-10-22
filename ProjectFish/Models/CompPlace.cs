using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectFish.Models
{
    public partial class CompPlace
    {
        [Required]
        [DisplayName("Composition")]
        public int CompositionId { get; set; }

        [Required]
        public string Coordinates { get; set; }

        public int CompPlaceId { get; set; }

        public virtual Composition Composition { get; set; }
        public virtual Place Place { get; set; }
    }
}
