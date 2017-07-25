using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BobbyBot
{
    public class Program
    {
        private DiscordSocketClient _client;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += MessageReceived;

            string token = "MzM4OTU2MTgwNTYwODA1ODg4.DFesaw.IbkfrUMw9s__akiZI7yRovMzYUY"; // Remember to keep this private!
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage message)
        {
            Reply rep = new Reply()
            {
                command = new Dictionary<string, string>()
            };

            rep.command = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("../../../JSONDico.txt"));

            string input;
            string[] learn;
            if (message.Content.StartsWith("!learn "))
            {
                learn = message.Content.Split('"');
                foreach (string l in learn)
                {
                    Console.Out.WriteLine(l);
                }
                if (!(learn[1] == null || learn[3] == null))
                {
                    rep.SetCommandItem(learn[1], learn[3]);
                    File.WriteAllText("../../../JSONDico.txt", JsonConvert.SerializeObject(rep.command));
                }
                else
                {
                    await message.Channel.SendMessageAsync("!learn need 2 arguments. \nex : !learn \"Qui est Robin ?\" \"Robin est le plus beau\"");
                }
            }
            else
            {
                if (rep.GetReply(message.Content).Equals("ERROR COMMAND UNKNOWN"))
                {
                    Console.Out.WriteLine(rep.GetReply(message.Content));
                }
                else
                {
                    await message.Channel.SendMessageAsync(rep.GetReply(message.Content));
                }
            }

            /*
            if (message.Content.StartsWith("!learn "))
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                
                using(JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    
                    writer.WriteStartObject();
                    writer.WritePropertyName("CPU");
                    writer.WriteValue("Intel");
                    writer.WritePropertyName("PSU");
                    writer.WriteValue("500W");
                    writer.WritePropertyName("Drives");
                    writer.WriteStartArray();
                    writer.WriteValue("DVD read/writer");
                    writer.WriteComment("(broken)");
                    writer.WriteValue("500 gigabyte hard drive");
                    writer.WriteValue("200 gigabype hard drive");
                    writer.WriteEnd();
                    writer.WriteEndObject();

                }
            }
            else
            {
                switch (message.Content)
                {
                    case "!ping":
                        await message.Channel.SendMessageAsync("Pong !");
                        break;
                    case "!pong":
                        await message.Channel.SendMessageAsync("Pung !");
                        break;
                    case "!pung":
                        await message.Channel.SendMessageAsync("Ta gueule.");
                        break;
                    case "!Kevin":
                        await message.Channel.SendMessageAsync("Rageux !");
                        break;
                    case "!Close":
                        await message.Channel.SendMessageAsync("Sale Tank !");
                        break;
                    case "!Norfl":
                        await message.Channel.SendMessageAsync("Tuto touchpad !");
                        break;
                    case "!Compote":
                        await message.Channel.SendMessageAsync("Beau Gosse !");
                        break;
                    case "!Tamere":
                        await message.Channel.SendMessageAsync("Ton pere !");
                        break;
                }
            }
            */
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        internal class Reply
        {
            public Dictionary<string, string> command;

            public Dictionary<string, string> Command { get => command; set => command = value; }

            public void SetCommandItem(string com, string item)
            {
                if (!(command.ContainsKey(com) || command.ContainsKey(com.ToLower()) || command.ContainsKey(com.ToUpper())))
                {
                    command.Add(com, item);
                    Console.Out.WriteLine("Command : " + com + " added with value : " + item);
                }
                else
                {
                    Console.Out.WriteLine("Command : " + com + " already registered.");
                }
            }

            public string GetReply(string com)
            {
                if (command.ContainsKey(com))
                {
                    return command[com];
                }
                else if (command.ContainsKey(com.ToLower()))
                {
                    return command[com.ToLower()];
                }   
                else if (command.ContainsKey(com.ToUpper()))
                {
                    return command[com.ToUpper()];
                }
                else
                {
                    return "ERROR COMMAND UNKNOWN";
                }
            }
        }
    }
}
