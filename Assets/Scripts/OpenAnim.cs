using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OpenAnim : MonoBehaviour
{
    [SerializeField] protected float openAnimDuration;

    [Header("Animation curves")]
    [SerializeField] protected AnimationCurve openAnimCurve;

    [Tooltip("Leave empty to use Open Curve")]
    [SerializeField] protected AnimationCurve closeAnimCurve;

    [Header("Sounds")]
    [SerializeField] protected AudioSource openAudio;
    [SerializeField] protected AudioSource closeAudio;

    [Header("Debug")]
    [Tooltip("Toggle this on to automatically play the animation back and forth when entering play mode.")]
    [SerializeField] protected bool debugAutoPlay = false;

    private Coroutine activeAnimCoroutine;
    private float elapsedTime = 0f;
    private float amountToAnimateThisFrame = 0f;

#if UNITY_EDITOR
    private void Start()
    {
        Time.timeScale = 0.3f;

        if (openAnimCurve.length < 2)
            openAnimCurve = new AnimationCurve(new(0f, 0f, 1f, 1f), new(1f, 1f, 1f, 1f));
        if (closeAnimCurve.length < 2)
            closeAnimCurve = openAnimCurve;

        StartCoroutine(AutoPlay());
    }

    private IEnumerator AutoPlay()
    {
        while (debugAutoPlay)
        {
            yield return Open();
            yield return new WaitForSeconds(1f);
            yield return Close();
            yield return new WaitForSeconds(1f);
        }
    }
#endif

    public IEnumerator Open()
    {
        if (openAudio != null)
            openAudio.Play();

        if (elapsedTime > 0f && elapsedTime < openAnimDuration)
            StopCoroutine(activeAnimCoroutine);

        activeAnimCoroutine = StartCoroutine(AnimateEachFrame(true));
        yield return activeAnimCoroutine;
    }

    public IEnumerator Close()
    {
        if (closeAudio != null)
            closeAudio.Play();


        if (elapsedTime > 0f && elapsedTime < openAnimDuration)
            StopCoroutine(activeAnimCoroutine);

        activeAnimCoroutine = StartCoroutine(AnimateEachFrame(false));
        yield return activeAnimCoroutine;
    }

    private IEnumerator AnimateEachFrame(bool doOpen)
    {
        // Take into account the amount animated in previous, now closed coroutine
        // For example: Previous coroutine got to point 0.5 on the animation curve, which evaluated to 0.3.
        // Now, we want to go from...
        // 0.3 to 1: 0.3 + (0.7 * progress). 0.7 being (1 - 0.3).
        // 0.3 to 0: 0.3 - (0.3 * progress). - (0.3 * progress) can be expressed as ((0 - 0.3) * progress).
        // These two can be simplified to 0.3 + (((1 or 0) - 0.3) * progress).
        var curveToUse = doOpen ? openAnimCurve : closeAnimCurve;
        var amountAlreadyAnimated = amountToAnimateThisFrame;
        var amountToAnimateThisCoroutine = (doOpen ? 1 : 0) - amountAlreadyAnimated;

        var alreadyElapsedTime = elapsedTime;
        var durationToUse = openAnimDuration - alreadyElapsedTime;
        elapsedTime = 0f;

        while (elapsedTime < durationToUse)
        {
            Animate(elapsedTime);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        Animate(elapsedTime);
        elapsedTime = 0f;

        void Animate(float elapsedTime)
        {
            float curvePoint = Mathf.InverseLerp(0f, durationToUse, elapsedTime);
            float curveEvaluated = curveToUse.Evaluate(curvePoint);
            amountToAnimateThisFrame = amountAlreadyAnimated + (amountToAnimateThisCoroutine * curveEvaluated);

            AnimateAtPoint(amountToAnimateThisFrame);
        }
    }

    /// <summary>
    /// Implement this in inheriting class
    /// </summary>
    /// <param name="percentage"></param>
    protected abstract void AnimateAtPoint(float evaluatedPoint);
}
