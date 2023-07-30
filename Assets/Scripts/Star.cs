using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Star : MonoBehaviour
{
    public bool isHovered;

    const float rotSpeed = 180f;
    const float pulseSpeed = 5;

    [SerializeField]SpriteRenderer sprRenderer;

    private void OnMouseEnter()
    {
        AudioManager.PlaySFX("starHover");

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

        sprRenderer.enabled = true;

        while (isHovered)
        {
            t += Time.deltaTime;

            if (isHovered)
            {
                sprRenderer.transform.rotation = Quaternion.Euler(0, 0, -rotSpeed * t);
                sprRenderer.transform.localScale = Vector3.one + (Vector3.one / 4) * Mathf.Sin(t * pulseSpeed);
            }

            yield return null;
        }

        sprRenderer.transform.rotation = Quaternion.identity;
        sprRenderer.transform.localScale = Vector3.one;
        sprRenderer.enabled = false;

        yield break;
    }
}
