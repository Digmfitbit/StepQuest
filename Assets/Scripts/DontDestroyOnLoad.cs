using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour {

	public static DontDestroyOnLoad Instance;

	void Awake () {
		if(Instance){
			DestroyImmediate(gameObject);
		}
		else{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
	}
}
