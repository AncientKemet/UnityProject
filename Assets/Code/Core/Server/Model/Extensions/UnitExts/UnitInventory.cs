
#if SERVER
using Server.SQL;
using System.Collections.Generic;
using Code.Core.Shared.Content.Types;
using Server.Model.Entities;
using Server.Model.Entities.Human;

namespace Server.Model.Extensions.UnitExts
{
    public class UnitInventory : EntityExtension
    {
        public enum AccessType
        {
            ALL,
            ONLY_THIS_UNIT
        }


        [SQLSerialize]
        private int _width = 1;

        [SQLSerialize]
        private int _height = 1;

        [SQLSerialize]
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
            for (int i = 0; i < Width * Height; i++)
            {
                _items.Add(null);
            }
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

        public bool AddItem(Item item)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (this[x, y] == null)
                    {
                        this[x, y] = item;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
#endif
