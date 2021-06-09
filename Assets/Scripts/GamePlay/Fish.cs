using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Fish : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickupFish();
        }
    }

    private void PickupFish()
    {
        // increment fish count 
        GameStats.Instance.CollectFish();
        
        // increment score
        
        // play sfx 
        // trigger animation
        anim.SetTrigger("Pickup");
    }

    public void OnShowChunk() // will reset fish if they've been collected already.  will be called via broadcast message from Chunk.cs
    {
        anim?.SetTrigger("Idle");

    }
}
