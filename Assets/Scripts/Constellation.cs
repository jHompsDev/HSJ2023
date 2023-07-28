using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constellation : MonoBehaviour
{
    [SerializeField]List<StarPath> starPaths;

    bool isComplete;

    public void SetComplete()
    {
        isComplete = true;
    }
}