using System;
using System.Collections.Generic;

namespace ProjectFish.Models
{
    public partial class Fish
    {
        public Fish()
        {
            Attraction = new HashSet<Attraction>();
            CompFish = new HashSet<CompFish>();
            Habitat = new HashSet<Habitat>();
        }

        public int FishId { get; set; }
        public string Species { get; set; }
        public string Waters { get; set; }
        public string WikiLink { get; set; }

        public virtual ICollection<Attraction> Attraction { get; set; }
        public virtual ICollection<CompFish> CompFish { get; set; }
        public virtual ICollection<Habitat> Habitat { get; set; }
    }
}
