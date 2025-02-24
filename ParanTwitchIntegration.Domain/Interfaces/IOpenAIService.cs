using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParanTwitchIntegration.Domain.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> GenerateResponseAsync(string prompt, string username, string character);
        Task<string> GenerateThanksAsync(string username, string character);
        Task<string> GenerateRandomResponseAsync(string username, string message, string character);
    }
}
