using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawnManager : MonoBehaviour
{
    [HideInInspector]
    public bool hasDodgeball = false;

    private Collider ballCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Loot")
        {
            ballCollider = other;
            hasDodgeball = true;
        }
    }

    private void Update()
    {
        if (ballCollider == null)
        {
            hasDodgeball = false;
        }
    }
}
