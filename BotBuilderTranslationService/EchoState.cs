﻿namespace BotBuilderTranslationService
{
    /// <summary>
    /// Class for storing conversation state. 
    /// </summary>
    public class EchoState
    {
        public int TurnCount { get; set; } = 0;
        public string UserNativeLanguage = "en";
    }
}
