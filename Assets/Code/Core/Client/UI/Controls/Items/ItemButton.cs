using Code.Core.Shared.Content.Types;
using Code.Libaries.GameObjects;
using Shared.Content.Types.ItemExtensions;
using UnityEngine;

namespace Code.Core.Client.UI.Controls.Items
{
    [RequireComponent(typeof(InterfaceButton))]

    public class ItemButton : MonoBehaviour 
    {
        private Item _item;
        private tk2dSlicedSprite _background;
        private Color _originalColor, _onHoverColor;
        private GameObject ItemModel;

        private void Start()
        {
            
        }

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
                ItemModel.transform.localPosition = _item.Position;
                ItemModel.transform.localEulerAngles = _item.Rotation;
                ItemModel.transform.localScale = _item.Scale;

                ItemModel.layer = gameObject.layer;

                foreach (var t in TransformHelper.GetChildren(ItemModel.transform))
                {
                    t.gameObject.layer = gameObject.layer;
                }

                ItemModel.AddComponent<ItemInUI>();

            }
        }

        public tk2dSlicedSprite Background
        {
            get { return _background; }
            set
            {
                _background = value;
                if (value != null)
                {
                    InterfaceButton button = GetComponent<InterfaceButton>();

                    _originalColor = _background.color;
                    _onHoverColor = _originalColor / 2f;
                    _onHoverColor.a = _originalColor.a;

                    button.OnMouseIn += () =>
                    {
                        _background.color = _onHoverColor;
                        _background.ForceBuild();
                    };

                    button.OnMouseOff += () =>
                    {
                        _background.color = _originalColor;
                        _background.ForceBuild();
                    };
                }
            }
        }
    }
}
