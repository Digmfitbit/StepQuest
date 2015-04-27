using UnityEngine;
using System.Collections;

public class DragonScript : MonoBehaviour {

    public GameObject target;
    public GameObject fireball;

    bool fired = false;
    GameObject fireBallInstance;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void ShootTarget()
    {
        fired = true;
        fireBallInstance = Instantiate(fireball) as GameObject;
        fireBallInstance.GetComponent<FireballController>().setTarget(target);
    }

    public void ShootTarget(GameObject target)
    {
        this.target = target;
        ShootTarget();
    }
}
