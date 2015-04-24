using UnityEngine;
using System.Collections;
using Assets.Scripts.fitbit;

public class StartButtonController : MonoBehaviour {
    string gameScreen = "MapTest";

    public void OnMouseDown()
    {
        Debug.Log("click start");
        if (FitBit.getInstance().isAuthenticated())
        {
            Application.LoadLevel(gameScreen);
        }
        Debug.Log("asdasd: " + FitBit.getInstance().isAuthenticated());
    }


}
