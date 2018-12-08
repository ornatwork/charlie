// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Charlie;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace Charlie
{
    /// <summary>
    /// Represents a bot that processes incoming activities.
    /// For each user interaction, an instance of this class is created and the OnTurnAsync method is called.
    /// This is a Transient lifetime service. Transient lifetime services are created
    /// each time they're requested. Objects that are expensive to construct, or have a lifetime
    /// beyond a single turn, should be carefully managed.
    /// For example, the <see cref="MemoryStorage"/> object and associated
    /// <see cref="IStatePropertyAccessor{T}"/> object are created with a singleton lifetime.
    /// </summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1"/>
    public class CharlieEcho : IBot
    {
        private readonly CharlieAccessors _accessors;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharlieEcho"/> class.
        /// </summary>
        /// <param name="accessors">A class containing <see cref="IStatePropertyAccessor{T}"/> used to manage state.</param>
        /// <param name="loggerFactory">A <see cref="ILoggerFactory"/> that is hooked to the Azure App Service provider.</param>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1#windows-eventlog-provider"/>
        public CharlieEcho(CharlieAccessors accessors, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new System.ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger<CharlieEcho>();
            _logger.LogTrace("Charlie turn start.");
            _accessors = accessors ?? throw new System.ArgumentNullException(nameof(accessors));
        }

        /// <summary>
        /// Every conversation turn for our Echo Bot will call this method.
        /// There are no dialogs used, since it's "single turn" processing, meaning a single
        /// request and response.
        /// </summary>
        /// <param name="turnContext">A <see cref="ITurnContext"/> containing all the data needed
        /// for processing this conversation turn. </param>
        /// <param name="cancellationToken">(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
        /// <seealso cref="BotStateSet"/>
        /// <seealso cref="ConversationState"/>
        /// <seealso cref="IMiddleware"/>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Handle Message activity type, which is the main activity type for shown within a conversational interface
            // Message activities may contain text, speech, interactive cards, and binary or unknown attachments.
            // see https://aka.ms/about-bot-activity-message to learn more about the message and other activity types
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                // Save the new turn count into the conversation state.
                await _accessors.ConversationState.SaveChangesAsync(turnContext);

                // Charlies response
                var responseMessage = string.Empty;

                // Current UTC time
                if (turnContext.Activity.Text.ToLower().Equals("utc"))
                {
                    responseMessage = "Current time UTC=" + DateTime.UtcNow + Environment.NewLine;
                }
                // Current time 
                else if (turnContext.Activity.Text.ToLower().Equals("time"))
                {
                    responseMessage = "Current time=" + DateTime.Now + Environment.NewLine;
                }
                // Base64 encode text
                else if (turnContext.Activity.Text.ToLower().IndexOf("base64encode") == 0)
                {
                    int where = "base64encode".Length;
                    string textToEncode = turnContext.Activity.Text.Substring(where, turnContext.Activity.Text.Length - where).Trim();
                    responseMessage = "base 64 encoded=" + Util.Base64Encode(textToEncode) + Environment.NewLine;
                }
                // Decode Base64
                else if (turnContext.Activity.Text.ToLower().IndexOf("base64decode") == 0)
                {
                    int where = "base64decode".Length;
                    string textToDecode = turnContext.Activity.Text.Substring(where, turnContext.Activity.Text.Length - where).Trim();
                    responseMessage = "your base64 decode=" + Util.Base64Decode(textToDecode) + Environment.NewLine;
                }
                // Random GUID 
                else if (turnContext.Activity.Text.ToLower().Equals("guid"))
                {
                    responseMessage = "random GUID=" + Guid.NewGuid() + Environment.NewLine;
                }
                // Weather 
                else if (turnContext.Activity.Text.ToLower().IndexOf("weather") == 0)
                {
                    int where = "weather".Length;
                    string location = turnContext.Activity.Text.Substring(where, turnContext.Activity.Text.Length - where).Trim();
                    responseMessage = "Current weather in " + Util.GetWeather(location) + Environment.NewLine;
                }
                // Nothing to do here 
                else
                {
                    responseMessage = "What does , " + turnContext.Activity.Text + " mean ? " + Environment.NewLine;
                }

                // Back to the user
                await turnContext.SendActivityAsync(responseMessage);
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected");
            }
        }
    }
}
