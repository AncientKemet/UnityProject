
using System;
using System.Collections.Generic;

namespace OldBlood.Code.Core.Server.Model
{
    public abstract class WorldEntity
    {
        public int ID {get;set;}
        private List<EntityExtension> extensions = new List<EntityExtension>();

        public void AddExtension(EntityExtension extension)
        {
            extensions.Add(extension);
        }

        public T GetExtension<T>() where T : EntityExtension
        {
            foreach (var item in extensions)
            {
                if (item is T)
                    return item as T;
            }
            return null;
        }

        public virtual void Progress()
        {
            foreach (var extension in extensions)
            {
                extension.Progress();
            }
        }
    }
}

