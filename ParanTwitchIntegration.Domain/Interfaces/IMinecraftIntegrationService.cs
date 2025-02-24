using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParanTwitchIntegration.Domain.Interfaces
{
    public interface IMinecraftIntegrationService
    {
       void ExecuteCommand(string command);
    }
}
