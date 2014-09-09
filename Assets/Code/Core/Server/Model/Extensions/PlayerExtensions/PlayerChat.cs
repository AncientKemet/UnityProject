using System.Collections.Generic;
using Code.Core.Server.Model.Entities;
using Code.Core.Shared.NET;
using UnityEngine;
using Code.Libaries.Net.Packets.ForServer;

namespace Code.Core.Server.Model.Extensions.PlayerExtensions
{
    public class PlayerChat : EntityExtension
    {
        public Player Player { get; private set; }


        public override void Progress()
        {
        }

        protected override void OnExtensionWasAdded()
        {
            base.OnExtensionWasAdded();
            Player = entity as Player;
        }

        internal void HandlePacket(ChatPacket p)
        {
            if(p.type == ChatPacket.ChatType.Public)
            {
                Say(p.text);
            }else if (p.type == ChatPacket.ChatType.Party)
            {
                SendPartyMessage(Player.name + ": " + p.text);
            }
        }

        private void Say(string message)
        {

            ChatPacket p = new ChatPacket();

            p.FROM_SERVER_UnitID = Player.ID;
            p.type = ChatPacket.ChatType.Public;
            p.text = Player.name+": "+message;

            foreach (var e in Player.CurrentBranch.ObjectsVisible)
            {
                if(e is Player)
                {
                    Player other = e as Player;

                    other.Client.ConnectionHandler.SendPacket(p);
                }
            }
        }

        public void RecievePartyMessage(string message)
        {
            ChatPacket p = new ChatPacket();

            p.type = ChatPacket.ChatType.Party;
            p.text = message;

            Player.Client.ConnectionHandler.SendPacket(p);
        }

        public void SendPartyMessage(string message)
        {
            if (Player.Party != null)
            {
                Player.Party.SayInParty(message);
            }
        }
    }
}
