using System.Collections.Generic;
using UnityEngine;
using Code.Core.Client.UI.Controls;

namespace Code.Core.Client.UI
{

    public class UIInterface<T> : MonoBehaviour where T : MonoBehaviour
    {
        public int ID = 999;
        private static T _instance;

        public static T I
        {
            get
            {
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        private bool _isVisible = true;

        [SerializeField]
        private InterfaceAnchor
            anchor;

        public bool Visible
        { 
            get { return _isVisible; } 
            set
            { 
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnVisibiltyChanged();
                    gameObject.SetActive(_isVisible);
                }
            }
        }

        private void Awake()
        {
            I = this.GetComponent<T>();
        }

        public List<InterfaceButton> Buttons; 

        private void Start()
        {
            int counter = 0;
            foreach (var button in GetComponentsInChildren<InterfaceButton>())
            {
                Buttons.Add(button);
                button.interfaceID = ID;
                button.index = counter;

                counter++;
            }
            OnStart();
        }

        private void Update()
        {
            OnUpdate();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate();
        }

        private void LateUpdate()
        {
            OnLateUpdate();
        }

        protected virtual void OnStart()
        {
        }
        protected virtual void OnUpdate()
        {
        }
        protected virtual void OnFixedUpdate()
        {
        }
        protected virtual void OnLateUpdate()
        {
        }
        protected virtual void OnVisibiltyChanged()
        {
        }
    }
}
