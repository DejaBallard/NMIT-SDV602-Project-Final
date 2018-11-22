using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.Linq;
/// <summary>
/// Base class for all different commands
/// </summary>
public class CommandScript : MonoBehaviour
{
    /// <summary>
    /// Connection to the SQL database
    /// </summary>
    protected DataService _db = new DataService();
    
    /// <summary>
    /// Do something based on the subclass
    /// </summary>
    /// <param name="prCommand"></param>
    public virtual void Do(CommandMapScript prCommand)
    {
    }
    
}





/// <summary>
/// Sub class for moving the player command
/// </summary>
public class GoCommand : CommandScript
{
   
    private string _direction;

    //General text outputs
    private string _NotScanned = "AI: We have not scanned this area yet";
    private string _Scanned = "AI: We have scanned this area";
  
    /// <summary>
    /// assigning data to local variables
    /// </summary>
    /// <param name="prDirection">direction given from the command map dictonary</param>
    public GoCommand(string prDirection)
    {
        //Assin the string to a local variable
        _direction = prDirection;
    }

    /// <summary>
    /// Moves the player based on the direction
    /// </summary>
    /// <param name="prCommand"> </param>
    public override void Do(CommandMapScript prCommand)
    {
        if (GameManagerScript._Instance._ActiveCanvas.name == "Main Canvas")
        {
            //Turn off display events
            TurnOffEvent();
            //Assign players current local to a local variable 
            SceneScript lcCurrentScene = PlayerManagerScript.GetCurrentScene();
            //Create a new scene variable
            SceneScript lcNewScene = new SceneScript();

            //Try get new scene. If not a valid direction, ignore
            try
            {
                //Get scene direction by selecting where From Scene name is current Scenes and direction is users input 
                SceneDirectionScript lcSceneDirection = _db.Connection.Table<SceneDirectionScript>().Where<SceneDirectionScript>(x => x.FromSceneName == lcCurrentScene.Name && x.Direction == _direction).ToList<SceneDirectionScript>().First<SceneDirectionScript>();

                //Get new scene by selecting where name is scene direction's to scene variable
                lcNewScene = _db.Connection.Table<SceneScript>().Where<SceneScript>(x => x.Name == lcSceneDirection.ToSceneName).ToList<SceneScript>().First<SceneScript>();
                PlayerManagerScript._CurrentPlayer.CurrentLocationID = lcNewScene.Name;
            }
            catch
            {
                lcNewScene = _db.Connection.Table<SceneScript>().Where<SceneScript>(x => x.Name == lcCurrentScene.Name).ToList<SceneScript>().First<SceneScript>();
                PlayerManagerScript._CurrentPlayer.CurrentLocationID = lcNewScene.Name;
            }

            //check if new scene has a display event
            HasEvent(lcNewScene.Event);

            //if scene has been visited and scanned, Set this as text output
            if (Visited(lcNewScene) && Scanned(lcNewScene))
            {
                prCommand._Result = lcNewScene.Name + "\n" +
                         lcNewScene.SceneStoryDescription + "\n" +
                         _Scanned;

            }
            //Else if scene has only been visited, set this as text output
            else if (Visited(lcNewScene))
            {
                prCommand._Result = lcNewScene.Name + "\n" +
                         lcNewScene.SceneStoryDescription + "\n" +
                         _NotScanned;
            }
            else
            {
                //Update player saying they have visited this area before
                PlayerManagerScript._CurrentPlayer.Visted = PlayerManagerScript._CurrentPlayer.Visted + "," + lcNewScene.Name;
                //Update player with more score
                PlayerManagerScript._CurrentPlayer.PlayerScore += 10;
                //Set this as text output
                prCommand._Result = lcNewScene.Name + "\n" + lcNewScene.SceneStoryDescription;
            }
           _db.Connection.InsertOrReplace(PlayerManagerScript._CurrentPlayer);

        }
        else { prCommand._Result = "switch to the terminal to move"; }
    }

