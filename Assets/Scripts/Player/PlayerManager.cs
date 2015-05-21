using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    public playerStats mainPlayer;


    void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);
        //Do load??
    }
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
