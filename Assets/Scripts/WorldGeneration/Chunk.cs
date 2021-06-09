using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Chunk : MonoBehaviour
{
    public float chunkLength;
    [SerializeField] private Transform[] FishTransforms1;
    [SerializeField] private Transform[] FishTransforms2;
    [SerializeField] private GameObject[] Fish;
    private Transform[] FishTransforms;

    private List<GameObject> createdFish = new List<GameObject>();
    public bool instatnces;
    public GameStats gs;
    


    public Chunk ShowChunk()
    {
     
        transform.gameObject.BroadcastMessage("OnShowChunk", SendMessageOptions.DontRequireReceiver); // will broadcat a message to all children, SendMessageOptions.DontRequireReceiver - allows to broadcast to objects with no receiver
        gameObject.SetActive(true);
        if (instatnces)
            InstantiateFish();

        return this;
    }
    public Chunk HideChunk()
    {
        foreach (var g in createdFish)
        {
            if (g.gameObject is { }) Destroy(g.gameObject);
        }
        createdFish.Clear();
        gameObject.SetActive(false);
        return this;
    }

    private void InstantiateFish ()
    {
        // decides where to place fish
        FishTransforms = Random.Range(1f, 0f) > 0.5f ? FishTransforms1 : FishTransforms2;
        foreach (var t in FishTransforms)
        {
            int fishType;
            FishManager.Instance.AddFish();
            if (FishManager.Instance.fishCreated == FishManager.Instance.specialFishIndex)
            {
                 fishType = 1;
                 FishManager.Instance.IncrementSpecialIndex();
            }
            else
            {
                 fishType = 0;
            }
            createdFish.Add( Instantiate(Fish[fishType], t.position, t.rotation));
            }
    }
        }
    
 

