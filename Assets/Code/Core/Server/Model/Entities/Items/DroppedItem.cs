#if SERVER
using Code.Core.Shared.Content.Types;
using Code.Core.Shared.Content.Types.ItemExtensions;
using Server.Model.Extensions.UnitExts;

namespace Server.Model.Entities.Items
{
    public class DroppedItem : ServerUnit
    {
        private Item _item;

        public Item Item
        {
            get { return _item; }
            set
            {
                _item = value;

                name = Item.name;

                Display.Item = _item;
                Movement.CanMove = false;
                Movement.CanRotate = false;

                ItemWithInventory withInventory = value.GetComponent<ItemWithInventory>();

                if (withInventory != null)
                {
                    UnitInventory inventory;

                    AddExt(inventory = new UnitInventory());

                    inventory.Width = withInventory.Width;
                    inventory.Height = withInventory.Height;
                }
            }
        }
    }
}

#endif
