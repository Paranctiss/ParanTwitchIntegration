using Microsoft.AspNetCore.Mvc;
using ParanTwitchIntegration.Application.Helpers;
using ParanTwitchIntegration.Application.Services;
using ParanTwitchIntegration.Domain.Interfaces;

namespace ParanTwitchIntegration.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BanjoTTSController : ControllerBase
    {
        private readonly OBSSockets _obsSockets;
        private readonly IBanjoTTSService _banjoTTSHelper;

        public BanjoTTSController(OBSSockets obsSockets, IBanjoTTSService banjoTTSHelper)
        {
            _obsSockets = obsSockets;
            _banjoTTSHelper = banjoTTSHelper;
        }

        [HttpGet(Name = "File")]
        public string Get()
        {
            FileHelper fileHelper = new FileHelper();
            return fileHelper.ReadFileText();
        }

        [HttpPost("Reset")]
        public IActionResult Reset()
        {
            try
            {
                _obsSockets.HideElementBanjo();
                return Ok("Élément caché avec succès !");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur : {ex.Message}");
            }
        }

        [HttpGet("NewFollow/{username}")]
        public void PlayNewFollow(string username) {
            _banjoTTSHelper.ThanksFollow(username);
        }

        [HttpGet("Message/{username}/{message}")]
        public void PlayTTSRndMessage(string username, string message)
        {
           _banjoTTSHelper.RandomTTS(username, message);
        }
    }
}
