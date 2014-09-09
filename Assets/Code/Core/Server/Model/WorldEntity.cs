using System;
using System.Collections.Generic;
using Code.Libaries.UnityExtensions;
using UnityEngine;

namespace Server.Model
{
    public abstract class WorldEntity : ServerMonoBehaviour
    {
        private World _currentWorld = null;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private Dictionary<Type, EntityExtension> extensions = new Dictionary<Type, EntityExtension>();

        [SerializeField]
        private int _id;

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
            extension.entity = this;
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

        private void OnDestroy()
        {
            CurrentWorld.RemoveEntity(this);
        }

    }
}

