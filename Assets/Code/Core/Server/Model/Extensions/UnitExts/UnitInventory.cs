using System;
using System.Collections.Generic;
using Code.Core.Client.UI;
using Code.Core.Server.Model.Entities;
using Code.Core.Shared.Content.Types;

namespace Code.Core.Server.Model.Extensions.UnitExts
{
    public class UnitInventory2 : EntityExtension
    {
        public enum AccessType
        {
            ALL,
            ONLY_THIS_UNIT
        }

        private int _width = 1;
        private int _height = 1;
        private List<Item> _items; 

        public ServerUnit Unit { get; private set; }
        public AccessType AccesType { get; set; }

        public List<Player> ListeningPlayers = new List<Player>(); 

        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                RecreateInventory();
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                RecreateInventory();
            }
        }

        private void RecreateInventory()
        {
            _items = new List<Item>(Width * Height);
        }

        public Item this[int x, int y]
        {
            get { return _items[y * Height + x]; }
            set
            {
                _items[y*Height + x] = value;
                if (ListeningPlayers.Count > 0)
                {
                    SendUpdateToPlayers(x, y);
                }
            }
        }

        private void SendUpdateToPlayers(int x, int y)
        {
            var item = this[x, y];
            foreach (var player in ListeningPlayers)
            {
                if(player == null)
                    continue;

                player.ClientUi.Inventories[Unit.ID, x, y] = item;
            }
        }

        protected override void OnExtensionWasAdded()
        {
            base.OnExtensionWasAdded();
            Unit = entity as ServerUnit;
            RecreateInventory();
        }

        public override void Progress()
        {
            
        }

        public bool HasAcces(ServerUnit unit)
        {
            if(AccesType == AccessType.ALL)
                return true;
            if (AccesType == AccessType.ONLY_THIS_UNIT)
                return unit == Unit;
            return false;
        }
    }
}
