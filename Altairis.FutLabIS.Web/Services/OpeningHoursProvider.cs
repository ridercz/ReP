using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.Services.DateProvider;
using Microsoft.Extensions.Options;

namespace Altairis.FutLabIS.Web.Services {
    public class OpeningHoursProvider {
        private readonly IOptions<AppSettings> optionsAccessor;
        private readonly IDateProvider dateProvider;

        public OpeningHoursProvider(IOptions<AppSettings> optionsAccessor, IDateProvider dateProvider) {
            this.optionsAccessor = optionsAccessor ?? throw new ArgumentNullException(nameof(optionsAccessor));
            this.dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
        }

        public OpeningHoursInfo GetOpeningHours(int dayOffset) => this.GetOpeningHours(this.dateProvider.Today.AddDays(dayOffset));

        public IEnumerable<OpeningHoursInfo> GetOpeningHours(int dayOffsetFrom, int dayOffsetTo) => this.GetOpeningHours(this.dateProvider.Today.AddDays(dayOffsetFrom), this.dateProvider.Today.AddDays(dayOffsetTo));

        public OpeningHoursInfo GetOpeningHours(DateTime date) {
            return this.GetStandardOpeningHours(date);
        }

        public IEnumerable<OpeningHoursInfo> GetOpeningHours(DateTime dateFrom, DateTime dateTo) {
            var date = dateFrom;
            while (date <= dateTo) {
                yield return this.GetStandardOpeningHours(date);
                date = date.AddDays(1);
            }
        }

        private OpeningHoursInfo GetStandardOpeningHours(DateTime date) {
            var value = this.optionsAccessor.Value.OpeningHours.FirstOrDefault(x => x.DayOfWeek == date.DayOfWeek);
            return value == null
                ? new OpeningHoursInfo { Date = date }
                : new OpeningHoursInfo { Date = date, OpeningTime = value.OpeningTime, ClosingTime = value.ClosingTime, IsException = false };
        }

    }

    public class OpeningHoursInfo {

        public DateTime Date { get; set; }

        public bool IsOpen => this.ClosingTime.Subtract(this.OpeningTime) > TimeSpan.Zero;

        public bool IsException { get; set; }

        public TimeSpan OpeningTime { get; set; }

        public TimeSpan ClosingTime { get; set; }

        public override string ToString() => this.IsOpen ? $"{this.OpeningTime:hh\\:mm} - {this.ClosingTime:hh\\:mm}" : string.Empty;

    }
}
