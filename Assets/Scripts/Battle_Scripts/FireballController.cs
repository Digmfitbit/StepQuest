using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour {

    GameObject target;

    float speed = 1.0F;
    private float startTime;
    private float journeyLength;
    //Change this to empty string in order to not load a level
    string gameScreen = "MapTest";

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position, fracJourney);
	}

    public void setTarget(GameObject target)
    {
        this.target = target;
        journeyLength = Vector3.Distance(this.transform.position, target.transform.position);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //TODO: delete the below line in order to 
        Application.LoadLevel(gameScreen);
        if(other.gameObject == target){
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
