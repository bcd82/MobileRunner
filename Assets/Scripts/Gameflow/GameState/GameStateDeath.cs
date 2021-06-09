
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameStateDeath : GameState, IUnityAdsListener
{
    public GameObject deathUI;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI fishTotal;
    [SerializeField] private TextMeshProUGUI currentFish;

    // completion circle field
    [SerializeField] private Image completionCircle;
    public float timeToDecision = 2.5f;
    private float deathTime;

    private void Start()
    {
        Advertisement.AddListener(this); // rewards will not work properly unless added ( finish / failed states for ads wont work )
    }
    public override void Construct()
    {
        base.Construct();
        deathTime = Time.time;

        GameManager.Instance.motor.PausePlayer();
        deathUI.SetActive(true);
        // prior to saving set the highscore if needed
        if (SaveManager.Instance.save.Highscore < (int)GameStats.Instance.score)
        {
            SaveManager.Instance.save.Highscore = (int)GameStats.Instance.score; // changes score (float) to int ! by using (int) before
            currentScore.color = Color.green;
        }
        else
            currentScore.color = Color.white;

        SaveManager.Instance.save.Fish += GameStats.Instance.fishCollectedThisSession;
        SaveManager.Instance.Save();

        highScore.text = "Highscore : " + SaveManager.Instance.save.Highscore.ToString();
        currentScore.text = GameStats.Instance.ScoreToText();
        fishTotal.text = "Total : " + (SaveManager.Instance.save.Fish).ToString();
        currentFish.text = GameStats.Instance.FishToText();


    }
    public override void Destruct()
    {
        deathUI.SetActive(false);
    }
    public override void UpdateState()
    {
        float ratio = (Time.time - deathTime) / timeToDecision; // gives ratio within 0 to 1 on how much time is left
        completionCircle.color = Color.Lerp(Color.green, Color.red, ratio); // changes color from green to red as decision time goes down
        completionCircle.fillAmount = 1 - ratio; // controls how much of the outer circle is filled

        if (ratio > 1)
        {
            completionCircle.gameObject.SetActive(false);
        }
    }
    public void TryResumeGame()
    {
        AdManager.Instance.ShowRewardedAd();
    }
    public void ResumeGame()
    {
        brain.ChangeState(GetComponent<GameStateGame>());
        GameManager.Instance.motor.RespawnPlayer();
    }
    public void ToMenu()
    {
        brain.ChangeState(GetComponent<GameStateInit>());

        GameManager.Instance.motor.ResetPlayer();
        GameManager.Instance.worldGeneration.ResetWorld();
        GameManager.Instance.sceneChunkGeneration.ResetWorld();
        GameManager.Instance.ChangeCamera(GameCamera.Init);
    }
    public void EnableRevive() // controld if revive is active on screen ( will allow to revive only once per game with ads )
    {
        completionCircle.gameObject.SetActive(true);
    }
    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        completionCircle.gameObject.SetActive(false); // will allow to only happen once
        switch (showResult)
        {
            case ShowResult.Failed:
                ToMenu();
                break;
            case ShowResult.Finished:
                ResumeGame();
                break;
            default:
                break;
        }
    }
}
