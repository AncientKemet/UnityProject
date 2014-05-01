using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Route : MonoBehaviour {

	[SerializeField]
	public List<Node> nodes;

	public Vector2[,] nodelocs ;
	public int[,] nodeids;
	void Start () {
		Node Base;
		Node Link;
		int writeloc=0;
		bool connectionfound=false;
		nodelocs= new Vector2[nodes.Count,2];
		nodeids = new int[nodes.Count, 2];
		for(int i=0;i<nodes.Count;i++){
			Base=nodes[i];
		Link=Base.connection1;

		for(int k=0;k<writeloc;k++){
			if((nodelocs[k,0]==Base.nodelocation&&nodelocs[k,1]==Link.nodelocation)||(nodelocs[k,1]==Base.nodelocation&&nodelocs[k,0]==Link.nodelocation)){
				connectionfound=true;
			break;
			}
			}
		if(connectionfound==false){
			nodelocs[writeloc,0]=Base.nodelocation;
			nodelocs[writeloc,1]=Link.nodelocation;
			nodeids[writeloc,0]=i;
			nodeids[writeloc,1]=nodes.IndexOf(Link);
			writeloc++;}
			connectionfound=false;}
		for(int i=0;i<nodes.Count;i++){
			Base=nodes[i];
			Link=Base.connection2;
			
			for(int k=0;k<writeloc;k++){
				if((nodelocs[k,0]==Base.nodelocation&&nodelocs[k,1]==Link.nodelocation)||(nodelocs[k,1]==Base.nodelocation&&nodelocs[k,0]==Link.nodelocation)){
					connectionfound=true;
					break;
				}
			}
			if(connectionfound==false){
				nodelocs[writeloc,0]=Base.nodelocation;
				nodelocs[writeloc,1]=Link.nodelocation;
				nodeids[writeloc,0]=i;
				nodeids[writeloc,1]=nodes.IndexOf(Link);
				writeloc++;}
			connectionfound=false;}
	}
}
