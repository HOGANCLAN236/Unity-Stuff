from discord.ext import commands
import discord
#This is old code, may not work properly!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
BOT_TOKEN = ""
ReportChat = 0
generalChat = 0

SEVER_ID = 0

list = ["fuck", "shit", "bitch", "nigger", "nigga", "cunt", "twat", "dick", "cock", "whore", "pussy"]

bot = commands.Bot(command_prefix="!", intents=discord.Intents.all())

@bot.event
async def on_ready():
    print('Logger Connected!')
    channel = bot.get_channel(generalChat)
    await channel.send("Logger Connected!!!")

@bot.event
async def on_message(message):
    guild = bot.get_guild(SEVER_ID)
    

    if guild is None:
        print("No Guild")
        return
        
    channel = guild.get_channel(ReportChat)

    if message.author == bot.user:
        return

    for i in list:   
        if message.content.startswith(i):
            await channel.send("Message: " + i + " | " + "User: " + message.author.name)

@bot.event
async def on_message_edit(before, after):
    guild = bot.get_guild(SEVER_ID)
    

    if guild is None:
        print("No Guild")
        return
        
    channel = guild.get_channel(ReportChat)
    if after.author == bot.user:
        return

    for i in list:   
        if after.content.startswith(i):
            await channel.send("Edited Message: " + i + " | " + "User: " + after.author.name)


bot.run(BOT_TOKEN)
