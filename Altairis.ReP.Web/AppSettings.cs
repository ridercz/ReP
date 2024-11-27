namespace Altairis.ReP.Web;

public class AppSettings {

    // Common configuration

    public string Database { get; set; } = "Sqlite";

    public FeaturesConfig Features { get; set; } = new();

    public class FeaturesConfig {
        public bool UseAttachments { get; set; } = true;
        public bool UseCalendarEntries { get; set; } = true;
        public bool UseJournal { get; set; } = true;
        public bool UseMemberDirectory { get; set; } = true;
        public bool UseNews { get; set; } = true;
        public bool UseOpeningHours { get; set; } = true;
    }

    // Design configuration

    public DesignConfig Design { get; set; } = new();

    public class DesignConfig {
        public string ApplicationName { get; set; } = "ReP";
        public string CalendarEntryBgColor { get; set; } = "#090";
        public string CalendarEntryFgColor { get; set; } = "#fff";
        public string HeaderImageUrl { get; set; } = "~/Content/Images/rep-logo.svg";
        public string StylesheetUrl { get; set; } = "~/Content/Styles/site.min.css";
        public string? AdditionalStylesheetUrl { get; set; }
    }

    // Proxy configuration

    public ProxyConfig Proxy { get; set; } = new();


    public class ProxyConfig {

        public bool AllowLocal { get; set; } = true;

        public bool AllowCloudlare { get; set; } = false;

        public string[] AdditionalAddresses { get; set; } = [];

    }

    // Authentication and security configuration

    public SecurityConfig Security { get; set; } = new();

    public class SecurityConfig {
        public int DefaultPasswordLength { get; set; } = 14;
        public int LoginCookieExpirationDays { get; set; } = 30;
        public int MinimumPasswordLength { get; set; } = 12;
    }

    // Mailing configuration

    public MailingConfig Mailing { get; set; } = new();

    public class MailingConfig {
        public string PickupFolder { get; set; } = @"C:\InetPub\MailRoot\pickup";
        public string SenderAddress { get; set; } = "example@example.com";
        public string SenderName { get; set; } = "ReP";
        public string? SendGridApiKey { get; set; }
        public string? SmtpHost { get; set; }
        public int SmtpPort { get; set; } = 25;
        public bool SmtpUseTls { get; set; } = false;
        public string? SmtpUsername { get; set; }
        public string? SmtpPassword { get; set; }

    }

    // Openings hours configuration

    public OpeningHoursConfig[] OpeningHours { get; set; } = [];

    public record OpeningHoursConfig(DayOfWeek DayOfWeek, TimeSpan ClosingTime, TimeSpan OpeningTime);

    // Attachment handling configuration

    public AttachmentsConfig Attachments { get; set; } = new();

    public class AttachmentsConfig {
        public long MaximumFileSize { get; set; } = 104_857_600; // 100 MB
    }

    // ICalendar export configuration

    public IcsExportConfig IcsExport { get; set; } = new();

    public class IcsExportConfig {
        public TimeSpan BackDays { get; set; } = TimeSpan.FromDays(90);
    }

    // Journal configuration

    public JournalConfig Journal { get; set; } = new();

    public class JournalConfig {
        public bool OnlyMastersCanWrite { get; set; } = false;
    }

}
