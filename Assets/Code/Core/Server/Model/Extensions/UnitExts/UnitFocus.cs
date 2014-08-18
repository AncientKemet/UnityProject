using System;
using System.Collections.Generic;
using Code.Core.Client.UI;
using Code.Core.Server.Model.Entities;
using Code.Core.Shared.Content.Types;
using Code.Libaries.Generic.Managers;
using Code.Libaries.Net.Packets.ForServer;
using Code.Libaries.UnityExtensions.Independent;
using UnityEngine;

namespace Code.Core.Server.Model.Extensions.UnitExts
{
    public class UnitFocus : EntityExtension
    {
        public List<Player> PlayersThatSelectedThisUnit = new List<Player>(5);

        public ServerUnit FocusedUnit { get; set; }

        public override void Progress()
        {
        }

    }
}
