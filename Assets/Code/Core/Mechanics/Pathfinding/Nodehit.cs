using UnityEngine;
using System.Collections;

public class Nodehit:MonoBehaviour{
	Vector2 target;
	private Route nodelist;
	// Use this for initialization
	void Start () {
		nodelist = (Route)FindObjectOfType (typeof(Route));
		findwherelinescolide(new Vector2(0,0),new Vector2(1,1),new Vector2(0,1),new Vector2(1,0));
	}

	Vector2 pathfinder(Vector2 location,Vector2 target,Node currentnodehit,Node othernodehit){
		Vector2 hit,previoushit;
		int[] ids=new int[2];
		float distance,distance1,mindistance=Mathf.Infinity;
		bool hitonce=false;
		for (int i=0; i<nodelist.nodes.Count; i++) {
			hit = findwherelinescolide (location, target, nodelist.nodelocs [i, 0], nodelist.nodelocs [i, 1]);
			distance=Mathf.Sqrt(Mathf.Pow((hit[0]-location[0]),2)+Mathf.Pow((hit[1]-location[1]),2));
			if(distance<mindistance){
				mindistance=distance;
				previoushit=hit;
				ids[0]=nodelist.nodeids[i,0];
				ids[1]=nodelist.nodeids[i,1];
				hitonce=true;
			}
		}

		if (hitonce == false)
			return(new Vector2(Mathf.Sqrt (Mathf.Pow ((target [0] - location [0]), 2) + Mathf.Pow ((target [1] - location [1]), 2)),-1));
		else {
			if (nodelist.nodes [ids [0]] == currentnodehit || nodelist.nodes [ids [1]] == currentnodehit|| nodelist.nodes [ids [0]] == othernodehit || nodelist.nodes [ids [1]] == othernodehit) {
				if(currentnodehit.connection1!=othernodehit)
					mindistance+=pathfinder (currentnodehit.connection1.walkablePoint.transform.position, target, currentnodehit.connection1,currentnodehit)[0];
				if(currentnodehit.connection2!=othernodehit)
					mindistance+=pathfinder (currentnodehit.connection2.walkablePoint.transform.position, target, currentnodehit.connection2,currentnodehit)[0];

			} else {
				distance=pathfinder (nodelist.nodes [ids [0]].walkablePoint.transform.position, target, nodelist.nodes [ids [0]],null)[0];
				distance1=pathfinder (nodelist.nodes [ids [1]].walkablePoint.transform.position, target, nodelist.nodes [ids [1]],null)[0];
				if(distance<distance1){


				}
				else{
				}
			}
		
		}

		return (new Vector2(mindistance,currentnodehit.id));
	}
	Vector2 findwherelinescolide(Vector2 loc11,Vector2 loc12 , Vector2 loc21 , Vector2 loc22){
		Vector2 loc13=new Vector2((loc11[0]+loc12[0])/2f,(loc11[1]+loc12[1])/2f);
		Vector2 loc23=new Vector2((loc21[0]+loc22[0])/2f,(loc21[1]+loc22[1])/2f);
		float a1,b1,g1,a2,b2,g2;

		a1=(loc11[0]-loc12[0])*(loc11[1]-loc13[1])/((loc11[1]-loc12[1])*(loc11[0]-loc13[0]));
		b1=-a1*(loc11[0]-loc12[0])/(loc11[1]-loc12[1]);
		g1=a1*loc11[0]+b1*loc11[1];

		a2=(loc21[0]-loc22[0])*(loc21[1]-loc23[1])/((loc21[1]-loc22[1])*(loc21[0]-loc23[0]));
		b2=-a2*(loc21[0]-loc22[0])/(loc21[1]-loc22[1]);
		g2=a2*loc21[0]+b2*loc21[1];
		Debug.Log (a1+" "+b1+" "+g1+" "+a2+b2+g2);
		float d ,dx ,dy;
		d=a1*b2-a2*b1;
		dx=g1*b2-g2*b1;
		dy=a1*g2-a2*g1;
		Vector2 hit=new Vector2(dx/d,dy/d);
		if (hit [0] > Mathf.Max (loc11 [0], loc12 [0]) || hit [0] > Mathf.Max (loc21 [0], loc22 [0]) || hit [0] < Mathf.Min (loc11 [0], loc12 [0]) || hit [0] < Mathf.Min (loc21 [0], loc22 [0]))
			return(new Vector2(Mathf.Infinity,Mathf.Infinity));
		if (hit [1] > Mathf.Max (loc11 [1], loc12 [1]) || hit [1] > Mathf.Max (loc21 [1], loc22 [1]) || hit [1] < Mathf.Min (loc11 [1], loc12 [1]) || hit [1] < Mathf.Min (loc21 [1], loc22 [1]))
			return(new Vector2(Mathf.Infinity,Mathf.Infinity));
		Debug.Log ("colide detect "+hit[0]+" "+hit[1]);
		return(hit);
	}

}
