[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/steam-giveaways-bot-server)](https://github.com/hmlendea/steam-giveaways-bot-server/releases/latest)
[![Build Status](https://github.com/hmlendea/steam-giveaways-bot-server/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/steam-giveaways-bot-server/actions/workflows/dotnet.yml)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://gnu.org/licenses/gpl-3.0)

# Steam Giveaways Bot Server

REST API server for managing and providing data for SteamGiveawaysBot instances.

The steam-giveaways-bot repository is not, and will not be made public.

## Features

- Records Steam game rewards (activation keys) won from giveaways
- Manages Steam accounts linked to users
- Tracks and updates user IP addresses
- Sends notifications when a new reward is recorded
- HMAC-authenticated API requests
- File-based persistence using JSON and XML

## Configuration

All settings are loaded from `appsettings.json`. The following keys are recognised:

| Section | Key | Description |
|---------|-----|-------------|
| `dataStoreSettings` | `RewardStorePath` | Path to the JSON file storing recorded rewards. |
| `dataStoreSettings` | `SteamAccountStorePath` | Path to the XML file storing Steam accounts. |
| `dataStoreSettings` | `UserStorePath` | Path to the XML file storing registered users. |
| `securitySettings` | `ApiKey` | API key required in the `X-Api-Key` request header. |
| `notificationSettings` | `EmailAddress` | E-mail address to notify when a reward is recorded. |
| `nuciNotificationsSettings` | `BaseUrl` | Base URL of the NuciNotifications API. |
| `nuciNotificationsSettings` | `ApiKey` | API key for the NuciNotifications service. |
| `nuciNotificationsSettings` | `HmacSharedSecretKey` | HMAC signing key for NuciNotifications requests. |
| `nuciLoggerSettings` | `LogFilePath` | Path to the log file. |
| `nuciLoggerSettings` | `IsFileOutputEnabled` | Whether file logging is enabled. |

Missing store files are created automatically on startup.

## API

All endpoints require an `X-Api-Key` header matching the configured `securitySettings.ApiKey`. Request and response bodies use JSON with HMAC authentication tokens (`hmacToken` field).

### Rewards

| Method | Path | Description |
|--------|------|-------------|
| `GET` | `/rewards` | Health check - returns 200 OK. |
| `POST` | `/rewards` | Records a new reward (activation key won from a giveaway). |

**POST /rewards body:**

```json
{
  "username": "user",
  "gaProvider": "SteamGifts",
  "gaId": "gYeA3",
  "steamUsername": "solaire_of_astora",
  "steamAppId": "730",
  "key": "ABCDE-FGHIJ-KLMNO",
  "hmacToken": "..."
}
```

### Steam Accounts

| Method | Path | Description |
|--------|------|-------------|
| `GET` | `/steamaccount/{username}` | Returns the Steam account assigned to the given user. |

**GET /steamaccount/{username} query parameters:** `username`, `gaProvider`, `hmacToken`.

### Users

| Method | Path | Description |
|--------|------|-------------|
| `POST` | `/users/ip/{username}` | Updates the IP address for the given user. |

**POST /users/ip/{username} body:**

```json
{
  "ip": "1.2.3.4",
  "hmacToken": "..."
}
```

## Development

### Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

All NuGet dependencies are restored automatically by `dotnet restore`.

### Build

```bash
dotnet build SteamGiveawaysBot.Server
```

### Run

```bash
dotnet run --project SteamGiveawaysBot.Server
```

### Test

```bash
dotnet test SteamGiveawaysBot.Server.slnx
```

### Release

The repository includes `release.sh`, which delegates to the upstream deployment script used by the project maintainer.

```bash
bash ./release.sh 1.0.0
```

This script downloads and executes an external release helper from `https://raw.githubusercontent.com/hmlendea/deployment-scripts/master/release/dotnet/10.0.sh`.

**Note:** Piping into `bash` is an intensely controversial topic. Please review any external scripts before running them in your environment!

## Project Structure

The solution contains the following projects:

- `SteamGiveawaysBot.Server`: Main REST API application
- `SteamGiveawaysBot.Server.UnitTests`: Unit tests

Key directories inside `SteamGiveawaysBot.Server/`:

| Directory | Purpose |
|-----------|---------|
| `Client/` | Steam Storefront API client for retrieving app data |
| `Configuration/` | Strongly-typed configuration model classes |
| `Controllers/` | ASP.NET Core API controllers |
| `DataAccess/` | Data objects and file-based persistence |
| `Logging/` | Logging operation and key definitions |
| `Requests/` | API request models |
| `Responses/` | API response models |
| `Service/` | Business logic services, domain models, and mapping extensions |

### Dependencies

| Package | Purpose |
|---------|---------|
| `NuciAPI` | Base API request/response utilities |
| `NuciAPI.Controllers` | Base controller with HMAC-authenticated request processing |
| `NuciAPI.Middleware` | Core API middleware |
| `NuciAPI.Middleware.ExceptionHandling` | Centralised exception handling middleware |
| `NuciAPI.Middleware.Logging` | Request/response logging middleware |
| `NuciAPI.Middleware.Security` | API key security enforcement middleware |
| `NuciDAL` | File-based data access layer (JSON and XML) |
| `NuciExtensions` | General-purpose extension methods |
| `NuciLog` | Structured application logging |
| `NuciLog.Core` | Core logging abstractions |
| `NuciNotifications.Client` | Client for sending notifications via NuciNotifications |
| `NuciSecurity.HMAC` | HMAC request signing and verification |

## Contributing

Contributions are welcome.

Please:

- keep the changes cross-platform
- keep the pull requests focused and consistent with the existing style
- update the documentation when the behaviour changes
- add or update the tests for any new behaviour

## Support

If you find this project useful, consider [funding it](https://hmlendea.go.ro/funding) or giving a ⭐️ on GitHub!

## Related projects

- [Steam Giveaways Bot](https://github.com/hmlendea/steam-giveaways-bot)

## License

Licensed under the GNU General Public License v3.0 or later.
See [LICENSE](./LICENSE) for details.
