using UnityEngine;
using System.Collections;

public class MouseInput : Monosingleton<MouseInput> {

	void FixedUpdate () {
    MoveMyPlayerToClick();
	}

  static void MoveMyPlayerToClick()
  {
    if (Input.GetMouseButton(0))
    {
      RaycastHit hit;
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      float distance = 100;
      int layerMask = 1 << 7;
      layerMask = ~layerMask;
      //Walkable layer
      if (Physics.Raycast(ray, out hit, distance, layerMask))
      {
        Player.MyPlayer.MovementTargetPosition = hit.point;
      }
    }
    else
    {
      Player.MyPlayer.MovementTargetPosition = Player.MyPlayer.transform.localPosition;
    }
  }
}
