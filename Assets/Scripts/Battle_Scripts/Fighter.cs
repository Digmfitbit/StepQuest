using UnityEngine;
using System.Collections;

public class Fighter : MonoBehaviour {

	protected float strength, strengthMax;
	protected float stamina, staminaMax;
	protected float health, healthMax;
	protected float recovery,recoveryMax;
	protected float damage, damageMax;

	protected string fighterName;

	protected bool selected = false;
	public bool Selected
	{
		get{
			return selected;
		}
		set{
			selected = value;
		}
	}


	
	protected virtual void Start () 
	{
	
	}
	
	protected virtual void Update ()
	{
//		checkSelection ();

	
	}

	protected virtual void attack(float damageOut)
	{

	}

	protected virtual void hit(float damageIn)
	{

	}

	protected virtual void winFight()
	{
	}

	protected virtual void loseFight()
	{

	}
	

	protected virtual void OnMouseDown()
	{
		Debug.Log ("I'm inside of OnMouseDown and my Name is: "+ gameObject.name);
	}

}
