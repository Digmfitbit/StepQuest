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
	protected float damage;
	protected float damageMax;
	
	protected bool selected = false;
	protected bool alive = true;

	//Components
	protected GameObject battelManager;
	protected BattleManager battelManagerScript;
	protected SpriteRenderer spriteRenderer;
	public TextMesh textUnderFighter;
	private Animator animationController;

	protected virtual void Awake () 
	{
		//Get components
		battelManager = GameObject.Find ("BattleManager");
		battelManagerScript = battelManager.GetComponent<BattleManager>();

		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		spriteRenderer.material.color = new Color (1f,1f,1f,0.5f);		//start sprite with half the opacity for testing

		animationController = gameObject.GetComponent<Animator> ();

		//initialize variables
		damage = Random.Range (10, 20);
	}
	
	protected virtual void Update ()
	{
		if (alive) 
		{
			ShowSelection ();
			if (health <= 0)
				dead ();

		}
	}

	public virtual void attack(GameObject _enemy)
	{
		//call hit function on selected opponent
		_enemy.SendMessage ("hit",damage);
	}

	protected virtual void hit(float _damageIn)
	{
		health -= _damageIn;
		//display health under fighter
		transform.FindChild ("HealthText").gameObject.SendMessage("setText", health.ToString());
	}

	protected virtual void dead()
	{
		alive = false;
		SetSelected (false);

		battelManagerScript.someoneDied (gameObject);
		//trigger dead animation
		animationController.SetBool ("isDead", true);
		transform.FindChild ("HealthText").gameObject.SendMessage("setText", "I'm dead!");
	}

	protected virtual void winFight()
	{

	}

	protected virtual void OnMouseDown()
	{
		if(alive)
		{
			//Tell the GameManager that sombody click on you, snitch!
			battelManagerScript.setSelection (gameObject);
		}
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
