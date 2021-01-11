using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Torchizm_Bot.Database;
using Torchizm_Bot.Models;

namespace Torchizm_Bot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("oyunkanalı")]
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
            var user = MySQL.Select<User>($"SELECT * FROM users WHERE uid={Context.User.Id}").FirstOrDefault();

            var embed = new EmbedBuilder()
            {
                Author = new EmbedAuthorBuilder()
                {
                    Name = Context.User.Username,
                    IconUrl = Context.User.GetAvatarUrl(size: 512)
                },
                Title = $"{Context.User.Username}, {user.Points} puana sahipsin."
            };

            await ReplyAsync(embed: embed.Build());
        }

        [Command("top")]
        public async Task TopTen(string arg)
        {
            var embed = new EmbedBuilder();

            string[] args = arg.Split(' ');

            if (args[0] == "yardım")
            {
                embed.Title = "Yardım";

                embed.AddField("10", "En çok puana sahip 10 kişiyi gösterir");
                embed.AddField("(@Kişi Etiketi)", "Etiketlediğiniz kişinin puanını gösterir");

                await ReplyAsync(embed: embed.Build());
            }

            if (args[0] == "10")
            {
                var users = MySQL.Select<User>($"SELECT * FROM users ORDER BY points DESC LIMIT 10;");

                embed.Title = "En çok oyun oynayan 10 kişi";

                foreach (var user in users)
                    embed.AddField(Global.Client.GetUser(Convert.ToUInt64(user.UID)).Username, $"{user.Points}");

                await ReplyAsync(embed: embed.Build());
            }

            if (args[0].StartsWith("<@"))
            {
                var mentionedUser = (Context.Message.MentionedUsers as IEnumerable<SocketUser>).FirstOrDefault();

                var user = MySQL.Select<User>($"SELECT * FROM users WHERE uid={mentionedUser.Id}").FirstOrDefault();

                if (user == null)
                {
                    embed.Description = "Veri bulunamadı!";
                    await ReplyAsync(embed: embed.Build());
                }
                else
                {
                    embed.Description = $"**{mentionedUser.Mention}, {user.Points} puana sahip.**";

                    await ReplyAsync(embed: embed.Build());
                }
            }
        }
    }
}