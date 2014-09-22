#if SERVER
using Server.SQL;
namespace Server.Model
{
    public abstract class EntityExtension : ISQLSerializable
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
#endif
