using OldBlood.Code.Core.Client.Terrain;
using UnityEngine;

namespace OldBlood.Code.Core.Client.Units.Extensions
{
    public class MoveableUnit : Unit
    {

        [SerializeField]
        private float
            _basemovementSpeed = 1;
        [SerializeField]
        private float
            _baseTurnSpeed = 1;

        /// <summary>
        /// Gets the current movement speed.
        /// </summary>
        /// <value>The current movement speed.</value>
        public float CurrentMovementSpeed
        {
            get
            {
                return _basemovementSpeed;
            }
        }

        /// <summary>
        /// Gets or sets the movement target position.
        /// The value has to be in parent's local space;
        /// </summary>
        /// <value>The movement target position.</value>
        public Vector3 MovementTargetPosition
        {
            get
            {
                return movementTargetPosition;
            }
            set
            {
                movementTargetPosition = value;
            }
        }

        private Vector3 movementTargetPosition;
        private Quaternion movementTargetRotation;
        private float distanceToTarget;

        public float VisualSpeed
        {
            get
            {
                return Mathf.Clamp(distanceToTarget, 0f, 8f);
            }
        }

        /// <summary>
        /// Raises the start event.
        /// Set's the MovementTargetPosition to current position. 
        /// </summary>
        protected override void OnStart()
        {
            base.OnStart();
            MovementTargetPosition = transform.localPosition;
        }

        /// <summary>
        /// Raises the update event.
        /// Moves the unit towards its MovementTargetPosition and fixes the Y coord to fit terrain height.
        /// </summary>
        protected override void OnUpdate()
        {
            base.OnUpdate();

            distanceToTarget = Vector3.Distance(transform.localPosition, movementTargetPosition);

            Vector3 calculatedPosition = transform.localPosition;

            {// Process position
      
                calculatedPosition = Vector3.Lerp(calculatedPosition, transform.localPosition + transform.forward, Time.deltaTime * VisualSpeed);
        
                FixYOnTerrain(ref calculatedPosition);
                transform.localPosition = calculatedPosition;
            }

            Quaternion calculatedRotation;

            {// Process rotation

                if (distanceToTarget > 0.25f)
                {
                    Quaternion oldRot = transform.rotation;

                    if (transform.parent != null)
                    {
                        transform.LookAt(transform.parent.position + MovementTargetPosition);
                    } else
                    {
                        transform.LookAt(MovementTargetPosition);
                    }

                    calculatedRotation = transform.rotation;
                    calculatedRotation.x = 0;
                    calculatedRotation.z = 0;

                    calculatedRotation = Quaternion.Lerp(oldRot, calculatedRotation, Time.deltaTime * _baseTurnSpeed);

                    transform.rotation = calculatedRotation;
                }
            }
        }

        private void FixYOnTerrain(ref Vector3 position)
        {
            Ray ray = new Ray(position + new Vector3(0, 50, 0), Vector3.down);
            RaycastHit hit = new RaycastHit();
            if (KemetTerrain.Instance.terrainCollider.Raycast(ray, out hit, 100.0f))
            {
                position.y = hit.point.y;
            }
        }

    }
}
