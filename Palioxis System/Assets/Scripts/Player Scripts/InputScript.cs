using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// View Model that collects user input and pass that data to the models
/// </summary>
public class InputScript : MonoBehaviour {
    public Text _TxtOutput;
    public Canvas _Canvas;
	private InputField _inputField;
	private InputField.SubmitEvent _submitEvent;
	private InputField.OnChangeEvent _onChangeEvent;
    private string[] _mobileInput = { "Go Left", "Go Up", "Go Right", "Go Down", "Scan Area", "Pick Up" , "Show Map", "Show Terminal", "Show Inventory", "Show Help", "Sell Items" };
    private int _mobileCurrentArrayText =0;
    private string _mobileCurrentText;




/// <summary>
/// Sets up the input field and updates the view with the users current in game scene
/// </summary>
	void Start () {
	// Adding the gameobject to the variable
		_inputField = this.GetComponent<InputField>();
		
		if (_inputField != null){
			_submitEvent = new InputField.SubmitEvent();
			_submitEvent.AddListener(submitInput);
			_inputField.onEndEdit = _submitEvent;

            //If this input field is attached to main canvas, show story
            if (_Canvas.name == "Main Canvas")
            {
                _TxtOutput.text = PlayerManagerScript.GetCurrentScene().Name + "\n" + PlayerManagerScript.GetCurrentScene().SceneStoryDescription;
            }
		}
        //checks the user for mobile input every 1.5 seconds
        //ERROR: when the user submits a command, it moves two spaces instead of one. I am unsure where the error is.
        InvokeRepeating("MobileInput", 1.5f, 1.5f);
	}

    /// <summary>
    /// Update the inventory canvas every frame
    /// </summary>
    private void Update()
    {
        //If on inventory canvas, display user inventory
        string lcList = "";
        if (_Canvas.name == "Inventory Canvas")
        {
            string[] Inventory = PlayerManagerScript._CurrentPlayer.InventoryList.Split(',');
            foreach (string i in Inventory)
            {
                if (i != "")
                {
                    GameItemScript lcItem = PlayerManagerScript.GetItem(i);
                    lcList = lcList + lcItem.Name + " - S" + lcItem.Score + " - $" + lcItem.Price + "\n";
                }
            }
            _TxtOutput.text = lcList;
            ActivateInputField();
        }
    }

    /// <summary>
    /// Checks the mobile accelration for inputs
    /// </summary>
    private void MobileInput()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.acceleration.x > 0.2f)
        {
            _mobileCurrentArrayText++;
            if(_mobileCurrentArrayText == 11)
            {
                _mobileCurrentArrayText = 0;
            }
            _mobileCurrentText = _mobileInput[_mobileCurrentArrayText];
            _inputField.text = _mobileCurrentText;
        }
        if (Input.acceleration.x < -0.2f )
        {
            _mobileCurrentArrayText--;
            if (_mobileCurrentArrayText == -1)
            {
                _mobileCurrentArrayText = 10;
            }
            _mobileCurrentText = _mobileInput[_mobileCurrentArrayText];
            _inputField.text = _mobileCurrentText;
        }
        if (Input.acceleration.y > -0.2)
        {
            submitInput(_mobileCurrentText);
        }
#endif
    }

    /// <summary>
    /// Activate the input field for another input
    /// only on PC build
    /// </summary>
    private void ActivateInputField()
    {
#if UNITY_EDITOR
        _inputField.ActivateInputField();
#endif
    }

    /// <summary>
    /// Submit the users input to be proccessed
    /// </summary>
    /// <param name="prArg">The users input</param>
    private void submitInput(string prArg){
        string lcCurrentText;
        CommandProccessorScript lcCommandPro = new CommandProccessorScript();

        if (!Input.GetKeyDown(KeyCode.Escape) && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
        {
            lcCurrentText = _TxtOutput.text;
            lcCommandPro = new CommandProccessorScript();
            //text being checked with command proccessor
            _TxtOutput.text = lcCommandPro.Parse(prArg);
            //Reset input field to blank
            _inputField.text = "";

#if UNITY_ANDROID || UNITY_IOS
            _inputField.text = _mobileCurrentText;
#endif
        }
        //Allow input to be used again
        ActivateInputField();
    }
}
