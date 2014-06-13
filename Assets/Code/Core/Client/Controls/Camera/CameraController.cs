﻿using Code.Core.Client.Units;
using Code.Core.Client.Units.Extensions;
using Code.Libaries.Generic;
using UnityEngine;

namespace Code.Core.Client.Controls.Camera
{
    public class CameraController : MonoSingleton<CameraController>
    {

        public enum CameraType
        {
            Cinematic,
            Locked,
            Follow
        }

        [SerializeField]
        private GameObject
            _objectToFollow;
        [SerializeField]
        private float
            _cameraY = 10;
        [SerializeField]
        private float
            _cameraToObjectDistance = 10;
        [SerializeField]
        private float
            _rotation;
        [SerializeField]
        private CameraType
            _type;

        private Vector3 lastObjectPosition;
        private Vector3 objectLookVector3;

        public float rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }

        void LateUpdate()
        {
            if (_objectToFollow == null)
            {
                if (PlayerUnit.MyPlayerUnit != null)
                    _objectToFollow = PlayerUnit.MyPlayerUnit.gameObject;

                if (_type == CameraType.Follow)
                {
                    transform.parent = _objectToFollow.transform;
                }
            }

            Vector3 lookAtOffset = Vector3.zero;

            if (_objectToFollow != null)
            {
                Vector3 objectPos = _objectToFollow.transform.position;

                if (lastObjectPosition == Vector3.zero)
                {
                    lastObjectPosition = objectPos;
                }

                //CINEMATIC CAMERA
                if (_type == CameraType.Cinematic)
                {
                    Vector3 positionAboveObject = objectPos;
                    positionAboveObject.y += _cameraY;

                    if (Vector3.Distance(transform.position, positionAboveObject) > _cameraToObjectDistance)
                    {
                        if (Vector3.Distance(transform.position, positionAboveObject) > _cameraToObjectDistance * 2)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, positionAboveObject, Time.deltaTime);
                        }
                        transform.position = Vector3.Lerp(transform.position, positionAboveObject, Time.deltaTime);
                    }
                }

                //Locked CAMERA
                if (_type == CameraType.Locked)
                {
                    PlayerUnit moveableUnit = _objectToFollow.GetComponent<PlayerUnit>();

                    objectLookVector3 = Vector3.Lerp(objectLookVector3, _objectToFollow.transform.forward * (objectPos - lastObjectPosition).magnitude, Time.deltaTime);


                    float x = objectPos.x + _cameraToObjectDistance* (Input.GetMouseButton(2) ? 0.5f : 1f) * Mathf.Cos(_rotation);
                    float z = objectPos.z + _cameraToObjectDistance* (Input.GetMouseButton(2) ? 0.5f : 1f) * Mathf.Sin(_rotation);

                    lookAtOffset = objectLookVector3;

                    Vector3 _targetPos = new Vector3(x, objectPos.y + _cameraY * (Input.GetMouseButton(2) ? 0.5f : 1f), z) + objectLookVector3;

                    transform.position = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * 10);

                    lastObjectPosition += (objectPos-lastObjectPosition) / 50f;
                }

                //Follow CAMERA
                if (_type == CameraType.Follow)
                {
                    transform.parent = _objectToFollow.transform;
                    transform.localPosition = new Vector3(0, _cameraY, _cameraToObjectDistance);
                    /*_rotation = _objectToFollow.transform.eulerAngles.y/16;
                    float x = _objectToFollow.transform.DirecionVector.x + _cameraToObjectDistance * Mathf.Cos(_rotation);
                    float z = _objectToFollow.transform.DirecionVector.z + _cameraToObjectDistance * Mathf.Sin(_rotation);
                    Vector3 _targetPos = new Vector3(x, _objectToFollow.transform.DirecionVector.y + _cameraY, z);

                    transform.DirecionVector = Vector3.Lerp(transform.DirecionVector, _targetPos, Time.deltaTime * 10);*/

                }

                transform.LookAt(objectPos + Vector3.up + lookAtOffset);
            }
        }

    }
}
