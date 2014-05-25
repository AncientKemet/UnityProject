using System;
using System.Collections.Generic;
using OldBlood.Code.Core.Server.Model.Entities;
using OldBlood.Code.Libaries.Generic.Trees;
using UnityEngine;

namespace OldBlood.Code.Core.Server.Model
{
    public class World
    {
        private readonly List<WorldEntity> entities = new List<WorldEntity>();
        public readonly List<Unit> Units = new List<Unit>();
        public readonly List<Player> Players = new List<Player>();

        public QuadTree Tree = new QuadTree(6, Vector2.zero, Vector2.one * 2048);

        public void AddEntity(WorldEntity entity)
        {
            entities.Add(entity);
            entity.ID = entities.IndexOf(entity);
            entity.CurrentWorld = this;

            if (entity is Unit)
            {
                Unit unit = entity as Unit;
                Units.Add(unit);
            }

            if (entity is Player)
            {
                Player player = entity as Player;
                Players.Add(player);
                player.OnEnteredWorld(this);
            }
        }

        public void RemoveEntity(WorldEntity entity)
        {
            entities.Remove(entity);

            if (entity is Unit)
            {
                Unit unit = entity as Unit;
                Units.Remove(unit);
            }
        }

        public void Progress()
        {
            Tree.Update();
            foreach (var e in entities)
            {
                e.Progress();
            }
        }

        #region Constructor
        private World()
        { }

        public static World CreateWorld(ServerWorldManager manager)
        {
            World w = new World();
            manager.AddWorld(w);
            return w;
        }
        #endregion

        public int ID { get; set; }
    }
}

