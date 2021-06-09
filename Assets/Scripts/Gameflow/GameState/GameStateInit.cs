using TMPro;
using UnityEngine;

public class GameStateInit : GameState
{
    public GameObject menuUI;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI fishCountText;
    [SerializeField] private AudioClip menuLoopMusic;
    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Init);
        highScoreText.text = "Highscore : " + SaveManager.Instance.save.Highscore.ToString();
        fishCountText.text = "Fish : " + SaveManager.Instance.save.Fish.ToString();

        menuUI.SetActive(true);

        AudioManager.Instance.PlayMusicWithXFade(menuLoopMusic, 0.5f);
    }
    public override void Destruct()
    {
        menuUI.SetActive(false);
    }

    public void OnPlayClick()
    {
        brain.ChangeState(GetComponent<GameStateGame>());
        GameStats.Instance.ResetSession(); // resets scores and such
        GetComponent<GameStateDeath>().EnableRevive();

    }
    public void OnShopClick()
    {
        brain.ChangeState(GetComponent<GameStateShop>());
        Debug.Log("Shop");
    }
}
