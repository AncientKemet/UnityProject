using System;
using System.Collections.Generic;
using System.Threading;
using Server.Model.Extensions.PlayerExtensions;

namespace Server
{
    /// <summary>
    /// A static class that runs on a separate thread, authenticating login attempts.
    /// </summary>
    public static class LoginServer
    {
        private static Thread _thread = new Thread(new ThreadStart(delegate
        {
            while (true)
            {
                Progress();
                Thread.Sleep(2000);
            }
        }));

        /// <summary>
        /// An index of next client to be progressed.
        /// </summary>
        private static int _currentIndex = 0;

        /// <summary>
        /// Current clients in a queue.
        /// </summary>
        private static List<ServerClient> _clients = new List<ServerClient>(); 

        /// <summary>
        /// When an new client socket connects. Hes registered.
        /// </summary>
        /// <param name="client">Client that just set up a socket.</param>
        public static void RegisterClient(ServerClient client)
        {
                _clients.Add(client);
                EnsureThreadIsRunning();
        }

        /// <summary>
        /// Makes sure the thread is running.
        /// </summary>
        private static void EnsureThreadIsRunning()
        {
            if(!_thread.IsAlive)
                _thread.Start();
        }

        /// <summary>
        /// Progress only one client per tick. Is called from thread.
        /// </summary>
        private static void Progress()
        {
            if (_clients.Count > 0)
            {
                if (_currentIndex >= _clients.Count)
                    _currentIndex = 0;

                if (_clients[_currentIndex] != null)
                {
                    ServerClient client = _clients[_currentIndex];
                    Action actionToRunOnUnityThread = client.Progress;

                    lock (ServerSingleton.StuffToRunOnUnityThread)
                    {
                        ServerSingleton.StuffToRunOnUnityThread.Add(actionToRunOnUnityThread);
                    }
                }

                _currentIndex++;
            }
        }
    }
}
