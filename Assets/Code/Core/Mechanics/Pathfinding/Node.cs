using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	[SerializeField] 
	private Node _walkablePoint;

	[SerializeField]
	private Node _connection1;

	[SerializeField]
	private Node _connection2;

	public Node connection2 {
		get {
			return _connection2;
		}
		set{
			_connection2 = value;
		}
	}

	public Node connection1 {
		get {
			return _connection1;
		}set{
			_connection1 = value;
		}
	}

	public Node walkablePoint {
		get {
			return _walkablePoint;
		}set{
			_walkablePoint = value;
		}
	}
	public int id {
		get {
			return id;
		}set{
			id = value;
		}
	}
	public Vector2 nodelocation {
		get {
			return (new Vector2(transform.position.x,transform.position.z));
		}
	}
}
