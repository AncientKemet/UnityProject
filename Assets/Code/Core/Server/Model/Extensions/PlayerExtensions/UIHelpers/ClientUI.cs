using System.Collections.Generic;
using Code.Core.Client.UI;
using Code.Core.Server.Model.Entities;
using Code.Core.Server.Model.Extensions.PlayerExtensions.UIHelpers.Interfaces;
using Code.Libaries.Net.Packets.ForServer;
using UnityEngine;

namespace Code.Core.Server.Model.Extensions.PlayerExtensions.UIHelpers
{
    
    public class ClientUI : EntityExtension
    {

        private Dictionary<InterfaceType, bool> OpenedInterfaces = new Dictionary<InterfaceType, bool>(); 

        public Player Player { get; private set; }
        public ClientInventoryInterface Inventories { get; private set; }

        protected override void OnExtensionWasAdded()
        {
            base.OnExtensionWasAdded();
            this.Player = entity as Player;
            Inventories = new ClientInventoryInterface(Player);
        }

        public override void Progress()
        {
        }

        public void OnUIEvent(UIInterfaceEvent e)
        {
            if (e.interfaceId == InterfaceType.ActionBars)
            {
                int spell = e.controlID;
                    if (e._eventType == UIInterfaceEvent.EventType.Button_Down)
                    {
                        Player.Actions.StartSpell(spell);
                        return;
                    }

                    if (e._eventType == UIInterfaceEvent.EventType.CLICK)
                    {
                        Player.Actions.FinishSpell(spell);
                        return;
                    }
                
            }

            if (e.interfaceId == InterfaceType.LowerLeftMenu)
            {
                if (e.controlID == 0) // open Inventory
                {
                    if (e._eventType == UIInterfaceEvent.EventType.CLICK)
                    {
                        OpenOrCloseInterface(InterfaceType.MyCharacterInventory);
                        return;
                    }
                }
            }

            if (e.interfaceId == InterfaceType.Chat)
            {
                if (e.controlID == 0) // start talking
                {
                    return;
                }
                return;
            }

            Debug.LogError("Unknown event: "+e._eventType+" interface id: "+e.interfaceId+" control id: "+e.controlID);
        }

        private void OpenOrCloseInterface(InterfaceType type)
        {
            if (!IsInterfaceOpened(type))
            {
                OpenInterface(type);
            }
            else
            {
                CloseInterface(type);
            }
        }

        private void CloseInterface(InterfaceType type)
        {
            UIInterfaceEvent closePacket = new UIInterfaceEvent();

            closePacket.interfaceId = type;
            closePacket._eventType = UIInterfaceEvent.EventType.HIDE_INTERFACE;

            Player.Client.ConnectionHandler.SendPacket(closePacket);

            OpenedInterfaces[type] = false;
        }

        private void OpenInterface(InterfaceType type)
        {
            UIInterfaceEvent showPacket = new UIInterfaceEvent();

            showPacket.interfaceId = type;
            showPacket._eventType = UIInterfaceEvent.EventType.SHOW_INTERFACE;

            Player.Client.ConnectionHandler.SendPacket(showPacket);

            OpenedInterfaces[type] = true;
        }

        private bool IsInterfaceOpened(InterfaceType type)
        {
            if (!OpenedInterfaces.ContainsKey(type))
            {
                OpenedInterfaces.Add(type, false);
            }
            return OpenedInterfaces[type];
        }

        public void ShowControl(InterfaceType interfaceType, int id)
        {
            UIInterfaceEvent showPacket = new UIInterfaceEvent();

            showPacket.interfaceId = interfaceType;
            showPacket._eventType = UIInterfaceEvent.EventType.SHOW;
            showPacket.controlID = id;

            Player.Client.ConnectionHandler.SendPacket(showPacket);
        }

        public void HideControl(InterfaceType interfaceType, int id)
        {
            UIInterfaceEvent hidePacket = new UIInterfaceEvent();

            hidePacket.interfaceId = interfaceType;
            hidePacket._eventType = UIInterfaceEvent.EventType.HIDE;
            hidePacket.controlID = id;

            Player.Client.ConnectionHandler.SendPacket(hidePacket);
        }

        public void SetControlValues(InterfaceType interfaceType, int id, List<float> values)
        {
            UIInterfaceEvent packet = new UIInterfaceEvent();

            packet.interfaceId = interfaceType;
            packet._eventType = UIInterfaceEvent.EventType.SEND_DATA;
            packet.controlID = id;
            packet.values = values;

            Player.Client.ConnectionHandler.SendPacket(packet);
        }
    }
}

