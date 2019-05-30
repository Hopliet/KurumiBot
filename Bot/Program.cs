using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;


/*
 * This bot has been made and managed by:
 * Michiel "Hopliet" Van Mol | michiel.vanmol@hotmail.com
 * Denis "Toy" Song | denissong98@gmail.com
 * 
 * Supervised and supported by:
 * Goran "Pardusus" Vandenbossche
 * 
 * Special thanks to:
 * Sjustein | https://www.youtube.com/channel/UCoC4QZeCMe7uf2ZMHambL8A
*/

namespace DiscordBot
{
    class Program
    {
        private DiscordSocketClient client;
        private CommandService commands;

        static void Main()
        //starts the bot
        => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            //configuring the client
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                //error handling, use debug only when programming not when hosting
                LogLevel = LogSeverity.Debug
            });

            //configuring the commands
            commands = new CommandService(new CommandServiceConfig
            {
                //Makes the commands not case sensitive
                CaseSensitiveCommands = false,
                //Makes the program not wait on commands to be finished before running new commands
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });

            //configuring the methods
            client.MessageReceived += Client_MessageReceived;
            //where to look for the commands
            /*await Commands.AddModuleAsync(Assembly.GetEntryAssembly());*/
            await commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null/*search everywhere in the project*/);

            //runs when bot is logged in succesfully
            client.Ready += Client_Ready;
            //runs whenever there is a log received
            client.Log += Client_Log;

            string token;
            using (var stream = new FileStream((Path.GetDirectoryName(Assembly.GetEntryAssembly().Location/*gets the file location of the bot*/ )).Replace(/*don't see the / as an escape character*/@"bin\Debug\netcoreapp2.2", @"Token\token.txt"), FileMode.Open, FileAccess.Read))
            using (var readToken = new StreamReader(stream))
            {
                //logs the bot in with the token
                token = readToken.ReadToEnd();
            }

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            //stop the bot from stopping
            await Task.Delay(-1);
        }


        private async Task Client_Log(LogMessage message)
        {
            Console.WriteLine(/*allows variables to be put into the string*/$"{DateTime.Now} at {message.Source} {message.Message}");
        }

        //checks if bot is loaded correctly
        private async Task Client_Ready()
        {
            //sets the streaming text in discord
            await client.SetGameAsync("k! prefix | I am best bot :3"/*, "TwitchChannel", StreamType.Twitch*/);
        }

        //runs whenever someone sends a message in the chat that the bot can read
        private async Task Client_MessageReceived(SocketMessage messageParam)
        {
            //configure the commands
            var message = messageParam as SocketUserMessage/*Gives more details about context and the user of the message*/;
            var context = new SocketCommandContext(client, message);

            //checking for false and empty commands
            if (context.Message == null || context.Message.Content == "") return;
            //checking for text by other bots
            if (context.User.IsBot) return;

            //configuring the prefix
            int argPos = 0;

            //prefix !
            if (message.HasStringPrefix("k!", ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos /*|| variable that has a list of all commands that don't need a prefix*/))
            {
                //executes the with a prefix command
                var results = await commands.ExecuteAsync(context, argPos, null);
                if (!results.IsSuccess)
                {
                    Console.WriteLine($"{DateTime.Now} at Commands. Something went wrong with executing a command. Text: {context.Message.Content} | Error: {results.ErrorReason}");
                }
            }
            //messages with no prefix
            return;
        }
    }
}
