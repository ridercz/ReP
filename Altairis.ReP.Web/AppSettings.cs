namespace Altairis.ReP.Web;

public class AppSettings {

    public FeaturesConfig Features { get; set; } = new FeaturesConfig();

    public class FeaturesConfig {
        public bool UseOpeningHours { get; set; } = true;
        public bool UseNews { get; set; } = true;

    }

    public DesignConfig Design { get; set; } = new DesignConfig();

    public class DesignConfig {
        public string ApplicationName { get; set; } = "ReP";
        public string HeaderImageUrl { get; set; } = "~/Content/Images/rep-logo.svg";
        public string StylesheetUrl { get; set; } = "~/Content/Styles/site.min.css";

    }

    public SecurityConfig Security { get; set; } = new SecurityConfig();

    public class SecurityConfig {
        public int MinimumPasswordLength { get; set; } = 12;
        public int DefaultPasswordLength { get; set; } = 14;
        public int LoginCookieExpirationDays { get; set; } = 30;
    }

    public MailingConfig Mailing { get; set; } = new MailingConfig();

    public class MailingConfig {
        public string PickupFolder { get; set; } = @"C:\InetPub\MailRoot\pickup";
        public string SendGridApiKey { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
    }

    public OpeningHoursConfig[] OpeningHours { get; set; }

    public class OpeningHoursConfig {

        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan OpeningTime { get; set; }

        public TimeSpan ClosingTime { get; set; }

    }

}
