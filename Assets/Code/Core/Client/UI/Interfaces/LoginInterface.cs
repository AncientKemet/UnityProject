using System.Security.Cryptography;
using Client.UI.Controls.Inputs;
using Code.Code.Libaries.Net.Packets;
using Code.Core.Client.Net;
using Code.Core.Client.UI;
using Code.Core.Client.UI.Controls;
using UnityEngine;

namespace Client.UI.Interfaces
{
    public class LoginInterface : UIInterface<LoginInterface>
    {
        public TextButton LoginButton;
        public TextField Username;
        public TextField Password;

        public bool WaitingForResponse { get; set; }

        protected override void OnStart()
        {
            base.OnStart();

            LoginButton.OnLeftClick += () =>
            {
                if (!WaitingForResponse)
                {
                    WaitingForResponse = true;
                    LoginButton.Hide();

                    AuthenticationPacket packet = new AuthenticationPacket();

                    packet.Username = Username.Text;
                    packet.Password = Password.Text;

                    ClientCommunicator.Instance.SendToServer(packet);
                }
            };
        }

        public override void Hide()
        {
            Destroy(gameObject);
        }
    }
}
