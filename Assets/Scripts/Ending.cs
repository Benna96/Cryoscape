using System;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField] private SimpleAnim[] endingAnims;
    private bool alreadyPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ending trigger");
        if (!alreadyPlayed && other.CompareTag("Player") && !other.isTrigger)
        {
            Debug.Log("Ending");
            Array.ForEach(endingAnims, anim => StartCoroutine(anim.AnimateNormal()));
            alreadyPlayed = true;
        }
    }
}
