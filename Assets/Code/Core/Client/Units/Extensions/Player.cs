using UnityEngine;
using System.Collections.Generic;

namespace OldBlood.Code.Core.Client.Units.Extensions
{
    public class Player : MoveableUnit
    {

        private static Player _myPlayerInstance;

        public static Player MyPlayer
        {
            get
            {
                if (_myPlayerInstance == null)
                {
                    _myPlayerInstance = UnitFactory.Instance.CreatePlayer(0);
                }
                return _myPlayerInstance;
            }
        }

        private string name;

    }
}