using UnityEngine;
using System.Collections;
using Assets.Scripts.map;

public class DestroyScene : MonoBehaviour {

	private GameObject sceneHolder;

	public void destroyScene()
	{
		Destroy (GameObject.Find ("FightSceneHolder").gameObject);
        Switcheroo.reEnable();
	}
}
