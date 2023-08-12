using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Rystem.OpenAi.Chat;

namespace ExceptionAnalyzer
{
    internal static class OpenAiChatExtension
    {
        internal static ChatRequestBuilder RequestCollection(this IOpenAiChat chat, [NotNull] IEnumerable<ChatMessage> messages)
        {
            if (messages == null) throw new ArgumentNullException(nameof(messages));
            ChatRequestBuilder chatRequestBuilder = chat.Request(messages.First());
            foreach (ChatMessage message in messages.Reverse())
            {
                if (message == messages.First()) break;
                chatRequestBuilder.AddMessage(message);
            }

            return chatRequestBuilder;
        }
    }
}
