using UnityEngine;
using System.Collections;

public class DetectHits  {

	bool DetectHitInCircle(float maxradius,float minradius,Vector2 xztarget,Vector2 xzattack){
		float distance=sqrt(Mathf.Pow(xztarget(0)-xzattack(0),2)+Mathf.Pow(xztarget(1)-xzattack(1),2));
		if(distance<maxradius&&distance>miminradius)
			return(true);
		else
			return(false);
	}

}
