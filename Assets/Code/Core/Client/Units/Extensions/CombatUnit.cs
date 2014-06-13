using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Core.Client.Units.Extensions
{
    public class CombatUnit : MonoBehaviour
    {
        [SerializeField]
        private tk2dSlicedSprite healthBar, energyBar;

        private float _fullHealthBarSize, _fullEnergyBarSize;

        private void Awake()
        {
            _fullHealthBarSize = 170;
            _fullEnergyBarSize = 170;
        }

        public void SetHealth(float health)
        {
            health /= 100f;
            healthBar.dimensions = new Vector2(_fullHealthBarSize * health, healthBar.dimensions.y);
            healthBar.ForceBuild();
        }

        public void SetEnergy(float energy)
        {
            energy /= 100f;
            energyBar.dimensions = new Vector2(_fullEnergyBarSize * energy, energyBar.dimensions.y);
            energyBar.ForceBuild();
        }
    }
}
