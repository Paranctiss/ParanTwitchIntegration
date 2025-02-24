﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParanTwitchIntegration.Domain.Models
{
    public class OpenAISettings
    {
        public string ApiKey { get; set; }
        public string Url { get; set; }
        public string Model { get; set; }
    }
}
