using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	[SerializeField]
	private GameObject _objectToFollow;
	[SerializeField]
	private float _cameraY = 10;
	[SerializeField]
	private float _cameraToObjectDistance = 10;

	void Update()
	{
		if (_objectToFollow != null) {
			transform.LookAt (_objectToFollow.transform.position + Vector3.up);

			Vector3 positionAboveObject = _objectToFollow.transform.position;
			positionAboveObject.y += _cameraY;

			if (Vector3.Distance (transform.position, positionAboveObject) > _cameraToObjectDistance) {
				transform.position = Vector3.MoveTowards (transform.position, positionAboveObject, Time.deltaTime);
				transform.position = Vector3.Lerp (transform.position, positionAboveObject, Time.deltaTime);
			}
		}
	}

}