    /// <summary>
    /// Check to see if user has visited this scene before
    /// </summary>
    /// <param name="prScene">scene being checked</param>
    /// <returns>true if the scene has been visted by the player before, else false</returns>
    private static bool Visited(SceneScript prScene)
    {
        //Create an array from visited list
        string[] lcArrayVisted = PlayerManagerScript._CurrentPlayer.Visted.Split(',');
        foreach (string SceneName in lcArrayVisted)
        {
            if (SceneName == prScene.Name)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Check to see if user has scanned this scene before
    /// </summary>
    /// <param name="prScene">scene being checked</param>
    /// <returns>true if the scene has been scanned by the player before, else false</returns>
    private bool Scanned(SceneScript prScene)
    {
        //Create Array from scanned list
        string[] lcArrayScanned = PlayerManagerScript._CurrentPlayer.Scanned.Split(',');
        foreach (string SceneName in lcArrayScanned)
        {
            if (SceneName == prScene.Name)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// checks to see if scene has a disaply event to play
    /// </summary>
    /// <param name="prEvent">name of event that is with the scene</param>
    public void HasEvent(string prEvent)
    {
        switch (prEvent)
        {
            case "Planet":
                GameManagerScript._Instance._StarAnim.SetTrigger("Warp");
                GameManagerScript._Instance._PlanetAnim.SetBool("ShipArrived", true);
                GameManagerScript._Instance._StarAnim.SetBool("Stop", true);
                break;
            case "Shop":
                GameManagerScript._Instance._StarAnim.SetTrigger("Warp");
                GameManagerScript._Instance._ShopAnim.SetBool("ShopArrived", true);
                GameManagerScript._Instance._StarAnim.SetBool("Stop", true);
                break;
            case "Enemy":
                GameManagerScript._Instance._StarAnim.SetTrigger("Warp");
                GameManagerScript._Instance._EnemyAnim.SetBool("EnemyArrived", true);
                GameManagerScript._Instance._StarAnim.SetBool("Stop", true);
                break;
            case "BrokenShip":
                GameManagerScript._Instance._StarAnim.SetTrigger("Warp");
                GameManagerScript._Instance._StarAnim.SetBool("Stop", true);
                break;
            default:
                GameManagerScript._Instance._StarAnim.SetTrigger("Warp");
                break;
        }
    }
    /// <summary>
    /// turn off all display events
    /// </summary>
    public void TurnOffEvent()
    {
        GameManagerScript._Instance._StarAnim.SetBool("Stop", false);
        GameManagerScript._Instance._ShopAnim.SetBool("ShopArrived", false);
        GameManagerScript._Instance._PlanetAnim.SetBool("ShipArrived", false);
        GameManagerScript._Instance._EnemyAnim.SetBool("EnemyArrived", false);
    }
    
}







public class PickCommand : CommandScript
{
    
    //User Input
    private string _item;
    


   
    public PickCommand(string prItem)
    {
        //Assign users input to local variable
        _item = prItem;
    }

    public override void Do(CommandMapScript prCommand)
    {
        if (GameManagerScript._Instance._ActiveCanvas.name == "Main Canvas")
        {
            switch (_item)
            {
                case "up":
                    SceneScript lcCurrentScene = PlayerManagerScript.GetCurrentScene();
                    //if items havent been picked up
                    if (!ItemsPickedUp(lcCurrentScene.Name))
                    {
                        string lcInventoryList = null;
                        List<SceneItemScript> lcSceneItems = new List<SceneItemScript>();
                        //Gets all Scene Item that have the current Scene Name
                        lcSceneItems = _db.Connection.Table<SceneItemScript>().Where<SceneItemScript>(x => x.SceneName == lcCurrentScene.Name).ToList<SceneItemScript>();
                        foreach (SceneItemScript SceneItem in lcSceneItems)
                        {
                            //Get the Item with the same ID
                            GameItemScript Item = _db.Connection.Table<GameItemScript>().Where<GameItemScript>(x => x.ItemId == SceneItem.ItemId).ToList<GameItemScript>().First<GameItemScript>();
                            //Add Item name to local variable
                            lcInventoryList = lcInventoryList + Item.Name + ",";
                        }
                        //Add current scene name to collected list
                        PlayerManagerScript._CurrentPlayer.CollectList = PlayerManagerScript._CurrentPlayer.CollectList + lcCurrentScene + ",";
                        //Update players inventory with local variable
                        PlayerManagerScript._CurrentPlayer.InventoryList = PlayerManagerScript._CurrentPlayer.InventoryList + lcInventoryList;
                        //Save to database
                        _db.Connection.InsertOrReplace(PlayerManagerScript._CurrentPlayer);
                        //Text output
                        prCommand._Result = "AI: Items picked up";
                    }
                    else
                    //Text Output
                    { prCommand._Result = "AI: No Items to pick up"; }
                    break;
            }
        }
        else { prCommand._Result = "switch to the terminal to pick Items up"; }
        //update Firebase Database

    }

    private bool ItemsPickedUp(string prScene)
    {
        bool lcResult = false;

        //needs Try/Catch because if user has null in collect list, it can not split
        try
        {
            //Put collected list into an array
            string[] lcCollectedArray = PlayerManagerScript._CurrentPlayer.CollectList.Split(',');
            foreach (string SceneName in lcCollectedArray)
            {
                if (SceneName == prScene)
                {
                    lcResult = true;
                }
            }
        }
        catch { }
        return lcResult;

    }
   
}






public class ShowCommand : CommandScript
{
    
    //User Input
    private string _Display;


    
    public ShowCommand(string prDisplay)
    {
        //Assigns user input to local variable
        _Display = prDisplay;
    }

    public override void Do(CommandMapScript prCommand)
    {
        //Default output
        string lcResult = "What are you wanting to show?";
        //Get current Scene
        SceneScript lcScene = PlayerManagerScript.GetCurrentScene();
        //Help Output
        string lcHelp = "Go: \n" +
                        "   Up - Down - Left - Right \n" +
                        "Show: \n" +
                        "   Map - Inventory - Terminal - Help\n" +
                        "Scan: \n" +
                        "   Area \n" +
                        "Pick: \n" +
                        "   Up \n" +
                        "Sell: \n" +
                        "   Items";

        switch (_Display)
        {

            case "map":
                lcResult = "";
                GameManagerScript._Instance.SetActiveCanvas("Map Canvas");
                break;
            case "inventory":
                GameManagerScript._Instance.SetActiveCanvas("Inventory Canvas");
                break;
            case "terminal":
                GameManagerScript._Instance.SetActiveCanvas("Main Canvas");
                lcResult = lcScene.SceneStoryDescription;
                break;
            case "help":
                lcResult = lcHelp;
                break;
        }

        prCommand._Result = lcResult;
    }
}





public class ScanCommand : CommandScript
{
    //User Input
    private string _Scan;



    public ScanCommand(string prScan)
    {
        //Assign User input to local variable
        _Scan = prScan;
    }

    public override void Do(CommandMapScript prCommand)
    {
        if (GameManagerScript._Instance._ActiveCanvas.name == "Main Canvas")
        {
            //Default Result
            string lcResult = "AI: Sorry my error, could you type that again?";

            switch (_Scan)
            {
                case "area":
                    //Get players current scene
                    SceneScript lcCurrentScene = PlayerManagerScript.GetCurrentScene();
                    //Add to users score
                    PlayerManagerScript._CurrentPlayer.PlayerScore += 25;
                    //Add scene name to scanned list
                    PlayerManagerScript._CurrentPlayer.Scanned += lcCurrentScene + ",";
                    //update database
                    _db.Connection.InsertOrReplace(PlayerManagerScript._CurrentPlayer);
                    //Output
                    lcResult = "AI: Scanning Area \n" +
                                lcCurrentScene.Scan +
                                "\n" +
                                "AI: Items found \n" +
                                ItemstoString(lcCurrentScene.Name);
                    break;
            }
            prCommand._Result = lcResult;
        }
        else { prCommand._Result = "switch to the terminal to scan"; }

        //update Firebase Database
    }

    //Convert items to a string
    private string ItemstoString(string prCurrentSceneName)
    {
        //if not already collected
        if (!AlreadyCollected(prCurrentSceneName))
        {
            string ItemList = null;
            List<SceneItemScript> lcSceneItems = new List<SceneItemScript>();
            //Get scene Items where scene name is the current scene name
            lcSceneItems = _db.Connection.Table<SceneItemScript>().Where<SceneItemScript>(x => x.SceneName == prCurrentSceneName).ToList<SceneItemScript>();
            foreach (SceneItemScript SceneItem in lcSceneItems)
            {
                //Get Item where ID is the current Scene Item ID
                GameItemScript Item = _db.Connection.Table<GameItemScript>().Where<GameItemScript>(x => x.ItemId == SceneItem.ItemId).ToList<GameItemScript>().First<GameItemScript>();
                //Add Item Name to list
                ItemList = ItemList + Item.Name + "\n";
            }
            return ItemList;
        }
        else { return "No Items Found"; }
    }

    //Check to see if items have already been collected
    private static bool AlreadyCollected(string prCurrentSceneName)
    {
        //Default Result
        bool lcResult = false;
        //Needs Try/Catch as if collect list is null, it will fail
        try
        {
            //Create array of scene names from collected list
            string[] lcCollected = PlayerManagerScript._CurrentPlayer.CollectList.Split(',');
            foreach (string SceneName in lcCollected)
            {
                if (SceneName == prCurrentSceneName)
                {
                    lcResult = true;
                }
            }
        }
        catch { }

        return lcResult;
    }
}




public class SellCommand : CommandScript
{
    //User input
    private string _Item;
    public SellCommand(string prItem)
    {
        //Assign user input to local variable
        _Item = prItem;
    }

    public override void Do(CommandMapScript prCommand)
    {
        if (GameManagerScript._Instance._ActiveCanvas.name == "Main Canvas")
        {
            //Default output
            string lcResult = "AI: No Items to Sell";

            //Check to see if user is in correct location to sell items
            if (PlayerManagerScript.GetCurrentScene().Name == "C4")
            {
                //Create Array of players inventory items
                string[] lcInventory = PlayerManagerScript._CurrentPlayer.InventoryList.Split(',');

                foreach (string ItemName in lcInventory)
                //Side effect of .Split, Creates one extra in Array
                {
                    if (ItemName != "")
                    {
                        //Get item with same name
                        GameItemScript lcItem = _db.Connection.Table<GameItemScript>().Where(x => x.Name == ItemName).First<GameItemScript>();
                        //Add item price to players money
                        PlayerManagerScript._CurrentPlayer.PlayerMoney += lcItem.Price;
                        //Add item score to players score
                        PlayerManagerScript._CurrentPlayer.PlayerScore += lcItem.Score;
                    }
                }
                //Empty Inventory
                PlayerManagerScript._CurrentPlayer.InventoryList = "";
                //Update data base
                _db.Connection.InsertOrReplace(PlayerManagerScript._CurrentPlayer);
                //New Output
                lcResult = "AI: All items sold";
            }
            else
            {
                //New Output
                lcResult = "AI: Not at the shop, Go to C4";
            }

            prCommand._Result = lcResult;
        }
        else { prCommand._Result = "switch to the terminal to sell items"; }

        //update Firebase Database

    }
}
