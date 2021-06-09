using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get { return instance; } }
    private static SaveManager instance;

    // Fields 
    public SaveState save;
    private const string saveFileName = "data.ss";
    private BinaryFormatter formatter; // will format the save file

    // Actions 
    public Action<SaveState> OnLoad;
    public Action<SaveState> OnSave;
    private void Awake()
    {
        formatter = new BinaryFormatter();
        instance = this;
        // try and load the previous save state
        Load();
    }
    public void Load()
    {
        //Debug.Log(Application.persistentDataPath);
        try
        {
            FileStream file = new FileStream(Application.persistentDataPath + saveFileName, FileMode.Open, FileAccess.Read); // added Application.persistentDataPath + for it to work on androids 
            save = formatter.Deserialize(file) as SaveState; // deserialize the save file if exists, and save it as SaveState
            // can also be written like " save = (SaveState)formatter.Deserialize(file);
            file.Close();
            OnLoad?.Invoke(save);
        }
        catch
        {
            Debug.Log("Save file not found , lets create one :( ");
            Save();
        }
    }
    public void Save()
    {
        // if theres no prev state create new one 
        if (save == null)
            save = new SaveState();

        // set the time at which we've tried saving 
        save.LastSaveTime = DateTime.Now;

        // open a file on our system and write to it 
        FileStream file = new FileStream(Application.persistentDataPath + saveFileName, FileMode.OpenOrCreate, FileAccess.Write);// added Application.persistentDataPath + for it to work on androids 
        formatter.Serialize(file, save);
        file.Close();

        OnSave?.Invoke(save);

    }
}
