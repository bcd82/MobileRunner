using UnityEngine;

[CreateAssetMenu(fileName = "Hat")] // allows to create this from the unity Create-> menu !!
public class Hat : ScriptableObject // can't be an object component
{
    public string ItemName;
    public int ItemPrice;
    public Sprite Thumbnail;
    public GameObject Model;

}
