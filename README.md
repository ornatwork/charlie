![alt text](robot-head.png) 

Charlie is a Microsoft Teams bot that will answer to different commands.

Based on code from https://github.com/Microsoft/BotBuilder-Samples

# To try this sample

- [Optional] Update the `appsettings.json` with your botFileSecret.  For Azure Bot Service bots, you can find the botFileSecret under application settings.

# Running Locally

## Visual Studio
- Open Charlie.csproj in Visual Studio.
- Run the project (press `F5` key).

## .NET Core CLI
- Install the [.NET Core CLI tools](https://docs.microsoft.com/dotnet/core/tools/?tabs=netcore2x).
- Type `dotnet run`.

## Testing the bot using Bot Framework Emulator
[Microsoft Bot Framework Emulator](https://github.com/microsoft/botframework-emulator) is a desktop application that allows bot
developers to test and debug their bots on localhost or running remotely through a tunnel.
- Install the Bot Framework emulator from [here](https://aka.ms/botframeworkemulator).

## Connect to bot using Bot Framework Emulator **V4**
- Launch the Bot Framework Emulator.
- File -> Open bot and navigate to `/src` folder.
- Select `charlie.bot` file.
# Bot state
A key to good bot design is to track the context of a conversation, so that your bot remembers things like the answers to previous questions. Depending on what your bot is used for, you may even need to keep track of conversation state or store user related information for longer than the lifetime of one given conversation.
