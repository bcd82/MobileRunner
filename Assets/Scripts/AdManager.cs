using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get { return instance; } }
    private static AdManager instance;

    [SerializeField] private string gameID;
    [SerializeField] private string rewardVideoPlacementId;
    [SerializeField] private bool testMode;

    private void Awake()
    {
        instance = this;
        Advertisement.Initialize(gameID, testMode); // inits unity ads ( test mode is true/false )
    }

    public void ShowRewardedAd()
    {
        ShowOptions so = new ShowOptions();
        Advertisement.Show(rewardVideoPlacementId, so);
    }
}
