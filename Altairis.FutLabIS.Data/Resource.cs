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

        [Required, MinLength(7), MaxLength(7), RegularExpression(@"^\#[0-9A-Fa-f]{6}$")]
        public string ForegroundColor { get; set; } = "#000000";

        [Required, MinLength(7), MaxLength(7), RegularExpression(@"^\#[0-9A-Fa-f]{6}$")]
        public string BackgroundColor { get; set; } = "#ffffff";

    }
}
