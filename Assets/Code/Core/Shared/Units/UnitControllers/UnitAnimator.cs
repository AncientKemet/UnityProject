using UnityEngine;
using System.Collections;

public class UnitAnimator : MonoBehaviour {
  const float FADE_OUT_TIME = 0.25f;

  [SerializeField]
  private MoveableUnit _unit;
  [SerializeField]
  private Animation _anim;


  private float __cachedWalkAnimLen = -1;
  private float __cachedRunAnimLen = -1;

  public float WalkAnimLen
  {
    get
    {
      if(__cachedWalkAnimLen == -1){
        __cachedWalkAnimLen = _anim["Walk"].length;
      }
      return __cachedWalkAnimLen;
    }
  }

  public float RunAnimLen
  {
    get
    {
      if(__cachedRunAnimLen == -1){
        __cachedRunAnimLen = _anim["Run"].length;
      }
      return __cachedRunAnimLen;
    }
  }

  public void Start()
  {
    _anim ["Idle"].wrapMode = WrapMode.Loop;
    _anim ["Run"].wrapMode = WrapMode.Loop;
    _anim ["Walk"].wrapMode = WrapMode.Loop;
  }

  public void LateUpdate()
  {
    float speed = _unit.VisualSpeed;

    float maxSpeed = 8;
    float weightRun = _unit.VisualSpeed / maxSpeed;
    float weightWalk = 1 - weightRun;

    Debug.Log("dt: " + speed);
    if (speed <= 0.1f)
    {
      _anim.Blend("Idle", 1f, 0.1f);
      _anim.Blend("Walk", 0f, FADE_OUT_TIME);
      _anim.Blend("Run", 0f, FADE_OUT_TIME);

    }
    else if (speed > 0.1f)
    {
      _anim.Stop("Idle");

      float walkSpeed = 0.5f+weightRun;

      _anim ["Walk"].speed = walkSpeed;
      _anim ["Run"].speed = walkSpeed;
      _anim.Blend("Idle", 0f, 0.5f);
      _anim.Blend("Walk", weightWalk, FADE_OUT_TIME);
      _anim.Blend("Run", weightRun, FADE_OUT_TIME);
    }



  }
	
}
