using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class PopupAnim : MonoBehaviour
{
    [Header("Animation Settings")]
    public Vector2 animDistance = new Vector2(0, 50); //offset distance for animation
    public float animDuration = 0.5f; //duration of animation ideal 0.5-0.8 is enough
    public AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); //smooth curve

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 initialPosition;
    private Coroutine currentAnimation;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = rectTransform.anchoredPosition;
    }

    public void ShowPopup()
    {
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        gameObject.SetActive(true); //ensure the popup is active
        currentAnimation = StartCoroutine(AnimatePopup(true));
    }

    public void HidePopup()
    {
        //check if GameObject active before starting the coroutine
        if (!gameObject.activeSelf)
        {
            Debug.LogWarning("PopupCanvas is inactive, cannot start coroutine.");
            return;
        }

        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimatePopup(false));
    }

    private IEnumerator AnimatePopup(bool show)
    {
        Vector2 startOffset = show ? initialPosition + animDistance : initialPosition;
        Vector2 targetOffset = show ? initialPosition : initialPosition + animDistance;
        float startAlpha = show ? 0f : 1f;
        float targetAlpha = show ? 1f : 0f;

        float elapsedTime = 0f;
        while (elapsedTime < animDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = animCurve.Evaluate(elapsedTime / animDuration);

            rectTransform.anchoredPosition = Vector2.Lerp(startOffset, targetOffset, t);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            yield return null;
        }

        rectTransform.anchoredPosition = targetOffset;
        canvasGroup.alpha = targetAlpha;

        if (!show)
        {
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(false); //disable the popup after hiding
        }
    }
}