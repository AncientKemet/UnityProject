using Code.Libaries.Generic.Managers;
using UnityEngine;
using System.Collections;
using Code.Core.Shared.Content.Types;

namespace Code.Core.Client.Units.UnitControllers
{
    [RequireComponent(typeof(UnitDisplay))]
    public class Equipment : MonoBehaviour
    {
        private UnitDisplay skeleton;
        
        public void EquipItem(EquipmentItem item)
        {
            if (item != null)
            {
                if (item.EquipType == EquipmentItem.Type.LeftHand)
                {
                    _replaceItem(skeleton._leftFingers, item);
                    if (item != null)
                    {
                        skeleton.PlayAnimation("LeftOneHand",2, 0.75f);
                    }
                }
                else if (item.EquipType == EquipmentItem.Type.RightHand)
                {
                    _replaceItem(skeleton._rightFingers, item);
                }
                else if (item.EquipType == EquipmentItem.Type.Helm)
                {
                    _replaceItem(skeleton._neck, item);
                }
            }
            else {
                Debug.LogError("Equiping null item.");
            }
        }

        private void Start()
        {
            skeleton = GetComponent<UnitDisplay>();
        }

        private void _replaceItem(Transform Head, EquipmentItem item)
        {
            for (int i = 0; i < Head.childCount; i++)
            {
                Destroy(Head.GetChild(i).gameObject);
            }

            GameObject itemModel = ((GameObject)Instantiate(item.Model.gameObject));
            
            itemModel.transform.parent = Head;
            itemModel.transform.localPosition = new Vector3(0, 0, 0);
            itemModel.transform.localRotation = Quaternion.identity;
        }
    }
}