using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;

namespace ParanTwitchIntegration.Domain.Events
{
    public class TwitchMessageReceivedEvent
    {
        public string Message { get; }
        public string Username { get; }
        public string ChannelName { get; }

        public TwitchClient Client { get; }

        public TwitchMessageReceivedEvent(string message, string username, string channelName, TwitchClient client)
        {
            Message = message;
            Username = username;
            ChannelName = channelName;
            Client = client;
        }
    }
}
