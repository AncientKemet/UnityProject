using Code.Core.Client.Units.Extensions;
using Code.Libaries.GameObjects;
using UnityEngine;

namespace Code.Core.Client.Units.UnitControllers
{
    public class UnitDisplay : MonoBehaviour
    {
        [SerializeField] private float MaxMovementSpeed = 0.2f;

        const float FADE_OUT_TIME = 0.25f;

        private PlayerUnit _unit;
        private Animation _animation;

        public Transform _neck, _chest, _leftFingers, _rightFingers, _leftShoulder, _rightShoulder;

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

        public void Awake()
        {
            _unit = GetComponent<PlayerUnit>();
            _lookAtPosition = transform.position;
            _lookAtPositionLerped = _lookAtPosition;

           /* if (_animation == null)
            {
                //Instantiate
                Animation _instantiatedMesh =
                    ((GameObject) Instantiate(_animation.gameObject)).GetComponent<Animation>();
                _instantiatedMesh.transform.parent = transform;
                _instantiatedMesh.transform.localPosition = Vector3.zero;

                _animation = _instantiatedMesh;

                _neck = TransformHelper.FindTraverseChildren("Neck", _animation.transform);
                _chest = TransformHelper.FindTraverseChildren("chest", _animation.transform);

                _rightFingers = TransformHelper.FindTraverseChildren("Right_fingers", _animation.transform);
                _leftFingers = TransformHelper.FindTraverseChildren("Left_fingers", _animation.transform);
                _leftShoulder = TransformHelper.FindTraverseChildren("Left_shoulder", _animation.transform);
                _rightShoulder = TransformHelper.FindTraverseChildren("Right_shoulder", _animation.transform);

                _animation["Idle"].wrapMode = WrapMode.Loop;
                _animation["Run"].wrapMode = WrapMode.Loop;
                _animation["Walk"].wrapMode = WrapMode.Loop;
            }*/
        }

        public virtual void Update()
        {
            if (_animation != null)
            {
                ProcessWalkAndStand();
                ProcessLookAtRotation();
            }
        }
    
        /// <summary>
        /// Processes the walk and stand.
        /// </summary>
        private void ProcessWalkAndStand()
        {
            float speed = _unit.VisualSpeed;
            float maxSpeed = MaxMovementSpeed;
            float weightRun = _unit.VisualSpeed / maxSpeed;
            float weightWalk = 1f - weightRun;

            if (speed <= 0.017f)
            {
                _animation.Blend("Idle", 1f, FADE_OUT_TIME);
                _animation.Blend("Walk", 0f, FADE_OUT_TIME);
                _animation.Blend("Run", 0f, FADE_OUT_TIME);
            } else {
                    float walkSpeed = 0.7f + weightRun*0.5f;
                    if(speed  <= MaxMovementSpeed/2f)
                    {
                        _animation ["Walk"].speed = walkSpeed;
                        _animation ["Run"].speed = walkSpeed;
                        _animation.Blend("Idle", 0f, FADE_OUT_TIME);
                        _animation.Blend("Walk", 1f, FADE_OUT_TIME);
                        _animation.Blend("Run", 0, FADE_OUT_TIME);
                    }else{
                        _animation ["Walk"].speed = walkSpeed;
                        _animation ["Run"].speed = walkSpeed;
                        _animation.Blend("Idle", 0f, FADE_OUT_TIME);
                        _animation.Blend("Walk", weightWalk, FADE_OUT_TIME);
                        _animation.Blend("Run", weightRun, FADE_OUT_TIME);
                        Debug.Log("walk: "+weightWalk+"run :"+weightRun);
                    }
                }
        }

        void ProcessLookAtRotation()
        {
            if(true)
                return;
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

        public void PlayAnimation(string id, int layer, float strenght)
        {
            if (_animation != null)
            {
                _animation[id].layer = layer;
                _animation.Blend(id, strenght, FADE_OUT_TIME);
                if (layer == 2)
                {
                    _animation[id].AddMixingTransform(_leftShoulder);
                }
                if (layer == 3)
                {
                    _animation[id].AddMixingTransform(_rightShoulder);
                }
            }
        }

        public void PlayAnimation(string id, int layer)
        {
            PlayAnimation(id , layer, 1f);
        }

        public void SetModel(GameObject model)
        {
            _animation = model.GetComponent<Animation>();

            model.transform.parent = transform;
            model.transform.localPosition = Vector3.zero;

            _neck = TransformHelper.FindTraverseChildren("Neck", model.transform);
            _chest = TransformHelper.FindTraverseChildren("chest", model.transform);

            _rightFingers = TransformHelper.FindTraverseChildren("Right_fingers", model.transform);
            _leftFingers = TransformHelper.FindTraverseChildren("Left_fingers", model.transform);
            _leftShoulder = TransformHelper.FindTraverseChildren("Left_shoulder", model.transform);
            _rightShoulder = TransformHelper.FindTraverseChildren("Right_shoulder", model.transform);

            _animation["Idle"].wrapMode = WrapMode.Loop;
            _animation["Run"].wrapMode = WrapMode.Loop;
            _animation["Walk"].wrapMode = WrapMode.Loop;
        }
    }
}
