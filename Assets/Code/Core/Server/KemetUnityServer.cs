using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace OldBlood.Code.Core.Server
{
    public class KemetUnityServer
    {
        [MenuItem("Kemet/Server/Run")]
        public static void RunServer()
        {
            Server server = new Server();
            server.StartServer();
        }

        [MenuItem("Kemet/Server/Stop")]
        public static void StopServer()
        {
            if (Server.Instance != null)
                Server.Instance.Stop();
        }
    }
}
