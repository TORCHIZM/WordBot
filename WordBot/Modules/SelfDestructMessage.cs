using Discord;
using Discord.WebSocket;

using System.Timers;

namespace Torchizm_Bot.Modules
{
    public static class SelfDestructMessage
    {
        public static async void Send(ISocketMessageChannel channel, string text = null, EmbedBuilder embed = null)
        {
            var _message = await channel.SendMessageAsync(text: text, embed: embed.Build());

            Timer t = new Timer()
            {
                Interval = 2000
            };

            t.Elapsed += async (sender, e) =>
            {
                await _message.DeleteAsync();
                t.Stop();
            };

            t.Start();
        }
    }
}
