namespace Altairis.ReP.Web.Services;

public class RoundedTimeProvider : TimeProvider {

    // Private constructor to prevent direct instantiation
    private RoundedTimeProvider() { }

    // Factory methods for different precisions

    public static RoundedTimeProvider NativePrecision(string? localTimeZoneName = null) => new() {
        LocalTimeZoneInternal = localTimeZoneName == null ? TimeZoneInfo.Local : TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneName),
        RoundingFunction = x => x,
    };

    public static RoundedTimeProvider SecondPrecision(string? localTimeZoneName = null) => new() {
        LocalTimeZoneInternal = localTimeZoneName == null ? TimeZoneInfo.Local : TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneName),
        RoundingFunction = x => x.AddTicks(-x.Ticks % TimeSpan.TicksPerSecond),
    };

    public static RoundedTimeProvider MinutePrecision(string? localTimeZoneName = null) => new() {
        LocalTimeZoneInternal = localTimeZoneName == null ? TimeZoneInfo.Local : TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneName),
        RoundingFunction = x => x.AddTicks(-x.Ticks % TimeSpan.TicksPerMinute),
    };

    // Properties to hold the rounding function and the local time zone

    private TimeZoneInfo LocalTimeZoneInternal { get; init; } = null!;

    private Func<DateTimeOffset, DateTimeOffset> RoundingFunction { get; init; } = null!;

    // Override the GetUtcNow method to apply the rounding function

    public override DateTimeOffset GetUtcNow() => this.RoundingFunction(base.GetUtcNow());

    // Override the LocalTimeZone property to return the local time zone

    public override TimeZoneInfo LocalTimeZone => this.LocalTimeZoneInternal;

}