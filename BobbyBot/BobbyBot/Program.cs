using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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
            string mess = (message.Content.Trim()).ToLower();
            Reply rep = new Reply("../../../JSONDicoCommand.txt", "../../../JSONDicoWord.txt");
            System.Threading.Thread.Sleep(1000);

            if (mess.StartsWith("bonjour") || mess.StartsWith("salut") || mess.StartsWith("coucou") || mess.StartsWith("hello")
                 || mess.StartsWith("yo") || mess.StartsWith("plop"))
            {
                await message.Channel.SendMessageAsync("Yo " + message.Author + " !");
            }
            else if (mess.StartsWith("!learn "))
            {
                string[] learn;
                learn = mess.Split('"');
                if (learn.Length > 3)
                {
                    //!(learn[1] == null || learn[3] == null)) {
                    rep.SetCommandItem(learn[1], learn[3]);
                    rep.SaveCommandToFile("../../../JSONDicoCommand.txt");
                }
                else
                {
                    await message.Channel.SendMessageAsync("!learn need 2 arguments. \nex : !learn \"Qui est Robin ?\" \"Robin est le plus beau\"");
                }
            }
            else if (mess.StartsWith("!word "))
            {
                string[] learn;
                learn = mess.Split('"');
                if (learn.Length > 3)
                {
                    System.Console.WriteLine("Before Regex : " + learn[3]);
                    Regex.Replace(learn[3], @"[^\w\s]", "");
                    System.Console.WriteLine("After Regex : " + learn[3]);
                    //!(learn[1] == null || learn[3] == null)) {
                    rep.SetWordItem(learn[1], learn[3]);
                    rep.SaveWordToFile("../../../JSONDicoWord.txt");
                }
                else
                {
                    await message.Channel.SendMessageAsync("!word need 2 arguments. \nex : !word \"poivron\" \"Berk ! J'aime pas les poivrons !\"");
                }
            }
            else
            {
                string reply = rep.GetCommandReply(mess);
                if (reply.Equals("ERROR COMMAND UNKNOWN"))
                {
                    Console.Out.WriteLine(reply);
                }
                else
                {
                    await message.Channel.SendMessageAsync(reply);
                }
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        internal class Reply
        {
            private Dictionary<string, string> command;
            private Dictionary<string, string> word;
            //public Dictionary<string, string> Command { get => command; set => command = value; }

            public Reply()
            {
                command = new Dictionary<string, string>();
                word = new Dictionary<string, string>();
            } 

            public Reply(string filepathCommand, string filepathWord)
            {
                command = new Dictionary<string, string>();
                command = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filepathCommand));

                word = new Dictionary<string, string>();
                word = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filepathWord));
            }

            // Command Methods

            public void LoadCommandFromFile(string filepath)
            {
                command = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filepath));
            }

            public void SaveCommandToFile (string filepath)
            {
                File.WriteAllText(filepath, JsonConvert.SerializeObject(command));
            }
            
            public string SetCommandItem(string com, string item)
            {
                string ret;
                if (!command.ContainsKey(com))
                {
                    command.Add(com, item);
                    ret = "Command : " + com + " added with value : " + item;
                }
                else
                {
                    ret = "Command : " + com + " already registered.";
                }
                return ret;
            }

            public string GetCommandReply(string com)
            {
                string ret;
                if (command.ContainsKey(com))
                {
                    ret = command[com];
                }
                else
                {
                    ret = "ERROR COMMAND UNKNOWN";
                }
                return ret;
            }

            // Word Methods

            public void LoadWordFromFile(string filepath)
            {
                word = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filepath));
            }

            public void SaveWordToFile(string filepath)
            {
                File.WriteAllText(filepath, JsonConvert.SerializeObject(word));
            }


            public string SetWordItem(string wor, string item)
            {
                string ret;
                if (!word.ContainsKey(wor))
                {
                    word.Add(wor, item);
                    ret = "Command : " + wor + " added with value : " + item;
                }
                else
                {
                    ret = "Command : " + wor + " already registered.";
                }
                return ret;
            }

            public string GetWordReply(string wor)
            {
                string ret;
                if (word.ContainsKey(wor))
                {
                    ret = word[wor];
                }
                else
                {
                    ret = "ERROR COMMAND UNKNOWN";
                }
                return ret;
            }
        }
    }
}
