using UnityEngine;

public class DisableSlidingDoors : MonoBehaviour
{
    void Start()
    {
        // Find all sliding door objects in the scene
        GameObject[] slidingDoors = GameObject.FindGameObjectsWithTag("SlidingDoor");

        // Disable each sliding door object
        foreach (GameObject door in slidingDoors)
        {
            door.SetActive(false);
        }

        // Print debug message
        Debug.Log("All sliding doors have been disabled.");
    }
}
