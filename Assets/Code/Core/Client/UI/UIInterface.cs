using UnityEngine;

namespace OldBlood.Code.Core.Client.UI
{
    public class UIInterface<T> :MonoBehaviour where T : MonoBehaviour
    {
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
        private tk2dCameraAnchor anchor;

        public bool Visible
        { 
            get { return _isVisible; } 
            set { 
                if(_isVisible != value)
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

        private void Start()
        {
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

        protected virtual void OnStart(){}
        protected virtual void OnUpdate(){}
        protected virtual void OnFixedUpdate(){}
        protected virtual void OnLateUpdate(){}
        protected virtual void OnVisibiltyChanged(){}
    }
}
