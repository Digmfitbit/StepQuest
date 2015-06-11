using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Player : Fighter 
{
    protected bool startAttack = true;
    protected bool comboFill = true;
    protected bool mouseReset = false;

    protected int maxCombo = 3;
    protected int comboCount = 0;

    public AttackBar attackBar;

	private void Start()
	{
        health = 20 * PlayerManager.mainPlayer.playerEndurance;
        healthMax = health;
        damage = 4 * PlayerManager.mainPlayer.playerStrength;

        maxCombo = PlayerManager.mainPlayer.playerStamina;

		probabilityOfMissing = Random.Range (5f, 30f);
	}

    protected override void Update()
    {
        ShowSelection();

        //Fight Code
        if (!startAttack)
        {
            if (!enemy.alive || comboCount >= maxCombo)
            {
                EndCombo();
            }

            // looks for mouse up before reading mouse downs again 
            // (prevents auto combo click after selecting enemy)
            if (!mouseReset && Input.GetMouseButtonUp(0))
                mouseReset = true;

            // if threshold has reached 1 (impossible), end the combo
            if (attackBar.GetThreshold() >= 1f)
            {
                EndCombo();
                Debug.Log("Max Combo, NICE!");
            }

            // if player clicks while combo-ing
            if (mouseReset && Input.GetMouseButtonDown(0))
            {
                // strike if above threshold
                if (attackBar.GetBarHeight() > attackBar.GetThreshold())
                {
                    attack();
                    attackBar.RaiseThreshold(maxCombo);
                    comboCount++;
                }
                // end combo if player fails
                else
                {
                    EndCombo();
                }
            }

            // move the attack bar
            if (comboFill)
            {
                // update bar height
                float newBarHeight = Mathf.Lerp(attackBar.GetBarHeight(), attackBar.GetBarHeight() + attackBar.barSpeed, Time.deltaTime);
                attackBar.SetBarHeight(newBarHeight);

                // check for bounds
                if (attackBar.GetBarHeight() >= 1f)
                    comboFill = false;
            }
            else
            {
                // update bar height
                float newBarHeight = Mathf.Lerp(attackBar.GetBarHeight(), attackBar.GetBarHeight() - attackBar.barSpeed, Time.deltaTime);
                attackBar.SetBarHeight(newBarHeight);

                // check for bounds
                if (attackBar.GetBarHeight() <= 0f)
                    comboFill = true;
            }
        } // end combo implementation

        if (!isAttacking)
        {
            //Bring Fighter back to home positon
            newPos = Vector3.Slerp(transform.position, homePos, Time.deltaTime * attackSpeed);
            transform.position = newPos;
        }
    }

    protected void EndCombo()
    {
        battleManager.SendMessage("SetComboOver", true);
        attackBar.Reset();
        startAttack = true;
        comboCount = 0;
    }

    public override void StartAttack(GameObject _enemy)
    {
		if (alive) 
		{
            if (_enemy.GetComponent<Fighter>())
                enemy = _enemy.GetComponent<Fighter>();
            else
                Debug.Log("enemy being set is not a Fighter");
		}

        if (startAttack)
        {
            attackBar.gameObject.SetActive(true);
            startAttack = false;
            comboFill = true;
            mouseReset = false;
        }
    }

    protected override IEnumerator FighterAttack()
    {
        if (attackSFX != null)
            attackSFX.Play();

		//Attacking Animation - Move the fighter to the opponent. If the fighter is close enough he hits him by calling the hit function on the opponent. 
		while (Vector3.Distance(transform.position,enemy.transform.position) > 0.2){
			newPos = Vector3.Slerp (transform.position, enemy.transform.position, Time.deltaTime * attackSpeed);
            transform.position = newPos;
            yield return null;
        }

        isAttacking = false;
					
		//how high is the propability of a failed attack, call hit function on selected opponent
		if (probabilityOfMissing < Random.Range (0, 100)) 
		{								
			enemy.SendMessage ("Hit", damage);
            GameObject.FindObjectOfType<ScreenShake>().startShakingCamera(.15f, .03f);
		} 
		else 
		{
			Debug.Log ("Shit I missed!!!");
		}
	}

    protected void SetAttackBar(GameObject ab)
    {
        attackBar = ab.GetComponent<AttackBar>();
    }

    protected override void Hit(float _damageIn)
    {
        base.Hit(_damageIn);
        GameObject.FindObjectOfType<ScreenShake>().startShakingCamera(.25f,.08f);
    }

}
