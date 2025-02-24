using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParanTwitchIntegration.Domain.Models
{
    public class EnvSettings
    {
        public string Environment { get; set; } // "local" ou "remote"
        public string LocalCommand { get; set; }
        public RemoteSettings Remote { get; set; }

        public class RemoteSettings
        {
            public string Hostname { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public int Port { get; set; }
        }
    }
}
