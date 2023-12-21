# ASF-Sticker-Claim-Plugin

## Description

ASF-Sticker-Claim-Plugin is a **[plugin](https://github.com/JustArchiNET/ArchiSteamFarm/wiki/Plugins)** for **[ArchiSteamFarm](https://github.com/JustArchiNET/ArchiSteamFarm)**. The goal is to have it check all accounts for the sticker of the day. The sales may change each time and may require a code change but hopefully as time moves on, this will be more self-sufficient and require less work.

## Compilation
You should be able to compile this with:
### Debug
```
dotnet build ASFStickerClaimPlugin` *debug build*
```
### Release
```
dotnet publish ASFStickerClaimPlugin -c "Release" -o "out"
```

## Ideas
* Add a date field so it doesn't keep when the event is over.
* Move config for this plugin into an .toml file so it doesn't need to be rebuilt per event
* Make a command so you can change the current one without having to edit the plugin.

Pull-requests welcome.

## Credits
* Rudokhvist - They actually wrote most of the code for an older event. I just made it compile with the latest and re-wrote some minor things.
* Archi - Helped me in the discord when I was having trouble building.
* Abyrnos - Pointed out the need for access validation for the bot command.