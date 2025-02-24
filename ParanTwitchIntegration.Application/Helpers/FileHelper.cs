using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParanTwitchIntegration.Application.Helpers
{
    public class FileHelper
    {

        private string filePath = @"C:\Users\fli1\source\repos\ParanTwitchIntegration\ParanTwitchIntegration\Assets\textToSpeech.txt";

        // Méthode pour écrire ou modifier le fichier texte
        public void WriteToFile(string character, string message)
        {
            string content = character + "|" + message.ToUpper();
            try
            {
                // Vérifie si le fichier existe
                if (File.Exists(filePath))
                {
                    // Si le fichier existe, écris le contenu et écrase l'ancien fichier
                    File.WriteAllText(filePath, content);
                }
                else
                {
                    // Si le fichier n'existe pas, le crée et écrit dedans
                    File.WriteAllText(filePath, content);
                }

                Console.WriteLine("Fichier mis à jour avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'écriture dans le fichier : {ex.Message}");
            }
        }

        public void AppendToFile(string content)
        {
            try
            {
                // Ajoute du texte à la fin du fichier
                File.AppendAllText(filePath, content);
                Console.WriteLine("Contenu ajouté avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout au fichier : {ex.Message}");
            }
        }


        public string ReadFileText()
        {
            return File.ReadAllText(filePath);
        }
    }
}
