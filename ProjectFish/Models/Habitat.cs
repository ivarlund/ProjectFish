using System;
using System.Collections.Generic;

namespace ProjectFish.Models
{
    public partial class Habitat
    {
        public int FishId { get; set; }
        public string Coordinates { get; set; }
        public int HabitatId { get; set; }

        public virtual Place CoordinatesNavigation { get; set; }
        public virtual Fish Fish { get; set; }
    }
}
