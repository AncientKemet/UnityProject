using UnityEngine;
using System.Collections;

// author paris stefanou

public class MouseClick : MonoBehaviour {

	void Update() {
		if(Input.GetMouseButtonDown(0))
			Debug.Log("detect colitions wip");
		if(Input.GetMouseButtonDown(1)){
			var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast (ray, out hit, 10000)) 
		{
			Unit firstUnit = UnitManager.Instance.GetUnit(0);
			firstUnit.targetPosition = new Vector3(hit.point.x, 0, hit.point.z);
			Debug.Log("click detected at loc");
			Debug.Log(firstUnit.targetPosition);
		}

	}
		if(Input.GetMouseButtonDown(2))
			Debug.Log("camera rotation active");
	}
		
}
