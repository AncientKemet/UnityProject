using System.Collections.Generic;
using Code.Core.Shared.Content.Spawns;

namespace Server.Model.ContentHandling
{
    public class ServerSpawnManager : WorldEntity
    {
        private List<ItemSpawn.ServerItemSpawn> _managedSpawnsUnits = new List<ItemSpawn.ServerItemSpawn>(); 

        public void AddItemSpawn(ItemSpawn.ServerItemSpawn serverItemSpawn)
        {
            _managedSpawnsUnits.Add(serverItemSpawn);
        }

        private void FixedUpdate()
        {
            foreach (var spawn in _managedSpawnsUnits)
            {
                if (spawn.DroppedItem == null)
                {
                    spawn.Spawn();
                    if(spawn.DroppedItem != null)
                    CurrentWorld.AddEntity(spawn.DroppedItem);
                }
            }
        }

        private static Dictionary<World, ServerSpawnManager> managers = new Dictionary<World, ServerSpawnManager>();  
        public static ServerSpawnManager Instance(World world)
        {
            if (!managers.ContainsKey(world))
            {
                managers.Add(world, CreateInstance<ServerSpawnManager>());
                world.AddEntity(managers[world]);
            }
            return managers[world];
        }
    }
}

