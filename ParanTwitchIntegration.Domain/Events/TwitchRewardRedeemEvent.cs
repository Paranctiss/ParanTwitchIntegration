using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParanTwitchIntegration.Domain.Events
{
    public class TwitchRewardRedeemEvent
    {
        public string ChannelID { get; set; }
        public string RewardTitle { get; set; }
        public string UserInput { get; set; }
        public string UserName { get; set; }

        public TwitchRewardRedeemEvent(string channelID, string rewardTitle, string userInput, string userName)
        {
            ChannelID = channelID;
            RewardTitle = rewardTitle;
            UserInput = userInput;
            UserName = userName;
        }
    }
}
