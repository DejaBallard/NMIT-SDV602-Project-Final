using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;
/// <summary>
/// Used to verify and authenticate users with the firebase
/// </summary>
public class AuthManagerScript : MonoBehaviour {

    /// <summary>
    /// Used to catch the authentication result
    /// </summary>
    /// <param name="task"> result of the firebase authentication</param>
    /// <param name="Operation"> name of the task type. example: signup / login</param>
    /// <returns></returns>
    public delegate IEnumerator AuthCallback(Task<Firebase.Auth.FirebaseUser> task, string Operation);

    /// <summary>
    /// public variable of AuthCallback above.
    /// </summary>
    public event AuthCallback _AuthCallback;

    /// <summary>
    /// Local variable of the firebase authentication service
    /// </summary>
    private Firebase.Auth.FirebaseAuth _auth;

    /// <summary>
    /// Attach local variable to the defult instance
    /// </summary>
    private void Awake()
    {
        _auth = FirebaseAuth.DefaultInstance;
    }

    /// <summary>
    /// Sign new user up to the fireback authentication
    /// </summary>
    /// <param name="prEmail">Users email</param>
    /// <param name="prPassword">Users password</param>
    public void SignupNewUser(string prEmail, string prPassword)
    {
        
        _auth.CreateUserWithEmailAndPasswordAsync(prEmail, prEmail).ContinueWith(task =>
         {
             StartCoroutine(_AuthCallback(task, "sign_up"));
         });
    }

    /// <summary>
    /// Log in existing user
    /// </summary>
    /// <param name="prEmail">users email</param>
    /// <param name="prPassword">users password</param>
    public void LoginExistingUser(string prEmail, string prPassword)
    {
        _auth.SignInWithEmailAndPasswordAsync(prEmail, prPassword).ContinueWith(task =>
        {
            StartCoroutine(_AuthCallback(task, "login"));
        });
    }
}
