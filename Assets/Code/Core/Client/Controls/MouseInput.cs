using OldBlood.Code.Core.Client.Units.Extensions;
using OldBlood.Code.Core.Client.Units.UnitControllers;
using UnityEngine;

namespace OldBlood.Code.Core.Client.Controls
{
    public class MouseInput : Monosingleton<MouseInput>
    {

        void FixedUpdate()
        {
            MoveMyPlayerToClick();
        }

        static void MoveMyPlayerToClick()
        {
            RaycastHit hit;
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance = 100;
            int layerMask = 1 << 7;
            layerMask = ~layerMask;
            //Walkable layer
            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                Player.MyPlayer.GetComponent<UnitAnimator>().lookAtPosition = hit.point;
    
                if (Input.GetMouseButton(0))
                {
                    Player.MyPlayer.MovementTargetPosition = hit.point;
                } else
                {
                    Player.MyPlayer.MovementTargetPosition = Player.MyPlayer.transform.localPosition;
                }
            }

        }
    }
}
