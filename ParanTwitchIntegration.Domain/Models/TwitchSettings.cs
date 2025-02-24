using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParanTwitchIntegration.Domain.Models
{
    public class TwitchSettings
    {
        public string BotUsername { get; set; }
        public string ChannelName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

}
