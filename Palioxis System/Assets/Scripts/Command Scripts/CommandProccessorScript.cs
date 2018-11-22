using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Proccesses the users command
/// </summary>
public class CommandProccessorScript : MonoBehaviour
{


    /// <summary>
    /// Divides the users input into sections
    /// </summary>
    /// <param name="prCommandStr"> the users input</param>
    /// <returns>what will be displayed onto the output</returns>
    public string Parse(string prCommandStr)
    {
        //Default Result
        string lcResult = "AI: Sorry, I dont understand: " + prCommandStr; ;

        //Converts to lower case
        prCommandStr = prCommandStr.ToLower();
        //divides input by spacebar
        string[] lcParts = prCommandStr.Split(' ');
        //if input parts are more than two, send parts to command map
        if (lcParts.Length >= 2)
        {
            //Connect input into one object
            string lcCommandStr = lcParts[0] + " " + lcParts[1];
            CommandMapScript lcMap = new CommandMapScript();
            //If command map knows the command string being sent, update the result
            if (lcMap.KnowsCommand(lcCommandStr))
            {
                lcResult = lcMap._Result;
            }
            else
            {
                lcResult = "AI: Sorry, I dont understand: " + prCommandStr;
            }
        }
        //If lcParts.Length > 2, display this output
        else
        {
            lcResult = "AI: What do you mean " + prCommandStr + "?";
        }
        return lcResult;
    }
}
