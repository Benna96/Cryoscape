using System.Collections;

using UnityEngine;

public class InteractableDoor : Interactable
{
    [SerializeField] private OpenAnim[] openableDoors;
    private bool isOpen;

    protected override void Awake()
    {
        base.Awake();
        if (openableDoors.Length == 0)
            openableDoors = new OpenAnim[] { GetComponent<OpenAnim>() };
    }

    public override void Interact()
    {
        if (!isOpen)
            foreach (var door in openableDoors)
                StartCoroutine(door.Open());
        else
            foreach (var door in openableDoors)
                StartCoroutine(door.Close());

        isOpen = !isOpen;
    }
}
