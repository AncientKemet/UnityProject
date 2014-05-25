using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OldBlood.Code.Core.Server.Model
{
    public abstract class EntityExtension
    {
        public WorldEntity entity { get; set; }

        public abstract void Progress();
    }
}
