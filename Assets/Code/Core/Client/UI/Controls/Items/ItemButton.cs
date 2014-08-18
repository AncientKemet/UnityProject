using Code.Core.Shared.Content.Types;
using Code.Libaries.GameObjects;
using UnityEngine;

namespace Code.Core.Client.UI.Controls.Items
{
    [RequireComponent(typeof(InterfaceButton))]

    public class ItemButton : MonoBehaviour 
    {
        private Item _item;
        private GameObject ItemModel;

        public Item Item
        {
            get { return _item; }
            set
            {
                if (ItemModel != null)
                {
                    Destroy(ItemModel);
                }

                _item = value;

                ItemModel = (GameObject) Instantiate(Item.gameObject);

                ItemModel.transform.parent = transform;
                ItemModel.transform.localPosition = Vector3.zero;
                ItemModel.transform.localRotation = Quaternion.identity;

                ItemModel.layer = gameObject.layer;

                foreach (var t in TransformHelper.GetChildren(ItemModel.transform))
                {
                    t.gameObject.layer = gameObject.layer;
                }

            }
        }
    }
}
