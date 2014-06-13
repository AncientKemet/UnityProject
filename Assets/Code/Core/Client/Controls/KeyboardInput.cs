using Code.Core.Client.Controls.Camera;
using Code.Core.Client.Net;
using Code.Core.Shared.NET;
using Code.Libaries.Generic;
using Code.Libaries.Net.Packets.ForServer;
using UnityEngine;

namespace Code.Core.Client.Controls
{
    public class KeyboardInput : MonoSingleton<KeyboardInput> {

        private KeyboardImputListener _fullListener;

        public KeyboardImputListener FullListener
        {
            get
            {
                return _fullListener;
            }
            set
            {
                if(_fullListener != null)
                {
                    _fullListener.ListenerWasDeclined();
                }
                _fullListener = value;
            }
        }

        void Update ()
        {
            if(_fullListener == null)
            {
                bool rotateLeft = Input.GetKey(KeyCode.A);
                bool rotateRight = Input.GetKey(KeyCode.S);
                bool shiftDown = Input.GetKeyDown(KeyCode.LeftShift);
                bool dontWalk = Input.GetKeyDown(KeyCode.Space);
                bool canWak = Input.GetKeyUp(KeyCode.Space);

                if(rotateLeft)
                    CameraController.Instance.rotation += 1.5f * Time.deltaTime;

                if(rotateRight)
                    CameraController.Instance.rotation -= 1.5f * Time.deltaTime;

                if (shiftDown)
                    ClientCommunicator.Instance.SendToServer(new InputEventPacket(PacketEnums.INPUT_TYPES.ToogleRun));

                if (dontWalk)
                    ClientCommunicator.Instance.SendToServer(new InputEventPacket(PacketEnums.INPUT_TYPES.StopWalk));

                if (canWak)
                    ClientCommunicator.Instance.SendToServer(new InputEventPacket(PacketEnums.INPUT_TYPES.ContinueWalk));

            }
            else
            {
                foreach (var c in Input.inputString.ToCharArray()) {
                    _fullListener.KeyWasPressed(c);
                }
            }
        }

        public abstract class KeyboardImputListener
        {
            public void Attach()
            {
                KeyboardInput.Instance.FullListener = this;
            }
        
            public void Deattach()
            {
                if(KeyboardInput.Instance.FullListener == this){
                    KeyboardInput.Instance.FullListener = null;
                }
            }
        
            public abstract void KeyWasPressed(char k);
            public abstract void ListenerWasDeclined();
        }
    }
}

