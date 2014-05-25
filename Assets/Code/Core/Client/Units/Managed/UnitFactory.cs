﻿using OldBlood.Code.Core.Client.Units.Extensions;
using UnityEngine;

namespace OldBlood.Code.Core.Client.Units.Managed
{
    public class UnitFactory : Monosingleton<UnitFactory> {

        [SerializeField]
        private Player _playerPrefab;

        /// <summary>
        /// Creates an player in the scene.
        /// </summary>
        /// <returns>The player.</returns>
        /// <param name="id">Identifier.</param>
        public Player CreatePlayer(int id){
            Player _player = ((GameObject)Instantiate (_playerPrefab.gameObject)).GetComponent<Player>();
            _player.ID = id;
            return _player;
        }

    }
}
