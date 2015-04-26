using UnityEngine;
using System.Collections;

public class DragonScript : MonoBehaviour {

    public GameObject target;
    public GameObject fireball;
    private Vector3 fireballLocalPos = new Vector3(-.34f,.134f,0f);

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
        fireBallInstance = Instantiate(fireball,this.transform.position + fireballLocalPos, this.transform.rotation) as GameObject;
        fireBallInstance.GetComponent<FireballController>().setTarget(target);
    }

    public void ShootTarget(GameObject target)
    {
        this.target = target;
        ShootTarget();
    }
}
