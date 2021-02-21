# Nano Twitch Bot
## A Nano donation bot for Twitch chat

This bot monitors a Nano account for pending receives and posts donation messages in a channel's Twitch chat.

## What's Nano?

Nano is a cryptocurrency that's instant, feeless, and eco-friendly. It's perfect for microdonations! Go to [nano.org](https://nano.org) for more information.

## Getting Started

Before you run the bot, make sure your Nano wallet is locked or that the wallet is set to manually receive pending receives. The bot can only see **pending** receives.

Run NanoTwitchBot.exe and answer the prompts. The answers to these prompts will be remembered for future runs, except for the Twitch Auth Token.

The Twitch Auth Token can be obtained from: [https://twitchapps.com/tmi](https://twitchapps.com/tmi)

At the end of your stream, receive any pending receives. If you don't, they'll show up as new donations the next time the bot is run.

## Chat Commands

### !nano

Displays a help message.

### !nano_add

Adds an alias. If an account has an alias, that alias is displayed instead of Anonymous. Alias are stored locally on the machine running the bot.

### !nana_delete

Deletes an alias.
