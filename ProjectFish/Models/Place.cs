using System;
using System.Collections.Generic;

namespace ProjectFish.Models
{
    public partial class Place
    {
        public Place()
        {
            CompPlace = new HashSet<CompPlace>();
            Habitat = new HashSet<Habitat>();
        }

        public string Coordinates { get; set; }

        public virtual ICollection<CompPlace> CompPlace { get; set; }
        public virtual ICollection<Habitat> Habitat { get; set; }
    }
}
