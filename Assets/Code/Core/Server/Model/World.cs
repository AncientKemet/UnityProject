using System;
using System.Collections.Generic;

namespace OldBlood.Code.Core.Server.Model
{
    public class World
    {
        private List<WorldEntity> entities = new List<WorldEntity>();

        public void AddEntity(WorldEntity entity)
        {
            entities.Add(entity);
            entity.ID = entities.IndexOf(entity);
        }

        public void Progress()
        {
            foreach (var e in entities)
            {
                e.Progress();
            }
        }

        private World()
        {}

        public static World CreateWorld(ServerWorldManager manager)
        {
            World w = new World();
            manager.AddWorld(w);
            return w;
        }
    }
}

