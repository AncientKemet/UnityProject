using System.Collections.Generic;
using Code.Libaries.UnityExtensions.Independent;
using UnityEngine;
using Code.Core.Client.UI.Controls;

namespace Code.Core.Client.UI
{
    public static class InterfaceManager
    {
        private static Dictionary<InterfaceType, UIInterface> ActiveInterfaces = new Dictionary<InterfaceType, UIInterface>();

        public static UIInterface GetInterface(InterfaceType type)
        {
            return ActiveInterfaces[type];
        }
        public abstract class UIInterface : MonoBehaviour
        {

            [SerializeField]
            public InterfaceType Type;

            public List<UIControl> Controls;

            protected virtual void Awake()
            {
                if (ActiveInterfaces.ContainsKey(Type))
                {
                    Debug.LogError("Duplicate of interfacetype: "+gameObject+" "+ActiveInterfaces[Type].gameObject);
                    return;
                }
                ActiveInterfaces.Add(Type, this);

                int counter = 0;

                Controls.Clear();

                List<UIControl> list = new List<UIControl>();
                foreach (var button in GetComponentsInChildren<UIControl>())
                {
                    list.Add(button);
                    button.InterfaceId = Type;
                    if (button.Index == -1)
                        button.Index = counter;

                    counter++;
                    Controls.Add(null);
                }

                foreach (var interfaceButton in list)
                {
                    Controls[interfaceButton.Index] = interfaceButton;
                }
            }

            public abstract void Hide();
            public abstract void Show();
        }
    }

    

    /// <summary>
    /// Singleton like InGameInterface generic class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UIInterface<T> : InterfaceManager.UIInterface where T : MonoBehaviour
    {

        private static T _instance;
        public bool Visible { get; set; }

        protected virtual float AnimSpeed { get { return 0.33f; } }

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


        protected override void Awake()
        {
            base.Awake();

            if(I == null)
                I = GetComponent<T>();

            OnStart();
        }

        public override void Hide()
        {
            Visible = false;
            CorotineManager.Instance.StartCoroutine(
                Ease.Vector(
                    transform.localScale,
                    Vector3.zero,
                    delegate(Vector3 vector3)
                    {
                        if(!Visible)
                        transform.localScale = vector3;
                    },
                    delegate { 
                        if(!Visible)
                        gameObject.SetActive(false); 
                    },
                    AnimSpeed
                    )
                );

        }

        public override void Show()
        {
            Visible = true;
            gameObject.SetActive(true);
            CorotineManager.Instance.StartCoroutine(
                Ease.Vector(
                    transform.localScale,
                    Vector3.one,
                    delegate(Vector3 vector3)
                    {
                        if (Visible)
                        transform.localScale = vector3;
                    },
                    delegate
                    {
                    },
                    AnimSpeed
                    )
                );
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
