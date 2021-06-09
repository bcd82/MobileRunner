using System;
using Unity.Collections;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get { return instance; } } // requires Awake() { instance = this } to work !!!
    private static GameStats instance; // requires Awake() { instance = this } to work !!!

    // Score
    public float score;
    public float highscore;
    public float distanceModifier = 1.5f;

    // Internal Cooldown 
    private float lastScoreUpdate;
    private float scoreUpdateDelta = 0.2f; // will be used to update score and UI 5 times per second ( instead of 60 or 50 )


    // Fish
    public int totalFish;
    public int fishCollectedThisSession;
    public int fishCreated;
    public float pointsPerFish = 10.0f;

    [SerializeField] private AudioClip collectFishSFX;


    // Action
    public Action<int> OnCollectFish;  // allows objects to be subscribed to this action and be updated each time it occurs 
    public Action<float> OnScoreChange; // update UI when score changes

    public string ScoreToText()
    {
        return score.ToString("0000000");// formats the score
    }
    public string FishToText()
    {
        return fishCollectedThisSession.ToString("000");// formats the score
    }

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        float s = GameManager.Instance.motor.transform.position.z * distanceModifier;
        s += fishCollectedThisSession * pointsPerFish;

        if (s > score)
        {
            if (Time.time - lastScoreUpdate > scoreUpdateDelta) // if ( time now - last time score was updated > the time which we defined for cooldown ) 
            {
                score = s;
                lastScoreUpdate = Time.time;
                OnScoreChange?.Invoke(score);
            }
        }
    }
    public void CollectFish() // will be called on collect
    {
        fishCollectedThisSession++;
        OnCollectFish?.Invoke(fishCollectedThisSession); // calls the action OnCollectFish , will receive the amount of fish 
        AudioManager.Instance.PlaySFX(collectFishSFX, 0.6f);
    }
    public void ResetSession() // resets stats
    {
        score = 0;
        fishCollectedThisSession = 0;
        OnScoreChange?.Invoke(score);
        OnCollectFish?.Invoke(fishCollectedThisSession);
    }

}
