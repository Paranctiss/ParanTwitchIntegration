// ParanTwitchIntegration.Application/Services/OpenAIService.cs
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ParanTwitchIntegration.Domain.Interfaces;
using ParanTwitchIntegration.Domain.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParanTwitchIntegration.Application.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _client;
        private readonly OpenAISettings _settings;

        public OpenAIService(HttpClient client, IOptions<OpenAISettings> options)
        {
            _client = client;
            _settings = options.Value;

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.ApiKey}");
        }

        public async Task<string> GenerateResponseAsync(string prompt, string username, string character)
        {
            string characterDescription = GetCharacterDescription(character);
            var requestBody = CreateRequestBody(characterDescription, prompt, username);
            return await SendRequestAsync(requestBody);
        }

        public async Task<string> GenerateThanksAsync(string username, string character)
        {
            string characterDescription = GetCharacterDescription(character);
            var requestBody = CreateRequestBody(characterDescription, $"{username} a rejoint la chaîne", username);
            return await SendRequestAsync(requestBody);
        }

        public async Task<string> GenerateRandomResponseAsync(string username, string message, string character)
        {
            string characterDescription = GetCharacterDescription(character);
            var requestBody = CreateRequestBody(characterDescription, message, username);
            return await SendRequestAsync(requestBody);
        }

        private string GetCharacterDescription(string character)
        {
            return character.ToLower() switch
            {
                "gruntilda" => $"Tu es Gruntilda du jeu Banjo-Kazooie. Tu es sarcastique, méchante et toujours prête à ridiculiser les autres. Soit méchante et fait preuve d'humour noir. Sois brève et directe, n'hésite pas à balancer des piques bien senties. N'oublie pas de ne pas être gentille et de garder une attitude détestable. N'ait pas peur de blesser les gens avec tes réponses très prononcées",
                "banjo" => $"Tu es Banjo du jeu Banjo-Kazooie. Tu es joyeux, enthousiaste et toujours prêt à encourager les autres. Réponds de manière chaleureuse et amicale. Sois excité, joyeux, et fais preuve d'un humour léger pour rendre la conversation agréable. Montre que tu es un vrai pote, prêt à rigoler avec les autres.",
                "kazooie" => $"Tu es Kazooie du jeu Banjo-Kazooie. Tu es sarcastique, un peu moqueuse et jamais à court de répliques piquantes. Fais des remarques sarcastiques et des commentaires irrévérencieux, tout en restant amusante. Garde ton ton mordant et sois toujours prête à taquiner, tu peux être sarcastique et hypocrite.",
                _ => "Tu es un personnage générique. Quand un utilisateur t'écrit, réponds de manière neutre et directe. Sois simple et concise, sans ajouter de personnalité particulière."
            };
        }

        private object CreateRequestBody(string characterDescription, string prompt, string username)
        {
            return new
            {
                model = _settings.Model,
                messages = new[]
                {
                    new
                    {
                        role = "developer",
                        content = $"{characterDescription} L'utilisateur {username} a envoyé ce message sur le tchat'{prompt}', trouve un moyen de rebondir sur son message en incarnant parfaitement ton personnage. Tu peux citer son pseudo au début, et même parfois un passage de son message pertinenet à ta réponse. Essaie de ne pas dépasser plus ou moins 150 caractères, mais ne te gène pas pour faire des phrases plus courtes et efficace. Répond sérieusement aux messages même si ils ont l'air un peu bête. Et tu adores la tarte au caca et tout ce qui touche au caca, mais si le message ne mentionne pas du caca ou autre en rapport avec le caca, ne parle pas de caca."
                    },
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                }
            };
        }

        private async Task<string> SendRequestAsync(object requestBody)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(_settings.Url, jsonContent);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);
                return jsonResponse.choices[0].message.content;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'appel à OpenAI: {ex.Message}");
                return null;
            }
        }
    }
}