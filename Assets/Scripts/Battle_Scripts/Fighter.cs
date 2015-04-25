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
	public bool alive = true;

	public GameObject HealthBar;
	private GameObject healthBar;
	public GameObject StaminaBar;
	private GameObject staminaBar;

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
//		damage = Random.Range (10, 20);
		damage = 20;

		//make health and stamina bar
		healthBar = Instantiate(HealthBar ,transform.position + new Vector3(0f,-0.7f,0f) , Quaternion.identity) as GameObject;
		healthBar.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
		healthBar.transform.SetParent (transform);

		staminaBar = Instantiate(StaminaBar ,transform.position + new Vector3(0f,-0.8f,0f) , Quaternion.identity) as GameObject;
		staminaBar.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
		staminaBar.transform.SetParent (transform);

	}
	
	protected virtual void Update ()
	{

		if (alive) //do this as long as fighter is alive
		{
			ShowSelection ();



		}
	}

	public virtual void attack(GameObject _enemy)
	{
		//call hit function on selected opponent
		_enemy.SendMessage ("Hit",damage);
	}

	protected virtual void Hit(float _damageIn)
	{
		health -= _damageIn;

		//if health is to low call dead function
		if (health <= 0)
			Dead ();

		//display health under fighter
//		transform.FindChild ("HealthText").gameObject.SendMessage("setText", health.ToString());
		healthBar.SendMessage ("UpdateStatusBar", health);
	}

	protected virtual void Dead()
	{
		alive = false;

		battelManagerScript.someoneDied (gameObject);

		//trigger dead animation
		animationController.SetBool ("isDead", true);
	}

	public virtual void WinFight()
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

	public virtual void SetSelected(bool _selected)
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
