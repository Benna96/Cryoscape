using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class SimpleAnim : MonoBehaviour
{
    [FormerlySerializedAs("openAnimDuration")]
    [SerializeField] protected float animDuration;

    [Header("Animation curves")]

    [FormerlySerializedAs("openAnimCurve")]
    [SerializeField] protected AnimationCurve normalAnimCurve;

    [Tooltip("Leave empty to use Normal Curve")]
    [FormerlySerializedAs("closeAnimCurve")]
    [SerializeField] protected AnimationCurve reversedAnimCurve;

    [Header("Sounds")]

    [FormerlySerializedAs("openAudio")]
    [SerializeField] protected AudioSource normalAnimAudio;

    [FormerlySerializedAs("closeAudio")]
    [SerializeField] protected AudioSource reversedAnimAudio;

    [Header("Debug")]
    [Tooltip("Toggle this on to automatically play the animation back and forth when entering play mode.")]
    [SerializeField] protected bool debugAutoPlay = false;

    private Coroutine activeAnimCoroutine;
    private float elapsedTime = 0f;
    private float amountToAnimateThisFrame = 0f;

#if UNITY_EDITOR
    private void Start()
    {
        if (normalAnimCurve.length < 2)
            normalAnimCurve = new AnimationCurve(new(0f, 0f, 1f, 1f), new(1f, 1f, 1f, 1f));
        if (reversedAnimCurve.length < 2)
            reversedAnimCurve = normalAnimCurve;

        StartCoroutine(AutoPlay());
    }

    private IEnumerator AutoPlay()
    {
        while (debugAutoPlay)
        {
            yield return AnimateNormal();
            yield return new WaitForSeconds(1f);
            yield return AnimateReversed();
            yield return new WaitForSeconds(1f);
        }
    }
#endif

    public IEnumerator AnimateNormal()
    {
        if (normalAnimAudio != null)
            normalAnimAudio.Play();

        if (elapsedTime > 0f && elapsedTime < animDuration)
            StopCoroutine(activeAnimCoroutine);

        activeAnimCoroutine = StartCoroutine(AnimateEachFrame(true));
        yield return activeAnimCoroutine;
    }

    public IEnumerator AnimateReversed()
    {
        if (reversedAnimAudio != null)
            reversedAnimAudio.Play();


        if (elapsedTime > 0f && elapsedTime < animDuration)
            StopCoroutine(activeAnimCoroutine);

        activeAnimCoroutine = StartCoroutine(AnimateEachFrame(false));
        yield return activeAnimCoroutine;
    }

    private IEnumerator AnimateEachFrame(bool reversed)
    {
        // Take into account the amount animated in previous, now closed coroutine
        // For example: Previous coroutine got to point 0.5 on the animation curve, which evaluated to 0.3.
        // Now, we want to go from...
        // 0.3 to 1: 0.3 + (0.7 * progress). 0.7 being (1 - 0.3).
        // 0.3 to 0: 0.3 - (0.3 * progress). - (0.3 * progress) can be expressed as ((0 - 0.3) * progress).
        // These two can be simplified to 0.3 + (((1 or 0) - 0.3) * progress).
        var curveToUse = reversed ? reversedAnimCurve : normalAnimCurve;
        var amountAlreadyAnimated = amountToAnimateThisFrame;
        var amountToAnimateThisCoroutine = (reversed ? 0 : 1) - amountAlreadyAnimated;

        var alreadyElapsedTime = elapsedTime;
        var durationToUse = animDuration - alreadyElapsedTime;
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
