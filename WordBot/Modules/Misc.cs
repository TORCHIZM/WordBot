using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using Torchizm_Bot.Models;

namespace Torchizm_Bot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("setChannel")]
        public async Task SetWordChannel(SocketTextChannel channel)
        {
            var server = Server.Get(channel.Guild.Id).FirstOrDefault();
            if (server == null)
            {
                Console.WriteLine("new came");
                Console.WriteLine($"Guild: {channel.Guild.Id}\nChannel: {channel.Id}");
                Server.New(channel.Guild.Id, channel.Id);
            }
            else
            {
                server.WordChannel = channel.Id;
                server.Save();
            }

            await Context.Message.AddReactionAsync(new Emoji("👍"));
        }

        [Command("top")]
        public async Task TopTen()
        {
            //var embed = new EmbedBuilder();

            //var logs = MySQL.Select<OrderedGameLog>($"SELECT COUNT(ID) AS Count, Details, UID FROM gamelogs WHERE uid={Context.User.Id} GROUP BY Details ORDER BY Count DESC LIMIT 10;");

            //foreach (var log in logs)
            //    embed.AddField(log.Details, $"{log.Count} dakika oynadı");

            //await ReplyAsync(embed: embed.Build());
        }

        [Command("top")]
        public async Task TopTen(string arg)
        {
            //var embed = new EmbedBuilder();

            //string[] args = arg.Split(' ');

            //if (args[0] == "yardım")
            //{
            //    embed.Title = "Yardım";

            //    embed.AddField("oyun", "En çok oynanan 10 oyunu gösterir");
            //    embed.AddField("uniqe", "Farklı kişiler tarafından oynanan ortak oyunlar");
            //    embed.AddField("10", "En çok oyun oynayan 10 kişi");
            //    embed.AddField("(@Kişi Etiketi)", "Kişinin oynadığı oyunları gösterir");

            //    await ReplyAsync(embed: embed.Build());
            //}

            //if (args[0] == "oyun")
            //{
            //    var logs = MySQL.Select<OrderedGameLog>("SELECT COUNT(ID) AS Count, Details, UID FROM gamelogs GROUP BY Details ORDER BY Count DESC LIMIT 10;");

            //    embed.Title = "En çok oynanan 10 oyun";

            //    foreach (var log in logs)
            //        embed.AddField(log.Details, $"{log.Count} dakika oynandı");

            //    await ReplyAsync(embed: embed.Build());
            //}

            //if (args[0] == "10")
            //{
            //    var logs = MySQL.Select<OrderedGameLog>("SELECT COUNT(ID) AS Count, Details, UID FROM gamelogs GROUP BY UID ORDER BY Count DESC LIMIT 10;");

            //    embed.Title = "En çok oyun oynayan 10 kişi";

            //    foreach (var log in logs)
            //        embed.AddField(Global.Client.GetUser(Convert.ToUInt64(log.UID)).Username, $"{log.Count} dakika {log.Details}");

            //    await ReplyAsync(embed: embed.Build());
            //}

            //if (args[0] == "uniqe")
            //{
            //    var logs = MySQL.Select<OrderedGameLog>("SELECT COUNT(DISTINCT UID) AS Count, Details, UID FROM gamelogs GROUP BY Details ORDER BY Count DESC LIMIT 10;");

            //    embed.Title = "Farklı kişiler tarafından oynanan oyunlar";

            //    foreach (var log in logs)
            //        embed.AddField(log.Details, $"{log.Count} farklı kişi tarafından oynandı");

            //    await ReplyAsync(embed: embed.Build());
            //}

            //if (args[0].StartsWith("<@"))
            //{
            //    var mentionedUser = (Context.Message.MentionedUsers as IEnumerable<SocketUser>).FirstOrDefault().Id;

            //    var logs = MySQL.Select<OrderedGameLog>($"SELECT COUNT(ID) AS Count, Details, UID FROM gamelogs WHERE uid={mentionedUser} GROUP BY Details ORDER BY Count DESC LIMIT 10;");

            //    if (logs.Count == 0)
            //    {
            //        embed.Description = "Veri bulunamadı!";
            //        await ReplyAsync(embed: embed.Build());
            //    }
            //    else
            //    {
            //        foreach (var log in logs)
            //            embed.AddField(log.Details, $"{log.Count} dakika oynadı");

            //        await ReplyAsync(embed: embed.Build());
            //    }
            //}
        }
    }
}