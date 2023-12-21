# ASF-Steam-Scream-Fest

## Description

ASF-Steam-Scream-Fest is a **[plugin](https://github.com/JustArchiNET/ArchiSteamFarm/wiki/Plugins)** for **[ArchiSteamFarm](https://github.com/JustArchiNET/ArchiSteamFarm)**. The goal is to have it check all accounts for the sticker of the day. This is for the Steam Scream Fest event. But should be easily editable for all future events, ones for stickers that is.

## Compilation
You should be able to compile this with:
`dotnet build ASFStickerClaimPlugin` *debug build*
or
`dotnet publish ASFStickerClaimPlugin -c "Release" -o "out"`

## Ideas
* Add a date field so it doesn't ping when the event is over.
* Move config for this plugin into an .toml file so it doesn't need to be rebuilt per event

Pull-requests welcome.

## Credits
* Rudokhvist - They actually wrote most of the code for an older event. I just made it compile with the latest and re-wrote some minor things.
* Archi - Helped me in the discord when I was having trouble building.
* Abyrnos - Pointed out the need for access validation for the bot command.