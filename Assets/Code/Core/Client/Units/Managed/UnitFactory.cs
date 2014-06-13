using Code.Core.Client.Units.Extensions;
using Code.Libaries.Generic;
using UnityEngine;

namespace Code.Core.Client.Units.Managed
{
    public class UnitFactory : MonoSingleton<UnitFactory> {

        [SerializeField]
        private PlayerUnit _playerUnitPrefab;

        /// <summary>
        /// Creates an player in the scene.
        /// </summary>
        /// <returns>The player.</returns>
        /// <param name="id">Identifier.</param>
        public PlayerUnit CreateNewUnit(int id){
            PlayerUnit playerUnit = ((GameObject)Instantiate (_playerUnitPrefab.gameObject)).GetComponent<PlayerUnit>();
            playerUnit.Id = id;
            return playerUnit;
        }

    }
}
