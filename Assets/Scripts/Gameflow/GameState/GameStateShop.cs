
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateShop : GameState
{
    public GameObject shopUI;
    public TextMeshProUGUI totalFish;
    public TextMeshProUGUI currentHatName;
    public HatLogic hatLogic;

    private bool isInit = false;
    private int hatCount;
    private int unlockedHatsCount;

    // Shop Item 

    public GameObject hatPrefab;
    public Transform hatContainer;
    public Hat[] hats; // Hat -> the scriptable object we've created 

    // completion circle
    public Image completionCircle;
    public TextMeshProUGUI completionText;
    protected override void Awake()
    {
        base.Awake();
    }
    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Shop);
        hats = Resources.LoadAll<Hat>("Hat");  // will load resources from a specific folder- Hats from "/Resources/Hat"  in this case
        shopUI.SetActive(true);
        totalFish.text = SaveManager.Instance.save.Fish.ToString("00");


        if (!isInit)
        {
            totalFish.text = SaveManager.Instance.save.Fish.ToString("00");
            currentHatName.text = hats[SaveManager.Instance.save.CurrentHatIndex].ItemName;
            PopulateShop();
            isInit = true;
        }
        ResetCompletionCircle();

    }
    public override void Destruct()
    {
        shopUI.SetActive(false);
    }
    private void PopulateShop()
    {
        for (int i = 0; i < hats.Length; i++)
        {
            int index = i;
            GameObject go = Instantiate(hatPrefab, hatContainer) as GameObject;
            // button
            go.GetComponent<Button>().onClick.AddListener(() => OnHatClick(index)); // adds listener to object , takes care of click action
            // thumbnail
            go.transform.GetChild(0).GetComponent<Image>().sprite = hats[index].Thumbnail; // gets the 2nd child (index 1 ) sprite from the object (the hat image UI )
            // item name
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = hats[index].ItemName;
            // price
            if (SaveManager.Instance.save.UnlockedHatFlag[i] == 0)
                go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = hats[index].ItemPrice.ToString();
            else
            {
                go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "owned";
                Image image = go.transform.GetComponent<Image>();
                image.color = Color.grey;
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
                unlockedHatsCount++;
                Debug.Log(unlockedHatsCount);
            }
        }

    }
    private void ResetCompletionCircle()
    {
        int hatCount = hats.Length - 1;// removes nonehat from amount
        int currentlyUnlockedCount = unlockedHatsCount - 1;
        completionCircle.fillAmount = (float)currentlyUnlockedCount / (float)hatCount;
        completionText.text = currentlyUnlockedCount + " / " + hatCount;
    }
    private void OnHatClick(int i)
    {
        // Debug.Log("hat " + i + " was clickd");
        if (SaveManager.Instance.save.UnlockedHatFlag[i] == 1)
        {
            SaveManager.Instance.save.CurrentHatIndex = i;
            currentHatName.text = hats[i].ItemName;
            hatLogic.SelectHat(i);
            SaveManager.Instance.Save();
        }
        // buy it 
        else if (hats[i].ItemPrice <= SaveManager.Instance.save.Fish)
        {
            SaveManager.Instance.save.Fish -= hats[i].ItemPrice;
            SaveManager.Instance.save.UnlockedHatFlag[i] = 1;
            SaveManager.Instance.save.CurrentHatIndex = i;

            currentHatName.text = hats[i].ItemName;
            hatLogic.SelectHat(i);
            totalFish.text = SaveManager.Instance.save.Fish.ToString("00");
            SaveManager.Instance.Save();
            hatContainer.GetChild(i).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "owned";
            unlockedHatsCount++;
            ResetCompletionCircle();
        }
        else
        {
            Debug.Log("Not enough fish =(");
        }


    }
    public void OnHomeClick()
    {
        brain.ChangeState(GetComponent<GameStateInit>());
    }
}
