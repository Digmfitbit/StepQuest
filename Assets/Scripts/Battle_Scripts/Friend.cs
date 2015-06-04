using UnityEngine;
using System.Collections;

public class Friend : Fighter 
{

    public void initPlayer(PlayerStats player){
        health = 20 * player.playerEndurance;
        healthMax = health;
        damage = 4 * player.playerStrength;
        probabilityOfMissing = Random.Range(5f, 30f);
    }
}
