using Code.Core.Shared.Content.Types;
using Code.Libaries.UnityExtensions;
using Server.Model.ContentHandling;
using Server.Model.Entities.Items;
using UnityEngine;

namespace Code.Core.Shared.Content.Spawns
{
    public class ItemSpawn : MonoBehaviour
    {

        public Item ItemToSpawn;
        public float RespawnTime = 30;

        private void Start()
        {
#if UNITY_EDITOR
            ServerSpawnManager.Instance(Server.Server.Instance.swm.Get.Kemet).AddItemSpawn(new ServerItemSpawn(transform.position, transform.eulerAngles, ItemToSpawn, RespawnTime));
            if(Application.isPlaying)
                Destroy(gameObject);
#endif
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position, Vector3.one /3f);
        }

        public class ServerItemSpawn
        {

            public DroppedItem DroppedItem { get; private set; }

            public ServerItemSpawn(Vector3 position, Vector3 eulerAngles, Item itemToSpawn, float respawnTime)
            {
                Position = position;
                EulerAngles = eulerAngles;
                ItemToSpawn = itemToSpawn;
                RespawnTime = respawnTime;
            }

            public void Spawn()
            {
                if (Time.realtimeSinceStartup - RespawnTime > LastTimeSpawned)
                {
                    LastTimeSpawned = Time.realtimeSinceStartup;
                    DroppedItem = ServerMonoBehaviour.CreateInstance<DroppedItem>();
                    DroppedItem.Item = ItemToSpawn;
                    DroppedItem.Movement.Teleport(Position);
                    DroppedItem.Movement.Rotation = EulerAngles.y;
                }
            }

            private float LastTimeSpawned { get; set; }

            private Vector3 EulerAngles { get; set; }

            private float RespawnTime { get; set; }

            private Item ItemToSpawn { get; set; }

            private Vector3 Position { get; set; }
        }
    }
}

