using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.FutLabIS.Data {
    public class NewsMessage {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

    }
}
