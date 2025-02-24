using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParanTwitchIntegration.Domain.Models
{
    public class TwitchToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiry { get; set; }
    }

}
