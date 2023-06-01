using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OpenAnim : MonoBehaviour
{
    [SerializeField] protected float duration;

    [Header("Animation curves")]
    [SerializeField] protected AnimationCurve openCurve;

    [Tooltip("Leave empty to use Open Curve")]
    [SerializeField] protected AnimationCurve closeCurve;

    [Header("Debug")]
    [Tooltip("Toggle this on to automatically play the animation back and forth when entering play mode.")]
    [SerializeField] protected bool debugAutoPlay = false;

#if UNITY_EDITOR
    private void Start()
    {
        if (openCurve.length == 0)
            openCurve = new AnimationCurve(new(0f, 0f, 1f, 1f), new(1f, 1f, 1f, 1f));
        if (closeCurve.length == 0)
            closeCurve = openCurve;

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
        yield return StartCoroutine(AnimateEachFrame(true));
    }

    public IEnumerator Close()
    {
        yield return StartCoroutine(AnimateEachFrame(false));
    }

    private IEnumerator AnimateEachFrame(bool doOpen)
    {
        float elapsedTime = 0f;
        var curveToUse = doOpen ? openCurve : closeCurve;

        while (elapsedTime < duration)
        {
            Animate(elapsedTime);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        Animate(elapsedTime);

        void Animate(float elapsedTime)
        {
            float progress = Mathf.InverseLerp(0f, duration, elapsedTime);
            float curveEvaluatedProgress = curveToUse.Evaluate(progress);
            if (!doOpen)
                curveEvaluatedProgress = 1 - curveEvaluatedProgress;

            AnimateAtPoint(curveEvaluatedProgress);
        }
    }

    /// <summary>
    /// Implement this in inheriting class
    /// </summary>
    /// <param name="percentage"></param>
    protected abstract void AnimateAtPoint(float evaluatedPoint);
}
