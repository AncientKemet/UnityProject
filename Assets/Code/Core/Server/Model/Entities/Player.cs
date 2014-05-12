using System;
using OldBlood.Code.Core.Server.Model.Extensions;

namespace OldBlood.Code.Core.Server.Model.Entities
{
    public class Player : WorldEntity
    {

        public Player(ServerClient client)
        {
            AddExtension(client);
        }

        public override void Progress()
        {
            base.Progress();
        }
    }
}

