#if UNITY_EDITOR
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using Code.Core.Client.Net;
using Code.Core.Client.Settings;
using Code.Core.Client.UI.Controls;
using Code.Core.Client.Units;
using Code.Libaries.Net.Packets.ForServer;
using UnityEngine;

namespace Code.Core.Shared.Content.Types.ItemExtensions
{
    [Serializable]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class ItemRigid : ItemExtension
    {
        [SerializeField]
        private List<RightClickAction> Actions = new List<RightClickAction>();

        [SerializeField] private bool KeepColliding = false;

        private bool _physicsEnabled;

        /// <summary>
        /// Use only in player.
        /// </summary>
        public bool PhysicsEnabled
        {
            get
            {
                return _physicsEnabled;
            }
            set
            {
                _physicsEnabled = value;

                //Only for medium and higher
                if (VideoSettings.Instance.Physics.Value >= VideoSettings.PhysicsQuality.Medium)
                {
                    rigidbody.isKinematic = !value;
                }
                else
                {
                    rigidbody.isKinematic = true;
                }

                if (value)
                {
                    if (transform.parent != null)
                    {
                        Clickable parent = transform.parent.GetComponent<Clickable>();
                        if (parent != null)
                        {
                            Clickable c = GetComponent<Clickable>();
                            if (c == null)
                                c = gameObject.AddComponent<Clickable>();


                            parent.RegisterChildClickable(c);

                            AddActions(parent);
                            parent._actions.AddRange(Actions);
                        }
                        else
                        {
                            Debug.LogError("Couldn't find parent clickable.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Couldn't find parent.");
                    }

                    //Only for medium and higher
                    if (VideoSettings.Instance.Physics.Value >= VideoSettings.PhysicsQuality.Medium)
                    {
                        if (!KeepColliding)
                        {
                            //automatic disable
                            StartCoroutine(Disable(0.3f));
                        }
                    }

                }
            }
        }

        private void AddActions(Clickable parent)
        {
            EquipmentItem equipmentItem = GetComponent<EquipmentItem>();
            ItemWithInventory itemWithInventory = GetComponent<ItemWithInventory>();

            if (equipmentItem != null)
            {
                if (equipmentItem.CanBeStoredInInventory)
                    parent._actions.Add(new RightClickAction(
                        "Take",
                        delegate
                        {
                            if (parent == null)
                                return;

                            PlayerUnit unit = parent.GetComponent<PlayerUnit>();
                            if (unit != null)
                            {
                                UnitActionPacket p = new UnitActionPacket();
                                p.UnitId = unit.Id;
                                p.ActionName = "Take";
                                ClientCommunicator.Instance.SendToServer(p);
                            }
                        }
                        ));
                else
                    parent._actions.Add(new RightClickAction(
                        "Pick-up",
                        delegate
                        {
                            if (parent == null)
                                return;

                            PlayerUnit unit = parent.GetComponent<PlayerUnit>();
                            if (unit != null)
                            {
                                UnitActionPacket p = new UnitActionPacket();
                                p.UnitId = unit.Id;
                                p.ActionName = "Pick-up";
                                ClientCommunicator.Instance.SendToServer(p);
                            }
                        }
                        ));
            }

            if (itemWithInventory != null)
            {
                parent._actions.Add(new RightClickAction(
                    "Open",
                    delegate
                    {
                        if (parent == null)
                            return;

                        PlayerUnit unit = parent.GetComponent<PlayerUnit>();
                        if (unit != null)
                        {
                            UnitActionPacket p = new UnitActionPacket();
                            p.UnitId = unit.Id;
                            p.ActionName = "Open";
                            ClientCommunicator.Instance.SendToServer(p);
                        }
                    }
                    ));
            }

            foreach (var action in Actions)
            {
                action.Action = delegate
                {
                    if (parent == null)
                        return;

                    PlayerUnit unit = parent.GetComponent<PlayerUnit>();
                    if (unit != null)
                    {
                        UnitActionPacket p = new UnitActionPacket();
                        p.UnitId = unit.Id;
                        p.ActionName = action.Name;
                        ClientCommunicator.Instance.SendToServer(p);
                    }
                };
            }
        }

        private void Awake()
        {
            PhysicsEnabled = false;
        }
        
        private void FixedUpdate()
        {
            if (transform.parent != null && !PhysicsEnabled)
            {
                
                //rigidbody.AddForce((transform.parent.position - transform.position) * 2);
                //rigidbody.AddForce((transform.parent.position - transform.position)*5);
            }
        }

        private IEnumerator Disable(float time)
        {
            /*if(this != null && collider != null && collider is BoxCollider)
                CorotineManager.Instance.RunCoroutine(Ease.Vector(transform.localPosition, new Vector3(0,(collider as BoxCollider).size.y, 0),
                delegate(Vector3 vector3)
                {
                    if(this != null)
                        transform.localPosition = vector3;
                }, null, time));*/
            yield return new WaitForSeconds(time);

            rigidbody.isKinematic = true;

            
        }
        
    }
}
