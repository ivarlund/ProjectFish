using System;
using System.Collections.Generic;

namespace ProjectFish.Models
{
    public partial class Rod
    {
        public Rod()
        {
            Composition = new HashSet<Composition>();
        }

        public int RodId { get; set; }
        public string Brand { get; set; }
        public int Length { get; set; }
        public int CastWeight { get; set; }

        public virtual ICollection<Composition> Composition { get; set; }
    }
}
