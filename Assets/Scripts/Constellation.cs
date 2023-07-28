using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constellation : MonoBehaviour
{
    [SerializeField] public List<StarPath> starPaths;
    [SerializeField] public List<Star> stars;

    bool isComplete;

    public void SetComplete()
    {
        isComplete = true;
    }
}