using Discord.WebSocket;

namespace Torchizm_Bot
{
    internal static class Global
    {
        internal static DiscordSocketClient Client { get; set; }
        internal static ulong MessageIdToTrack { get; set; }
    }
}
