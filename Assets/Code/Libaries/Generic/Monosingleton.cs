using UnityEngine;
using System.Collections;

public class Monosingleton<T> :MonoBehaviour where T : MonoBehaviour {

	private static T _instance;

	private static GameObject _singletons;

	public static GameObject singletons {
		get {
			if(_singletons == null){
				_singletons = GameObject.Find("Singletons");
			}
			return _singletons;
		}
	}

	private static object _lock = new object();

	public static T Instance {
		get {

			
			lock(_lock)
			{
				if (_instance == null)
				{
					_instance = (T) FindObjectOfType(typeof(T));
					
					if ( FindObjectsOfType(typeof(T)).Length > 1 )
					{
						Debug.LogError("[Singleton] Something went really wrong " +
						               " - there should never be more than 1 singleton!" +
						               " Reopenning the scene might fix it.");
						return _instance;
					}

					GameObject _prefab = ((GameObject)Resources.Load(typeof(T).Name));
					if(_prefab !=null && _prefab is GameObject){
						if(_prefab.GetComponent<T>() != null){
							_instance = _prefab.GetComponent<T>();
							return _instance;
						}
					}
					
					if (_instance == null)
					{
						GameObject singleton = new GameObject();
						singleton.transform.parent = singletons.transform;

						_instance = singleton.AddComponent<T>();
						singleton.name = "(singleton) "+ typeof(T).ToString();
						
						DontDestroyOnLoad(singleton);
						
						Debug.Log("[Singleton] An instance of " + typeof(T) + 
						          " is needed in the scene, so '" + singleton +
						          "' was created with DontDestroyOnLoad.");
					} else {
						Debug.Log("[Singleton] Using instance already created: " +
						          _instance.gameObject.name);
					}
				}
				
				return _instance;
			}
		}
	}
}
