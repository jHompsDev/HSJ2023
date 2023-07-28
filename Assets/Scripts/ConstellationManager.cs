using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class ConstellationManager : MonoBehaviour
{
    [SerializeField] List<Constellation> constellations;
    [SerializeField] List<Star> stars;

    [SerializeField] Constellation currentConstellation;

    [SerializeField] GameObject pointer;
    [SerializeField] Star hoveredStar;
    [SerializeField] Star selectedStar;

    [SerializeField] LineRenderer pointerPathRenderer;

    void Initialise()
    {
        foreach (Constellation c in constellations)
        {
            foreach (StarPath p in c.starPaths)
            {
                if (!c.stars.Contains(p.starA)) c.stars.Add(p.starA);
                if (!c.stars.Contains(p.starB)) c.stars.Add(p.starB);
            }
        }
    }

    void UpdatePointerPos()
    {
        Vector3 pointerPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 screenPointerPos = Camera.main.ScreenToWorldPoint(pointerPos);
        pointer.transform.position = new Vector3(screenPointerPos.x, screenPointerPos.y, 0);
    }

    void TogglePointerLine(bool foo)
    {
        pointerPathRenderer.enabled = foo;
        if (foo) pointerPathRenderer.SetPosition(0, selectedStar.transform.position);
    }

    void UpdatePointerLine()
    {
        pointerPathRenderer.SetPosition(1, pointer.transform.position);
    }

    bool AreStarsClickedPath(out StarPath path)
    {
        foreach (StarPath p in currentConstellation.starPaths)
        {
            if ((p.starA == hoveredStar && p.starB == selectedStar) || (p.starB == hoveredStar && p.starA == selectedStar))
            {
                Debug.Log("PATH FOUND");
                path = p;
                return true;
            }
        }

        path = null;
        return false;
    }

    private void Awake()
    {
        Initialise();
    }

    private void Update()
    {
        UpdatePointerPos();

        foreach (Star star in stars)
        {
            if (star.isHovered)
            {
                hoveredStar = star;
                break;
            }
            else
            {
                hoveredStar = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            StarPath p = null;

            if (AreStarsClickedPath(out p))
            {
                p.SetEnabled();

                if (currentConstellation.CheckIfComplete()) currentConstellation.SetComplete();

                selectedStar = null;
                TogglePointerLine(false);
            }
            else
            {
                if (hoveredStar != null)
                {
                    selectedStar = hoveredStar;
                    TogglePointerLine(true);
                }
                else
                {
                    TogglePointerLine(false);
                }
            }


        }
        else if (Input.GetMouseButtonDown(1))
        {
            TogglePointerLine(false);
        }

        if (pointerPathRenderer.enabled) UpdatePointerLine();
    }
}
