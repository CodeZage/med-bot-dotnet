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
#if DEBUG // Loads the correct .env file
        var path = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
        Console.WriteLine($"Looking for .env files here: {path!}");
        DotEnv.LoadEnvironmentVariables(Path.Combine(path!, ".env"));
        
        // When running in production add the environment variable on startup
#endif

        var token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");

        Console.WriteLine("this is your token " + token);

        return new Program().MainAsync();
    }

    private async Task MainAsync()
    {
        _client = new DiscordSocketClient();

        _client.Log += Log;
        _client.Ready += ClientReady;
        _client.SlashCommandExecuted += SlashCommandHandler;

        await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN"));
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    #region Client Functions

    private async Task ClientReady()
    {
        var guild = _client.GetGuild(813033894659620874); // Defines the Medialogy discord server

        var studentCommand =
            new SlashCommandBuilder().WithName("student").WithDescription("Declare yourself a student.");

        var selectYearCommand = new SlashCommandBuilder().WithName("select-year")
            .WithDescription("Select your current study year.").AddOption(new SlashCommandOptionBuilder()
                .WithName("year")
                .WithDescription("Your current study year.")
                .WithRequired(true)
                .AddChoice("1st Year", 1)
                .AddChoice("2nd Year", 2)
                .AddChoice("3rd Year", 3)
                .AddChoice("4th Year", 4)
                .AddChoice("5th Year", 5)
                .WithType(ApplicationCommandOptionType.Integer)
            );

        var commands = new List<SlashCommandBuilder> { studentCommand, selectYearCommand };

        try
        {
            foreach (var command in commands) await guild.CreateApplicationCommandAsync(command.Build());
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

    #endregion

    #region Command Handling

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        switch (command.Data.Name)
        {
            case "student":
                await StudentCommandHandler(command);
                break;

            case "select-year":
                await SelectYearCommandHandler(command);
                break;
        }
    }

    private async Task SelectYearCommandHandler(SocketSlashCommand command)
    {
        switch ((long)command.Data.Options.First().Value)
        {
            case 1:
                await ((IGuildUser)command.User).AddRoleAsync(813118674642403343);
                await command.RespondAsync("You are now a 1st year student.", ephemeral: true);
                break;
            case 2:
                await ((IGuildUser)command.User).AddRoleAsync(813124643799302146);
                await command.RespondAsync("You are now a 2nd year student.", ephemeral: true);
                break;
            case 3:
                await ((IGuildUser)command.User).AddRoleAsync(813124991641845771);
                await command.RespondAsync("You are now a 3rd year student.", ephemeral: true);
                break;
            case 4:
                await ((IGuildUser)command.User).AddRoleAsync(813127288836849694);
                await command.RespondAsync("You are now a 4th year student.", ephemeral: true);
                break;
            case 5:
                await ((IGuildUser)command.User).AddRoleAsync(813131657791012884);
                await command.RespondAsync("You are now a 5th year student.", ephemeral: true);
                break;
        }
    }

    private async Task StudentCommandHandler(SocketSlashCommand command)
    {
        await ((IGuildUser)command.User).AddRoleAsync(892799588191313950);
        await command.RespondAsync("You are now a student 🐸.", ephemeral: true);
    }

    #endregion
}