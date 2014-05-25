using Code.Libaries.GameObjects;
using OldBlood.Code.Core.Client.Units.Extensions;
using UnityEngine;

namespace OldBlood.Code.Core.Client.Units.UnitControllers
{
    public class UnitAnimator : MonoBehaviour
    {
        const float FADE_OUT_TIME = 0.25f;
        [SerializeField]
        private MoveableUnit
            _unit;
        [SerializeField]
        private Animation
            _animation;
        private Transform _neck, _chest;

        public Vector3 lookAtPosition
        {
            get
            {
                return _lookAtPosition;
            }
            set
            {
                _lookAtPosition = value;
            }
        }

        private Vector3 _lookAtPosition;
        private Vector3 _lookAtPositionLerped;
        private float __cachedWalkAnimLen = -1;
        private float __cachedRunAnimLen = -1;

        public float WalkAnimLen
        {
            get
            {
                if (__cachedWalkAnimLen == -1)
                {
                    __cachedWalkAnimLen = _animation ["Walk"].length;
                }
                return __cachedWalkAnimLen;
            }
        }

        public float RunAnimLen
        {
            get
            {
                if (__cachedRunAnimLen == -1)
                {
                    __cachedRunAnimLen = _animation ["Run"].length;
                }
                return __cachedRunAnimLen;
            }
        }

        public void Start()
        {
            _lookAtPosition = transform.position;
            _lookAtPositionLerped = _lookAtPosition;

            //Instantiate
            Animation _instantiatedMesh = ((GameObject)Instantiate(_animation.gameObject)).GetComponent<Animation>();
            _instantiatedMesh.transform.parent = transform;
            _instantiatedMesh.transform.localPosition = Vector3.zero;

            _animation = _instantiatedMesh;

            _neck = TransformHelper.FindTraverseChildren("Neck", _animation.transform);
            _chest = TransformHelper.FindTraverseChildren("chest", _animation.transform);

            _animation ["Idle"].wrapMode = WrapMode.Loop;
            _animation ["Run"].wrapMode = WrapMode.Loop;
            _animation ["Walk"].wrapMode = WrapMode.Loop;
        }

        public void LateUpdate()
        {
            ProcessWalkAndStand();
            ProcessLookAtRotation();
        }
    
        /// <summary>
        /// Processes the walk and stand.
        /// </summary>
        private void ProcessWalkAndStand()
        {
            float speed = _unit.VisualSpeed;
            float maxSpeed = 8;
            float weightRun = _unit.VisualSpeed / maxSpeed;
            float weightWalk = 1 - weightRun;
            //Debug.Log("dt: " + speed);
            if (speed <= 0.1f)
            {
                _animation.Blend("Idle", 1f, 0.1f);
                _animation.Blend("Walk", 0f, FADE_OUT_TIME);
                _animation.Blend("Run", 0f, FADE_OUT_TIME);
            } else
                if (speed > 0.1f)
                {
                    _animation.Stop("Idle");
                    float walkSpeed = 0.5f + weightRun;
                    if(speed  <= 2)
                    {
                        _animation ["Walk"].speed = walkSpeed;
                        _animation ["Run"].speed = walkSpeed;
                        _animation.Blend("Idle", 0f, 0.5f);
                        _animation.Blend("Walk", 1, FADE_OUT_TIME);
                        _animation.Blend("Run", 0, FADE_OUT_TIME);
                    }else{
                        _animation ["Walk"].speed = walkSpeed;
                        _animation ["Run"].speed = walkSpeed;
                        _animation.Blend("Idle", 0f, 0.5f);
                        _animation.Blend("Walk", weightWalk, FADE_OUT_TIME);
                        _animation.Blend("Run", weightRun, FADE_OUT_TIME);
                    }
                }
        }

        void ProcessLookAtRotation()
        {
            _lookAtPositionLerped = Vector3.Lerp(_lookAtPositionLerped, _lookAtPosition, Time.deltaTime *10);
            LookAtBone(_chest, 0.75f);
            LookAtBone(_neck, 0.75f);
        }

        void LookAtBone(Transform bone, float strenght)
        {
            if (bone != null)
            {
                Vector3 euler = Quaternion.LookRotation(_lookAtPositionLerped - bone.position, Vector3.up).eulerAngles;
                euler.z -= 90;
                Quaternion rot = Quaternion.Lerp(Quaternion.Euler(euler), bone.rotation, strenght);
                bone.rotation = rot;
            }
        }
    }
}
