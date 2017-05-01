using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot02
{
    class NovaBot
    {
        DiscordClient discord;
        CommandService commands;

        //Dictionary<string, Dictionary<string, bool>> muted;
        Dictionary<string, Dictionary<string, List<string>>> message_log = new Dictionary<string, Dictionary<string, List<string>>>();

        List<string> blacklisted_url_keywords = new List<string>();

        public NovaBot()
        {
            if (!File.Exists("blacklisted_url_keywords.txt"))
            {
                File.WriteAllText("blacklisted_url_keywords.txt", "goo.gl");
            }
            string[] blsurl = File.ReadAllLines("blacklisted_url_keywords.txt");
            for(int i = 0; i < blsurl.Length; i++)
            {
                blacklisted_url_keywords.Add(blsurl[i]);
            }

            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
                Console.WriteLine("client created");
            });

            discord.MessageReceived += async (s, e) =>
            {
                if(e.Message.IsAuthor && !e.Message.IsAuthor)
                {
                    await e.Channel.SendMessage("Something went wrong. @Accyrsed#5387, please look into this.");
                }

                if(message_log.ContainsKey(e.Server.Name))
                {
                    
                    if (message_log[e.Server.Name].ContainsKey(e.Channel.Name))
                    {
                        Dictionary<string, List<string>> channel_messages = message_log[e.Server.Name];
                        channel_messages[e.Channel.Name].Add(e.User + " : " + e.Message.Text + " (" + e.Message.Timestamp + ")");
                        //Console.WriteLine(e.User + " has said " + e.Message + " to " + e.Channel);
                    }
                    else
                    {
                        Dictionary<string, List<string>> channel_messages = message_log[e.Server.Name];
                        List<string> msgs = new List<string>();
                        channel_messages.Add(e.Channel.Name, msgs);
                        channel_messages[e.Channel.Name].Add(e.User + " : " + e.Message.Text + " (" + e.Message.Timestamp + ")");
                        //Console.WriteLine(e.User + " has said " + e.Message + " to " + e.Channel);
                    }
                }
                else
                {
                    Dictionary<string, List<string>> channel_messages = new Dictionary<string, List<string>>();
                    message_log.Add(e.Server.Name, channel_messages);

                    List<string> msgs = new List<string>();
                    channel_messages.Add(e.Channel.Name, msgs);
                    channel_messages[e.Channel.Name].Add(e.User + " : " + e.Message.Text + " (" + e.Message.Timestamp + ")");
                    //Console.WriteLine(e.User + " has said " + e.Message + " to " + e.Channel);
                }
            };

            discord.MessageUpdated += async (s, e) =>
            {
                if (e.Before.IsAuthor && !e.Before.IsAuthor)
                {
                    await e.Channel.SendMessage("Something went wrong. @Accyrsed#5387, please look into this.");
                }

                if (message_log.ContainsKey(e.Server.Name))
                {

                    if (message_log[e.Server.Name].ContainsKey(e.Channel.Name))
                    {
                        Dictionary<string, List<string>> channel_messages = message_log[e.Server.Name];
                        channel_messages[e.Channel.Name].Add(e.User + " : [" + e.After.Text + "] --> [" + e.After.Text + " (" + e.After.Timestamp + ")");
                        //Console.WriteLine(e.User + " has edited " + e.Before + " to " + e.After);
                    }
                    else
                    {
                        Dictionary<string, List<string>> channel_messages = message_log[e.Server.Name];
                        List<string> msgs = new List<string>();
                        channel_messages.Add(e.Channel.Name, msgs);
                        channel_messages[e.Channel.Name].Add(e.User + " : [" + e.After.Text + "] --> [" + e.After.Text + " (" + e.After.Timestamp + ")");
                        //Console.WriteLine(e.User + " has edited " + e.Before + " to " + e.After);
                    }
                }
                else
                {
                    Dictionary<string, List<string>> channel_messages = new Dictionary<string, List<string>>();
                    message_log.Add(e.Server.Name, channel_messages);

                    List<string> msgs = new List<string>();
                    channel_messages.Add(e.Channel.Name, msgs);
                    channel_messages[e.Channel.Name].Add(e.User + " : [" + e.After.Text + "] --> [" + e.After.Text + " (" + e.After.Timestamp + ")");
                    //Console.WriteLine(e.User + " has edited " + e.Before + " to " + e.After);
                }
            };

            discord.MessageDeleted += async (s, e) =>
            {
                if (e.Message.IsAuthor && !e.Message.IsAuthor)
                {
                    await e.Channel.SendMessage("Something went wrong. @Accyrsed#5387, please look into this.");
                }

                if (message_log.ContainsKey(e.Server.Name))
                {

                    if (message_log[e.Server.Name].ContainsKey(e.Channel.Name))
                    {
                        Dictionary<string, List<string>> channel_messages = message_log[e.Server.Name];
                        channel_messages[e.Channel.Name].Add(e.User + " : " + e.Message.Text + " (" + e.Message.Timestamp + ")");
                        //Console.WriteLine(e.User + " has deleted " + e.Message + " from " + e.Channel);
                    }
                    else
                    {
                        Dictionary<string, List<string>> channel_messages = message_log[e.Server.Name];
                        List<string> msgs = new List<string>();
                        channel_messages.Add(e.Channel.Name, msgs);
                        channel_messages[e.Channel.Name].Add(e.User + " : " + e.Message.Text + " (" + e.Message.Timestamp + ")");
                        //Console.WriteLine(e.User + " has deleted " + e.Message + " from " + e.Channel);
                    }
                }
                else
                {
                    Dictionary<string, List<string>> channel_messages = new Dictionary<string, List<string>>();
                    message_log.Add(e.Server.Name, channel_messages);

                    List<string> msgs = new List<string>();
                    channel_messages.Add(e.Channel.Name, msgs);
                    channel_messages[e.Channel.Name].Add(e.User + " : " + e.Message.Text + " (" + e.Message.Timestamp + ")");
                    //Console.WriteLine(e.User + " has deleted " + e.Message + " from " + e.Channel);
                }
            };

            discord.UsingCommands(x =>
            {
                x.PrefixChar = ':';
                x.AllowMentionPrefix = true;
                Console.WriteLine("commands set up with char " + x.PrefixChar);
            });

            commands = discord.GetService<CommandService>();

            Console.WriteLine("command set up");

            discord.ExecuteAndWait(async () =>
            {
                await (discord.Connect("MzA4NzIwODE3NjcwNDU1Mjk3.C-k-Lw.pBo6XzY2bbG1eDkbBrtoXnmmkQI", TokenType.Bot));
                Console.WriteLine("client connected");
            });
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
