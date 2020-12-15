using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Altairis.FutLabIS.Data {
    [Index(nameof(Date), IsUnique = true)]
    public class OpeningHoursChange {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Range(typeof(TimeSpan), "00:00:00", "23:59:59")]
        public TimeSpan OpeningTime { get; set; }

        [Range(typeof(TimeSpan), "00:00:00", "23:59:59")]
        public TimeSpan ClosingTime { get; set; }
    }
}
