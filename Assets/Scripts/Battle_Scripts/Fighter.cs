using UnityEngine;
using System.Collections;

public class Fighter : MonoBehaviour {

	protected float strength;
	protected float strengthMax;
	protected float stamina;
	protected float staminaMax;
	public float health;
	protected float healthMax;
	protected float recovery;
	protected float recoveryMax;
	protected float damage = 10f;
	protected float damageMax;

	protected string fighterName;

	protected bool selected;

	protected GameObject battelManager;
	protected BattleManager battelManagerScript;
	protected SpriteRenderer spriteRenderer;
	

	protected virtual void Awake () 
	{
		battelManager = GameObject.Find ("BattleManager");
		battelManagerScript = battelManager.GetComponent<BattleManager>();

		selected = false;

		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		spriteRenderer.material.color = new Color (1f,1f,1f,0.5f);		//start sprite with half the opacity for testing

		health = 100f;
	}
	
	protected virtual void Update ()
	{
		ShowSelection ();
		if (health <= 0)
			die ();
	}

	public virtual void attack(GameObject _enemy)
	{
		_enemy.SendMessage ("hit",damage);
	}

	protected virtual void hit(float _damageIn)
	{
		health -= _damageIn;
	}

	protected virtual void die()
	{
		SetSelected (false);
		Destroy (gameObject);
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
