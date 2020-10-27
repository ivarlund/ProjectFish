using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFish.Models
{
    public class VMCompFishPlaceLure
    {

        public Composition Composition { get; set; }

        public IEnumerable<int> Fishes { get; set; }

        public IEnumerable<int> Lures { get; set; }

        public IEnumerable<string> Places { get; set; }

    }
}
