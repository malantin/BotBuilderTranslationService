# An echo bot using a custom service class for translating user messages through the Microsoft Translator API
Sometimes you need translation capabilities in your bot but require a more granular control than the [https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-howto-translation?view=azure-bot-service-4.0&tabs=cs](Translation Middleware) offers.
For example, you want to decide for each message you receive whether you want to translate the message or not. Or you might want to translate back just parts of your bot's messages to the user.

This sample example is built on the Echo bot template and shows how use a custom service class for detecting the input language and translating text.

Feel free to comment of contribute.
