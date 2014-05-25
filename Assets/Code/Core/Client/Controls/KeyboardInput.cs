using OldBlood.Code.Core.Client.Controls.Camera;
using UnityEngine;

namespace OldBlood.Code.Core.Client.Controls
{
    public class KeyboardInput : Monosingleton<KeyboardInput> {

        private KeyboardImputListener _listener;

        public KeyboardImputListener listener
        {
            get
            {
                return _listener;
            }
            set
            {
                if(_listener != null)
                {
                    _listener.ListenerWasDeclined();
                }
                _listener = value;
            }
        }

        void Update ()
        {
            if(_listener == null)
            {
                bool rotateLeft = Input.GetKey(KeyCode.A);
                bool rotateRight = Input.GetKey(KeyCode.S);

                if(rotateLeft)
                    CameraController.Instance.rotation += .05f * Time.deltaTime;

                if(rotateRight)
                    CameraController.Instance.rotation -= .05f * Time.deltaTime;
            }
            else
            {
                foreach (var c in Input.inputString.ToCharArray()) {
                    _listener.KeyWasPressed(c);
                }
            }
        }

        public abstract class KeyboardImputListener
        {
            public void Attach()
            {
                KeyboardInput.Instance.listener = this;
            }
        
            public void Deattach()
            {
                if(KeyboardInput.Instance.listener == this){
                    KeyboardInput.Instance.listener = null;
                }
            }
        
            public abstract void KeyWasPressed(char k);
            public abstract void ListenerWasDeclined();
        }
    }
}

