using UnityEngine;
using System.Collections;

public class CameraController : Monosingleton<CameraController> {

  public enum CameraType
  {
    Cinematic, Locked
  }

	[SerializeField]
	private GameObject _objectToFollow;
	[SerializeField]
	private float _cameraY = 10;
	[SerializeField]
	private float _cameraToObjectDistance = 10;
  [SerializeField]
  private float _rotation;
  [SerializeField]
  private CameraType _type;
 

	void Update()
	{
    if (_objectToFollow == null)
    {
      _objectToFollow = Player.MyPlayer.gameObject;
    }

		if (_objectToFollow != null) {
			transform.LookAt (_objectToFollow.transform.position + Vector3.up);

      //CINEMATIC CAMERA
      if(_type == CameraType.Cinematic)
      {
  			Vector3 positionAboveObject = _objectToFollow.transform.position;
  			positionAboveObject.y += _cameraY;

  			if (Vector3.Distance (transform.position, positionAboveObject) > _cameraToObjectDistance) {
          if (Vector3.Distance (transform.position, positionAboveObject) > _cameraToObjectDistance * 2) {
  				  transform.position = Vector3.MoveTowards (transform.position, positionAboveObject, Time.deltaTime);
          }
  				transform.position = Vector3.Lerp (transform.position, positionAboveObject, Time.deltaTime);
  			}
      }

      //Locked CAMERA
      if(_type == CameraType.Locked)
      {
        float x = _objectToFollow.transform.position.x + _cameraToObjectDistance * Mathf.Cos(_rotation);
        float z = _objectToFollow.transform.position.z + _cameraToObjectDistance * Mathf.Sin(_rotation);
        Vector3 _targetPos = new Vector3(x, _objectToFollow.transform.position.y + _cameraY, z);

        transform.position = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * 5);

      }
		}
	}

}
