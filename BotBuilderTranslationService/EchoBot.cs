using System.Threading.Tasks;
using BotBuilderTranslationService.Services;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;

namespace BotBuilderTranslationService
{
    public class EchoBot : IBot
    {
        private IConfiguration configuration;
        private EchoState state;

        public EchoBot(IConfiguration config)
        {
            this.configuration = config;
        }

        /// <summary>
        /// Every Conversation turn for our EchoBot will call this method. In here
        /// the bot checks the Activty type to verify it's a message, bumps the 
        /// turn conversation 'Turn' count, and then echoes the users typing
        /// back to them. 
        /// </summary>
        /// <param name="context">Turn scoped context containing all the data needed
        /// for processing this conversation turn. </param>        
        public async Task OnTurn(ITurnContext context)
        {
            // This bot is only handling Messages
            if (context.Activity.Type == ActivityTypes.Message)
            {
                // Get the conversation state from the turn context
                state = context.GetConversationState<EchoState>();

                // Bump the turn count. 
                state.TurnCount++;

                // Detect user language for this message 
                var userLanguageCandidates = await TranslationService.DetectAsync(context.Activity.Text, configuration["TranslationAPI"]);
                string userInputGerman = context.Activity.Text;
                // If the language of the user input could be detected and if it not English, set the native language
                if (userLanguageCandidates[0] != null && !userLanguageCandidates[0].language.Equals("en"))
                {
                    state.UserNativeLanguage = userLanguageCandidates[0].language;
                    var translationCandidates = await TranslationService.TranslateAsync(context.Activity.Text, "en", configuration["TranslationAPI"]);
                    if (translationCandidates[0].translations[0] != null)
                    {
                        await context.SendActivity(await translateToNativeLanguageAsync($"This was turn {state.TurnCount}: You sent ") + $"'{context.Activity.Text}'");
                    }
                }
                else
                {
                    // Echo back to the user whatever they typed.
                    await context.SendActivity($"This was turn {state.TurnCount}: You sent '{context.Activity.Text}'");
                }
            }
        }

        /// <summary>
        /// Helper method to translate text messages to the user's native language
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task<string> translateToNativeLanguageAsync(string text)
        {
            if (!state.UserNativeLanguage.Equals("en"))
            {
                string translation = text;
                var results = await TranslationService.TranslateAsync(text, state.UserNativeLanguage, configuration["TranslationAPI"]);
                if (results[0] != null)
                {
                    translation = results[0].translations[0].text;
                }
                return translation;
            }
            else return text;
        }
    }    
}
