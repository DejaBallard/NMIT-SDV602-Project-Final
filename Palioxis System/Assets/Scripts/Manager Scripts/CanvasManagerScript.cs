using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add all canvas's to the game instance
/// </summary>
public class CanvasManagerScript : MonoBehaviour {
	
	/// <summary>
    /// create an array of canvas's then add them to the instance
    /// </summary>
	void Awake () {
		Canvas[] _CanvasArray = gameObject.GetComponentsInChildren<Canvas>();
		foreach (Canvas iCanvas in _CanvasArray) {
            //Adding each canvas to the instance
			GameManagerScript._Instance._CanvasDic.Add(iCanvas.name, iCanvas);
			Debug.Log(name + ": " + iCanvas.name + " has been added to GameManager");
		}
	}	
}
