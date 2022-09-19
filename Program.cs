using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace MedBot;

public class Program
{
    private DiscordSocketClient _client;

    public static Task Main(string[] args)
    {
        return new Program().MainAsync();
    }

    private async Task MainAsync()
    {
        _client = new DiscordSocketClient();

        _client.Log += Log;
        _client.Ready += ClientReady;

        var token = "ODEzMDQwNzQyNzU3OTU3NjUy.Gy_Nzp.TVGI6exLy3t7zZiLkLIeHH8GZrWxk09I4GqC7o";

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private async Task ClientReady()
    {
        var guild = _client.GetGuild(813033894659620874);

        var studentCommand =
            new SlashCommandBuilder().WithName("student").WithDescription("Declare yourself a student.");

        try
        {
            await guild.CreateApplicationCommandAsync(studentCommand.Build());
        }
        catch (HttpException exception)
        {
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            Console.WriteLine(json);
        }
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }


    #region Command Handling

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        switch (command.Data.Name)
        {
            case "student":
                await StudentCommandHandler(command);
                break;
        }
    }

    private async Task StudentCommandHandler(SocketSlashCommand command)
    {
        await (command.User as IGuildUser).AddRoleAsync(892799588191313950);
        await command.RespondAsync("You are now a student 🐸.");
    }

    #endregion
}