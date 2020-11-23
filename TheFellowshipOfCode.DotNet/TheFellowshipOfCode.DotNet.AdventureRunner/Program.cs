using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HTF2020.GameController.Builders;
using HTF2020.GameController.State;
using McMaster.Extensions.CommandLineUtils;

namespace TheFellowshipOfCode.DotNet.AdventureRunner
{
    [Command(Name = "htf2020", Description = "The HTF2020 .NET Challenge")]
    public class Program
    {
        [Argument(0, Description = "Path to a valid map file. Map files have the '.htf' extension.")]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        private string MapPath { get; } = "TheFellowshipOfCode.DotNet.YourAdventure.htf";

        [Argument(1, Description = "Path to a valid DLL containing a class that implements the 'IAdventure' interface from the 'HTF2020.Contracts' nugetpackage.")]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        private string AdventureDllPath { get; } = "TheFellowshipOfCode.DotNet.YourAdventure.dll";

        [Option("-v|--verbose", "This is the verbose option. If you turn this on the game will be displayed.", CommandOptionType.NoValue)]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        private bool OutputGame { get; } = true;

        [Option("-t|--time",
            "This is the time between turns in milliseconds. Only use this if you also use the '-v|--verbose' option.",
            CommandOptionType.SingleValue)]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        private int TimeBetweenTurns { get; } = 1000;

        public static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        private async Task OnExecute()
        {
            if (string.IsNullOrEmpty(MapPath)) return;
            if (string.IsNullOrEmpty(AdventureDllPath)) return;

            var builder = new GameBuilder();
            var runner = builder.BuildGameRunner(MapPath, AdventureDllPath).Result;

            GameState turn;
            do
            {
                await Task.Delay(TimeBetweenTurns);
                turn = await runner.PlayTurn();
                if (!OutputGame) continue;
                Console.WriteLine(turn.TurnCounter);
                Console.WriteLine(runner.ToString());
            } while (!turn.IsFinished);

            Console.WriteLine("============ FINISHED ============");
            Console.WriteLine("Total rounds played: " + turn.TurnCounter);
            Console.WriteLine("Total balance: " + turn.Party.Balance);
            Console.WriteLine("Total health left: " +
                              turn.Party.Members.Select(member => member.HealthPoints).Sum());
            Console.WriteLine("==================================");
        }
    }
}