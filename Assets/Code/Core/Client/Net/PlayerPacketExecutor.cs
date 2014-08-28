using Code.Code.Libaries.Net;
using Code.Code.Libaries.Net.Packets;
using Code.Core.Client.UI;
using Code.Core.Client.UI.Controls.Items;
using Code.Core.Client.UI.Interfaces;
using Code.Core.Client.UI.Interfaces.UpperLeft;
using Code.Core.Client.Units;
using Code.Core.Client.Units.Extensions;
using Code.Core.Client.Units.Managed;
using Code.Libaries.Net.Packets.ForClient;
using Code.Libaries.Net.Packets.ForServer;
using UnityEngine;

namespace Code.Core.Client.Net
{
    public class PlayerPacketExecutor : PacketExecutor
    {
        protected override void aExecutePacket(BasePacket packet)
        {
            ClientCommunicator.Instance.PacketHistory.Add(packet.GetType().Name);
            if (packet is UIPacket)
            {
                UIPacket p = packet as UIPacket;
                if (p.type == UIPacket.UIPacketType.SEND_MESSAGE)
                {
                    if (ChatPanel.I != null)
                        ChatPanel.I.AddMessage(p.textData);
                }
            }
            else if (packet is EnterWorldPacket)
            {
                EnterWorldPacket p = packet as EnterWorldPacket;
                //load some world

                PlayerUnit.MyPlayerUnit = UnitManager.Instance[p.myUnitID];
                PlayerUnit.MyPlayerUnit.transform.position = p.Position;
            }
            else if (packet is UnitUpdatePacket)
            {
                UnitUpdatePacket p = packet as UnitUpdatePacket;
                UnitManager.Instance[p.UnitID].DecodeUnitUpdate(p);
            }
            else if (packet is UIInterfaceEvent)
            {
                UIInterfaceEvent Event = packet as UIInterfaceEvent;

                if (Event._eventType == UIInterfaceEvent.EventType.HIDE_INTERFACE)
                {
                    InterfaceManager.GetInterface(Event.interfaceId).Hide();
                }
                else if (Event._eventType == UIInterfaceEvent.EventType.SHOW_INTERFACE)
                {
                    InterfaceManager.GetInterface(Event.interfaceId).Show();
                }
                else if (Event._eventType == UIInterfaceEvent.EventType.SHOW)
                {
                    InterfaceManager.GetInterface(Event.interfaceId).Controls[Event.controlID].Show();
                }
                else if (Event._eventType == UIInterfaceEvent.EventType.HIDE)
                {
                    InterfaceManager.GetInterface(Event.interfaceId).Controls[Event.controlID].Hide();
                }
                else if (Event._eventType == UIInterfaceEvent.EventType.SEND_DATA)
                {
                    InterfaceManager.GetInterface(Event.interfaceId).Controls[Event.controlID].OnSetData(Event.values);
                }
                else
                {
                    Debug.LogError("Bad ui event type: " + packet.GetType());
                }
            }
            else if (packet is UnitSelectionPacketData)
            {
                UnitSelectionInterface.I.OnDataRecieved(packet as UnitSelectionPacketData);
            }
            else if (packet is UIInventoryInterfacePacket)
            {
                UIInventoryInterfacePacket p = packet as UIInventoryInterfacePacket;
                ItemInventoryInterface.I.Handle(p);
            }
            else if (packet is ChatPacket)
            {
                ChatPacket p = packet as ChatPacket;
                ChatPanel.I.AddMessage(p);
            }


            else
            {
                Debug.LogError("Unknown packet type: " + packet.GetType());
            }
        }
    }
}
