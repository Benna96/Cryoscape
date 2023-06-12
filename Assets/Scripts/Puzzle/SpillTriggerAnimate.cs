using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpillTriggerAnimate : MonoBehaviour
{
    [SerializeField] SimpleAnim anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
            StartCoroutine(anim.AnimateNormal());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
            StartCoroutine(anim.AnimateReversed());
    }
}
