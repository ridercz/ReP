using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.FutLabIS.Data {
    public class Resource {

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required, Range(0, 1440)]
        public int MaximumReservationTime { get; set; }

        public bool Enabled { get; set; } = true;

    }
}
