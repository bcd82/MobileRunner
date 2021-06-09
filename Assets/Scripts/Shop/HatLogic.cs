using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatLogic : MonoBehaviour
{
    [SerializeField] private Transform hatContainer; //position to put the hat on
    private List<GameObject> hatModels = new List<GameObject>(); // "= new List<GameObject>();" instantiates the list 
    private Hat[] hats;
    private void Start()
    {
        hats = Resources.LoadAll<Hat>("Hat");
        SpawnHats();
        SelectHat(SaveManager.Instance.save.CurrentHatIndex);
    }
    private void SpawnHats()
    {
        for (int i = 0; i < hats.Length; i++)
        {
            hatModels.Add(Instantiate(hats[i].Model, hatContainer) as GameObject); // instatiates the hads and keeps a refernece to it in a list
        }
    }
    public void DisableAllHats()
    {
        for (int i = 0; i < hatModels.Count; i++)
        {
            hatModels[i].SetActive(false);
        }
    }
    public void SelectHat(int index)
    {
        DisableAllHats();
        hatModels[index].SetActive(true);
    }
}
