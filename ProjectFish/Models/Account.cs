using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectFish.Models
{
    public partial class Account
    {
        public Account()
        {
            Composition = new HashSet<Composition>();
        }

        public int AccountId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual ICollection<Composition> Composition { get; set; }
    }
}
