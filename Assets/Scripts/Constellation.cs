using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constellation : MonoBehaviour
{
    [SerializeField] public List<StarPath> starPaths;
    [SerializeField] public List<Star> stars;
    [SerializeField] public Sprite sprite;

    public bool isComplete;

    public bool CheckIfComplete()
    {
        bool foo = true;

        foreach (StarPath p in starPaths) if (!p.isEnabled) foo = false;

        return foo;
    }

    public void SetComplete()
    {
        isComplete = true;

        foreach (Star s in stars) s.gameObject.SetActive(false);

        StartCoroutine(GameManager.Instance.conManager.FadeConstellationIn());
        Debug.Log("COMPLETE");
    }
}