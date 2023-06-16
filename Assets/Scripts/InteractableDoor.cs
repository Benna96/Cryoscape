using System.Linq;

using UnityEngine;

public class InteractableDoor : Activatable
{
    // Only left in for backwards compatibility, will be auto added to interactAnims
    // FormlerlySerializedAs doesn't work, probably because interactAnims already existed
    [HideInInspector]
    [SerializeField] private SimpleAnim[] openableDoors;

    protected override void Awake()
    {
        interactAnims = interactAnims.Concat(openableDoors).ToArray();
        if (interactAnims.Length == 0)
            interactAnims = new SimpleAnim[] { GetComponent<SimpleAnim>() };

        base.Awake();
    }
}
