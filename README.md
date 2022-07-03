# ReP - Rezervace Prostředků (Resource Reservation)

This project started as a specific resource reservation system for the [FutLab](https://www.futlab.cc) makerspace in Prague, with the following [design document](https://1drv.ms/w/s!Apo4M7bgM3zBz69X-y8zIZAgBQDN5w) (Czech only).

It was written mainly during [three live coding sessions](https://www.youtube.com/playlist?list=PLoOpAe_g1x4IxYK9A8aT0To60DF6IHTFl) and accompanied by [posts on my blog](https://www.altair.blog/2021/02/futlab).

Sadly, before the system could be deployed, the makerspace closed down, asi it could not survive the restriction imposed by the Czech government, supposedly to fight the Covid pandemic.

So I renamed the application, removed all the traces of FutLab from it and you may use it for your purposes.

## Features

ReP is a generic shared resource reservation system. 

**See this [introduction video](https://youtu.be/Y0NzSa7QJDE) (in Czech)**

* _Resource_ can be almost anything, like a specific machine, specific room or area, anything that is supposed to be used by a single person at single time and you need to coordinate the users somehow.
* _Users_ are the people who are using the shared resources. There are three types of users (three roles):
  * _Regular users_ can make, edit and delete their reservations, based on the options set by adminstrators (ie. the maximum length of reservation can be set for each resource).
  * _Masters_ can in addition edit or delete reservations of others and also can make their own reservations. They can also do so-called "system" reservations, which are not limited to the above constraints. System reservations are intended for example for maintenance etc.
  * _Administrators_ can in addition create, edit and delete shared resources, create, edit and delete users and manage opening hours.

In addition to its core task of managing shared resource usage, the system also has several features that may come handy:

* _News_ for members, that can be published on the web site and (optionally) distributed to members via e-mail.
* _Opening hours_ management. Default opening hours can be configured (for each day of week) and then exceptions can be made for specific dates.
* _Directory_, which can contain contact details of members and also other important records.

## Deploying the system

ReP can be deployed on premises or in the cloud. It's fairly standard ASP.NET Core web application.

### Technical requirements

* Web server supporting ASP.NET Core 6.0.
* Microsoft SQL Server (any supported version and edition), Azure SQL Database or Sqlite.

### Configuration

The configuration is stored in `appsettings.json`. You can set the following configuration options:
* `Database` - either `Sqlite` or `SqlServer`.
* `ConnectionStrings` - connection strings:
  * `SqlServer` - connection string for Microsoft SQL Server or Azure SQL Database.
  * `Sqlite` - connection string for Sqlite.
* `Features` - turn application features on or off:
  * `UseOpeningHours` - `true` or `false`.
  * `UseNews` - `true` or `false`.
  * `UseMemberDirectory` - `true` or `false`.
* `Design` - design configuration:
  * `ApplicationName` - application name (title) used trough the application UI.
  * `HeaderImageUrl` - URL of image used in header instead of default one.
  * `StylesheetUrl` - URL of stylesheet used instead of default one.
* `Mailing` - outgoing e-mail configuration:
  * `PickupFolder` - path where `.eml` files for sent messages are generated.
  * `SendGridApiKey` - API key for SendGrid. If not specified, mail pickup is used.
  * `SenderName` - sender display name.
  * `SenderAddress` - sender e-mail address.
* `OpeningHours` - array of opening hour specifications:
  * `DayOfWeek` - day name in English, `Monday` to `Sunday`.
  * `OpeningTime` - opening time in this particular day of week.
  * `ClosingTime` - closing time in this particular day of week.

### Customization

You might want to customize the following files to suit your needs:

* Styles:
  * `wwwroot/Content/Styles/_vars.scss` - font family and accent color.
  * `wwwroot/Content/Styles/*.scss` - other styles.
  * Or use the `Design:StylesheetUrl` and `Design:HeaderImageUrl` configuration options.
* HTML:
  * `Pages/_Layout.cshtml` - general page layout.
* E-mail:
  * `Resources/Mailing/*/BodyTextFormatString.txt` - general message format, add common footer here.
  * `Resources/Mailing/*/*.txt` - text of e-mail messages.
