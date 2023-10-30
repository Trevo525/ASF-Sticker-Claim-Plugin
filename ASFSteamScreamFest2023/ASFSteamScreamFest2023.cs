using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Steam.Interaction;
using ArchiSteamFarm.Web.Responses;
using JetBrains.Annotations;
using SteamKit2;

namespace ASFSteamScreamFest2023;

#pragma warning disable CA1812 // ASF uses this class during runtime
[UsedImplicitly]
internal sealed partial class ASFSteamScreamFest2023 : IDisposable, IBotCommand2 {
	public string Name => nameof(ASFSteamScreamFest2023);
	public Version Version => typeof(ASFSteamScreamFest2023).Assembly.GetName().Version ?? throw new InvalidOperationException(nameof(Version));

	public Timer? RefreshTimer;

	public Task OnLoaded() {
		ASF.ArchiLogger.LogGenericInfo($"Hello {Name}!");
		RefreshTimer = new Timer(
			async e => await ClaimSticker("ASF").ConfigureAwait(false),
			null,
			TimeSpan.FromHours(1),
			TimeSpan.FromHours(8)
		);
		return Task.CompletedTask;
	}

	/// <summary>
	/// Event Handler for timer. Calls the functions to get stickers for each bot.
	/// </summary>
	/// <param name="botNames"></param>
	/// <returns></returns>
	private static async Task<string?> ClaimSticker(string botNames) {
		HashSet<Bot>? bots = Bot.GetBots(botNames);

		if (bots == null || bots.Count == 0) {
			IFormatProvider formatProvider = CultureInfo.InvariantCulture;
			return Commands.FormatStaticResponse(string.Format(formatProvider, Strings.BotNotFound, botNames));
		}

		// Send the message for each bot using GetSticker.
		IList<string> results = await Utilities.InParallel(bots.Select(bot => GetSticker(bot))).ConfigureAwait(false);

		// Put the responses in a list if they are not null or empty.
		List<string> responses = new List<string>(results.Where(result => !string.IsNullOrEmpty(result)));

		ASF.ArchiLogger.LogGenericInfo(Environment.NewLine + string.Join(Environment.NewLine, responses));
		return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
	}

	private static async Task<string> GetSticker(Bot bot) {
		const string post_request = "https://api.steampowered.com/ISaleItemRewardsService/ClaimItem/v1?access_token=";
		const string html_request = "/category/scream";

		// Query the page.
		HtmlDocumentResponse? html_response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(new Uri(ArchiWebHandler.SteamStoreURL, html_request)).ConfigureAwait(false);
		IDocument? html = html_response?.Content;
		if (html == null) {
			return "<" + bot.BotName + "> Failed!";
		}

		// Get webapi token
		Regex re = WebApiTokenRegex();
		MatchCollection reResult = re.Matches(html.DocumentElement.InnerHtml);
		if (reResult.Count < 1 || reResult[0].Groups.Count < 2) {
			return "<" + bot.BotName + "> Failed!";
		}
		string webApiToken = reResult[0].Groups[1].Value;


		Dictionary<string, string> data = new Dictionary<string, string>(1, StringComparer.Ordinal) { { "input_protobuf_encoded", "" } };

		BasicResponse? response = await ArchiWebHandler.WebLimitRequest(WebAPI.DefaultBaseAddress, async () => await bot.ArchiWebHandler.WebBrowser.UrlPost(new Uri(post_request + webApiToken), data, new Uri(ArchiWebHandler.SteamStoreURL, html_request)).ConfigureAwait(false)).ConfigureAwait(false);

		if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK) {
			return "<" + bot.BotName + "> Done!";
		} else {
			return "<" + bot.BotName + "> Failed!";
		}

	}

	public void Dispose() {
		if (RefreshTimer != null) { RefreshTimer.Dispose(); }
	}

	[GeneratedRegex("&quot;webapi_token&quot;:&quot;([^&]*)&quot;")]
	private static partial Regex WebApiTokenRegex();

	// Call the command manually incase you want to test it.
	public Task<string?> OnBotCommand(Bot bot, EAccess access, string message, string[] args, ulong steamID = 0) {
		if (access < EAccess.FamilySharing) {
			return Task.FromResult<string?>(null);
		}

		switch (args[0].ToUpperInvariant()) {
			case "GETSTICKER" when args.Length > 1:
				return ClaimSticker(Utilities.GetArgsAsText(args, 1, ","));
			case "GETSTICKER":
				return ClaimSticker(bot.BotName);
			default:
				return Task.FromResult<string?>(null);
		}
	}
}
#pragma warning restore CA1812 // ASF uses this class during runtime
