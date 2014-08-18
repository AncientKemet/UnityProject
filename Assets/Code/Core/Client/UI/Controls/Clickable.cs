using System;
using System.Collections.Generic;
using Code.Core.Client.UI.Interfaces;
using Code.Core.Client.Units;
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

        public Action OnLeftDown;
        public Action OnLeftUp;

        [SerializeField] protected bool HasRightClickMenu = false;

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

        protected virtual void Start()
        {
            //Left click is the first action
            if(!(this is PlayerUnit))
                OnLeftClick += Actions[0].Action;

            if (HasRightClickMenu)
                OnRightClick += OpenRightClickMenu;
        }

        protected void OpenRightClickMenu()
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

            if (Input.GetMouseButtonDown(0))
                if (OnLeftDown != null)
                    OnLeftDown();
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
        public List<RightClickAction> _actions = new List<RightClickAction>();

        public void AddAction(RightClickAction action)
        {
           _actions.Add(action);
        }

        public void RegisterChildClickable(Clickable child)
        {
            child.OnLeftClick += OnLeftClick;
            child.OnRightClick += OnRightClick;
            child.OnMouseIn += OnMouseIn;
            child.OnMouseOff += OnMouseOff;
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
