[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/steam-giveaways-bot-server)](https://github.com/hmlendea/steam-giveaways-bot-server/releases/latest)
[![Build Status](https://github.com/hmlendea/steam-giveaways-bot-server/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/steam-giveaways-bot-server/actions/workflows/dotnet.yml)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://gnu.org/licenses/gpl-3.0)

# Steam Giveaways Bot Server

REST API server for managing and providing data for SteamGiveawaysBot instances.

The steam-giveaways-bot repository is not, and will not be made public.

## Requirements

- .NET SDK/runtime with support for `net10.0`

## Development

### Build

```bash
dotnet build
```

### Run

```bash
dotnet run
```

### Release

The repository includes `release.sh`, which delegates to the upstream deployment script used by the project maintainer.

```bash
bash ./release.sh 1.0.0
```

This script downloads and executes an external release helper from: `https://raw.githubusercontent.com/hmlendea/deployment-scripts/master/release/dotnet/10.0.sh`

**Note:** Piping into `bash` is an intensely controversial topic. Please review any external scripts before running them in your environment!

## Contributing

Contributions are welcome.

Please:

- keep the changes cross-platform
- keep the pull requests focused and consistent with the existing style
- update the documentation when the behaviour changes
- add or update the tests for any new behaviour

## Related projects

- [Steam Giveaways Bot](https://github.com/hmlendea/steam-giveaways-bot)
- [Steam Giveaways Bot Server](https://github.com/hmlendea/steam-giveaways-bot-server)

## License

Licensed under the GNU General Public License v3.0 or later.
See [LICENSE](./LICENSE) for details.