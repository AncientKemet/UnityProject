using Code.Code.Libaries.Net.Packets;
using Code.Core.Client.Net;
using Code.Libaries.Net.Packets.ForServer;
using UnityEngine;
using EventType = Code.Libaries.Net.Packets.ForServer.EventType;

namespace Code.Core.Client.UI.Controls
{
    [RequireComponent(typeof (BoxCollider))]
    [ExecuteInEditMode]
    public class InterfaceButton : Clickable
    {
        
        protected virtual void Start()
        {
            GetComponent<Clickable>().OnLeftClick += OnClick;
        }

        private void OnClick()
        {
            UIInterfaceEvent packetEvent = new UIInterfaceEvent();

            packetEvent.controlID = index;
            packetEvent.interfaceId = this.interfaceID;
            packetEvent._eventType = EventType.CLICK;

            ClientCommunicator.Instance.SendToServer(packetEvent);
        }

        public int index { get; set; }

        public int interfaceID { get; set; }
   }
}
