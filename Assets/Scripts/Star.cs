using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Star : MonoBehaviour
{
    public bool isHovered;

    const float rotSpeed = 180f;
    const float pulseSpeed = 5;

    private void OnMouseEnter()
    {
        isHovered = true;
        StartCoroutine(Animate());
    }

    private void OnMouseExit()
    {
        isHovered = false;
    }

    IEnumerator Animate()
    {
        float t = 0;
        SpriteRenderer renderer = transform.GetComponentInChildren<SpriteRenderer>();

        renderer.enabled = true;

        while (isHovered)
        {
            t += Time.deltaTime;

            if (isHovered)
            {
                renderer.transform.rotation = Quaternion.Euler(0, 0, -rotSpeed * t);
                renderer.transform.localScale = Vector3.one + (Vector3.one / 4) * Mathf.Sin(t * pulseSpeed);
            }

            yield return null;
        }

        renderer.transform.rotation = Quaternion.identity;
        renderer.transform.localScale = Vector3.one;
        renderer.enabled = false;

        yield break;
    }
}
