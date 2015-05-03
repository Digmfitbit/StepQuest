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

	protected float probabilityOfMissing = 20f;
	
	protected bool selected = false;
	public bool alive = true;
	protected bool isAttacking = false;

	public GameObject HealthBar;
	private GameObject healthBar;
	public GameObject StaminaBar;
	private GameObject staminaBar;

	//Components
	protected GameObject battelManager;
	protected BattleManager battelManagerScript;
	protected SpriteRenderer spriteRenderer;
	protected TextMesh textUnderFighter;
	protected Animator animationController;
	protected LineRenderer lineRenderer;
	protected GameObject enemy;

	protected Vector3 homePos;
	protected Vector3 newPos;
	protected float attackSpeed = 20f;

	protected virtual void Awake () 
	{
		//Get components
		battelManager = GameObject.Find ("BattleManager");
		battelManagerScript = battelManager.GetComponent<BattleManager>();

		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		spriteRenderer.material.color = new Color (1f,1f,1f,0.5f);		//start sprite with half the opacity for testing

		animationController = gameObject.GetComponent<Animator> ();

		//initialize variables
		damage = 20;
		homePos = transform.position;

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
		if (alive)
			ShowSelection ();

		//Fight Code
		if (isAttacking) 
		{
			attack ();
		}
		else
		{
			//Bring Fighter back to home positon
			newPos = Vector3.Slerp (transform.position, homePos, Time.deltaTime * attackSpeed);
			transform.position = newPos;
		}
	}

	public virtual void StartAttack (GameObject _enemy)
	{
		if (alive) 
		{
			enemy = _enemy;
			isAttacking = true;
		}
	}


	public virtual void attack()
	{
		//Attacking Animation - Move the fighter to the opponent. If the fighter is close enough he hits him by calling the hit function on the opponent. 
		if (isAttacking) 
		{
			newPos = Vector3.Slerp (transform.position, enemy.transform.position, Time.deltaTime * attackSpeed);
			transform.position = newPos;
			
			if(Vector3.Distance(transform.position,enemy.transform.position) < 0.2)
			{
				isAttacking = false;
				
				//how high is the propability of a failed attack, call hit function on selected opponent
				if (probabilityOfMissing < Random.Range (0, 100)) 
				{								
					enemy.SendMessage ("Hit", damage);
				} 
				else 
				{
					Debug.Log ("Shit I missed!!!");
				}
			}
		}
	}


	protected virtual void Hit(float _damageIn)
	{
		health -= _damageIn;

		//if health is to low call dead function
		if (health <= 0) 
		{
			Dead ();
		}
		//display health under fighter
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
