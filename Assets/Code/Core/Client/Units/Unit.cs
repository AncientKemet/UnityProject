using UnityEngine;

namespace OldBlood.Code.Core.Client.Units
{
    public class Unit : MonoBehaviour
    {

        private int _id = -1;

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        void Start()
        {
            OnStart();
        }

        void Update()
        {
            OnUpdate();
        }

        void FixedUpdate()
        {
            OnFixedUpdate();
        }

        protected virtual void OnStart()
        {}

        protected virtual void OnUpdate()
        {}

        protected virtual void OnFixedUpdate()
        {}

    }
}
