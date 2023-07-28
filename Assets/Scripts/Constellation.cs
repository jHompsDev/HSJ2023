using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constellation : MonoBehaviour
{
    [SerializeField] public List<StarPath> starPaths;
    [SerializeField] public List<Star> stars;

    bool isComplete;

    public bool CheckIfComplete()
    {
        bool foo = true;

        foreach (StarPath p in starPaths) if (!p.isEnabled) foo = false;

        return foo;
    }

    public void SetComplete()
    {
        isComplete = true;
    }
}