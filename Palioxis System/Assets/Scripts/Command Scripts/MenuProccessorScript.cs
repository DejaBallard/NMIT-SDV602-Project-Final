using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using System.Text;
using System.Text.RegularExpressions;
//Script Purpose:
//Proccesses data within the Main Menu
//--------------------------------------- Start Of  MenuProccessorScript: Mono Class ------------------------------------------------------------------------------
public class MenuProccessorScript : MonoBehaviour
{
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
    public DataService _db = new DataService();
    public AuthManagerScript _AuthManager;
    public InputField _UserNameInput;
    public InputField _PassInput;
    public InputField _RePassInput;
    public Text _Output;
    //--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------

    private void Awake()
    {
        _AuthManager._AuthCallback += HandleAuthCallBack;
    }

    IEnumerator HandleAuthCallBack(Task<Firebase.Auth.FirebaseUser>prtask,string prOperation)
    {
        if (prtask.IsFaulted || prtask.IsCanceled)
        {
            Debug.Log("authentication Error: " + prtask.Exception.ToString());
            _Output.text = ("Error");
        }
        else if (prtask.IsCompleted)
        {
            if (prOperation == "sign_up")
            {
                Firebase.Auth.FirebaseUser lcAuthUser = prtask.Result;
                Debug.Log("signing up");
                PlayerScript lcSQLplayer = new PlayerScript
                {
                    Id = lcAuthUser.UserId,
                    CurrentLocationID = "E3",
                    PlayerEmail = lcAuthUser.Email,
                    PlayerHealth = 3,
                    PlayerScore = 0,
                    PlayerMoney = 0,
                    InventoryList = "",
                    CollectList = "",
                    Visted = "",
                    Scanned = "",
                };
                PlayerManagerScript._CurrentPlayer = lcSQLplayer;
                _db.Connection.InsertOrReplace(PlayerManagerScript._CurrentPlayer);
                PlayerManagerScript._LoggedIn = true;
            }
            else
            {
                Firebase.Auth.FirebaseUser lcAuthUser = prtask.Result;
                try
                {
                    
                    PlayerManagerScript._CurrentPlayer = _db.Connection.Table<PlayerScript>().Where<PlayerScript>(x => x.Id == lcAuthUser.UserId).ToList<PlayerScript>().First<PlayerScript>();
                    PlayerManagerScript._LoggedIn = true;
                }
                catch
                {
                    PlayerScript lcSQLplayer = new PlayerScript
                    {
                        Id = lcAuthUser.UserId,
                        CurrentLocationID = "E3",
                        PlayerEmail = lcAuthUser.Email,
                        PlayerHealth = 3,
                        PlayerScore = 0,
                        PlayerMoney = 0,
                        InventoryList = "",
                        CollectList = "",
                        Visted = "",
                        Scanned = "",
                    };
                    PlayerManagerScript._CurrentPlayer = lcSQLplayer;
                    _db.Connection.InsertOrReplace(PlayerManagerScript._CurrentPlayer);
                }
            }
            _Output.text = "Loading...";
            yield return new WaitForSeconds(3f);
            GameManagerScript._Instance.SetActiveCanvas("Lobby Canvas");
        }
    }

    private void OnDestroy()
    {
        _AuthManager._AuthCallback -= HandleAuthCallBack;
    }
    public void SignUp()
    {
        //Gathers Information and sets locally
        string lcEmail = _UserNameInput.text;
        string lcPassword = _PassInput.text;
        string lcRePassword = _RePassInput.text;

        //if Password and re-enter match
        if (lcPassword == lcRePassword)
        {
            if (ValidatePassword())
            {
                _AuthManager.SignupNewUser(lcEmail, lcPassword);
            }
            else { _Output.text = "Password needs to be 8 or more"; }
        }
        else
        {
            _Output.text = "Password's Don't Match";
        }
    }

    public void LogIn()
    {
        //Default result
        PlayerManagerScript._LoggedIn = false;
        //Gather Information and sets locally
        string lcUsername = _UserNameInput.text;
        string lcPassword = _PassInput.text;
    
        _AuthManager.LoginExistingUser(lcUsername, lcPassword);
    }


    public bool ValidatePassword()
    {
        string lcPassword = _PassInput.text;
        return lcPassword.Count() > 8;

    }
}
