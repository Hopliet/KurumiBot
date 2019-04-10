using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Discord;
using Discord.Commands;

namespace DiscordBot.Core.Commands
{
    public class Ping : ModuleBase<SocketCommandContext/*comes from MessageReceived*/>
    {
        //specifying a command in discord
        [Command("Ping"), Alias("Check", "ping"), Summary("Runs a modified ping command")]
        public async Task ReplyPing()
        {
            await Context.Channel.SendMessageAsync("ara ara, I am working now");
        }

        [Command("Baka"), Summary("Shows who the real baka is")]
        public async Task ReplyBaka()
        {
            await Context.Channel.SendMessageAsync($"no, {Context.User.Mention} is!");
        }

        [Command("Sensei"), Alias("Mentor", "Stuck", "support")]
        public async Task MentionMentor()
        {
            await Context.Channel.SendMessageAsync(Context.Guild.GetRole(561965462918004737).Mention + " TASUKETE-KUDASAI");
        }
    }
}
