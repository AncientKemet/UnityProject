namespace Server.Model
{
    public abstract class EntityExtension
    {

        private WorldEntity _entity;

        public WorldEntity entity { get { return _entity; }
            set { _entity = value; OnExtensionWasAdded(); }
        }

        protected virtual void OnExtensionWasAdded()
        {

        }

        public abstract void Progress();
    }
}
