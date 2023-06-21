using UnityEngine;

[RequireComponent(typeof(Light))]
public class SimpleAnimLight : SimpleAnim
{
    [Tooltip("Light will be set to this color on awake")]
    [SerializeField] public Color startColor = Color.white;

    [SerializeField] public Color endColor = Color.white;

    private Light lightComponent;

    private void Awake()
    {
        lightComponent = GetComponent<Light>();
        SyncLight();
    }

    public void SyncLight() => lightComponent.color = startColor;

    protected override void AnimateAtPoint(float evaluatedPoint)
        => lightComponent.color = Color.LerpUnclamped(startColor, endColor, evaluatedPoint);
}
