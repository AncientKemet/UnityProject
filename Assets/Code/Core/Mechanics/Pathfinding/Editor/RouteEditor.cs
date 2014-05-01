using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Route))] 
public class RouteEditor : Editor {

	private Route selected;

	void OnSceneGUI(){
		selected = ((GameObject)Selection.activeObject).GetComponent<Route> ();
		selected.nodes.RemoveRange(0,selected.nodes.Count);

		Node lastNode = null;
		for (int i = 0; i < selected.transform.childCount; i++) {
			Node n = selected.transform.GetChild(i).GetComponent<Node>();

			if(lastNode != null)
			{
				n.connection1 = lastNode;
				lastNode.connection2 = n;
			}

			n.walkablePoint = n.transform.GetChild(0).GetComponent<Node>();
			selected.nodes.Add(n);

			lastNode = n;
		}
		if (selected != null) {
			foreach (Node node in selected.nodes) {
				if(node.connection1 != null){
					Handles.color = Color.red;
					Handles.DrawLine(node.transform.position, node.connection1.transform.position);
				}
				if(node.connection2 != null){
					Handles.color = Color.red;
					Handles.DrawLine(node.transform.position, node.connection2.transform.position);
				}
				if(node.walkablePoint != null){
					Handles.color = Color.blue;
					Handles.DrawLine(node.transform.position, node.walkablePoint.transform.position);
				}
			}
		}
	}
}
