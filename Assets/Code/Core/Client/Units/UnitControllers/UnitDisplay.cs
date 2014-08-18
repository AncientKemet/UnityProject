using System.Collections;
using Code.Core.Client.Units.Extensions;
using Code.Core.Shared.Content;
using Code.Libaries.GameObjects;
using Code.Libaries.Generic.Managers;
using UnityEngine;

namespace Code.Core.Client.Units.UnitControllers
{
    public class UnitDisplay : MonoBehaviour
    {
        [SerializeField]
        private float MaxMovementSpeed = 0.2f;

        const float FADE_OUT_TIME = 0.25f;

        private PlayerUnit _unit;
        private Animation _animation;
        
        private Vector3 _lookAtPosition;
        private Vector3 _lookAtPositionLerped;

        private float _cachedWalkAnimLen = -1;
        private float _cachedRunAnimLen = -1;
        private int _model = -1;
        private bool _updateWalkRunStand = true;

        public Transform _neck, _chest, _leftFingers, _rightFingers, _leftShoulder, _rightShoulder;

        private string _standAnimation = "Idle";
        private string _walkAnimation = "Walk";
        private string _runAnimation = "Run";
        private string _actionAnimation;

        public string StandAnimation
        {
            get { return _standAnimation; }
            set { _standAnimation = value; }
        }

        public string WalkAnimation
        {
            get { return _walkAnimation; }
            set { _walkAnimation = value; }
        }

        public string RunAnimation
        {
            get { return _runAnimation; }
            set { _runAnimation = value; }
        }

        public string ActionAnimation
        {
            get { return _actionAnimation; }
            set
            {
                if(!string.IsNullOrEmpty(_actionAnimation))
                if (_animation.IsPlaying(_actionAnimation))
                {
                    _animation.Blend(_actionAnimation, 0);
                }
                _actionAnimation = value;

                StartCoroutine(FocusAnimation(value));

            }
        }

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

        public int Model
        {
            get { return _model; }
            set
            {
                if (_model != value)
                {
                    _model = value;
                    if (_model > ContentManager.I.Models.Count)
                    {
                        Debug.Log("Invalid model Id: "+value);
                        return;
                    }
                    SetModel((GameObject)Instantiate(ContentManager.I.Models[_model]));
                }
            }
        }

        public void Awake()
        {
            _unit = GetComponent<PlayerUnit>();
            _lookAtPosition = transform.position;
            _lookAtPositionLerped = _lookAtPosition;
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
            if(!_updateWalkRunStand)
                return;

            float speed = _unit.VisualSpeed;
            float maxSpeed = MaxMovementSpeed;
            float weightRun = _unit.VisualSpeed / maxSpeed;
            float weightWalk = 1f - weightRun;

            if (speed <= 0.017f)
            {
                _animation.Blend(StandAnimation, 1f, FADE_OUT_TIME);
                _animation.Blend(WalkAnimation, 0f, FADE_OUT_TIME);
                _animation.Blend(RunAnimation, 0f, FADE_OUT_TIME);
            }
            else
            {
                float walkSpeed = 0.7f + weightRun * 0.7f;
                if (speed <= MaxMovementSpeed / 2f)
                {
                    _animation[WalkAnimation].speed = walkSpeed;
                    _animation[RunAnimation].speed = walkSpeed;
                    _animation.Blend(StandAnimation, 0f, FADE_OUT_TIME);
                    _animation.Blend(WalkAnimation, 1f, FADE_OUT_TIME);
                    _animation.Blend(RunAnimation, 0, FADE_OUT_TIME);
                }
                else
                {
                    _animation[WalkAnimation].speed = walkSpeed;
                    _animation[RunAnimation].speed = walkSpeed;
                    _animation.Blend(StandAnimation, 0f, FADE_OUT_TIME);
                    _animation.Blend(WalkAnimation, weightWalk, FADE_OUT_TIME);
                    _animation.Blend(RunAnimation, weightRun, FADE_OUT_TIME);
                }
            }
        }

        void ProcessLookAtRotation()
        {
            if (true)
                return;
            _lookAtPositionLerped = Vector3.Lerp(_lookAtPositionLerped, _lookAtPosition, Time.deltaTime * 10);
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
            if (id != "-1")
                if (_animation != null)
                {
                    if (_animation[id] == null)
                    {
                        Debug.LogError("missing anim id: " + id);
                        return;
                    }

                    _animation[id].layer = layer;
                    _animation.Blend(id, strenght, FADE_OUT_TIME);
                    if (layer == 1)
                    {
                        _animation[id].AddMixingTransform(_chest);
                    }
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
            PlayAnimation(id, layer, 1f);
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

            _animation[StandAnimation].wrapMode = WrapMode.Loop;
            _animation[RunAnimation].wrapMode = WrapMode.Loop;
            _animation[WalkAnimation].wrapMode = WrapMode.Loop;
        }

        private bool IsPowerAnim(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;

            if (id.Contains("Power"))
            {
                return true;
            }
            return false;
        }


        private string _lastActivatedPowerAnim = "";
        IEnumerator FocusAnimation(string animationName)
        {

            if (!IsPowerAnim(animationName))
            {

                /*if (!string.IsNullOrEmpty(_lastActivatedPowerAnim))
                {
                    animation.Blend(_lastActivatedPowerAnim, 0, FADE_OUT_TIME);
                    _lastActivatedPowerAnim = "";
                }*/

                _updateWalkRunStand = false;

                _animation.Blend(StandAnimation, 0);
                _animation.Blend(WalkAnimation, 0);
                _animation.Blend(RunAnimation, 0);

                _animation.Blend(animationName, 1, FADE_OUT_TIME);
                yield return new WaitForSeconds(_animation[animationName].length - FADE_OUT_TIME);
                _animation.Blend(animationName, 0, FADE_OUT_TIME);

                _updateWalkRunStand = true;

            }
            else
            {
                _lastActivatedPowerAnim = animationName;

                _animation[animationName].layer = 1;
                _animation[animationName].AddMixingTransform(_chest);
                _animation.Blend(animationName, 1, FADE_OUT_TIME);
            }
        }
    }
}
