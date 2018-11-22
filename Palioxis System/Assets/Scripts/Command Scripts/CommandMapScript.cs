using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Creates a map of possible commands that the user can enter then directs user to that command.
/// </summary>
public class CommandMapScript : MonoBehaviour
{
    /// Variable being sent back to CommandProccessor for text output
    public string _Result = "";
    /// Storing all directions of possible commands
    private Dictionary<string, CommandScript> _commandsDic;

    /// <summary>
    /// Adding all possible commands to a dictionary
    /// </summary>
    public CommandMapScript()
    {
        _commandsDic = new Dictionary<string, CommandScript>();
        _commandsDic.Add("go up", new GoCommand("up"));
        _commandsDic.Add("go down", new GoCommand("down"));
        _commandsDic.Add("go left", new GoCommand("left"));
        _commandsDic.Add("go right", new GoCommand("right"));
        _commandsDic.Add("show map", new ShowCommand("map"));
        _commandsDic.Add("show terminal", new ShowCommand("terminal"));
        _commandsDic.Add("show inventory", new ShowCommand("inventory"));
        _commandsDic.Add("show help", new ShowCommand("help"));
        _commandsDic.Add("pick up", new PickCommand("up"));
        _commandsDic.Add("scan area", new ScanCommand("area"));
        _commandsDic.Add("sell items", new SellCommand("Items"));
    }

    /// <summary>
    /// Checks to see if user command in part of dictionary	
    /// </summary>
    /// <param name="prCommand">users command that want to do. example: "go up".</param>
    /// <returns>If the user command exists in the dictionary</returns>
    public bool KnowsCommand(string prCommand)
    {
        bool lcResult = false;
        CommandScript lcCommand;
        /// If command string value is part of dictonary, do that command
        if (_commandsDic.ContainsKey(prCommand))
        {
            ///Select that command
            lcCommand = _commandsDic[prCommand];
            /// Do that command
            lcCommand.Do(this);
            lcResult = true;
        }
        else
        {
            lcResult = false;
        }
        return lcResult;
    }
}