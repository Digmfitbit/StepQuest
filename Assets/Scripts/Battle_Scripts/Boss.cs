using UnityEngine;
using System.Collections;

public class Boss : Fighter
{

    private void Start()
    {
        health = 500f;
        healthMax = health;
        damage = 30f;
        probabilityOfMissing = Random.Range(25f, 35f);
    }
}
