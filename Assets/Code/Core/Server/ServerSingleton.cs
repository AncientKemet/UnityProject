using System;
using System.Collections;
using System.Collections.Generic;
using Code.Libaries.Generic;
using UnityEngine;

namespace Server
{
    public class ServerSingleton : MonoSingleton<ServerSingleton>
    {

        public static LinkedList<Action> StuffToRunOnUnityThread;

        private Server _server;

        public Server Server
        {
            get { return _server; }
        }

        protected override void OnAwake()
        {
            StuffToRunOnUnityThread = new LinkedList<Action>();
        }

        private void OnEnable()
        {
            _server = new Server();;
            Server.StartServer();
        }

        private void OnDisable()
        {
            Server.Stop();
        }
	
        void FixedUpdate () {
            //Run stuff that needs to be ran
            lock (StuffToRunOnUnityThread)
            {
                foreach (Action action in StuffToRunOnUnityThread)
                {
                    action();
                }

                StuffToRunOnUnityThread.Clear();
            }

            Server.ServerUpdate();
        }

        private void OnDrawGizmos()
        {
            if(Application.isPlaying)
                if(Server != null)
                    Server.swm.Get.Kemet.Tree.DrawGizmos();
        }

        public void RunCoroutine(IEnumerator function)
        {
            StartCoroutine(function);
        }
    }
}
