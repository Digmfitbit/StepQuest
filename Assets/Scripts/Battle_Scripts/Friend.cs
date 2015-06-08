using UnityEngine;
using System.Collections;

public class Friend : Fighter 
{
    public void initPlayer(PlayerStats player){
        health = 20 * player.playerEndurance;
        healthMax = health;
        damage = 4 * player.playerStrength;
        probabilityOfMissing = Random.Range(5f, 30f);

        transform.Find("name_text").localPosition = new Vector2(0f, transform.GetComponent<Renderer>().bounds.extents.y * 2);
        transform.Find("name_text").GetComponent<TextMesh>().text = player.playerName;
    }
}
