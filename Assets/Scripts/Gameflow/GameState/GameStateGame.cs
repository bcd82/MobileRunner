using UnityEngine;
using TMPro;
public class GameStateGame : GameState
{
    [SerializeField] private TextMeshProUGUI fishCount;
    [SerializeField] private TextMeshProUGUI scoreCount;
    [SerializeField] private AudioClip gameLoopMusic;

    public GameObject gameUI;
    public override void Construct()
    {
        base.Construct();
        GameManager.Instance.motor.ResumePlayer();
        GameManager.Instance.ChangeCamera(GameCamera.Game);

        GameStats.Instance.OnCollectFish += OnCollectFish; // subscribing to the Action
        GameStats.Instance.OnScoreChange += OnScoreChange;
        //GameStats.Instance.OnScoreChange += (s) => { scoreCount.text = s.ToString(); }; // lambda expression

        gameUI.SetActive(true);

        AudioManager.Instance.PlayMusicWithXFade(gameLoopMusic, 0.3f);
    }

    private void OnCollectFish(int amnCollected)
    {
        fishCount.text = GameStats.Instance.FishToText();
    }
    private void OnScoreChange(float score)
    {
        scoreCount.text = GameStats.Instance.ScoreToText();
    }
    public override void Destruct()
    {
        gameUI.SetActive(false);


        GameStats.Instance.OnCollectFish -= OnCollectFish; // UN-subscribing to the Action !!
        GameStats.Instance.OnScoreChange -= OnScoreChange;
    }
    public override void UpdateState()
    {
        GameManager.Instance.worldGeneration.ScanPosition();
        GameManager.Instance.sceneChunkGeneration.ScanPosition();
    }
}
