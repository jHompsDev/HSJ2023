using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ConstellationManager : MonoBehaviour
{
    [SerializeField] List<Constellation> constellations;
    [SerializeField] List<Star> stars;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] public Constellation currentConstellation;

    [SerializeField] GameObject pointer;
    [SerializeField] Star hoveredStar;
    [SerializeField] Star selectedStar;

    [SerializeField] LineRenderer pointerPathRenderer;

    void Initialise(int i = 0)
    {
        currentConstellation = constellations[i];
        currentConstellation.isComplete = false;
        spriteRenderer.sprite = currentConstellation.sprite;
        spriteRenderer.enabled = false;

        stars = FindObjectsOfType<Star>().ToList();

        foreach (Star s in stars) s.gameObject.SetActive(false);

        foreach (StarPath p in currentConstellation.starPaths)
        {
            if (!p.starA.gameObject.activeSelf) p.starA.gameObject.SetActive(true);
            if (!p.starB.gameObject.activeSelf) p.starB.gameObject.SetActive(true);
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
                path = p;
                return true;
            }
        }

        path = null;
        return false;
    }

    public IEnumerator FadeConstellationIn()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);

        while (spriteRenderer.color.a < 1)
        {
            float t = Time.deltaTime / 2f;
            Debug.Log(t);
            spriteRenderer.color += new Color(0,0,0,t);
            yield return null;
        }

        yield break;
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

                if (currentConstellation.CheckIfComplete())
                {
                    currentConstellation.SetComplete();
                    spriteRenderer.enabled = true;
                }

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
