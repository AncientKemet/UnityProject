
using System;
using System.Collections.Generic;

namespace OldBlood.Code.Core.Server.Model
{
    public abstract class WorldEntity
    {
        private World _currentWorld = null;
       
        public int ID {get;set;}

        private Dictionary<Type, EntityExtension> extensions = new Dictionary<Type, EntityExtension>();

        public Dictionary<Type, EntityExtension>.ValueCollection Extensions
        {
            get { return extensions.Values; }
        }

        public virtual World CurrentWorld
        {
            get { return _currentWorld; }
            set { _currentWorld = value; }
        }

        public void AddExt(EntityExtension extension)
        {
            extensions[extension.GetType()] = extension;
        }

        public T GetExt<T>() where T : EntityExtension
        {
            if (extensions.ContainsKey(typeof (T)))
            {
                return extensions[typeof (T)] as T;
            }
            return null;
        }

        public virtual void Progress()
        {
            foreach (var extension in extensions.Values)
            {
                extension.Progress();
            }
        }
    }
}

