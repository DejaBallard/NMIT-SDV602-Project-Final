using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
/// <summary>
/// Singleton script. Manage the current player.
/// login and SQL database connection
/// </summary>
public class PlayerManagerScript : MonoBehaviour
{
    public static bool _Created = false;
    public static bool _LoggedIn = false;
    public static PlayerScript _CurrentPlayer { get; set; }
    public static DataService _db = new DataService();


/// <summary>
/// Makes sure this is a singleton
/// </summary>
    void Awake()
    {
        //if not created, create this
        if (!_Created)
        {
            DontDestroyOnLoad(this.gameObject);
            _Created = true;
        }
    }

    /// <summary>
    /// Get the current players game scene from the SQL database
    /// </summary>
    /// <returns>the result of the SQL search</returns>
    public static SceneScript GetCurrentScene()
    {
        SceneScript lcresult;
        lcresult = _db.Connection.Table<SceneScript>().Where(x => x.Name == _CurrentPlayer.CurrentLocationID).First<SceneScript>();
        return lcresult;
    }

    /// <summary>
    /// Get the game item from the SQL database
    /// </summary>
    /// <param name="prName">Name of the Item</param>
    /// <returns>the Item</returns>
    public static GameItemScript GetItem(string prName)
    {
        GameItemScript lcresult;
        lcresult = _db.Connection.Table<GameItemScript>().Where(x => x.Name == prName).First<GameItemScript>();
        return lcresult;
    }
}
