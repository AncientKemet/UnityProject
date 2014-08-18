using Code.Code.Libaries.Net.Packets;
using Code.Core.Client.Net;
using Code.Libaries.Net.Packets.ForServer;
using UnityEngine;
using EventType = Code.Libaries.Net.Packets.ForServer.UIInterfaceEvent.EventType;

namespace Code.Core.Client.UI.Controls
{
    [RequireComponent(typeof (BoxCollider))]
    [ExecuteInEditMode]
    public class InterfaceButton : UIControl
    {

        protected virtual void Start()
        {
            GetComponent<Clickable>().OnLeftClick += SendClickPacket;
        }

        protected void SendClickPacket()
        {
            UIInterfaceEvent packetEvent = new UIInterfaceEvent();

            packetEvent.controlID = Index;
            packetEvent.interfaceId = InterfaceId;
            packetEvent._eventType = EventType.CLICK;

            ClientCommunicator.Instance.SendToServer(packetEvent);
        }
   }
}
