using UnityEngine;
using System.Collections;

public class MoveableUnit : Unit {

	private Vector3 targetPosition ;
	[SerializeField]
  private float _basemovementSpeed=0;

  public float CurrentMovementSpeed
  {
    get
    {
      return _basemovementSpeed;
    }
  }



	void Update () {
		transform.LookAt (targetPosition, Vector3.up);
    transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, Time.deltaTime * CurrentMovementSpeed );
	}
}
