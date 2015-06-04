using UnityEngine;
using System.Collections;

public class ScaleMe : MonoBehaviour 
{

	public void ScaleGameObject(float _scale)
	{
		transform.localScale = new Vector2 (_scale, _scale);
	}
	
}
