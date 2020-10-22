using System;
using System.Collections.Generic;

namespace ProjectFish.Models
{
    public partial class Attraction
    {
        public int FishId { get; set; }
        public int LureId { get; set; }
        public int AttractionId { get; set; }

        public virtual Fish Fish { get; set; }
        public virtual Lure Lure { get; set; }
    }
}
