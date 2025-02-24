using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParanTwitchIntegration.Domain.Models
{
    public class TwitchAccount
    {
        public string BotUsername { get; set; }
        public string ChannelName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public string UserId { get; set; }

        public DateTime Expiry { get; set; }
    }
}
