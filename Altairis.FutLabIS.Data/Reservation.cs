using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.ValidationToolkit;

namespace Altairis.FutLabIS.Data {
    public class Reservation {

        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(ResourceId))]
        public Resource Resource { get; set; }

        [ForeignKey(nameof(Resource))]
        public int ResourceId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        public DateTime DateBegin { get; set; }

        [Required, GreaterThan(nameof(DateBegin))]
        public DateTime DateEnd { get; set; }

        public bool System { get; set; } = false;

        public string Comment { get; set; }

    }
}
