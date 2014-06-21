﻿using StarLess;

namespace MiniMAL.Console.Commands
{
    public abstract class MiniMALCommand : Command
    {
        protected MiniMALClient _client;

        protected MiniMALCommand(MiniMALClient client, string keyword, string description)
            : base(keyword, description)
        {
            this._client = client;
        }
    }
}