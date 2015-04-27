using UnityEngine;
using System.Collections;

public class DestroyScene : MonoBehaviour {

	private GameObject sceneHolder;

	public void destroyScene()
	{
		Destroy (GameObject.Find ("FightSceneHolder").gameObject);
	}
}
