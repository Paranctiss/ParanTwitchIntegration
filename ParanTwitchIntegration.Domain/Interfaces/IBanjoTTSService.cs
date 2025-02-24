using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;

namespace ParanTwitchIntegration.Domain.Interfaces
{
    public interface IBanjoTTSService
    {
        public Task BanjoTTSFromChatAsync(string message, string username, string channelName, TwitchClient _twitchClient);
        public Task ThanksFollow(string username);

        public Task RandomTTS(string username, string message);
    }
}
