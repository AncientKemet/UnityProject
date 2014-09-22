using Code.Core.Client.UI;
using Code.Core.Client.Units;
using UnityEngine;

namespace Client.UI.Interfaces.Profile
{
    public class ProfileInterface : UIInterface<ProfileInterface>
    {

        public PlayerUnit Unit { get; set; }
        
    }
}
