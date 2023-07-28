using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region VARIABLES
    bool debug;
    #endregion

    #region FUNCTIONS
    void Initialise()
    {
        if (debug) Debug.Log("Initialise!");
    }

    public void ToggleDebug(bool var)
    {
        debug = var;
    }
    #endregion

    #region MONOBEHAVIOUR
    private void Awake()
    {
        ToggleDebug(true);

        Initialise();
    }
    #endregion
}