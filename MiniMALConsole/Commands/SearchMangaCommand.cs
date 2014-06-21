﻿using System.Collections.Generic;
using System.Linq;
using MiniMAL.Exceptions;

namespace MiniMAL.Console.Commands
{
    public class SearchMangaCommand : MiniMALCommand
    {
        public SearchMangaCommand(MiniMALClient client)
            : base(client, "search-manga", "Search a manga in MyAnimeList database.")
        {
            UnlimitedArguments = true;
        }

        protected override void Action(ArgumentsValues arguments, OptionsValues options)
        {
            try
            {
                List<MangaSearchEntry> search = client.SearchManga(arguments.Values.ToArray());
                foreach (MangaSearchEntry a in search)
                    System.Console.WriteLine(a.Title);
                System.Console.WriteLine(search.Count() + " entries");
            }
            catch (UserNotConnectedException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}