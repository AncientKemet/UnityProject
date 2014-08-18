using System;
using System.Collections.Generic;
using Code.Core.Server.Model.Entities;
using Code.Libaries.Generic.Trees;
using Code.Libaries.UnityExtensions;
using UnityEngine;
using Code.Core.Server.Model.Extensions.UnitExts;

namespace Code.Core.Server.Model
{
    public class World : ServerMonoBehaviour
    {
        private List<WorldEntity> entities;
        public List<ServerUnit> Units;
        public List<Player> Players;

        public QuadTree Tree;

        public void AddEntity(WorldEntity entity)
        {
            bool foundNullIndex = false;
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] == null)
                {
                    entities[i] = entity;
                    entity.ID = i;
                    foundNullIndex = true;
                    break;
                }
            }

            if (!foundNullIndex)
            {
                entities.Add(entity);
                entity.ID = entities.IndexOf(entity);
            }

            entity.CurrentWorld = this;

            if (entity is ServerUnit)
            {
                ServerUnit serverUnit = entity as ServerUnit;
                Units.Add(serverUnit);
                //Debug.Log("Added server unit: "+serverUnit+" id: "+serverUnit.ID);
            }

            if (entity is Player)
            {
                Player player = entity as Player;
                Players.Add(player);
                player.OnEnteredWorld(this);
            }

            entity.transform.parent = transform;
        }

        public void RemoveEntity(WorldEntity entity)
        {
            entities[entity.ID] = null;
        }

        public void Progress()
        {
            foreach (var e in entities)
            {
                if(e != null)
                e.Progress();
            }
            Tree.Update();
        }

        #region Constructor

        private static World CreateWorldInstance()
        {
            World world = CreateInstance<World>(null);

            world.entities = new List<WorldEntity>();
            world.Units = new List<ServerUnit>();
            world.Players = new List<Player>();
            world.Tree = new QuadTree(2, Vector2.zero, Vector2.one * 256);

            /*Npc npc = CreateInstance<Npc>(world);
            npc.GetExt<Movement>().Teleport(new Vector3(50,0,50));
            world.AddEntity(npc);*/

            return world;
        }

        public static World CreateWorld(ServerWorldManager manager)
        {
            World w = CreateWorldInstance();
            manager.AddWorld(w);
            return w;
        }
        #endregion

        public int ID { get; set; }

        public ServerUnit this[int unitId]
        {
            get
            {
                if (unitId > entities.Count-1 || unitId < 0)
                {
                    //throw  new Exception("Wrong unit index: "+unitId);
                    return null;
                }
                return entities[unitId] as ServerUnit;
            }
        }
    }
}

