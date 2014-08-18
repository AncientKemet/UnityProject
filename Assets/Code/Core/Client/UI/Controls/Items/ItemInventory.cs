using System.Collections.Generic;
using Code.Core.Shared.Content.Types;
using Code.Libaries.Generic.Managers;
using UnityEngine;

namespace Code.Core.Client.UI.Controls.Items
{
    [AddComponentMenu("Kemet/UI/Item Inventory")]
    [ExecuteInEditMode]
    public class ItemInventory : UIControl
    {
        [Range(1, 10)]
        public int Width = 1;
        [Range(1, 10)]
        public int Height = 1;

        private bool _rebuild = false;

        [HideInInspector]
        [SerializeField] private List<ItemButton> buttons = new List<ItemButton>();

        public void ForceRebuild()
        {
            Build();
        }

        public void SetItem(int x, int y, int itemId)
        {
            buttons[x + y * Height].Item = ContentManager.I.Items[itemId];
        }
        public void SetItem(int x, int y, Item item)
        {
            buttons[x + y*Height].Item = item;
        }

        private void Build()
        {
            if (UIContentManager.I == null || UIContentManager.I.ItemButtonBackGround == null || UIContentManager.I.ItemButtonBackGround.renderer == null)
            {
                Debug.LogError("Error");
                return;
            }

            Vector3 step = Vector3.zero;

            for (int i = 0; i < buttons.Count; i++)
            {
                var itemButton = buttons[i];
                if(itemButton == null) continue;
#if UNITY_EDITOR
                DestroyImmediate(itemButton.gameObject);
#else
                Destroy(itemButton.gameObject);
#endif
            }

            buttons = new List<ItemButton>(Width * Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    ItemButton button = new GameObject("itemButton").AddComponent<ItemButton>();

                    tk2dSlicedSprite buttonBackGround = ((GameObject)Instantiate(UIContentManager.I.ItemButtonBackGround.gameObject)).GetComponent<tk2dSlicedSprite>();

                    step = buttonBackGround.renderer.bounds.size;

                    buttonBackGround.transform.parent = button.transform;
                    buttonBackGround.transform.localPosition = new Vector3(0,0,1);
                    
                    buttonBackGround.gameObject.layer = gameObject.layer;
                    button.gameObject.layer = gameObject.layer;

                    button.transform.parent = transform;
                    button.transform.localPosition = new Vector3((-Width+1) / 2f * step.x + x * step.x, -y * step.y, -1);
                    button.transform.localScale = Vector3.one;

                    (button.collider as BoxCollider).size = step;

                    buttons.Add(button);
                }
            }
        }
        
    }
}
