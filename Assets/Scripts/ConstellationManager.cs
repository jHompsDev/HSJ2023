using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConstellationManager : MonoBehaviour
{
    [SerializeField] List<Constellation> constellations;
    [SerializeField] List<Star> stars;

    [SerializeField] GameObject pointer;
    [SerializeField] Star hoveredStar;
    [SerializeField] Star selectedStar;

    [SerializeField] LineRenderer pointerPathRenderer;

    void UpdatePointerPos()
    {
        Vector3 pointerPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 screenPointerPos = Camera.main.ScreenToWorldPoint(pointerPos);
        pointer.transform.position = new Vector3(screenPointerPos.x, screenPointerPos.y, 0);
    }

    void TogglePointerLine(bool foo)
    {
        pointerPathRenderer.enabled = foo;
        if (foo) pointerPathRenderer.SetPosition(0, pointer.transform.position);
    }

    void UpdatePointerLine()
    {
        pointerPathRenderer.SetPosition(1, pointer.transform.position);
    }

    private void Update()
    {
        UpdatePointerPos();

        foreach (Star star in stars)
        {
            if (star.isHovered) hoveredStar = star;
        }

        if (Input.GetMouseButtonDown(0))
        {
            selectedStar = hoveredStar;
            TogglePointerLine(true);

        }
        else if (Input.GetMouseButtonDown(1))
        {
            TogglePointerLine(false);
        }

        if (pointerPathRenderer.enabled) UpdatePointerLine();
    }
}
