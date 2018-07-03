using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BotBuilderTranslationService.Services
{
    public class Candidate
    {
        public string language { get; set; }
        public float score { get; set; }
        public bool isTranslationSupported { get; set; }
        public bool isTransliterationSupported { get; set; }
        public Alternative[] alternatives { get; set; }
    }

    public class Textausgabe
    {
        public string text { get; set; }
        public string to { get; set; }
    }

    public class Alternative
    {
        public string language { get; set; }
        public float score { get; set; }
        public bool isTranslationSupported { get; set; }
        public bool isTransliterationSupported { get; set; }
    }

    public class TranslationResult
    {
        public Candidate detectedLanguage { get; set; }
        public Textausgabe[] translations { get; set; }
    }

    public static class TranslationService
    {
        public async static Task<Candidate[]> DetectAsync(string text, string apiKey)
        {
            System.Object[] body = new System.Object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri("https://api.cognitive.microsofttranslator.com/detect?api-version=3.0");
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", apiKey);

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Candidate[]>(responseBody);

                return result;
            }
        }

        public async static Task<TranslationResult[]> TranslateAsync(string text, string targetLanguage, string apiKey)
        {
            System.Object[] body = new System.Object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri("https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&to=" + targetLanguage);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", apiKey);

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TranslationResult[]>(responseBody);

                return result;
            }
        }
    }
}
