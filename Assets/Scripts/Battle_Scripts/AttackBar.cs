using UnityEngine;
using System.Collections;

public class AttackBar : MonoBehaviour 
{
    public float barSpeed = 100f;
    public float startingThreshold = 0.8f;
    //public float incrementAmount = 0.05f;
    
    // current stats
    private float _barHeight = 0.05f;
    private float _threshold = 0.5f;

    // the bar that moves
    public Transform innerBar;

    // displays the current threshold
    public Transform thresholdObj;  

    // used to calculate threshold position for thresholdObj
    public Transform thresholdBar;
    public Transform thresholdTip;

	// Use this for initialization
	//void Start () 
	//{
	
	//}
	
	// Update is called once per frame
	void Update () 
	{
        // clamp _threshold and scale values
        _threshold = Mathf.Clamp(_threshold, 0f, 1f);
        _barHeight = Mathf.Clamp(_barHeight, 0f, 1f);

        // set inner bar scale
        innerBar.localScale = new Vector3(1f, _barHeight, 1f);

        // set _threshold scale and display object's position
        thresholdBar.localScale = new Vector3(1f, _threshold, 1f);
        thresholdObj.position = thresholdTip.position;
	}

    public float GetThreshold()
    {
        return _threshold;
    }

    public void RaiseThreshold(int maxCombo)
    {
        // increments threshold by...
        //_threshold += ((1f - _threshold) * 0.5f); // half the remaining distance
        _threshold += (1f - startingThreshold) / maxCombo; // equal amounts based on maxCombo allowed
    }

    public float GetBarHeight()
    {
        return _barHeight;
    }

    public void SetBarHeight(float newHeight)
    {
        _barHeight = newHeight;
    }

    public void Reset()
    {
        // reset stats
        _threshold = startingThreshold;
        _barHeight = 0.05f;

        // set inner bar scale
        innerBar.localScale = new Vector3(1f, _barHeight, 1f);

        // set threshold scale and display object's position
        thresholdBar.localScale = new Vector3(1f, _threshold, 1f);
        thresholdObj.position = thresholdTip.position;

        gameObject.SetActive(false);
    }
}
