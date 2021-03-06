using Server;
using Server.Model;
using UnityEngine;

namespace Code.Libaries.UnityExtensions
{
    public class ServerMonoBehaviour : MonoBehaviour
    {
        public static T CreateInstance<T>() where T : ServerMonoBehaviour
        {
            return CreateInstance<T>(null);
        }

        public static T CreateInstance<T>(World world) where T : ServerMonoBehaviour
        {
            T t = new GameObject(typeof(T).Name).AddComponent<T>();

            if (world == null)
            {
                t.transform.parent = ServerSingleton.Instance.transform;
            }
            else
            {
                t.transform.parent = world.transform;
            }

            return t;
        }
    }
}

