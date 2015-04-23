using UnityEngine;
using System.Collections;

public class Fighter : MonoBehaviour {

	protected float strength, strengthMax;
	protected float stamina, staminaMax;
	protected float health, healthMax;
	protected float recovery,recoveryMax;
	protected float damage, damageMax;

	protected string fighterName;

	protected bool selected;

	protected GameObject battelManager;
	protected BattleManager battelManagerScript;
	protected SpriteRenderer spriteRenderer;

	
	protected virtual void Start () 
	{
		battelManager = GameObject.Find ("BattleManager");
		battelManagerScript = battelManager.GetComponent<BattleManager>();

		selected = false;

		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		spriteRenderer.material.color = new Color (1f,1f,1f,0.5f);		//start sprite with half the opacity for testing
	}
	
	protected virtual void Update ()
	{
		ShowSelection ();
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
		//Tell the GameManager that sombody click on you, snitch!
		battelManagerScript.setSelection (gameObject);
	}

	protected virtual void SetSelected(bool _selected)
	{
		selected = _selected;
	}

	protected virtual void ShowSelection()
	{
		if (selected) {	
			spriteRenderer.material.color = new Color (1f, 1f, 1f, 1f);
		}else {
			spriteRenderer.material.color = new Color (1f,1f,1f,0.5f);
		}
	}
}
