using System.Collections;

using UnityEngine;

public class InteractableDoor : Interactable
{
    [SerializeField] private SimpleAnim[] openableDoors;
    private bool isOpen;

    protected override void Awake()
    {
        base.Awake();
        if (openableDoors.Length == 0)
            openableDoors = new SimpleAnim[] { GetComponent<SimpleAnim>() };
    }

    public override void Interact()
    {
        if (!isOpen)
            foreach (var door in openableDoors)
                StartCoroutine(door.AnimateNormal());
        else
            foreach (var door in openableDoors)
                StartCoroutine(door.AnimateReversed());

        isOpen = !isOpen;
    }
}
