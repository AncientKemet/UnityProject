using UnityEngine;
using System.Collections;

// author paris stefanou

public class movesimple : MonoBehaviour  {

	public Vector3 targetPosition { get; set; }
	public float movementSpeed { get; set; }
	 void Update () {
		transform.LookAt (targetPosition, Vector3.up);
		transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, Time.deltaTime * movementSpeed );
	}
}
