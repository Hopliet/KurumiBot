using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
//test
namespace DiscordBot
{
    class Program
    {
        private DiscordSocketClient Client;
        private CommandService Commands;

        static void Main(string[] args)
        //starts the bot
        => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            //configuring the client
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                //error handling, use debug only when programming not when hosting
                LogLevel = LogSeverity.Debug
            });

            //configuring the commands
            Commands = new CommandService(new CommandServiceConfig
            {
                //Makes the commands not case sensitive
                CaseSensitiveCommands = true,
                //Makes the program not wait on commands to be finished before running new commands
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });

            //configuring the methods
            Client.MessageReceived += Client_MessageReceived;
            //where to look for the commands
            /*await Commands.AddModuleAsync(Assembly.GetEntryAssembly());*/
            await Commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null/*search everywhere in the project*/);

            //runs when bot is logged in succesfully
            Client.Ready += Client_Ready;
            //runs whenever there is a log received
            Client.Log += Client_Log;

            string token;
            using (var stream = new FileStream((Path.GetDirectoryName(Assembly.GetEntryAssembly().Location/*gets the file location of the bot*/ )).Replace(/*don't see the / as an escape character*/@"bin\Debug\netcoreapp2.2", @"Token\token.txt"), FileMode.Open, FileAccess.Read))
            using (var readToken = new StreamReader(stream))
            {
                //logs the bot in with the token
                token = readToken.ReadToEnd();
            }

            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

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
            await Client.SetGameAsync("I am best bot :3"/*, "TwitchChannel", StreamType.Twitch*/);
        }

        //runs whenever someone sends a message in the chat that the bot can read
        private async Task Client_MessageReceived(SocketMessage messageParam)
        {
            //configure the commands
            var message = messageParam as SocketUserMessage/*Gives more details about context and the user of the message*/;
            var context = new SocketCommandContext(Client, message);

            //checking for false and empty commands
            if (context.Message == null || context.Message.Content == "") return;
            //checking for text by other bots
            if (context.User.IsBot) return;

            //configuring the prefix
            int argPos = 0;
            if (!(message.HasStringPrefix("!", ref argPos) || message.HasMentionPrefix(Client.CurrentUser, ref argPos))) return;

            var results = await Commands.ExecuteAsync(context, argPos, null);
            if (!results.IsSuccess)
                Console.WriteLine($"{DateTime.Now} at Commands. Something went wrong with executing a command. Text: {context.Message.Content} | Error: {results.ErrorReason}");
        }
    }
}
