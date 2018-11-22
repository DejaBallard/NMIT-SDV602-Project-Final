using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SQLite4Unity3d;
/// <summary>
/// Manages the display score
/// </summary>
public class ScoreManagerScript : MonoBehaviour {
    public Text _Output;
    /// <summary>
    /// updates the display every frame with the users scores
    /// </summary>
    void Update () {
        _Output.text = "S:" + PlayerManagerScript._CurrentPlayer.PlayerScore.ToString() + "\n" +
                        "$:" + PlayerManagerScript._CurrentPlayer.PlayerMoney.ToString();
	}
}
