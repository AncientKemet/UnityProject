using System.Collections.Generic;
using Code.Core.Client.Controls;
using UnityEngine;

namespace Code.Core.Client.UI.Interfaces
{
    public class ChatPanel : UIInterface<ChatPanel> {

        public tk2dTextMesh exampleLine;
        public tk2dTextMesh inputTextField;
        public tk2dSlicedSprite slider;
        public tk2dSlicedSprite background;

        [SerializeField]
        private List<tk2dTextMesh> messages = new List<tk2dTextMesh>();

        private bool _isTyping = false;

        private ChatKeyListener listener = new ChatKeyListener();

        public bool isTyping
        {
            get
            {
                return _isTyping;
            }
            set
            {
                _isTyping = value;
                inputTextField.transform.parent.gameObject.SetActive(_isTyping);
                slider.gameObject.SetActive(isTyping);
                background.renderer.enabled = _isTyping;

                if(_isTyping)
                {
                    listener.Attach();
                }
                else
                {
                    listener.Deattach();
                }
            }
        }


        public void AddMessage(string message)
        {
            for (int i = messages.Count-2; i > 0; i--)
            {
                messages[i+1].text = messages[i].text;
                messages[i+1].ForceBuild();
            }
            messages[0].text = message;
            messages[0].ForceBuild();
        }

        private void CompleteChatInput()
        {
            AddMessage(inputTextField.text);
            inputTextField.text = "";
            inputTextField.ForceBuild();
        }
	
        #region implemented abstract members of Interface
        protected override void OnStart()
        {
            for (int i = 0; i < 7; i++)
            {
                tk2dTextMesh newLine = ((GameObject)Instantiate(exampleLine.gameObject)).GetComponent<tk2dTextMesh>();
                newLine.transform.parent = exampleLine.transform.parent;
                newLine.transform.localPosition = exampleLine.transform.localPosition + new Vector3(0, i * 1,0); 
                newLine.text = "";
                newLine.ForceBuild();
                messages.Add(newLine);
            }
            exampleLine.gameObject.SetActive(false);
            AddMessage("test message");
            isTyping = false;
        }

        protected override void OnUpdate()
        {
            if(Input.GetKeyUp(KeyCode.Return))
            {
                isTyping = !isTyping;
                if(inputTextField.text.Length > 0){
                    CompleteChatInput();
                }
            }
        }
        protected override void OnFixedUpdate()
        {
        }
        protected override void OnLateUpdate()
        {
        }
        protected override void OnVisibiltyChanged()
        {
        }
        #endregion

        private class ChatKeyListener : KeyboardInput.KeyboardImputListener
        {
        
            public override void KeyWasPressed(char c)
            {
                string text = ChatPanel.I.inputTextField.text;
                if (c == "\b"[0])
                {
                    if (text.Length != 0)
                    {
                        text = text.Substring(0, text.Length - 1);
                    }
                }
                else if (c == "\r"[0])
                {
                
                }
                else if ((int)c!=9 && (int)c!=27) //deal with a Mac only Unity bug where it returns a char for escape and tab
                {
                    text += c;
                }
                ChatPanel.I.inputTextField.text = text;
                ChatPanel.I.inputTextField.ForceBuild();

            }

            public override void ListenerWasDeclined()
            {
            }
        };
    }
}
