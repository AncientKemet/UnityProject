using System;
using OldBlood.Code.Core.Server.Model;
using System.Collections.Generic;

namespace OldBlood.Code.Core.Server
{
    public class ServerWorldManager
    {
        private List<World> worlds = new List<World>();

        private World _kemet;
        public World Kemet
        {
            get
            {
                if(_kemet == null)
                {
                    _kemet = World.CreateWorld(this);
                }
                return _kemet;
            }
        }

        public void AddWorld(World w)
        {
            worlds.Add(w);
            w.ID = worlds.IndexOf(w);
        }

        public void ProgressWorlds()
        {
            foreach (var w in worlds)
            {
                w.Progress();
            }
        }
    }
}

