using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
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

    [SerializeField] public TMP_Text firstText;
    [SerializeField] public TMP_Text starCountText;

    public void Initialise(int i = 0)
    {
        currentConstellation = constellations[i];
        currentConstellation.isComplete = false;
        spriteRenderer.sprite = currentConstellation.sprite;
        spriteRenderer.enabled = false;

        currentConstellation.gameObject.SetActive(true);

        foreach (Constellation c in constellations)
        {
            if (c != currentConstellation) c.gameObject.SetActive(false);
        }

        stars = FindObjectsOfType<Star>().ToList();

        foreach (Star s in stars) s.gameObject.SetActive(false);

        foreach (StarPath p in currentConstellation.starPaths)
        {
            if (!p.starA.gameObject.activeSelf) p.starA.gameObject.SetActive(true);
            if (!p.starB.gameObject.activeSelf) p.starB.gameObject.SetActive(true);
        }

        starCountText.text = "Star Paths: 0/" + currentConstellation.starPaths.Count;

        AudioManager.PlaySFX("conStart");

        string fadeText = "";

        switch (i)
        {
            case 0: fadeText = "Click the Stars and make lines<br>between them to form a constellation!";break;
            case 1: fadeText = "Love can slumber in a boiling pot.<br>Meals shared with family is never forgot."; break;
            case 2: fadeText = "Loving is not perfect, nor always in tune.<br>Notes sung with the heart can make one swoon."; break;
            case 3: fadeText = "Fiercly defend those under your care,<br>Wings outstretched, and a long neck in the air."; break;
            default: break;
        }

        firstText.text = fadeText;

        StartCoroutine(FadeFirstTimeText());
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

    void UpdateUI()
    {
        uint i = 0;

        foreach (StarPath p in currentConstellation.starPaths)
        {
            if (p.isEnabled) i++;
        }

        starCountText.text = "Star Paths: " + i + "/" + currentConstellation.starPaths.Count;
    }

    public void ToggleConstellationUI(bool foo)
    {
        firstText.gameObject.SetActive(foo);
    }

    public IEnumerator FadeConstellationIn()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);

        while (spriteRenderer.color.a < 1f / 3)
        {
            float t = Time.deltaTime * 1f / 3;
            spriteRenderer.color += new Color(0, 0, 0, t);
            yield return null;
        }

        yield break;
    }

    public IEnumerator FadeFirstTimeText()
    {
        float t = 0;
        firstText.gameObject.SetActive(true);
        firstText.color = new Color(1, 1, 1, 0);

        while (t < 1)
        {
            t += Time.deltaTime / 2;
            firstText.color = new Color(1, 1, 1, t);
            yield return null;
        }

        t = 0;
        yield return new WaitForSeconds(2);

        while (t < 1)
        {
            {
                t += Time.deltaTime / 2;
                firstText.color = new Color(1, 1, 1, 1 - t);
                yield return null;
            }
        }

        //firstText.gameObject.SetActive(false);
        //firstText.color = new Color(1, 1, 1, 1);

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
                UpdateUI();

                if (currentConstellation.CheckIfComplete())
                {
                    currentConstellation.SetComplete();
                    spriteRenderer.enabled = true;

                    AudioManager.PlaySFX("conComplete");
                }
                else
                {
                    AudioManager.PlaySFX("starPath");
                }

                selectedStar = null;
                TogglePointerLine(false);
            }
            else
            {
                if (hoveredStar != null && !currentConstellation.isComplete)
                {
                    if (selectedStar == null)
                    {
                        selectedStar = hoveredStar;
                        AudioManager.PlaySFX("starPress");
                    }

                    TogglePointerLine(true);
                }
                else
                {
                    selectedStar = null;
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
