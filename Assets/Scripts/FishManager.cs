
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static FishManager Instance { get { return instance; } } // will not overwrite this instance 
    private static FishManager instance;


    public int fishCreated;
    public int specialFishIndex;
    [SerializeField] private int incrementBy;
    private void Awake()
    {
        instance = this;
    }

    public void AddFish()
    {
        fishCreated++;
    }
    public void IncrementSpecialIndex()
    {
        specialFishIndex += incrementBy;
    }

    public void ResetFish()
    {
        fishCreated = 0;
        specialFishIndex = incrementBy;
    }
}