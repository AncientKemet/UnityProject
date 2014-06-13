using System;
using System.Collections.Generic;
using Code.Core.Client.UI.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Code.Core.Client.UI.Controls
{
    public class Clickable : MonoBehaviour
    {
        private static RightClickAction CANCEL = new RightClickAction("Cancel");
        
        public Action OnLeftClick;
        public Action OnRightClick;
        public Action OnWheelClick;
        public Action OnHover;
        public Action OnMouseIn;
        public Action OnMouseOff;
        public Action OnLeftMouseHold;
        public Action OnRightMouseHold;

        [SerializeField] protected bool HasRightClickMenu = true;

        public virtual List<RightClickAction> Actions
        {
            get
            {
                List<RightClickAction> list = new List<RightClickAction>();

                if (_actions.Count > 0)
                    list.AddRange(_actions);

                list.Add(CANCEL);

                return list;
            }
        }

        protected void Start()
        {
            //Left click is the first action
            OnLeftClick += Actions[0].Action;

            if (HasRightClickMenu)
                OnRightClick += OpenRightClickMenu;
        }

        private void OpenRightClickMenu()
        {
            RightClickMenu.Open(this);
        }

        private void OnMouseOver()
        {
            if (OnHover != null)
                OnHover();

            if (Input.GetMouseButton(0))
                if (OnLeftMouseHold != null)
                    OnLeftMouseHold();

            if (Input.GetMouseButton(1))
                if (OnRightMouseHold != null)
                    OnRightMouseHold();
            
            if (Input.GetMouseButtonUp(0))
                if (OnLeftClick != null)
                    OnLeftClick();

            if (Input.GetMouseButtonUp(1))
                if (OnRightClick != null)
                    OnRightClick();

            if (Input.GetMouseButtonUp(2))
                if (OnWheelClick != null)
                    OnWheelClick();
        }

        private void OnMouseExit()
        {
            if (OnMouseOff != null)
                OnMouseOff();
        }

        private void OnMouseEnter()
        {
            if (OnMouseIn != null)
                OnMouseIn();
        }
        
        [SerializeField]
        private List<RightClickAction> _actions;

        public void AddAction(RightClickAction action)
        {
           _actions.Add(action);
        }
    }

    [Serializable]
    public class RightClickAction
    {
        public string Name;
        public Action Action;

        public RightClickAction()
        {}

        public RightClickAction(string name)
        {
            Name = name;
        }

        public RightClickAction(string name, Action action)
        {
            Name = name;
            Action = action;
        }

        public void OnGUI()
        {
            Name = EditorGUILayout.TextField("Name", Name);
        }
    }
}
