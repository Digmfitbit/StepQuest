using UnityEngine;
using System.Collections;

public class boardNode : MonoBehaviour {

    public enum Type{Empty, Challenge, Cosmetic, Finish};
    
    public int id;
    public GameObject adjFwd;
    public GameObject adjBck;
    
    public Type type;
    
    void Start () {
	   switch(type){
        case Type.Empty:
           this.name = "node_empty";
           // Sprite Swap
           break;
        case Type.Challenge:
           this.name = "node_challenge";
           //Sprite Swap
           break;
        case Type.Cosmetic:
           this.name = "node_cosmetic";
           //Sprite Swap
           break;
        case Type.Finish:
           this.name = "node_finish";
           //Sprite Swap
           break;
        default:
           this.name = "node_no_type";
           //Sprite Swap
           Debug.Log("Error");
           break;
       }
	}

    void Update () {
	
	}
}
