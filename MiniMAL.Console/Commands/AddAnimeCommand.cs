﻿using MiniMAL.Anime;
using MiniMAL.Console.Commands.Abstract;
using MiniMAL.Types;
using StarLess;

namespace MiniMAL.Console.Commands
{
    internal class AddAnimeCommand : MiniMALCommand
    {
        public AddAnimeCommand(MiniMALClient client)
            : base(client, "add-anime", "Add an anime in the user's animelist")
        {
            RequiredArguments.Add(new Argument<int>("id", "anime id in MyAnimeList database"));
            RequiredArguments.Add(new Argument<WatchingStatus>("status",
                "1-watching, 2-completed, 3-on hold, 4-dropped, 6-plan to watch"));

            var score = new Option("s", "score", "change score");
            score.Arguments.Add(new Argument<int>("value", "1-10, 0=none", Validator.ValueRange("value", 0, 10)));
            Options.Add(score);

            var priority = new Option("p", "priority", "define priority");
            priority.Arguments.Add(new Argument<Priority>("value", "0-low, 1-medium, 2-high"));
            Options.Add(priority);
        }

        protected override void Action(ArgumentsValues args, OptionsValues options)
        {
            AnimeRequestData data = AnimeRequestData.DefaultAddRequest(args.Value<WatchingStatus>("status"));

            if (options.ContainsKey("score"))
                data.Score = options["score"].Value<int>("value");

            if (options.ContainsKey("priority"))
                data.Priority = options["priority"].Value<Priority>("value");

            MiniMALClient.AddRequestResult result = Client.AddAnime(args.Value<int>("id"), data);

            switch (result)
            {
                case MiniMALClient.AddRequestResult.Created:
                    System.Console.WriteLine("Created");
                    break;
                case MiniMALClient.AddRequestResult.AlreadyInTheList:
                    System.Console.WriteLine("Already in your list.");
                    break;
            }
        }
    }
}