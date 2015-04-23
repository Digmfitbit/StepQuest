using UnityEngine;
using System.Collections;

public class setHealth : MonoBehaviour {

	private TextMesh textToDisplay;
	// Use this for initialization
	void Start () 
	{
		textToDisplay = gameObject.GetComponent<TextMesh> ();
	}
	
	public void setText(string _textToDisplay)
	{
		textToDisplay.text = _textToDisplay;
	}
}
