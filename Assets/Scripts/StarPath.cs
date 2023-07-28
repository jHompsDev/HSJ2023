using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPath : MonoBehaviour
{
    public bool isEnabled;
    public LineRenderer line;

    public Star starA, starB;

    private void Awake()
    {
        line.SetPosition(0, starA.transform.position);
        line.SetPosition(1, starB.transform.position);
    }

    public void SetEnabled()
    {
        isEnabled = true;
        line.enabled = true;
    }
}
