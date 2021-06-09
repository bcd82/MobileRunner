
using UnityEngine;

public enum GameCamera
{
    Init = 0,
    Game = 1,
    Shop = 2,
    Respawn = 3
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return instance; } }
    private static GameManager instance;

    private GameState state;

    public PlayerMotor motor;
    public WorldGeneration worldGeneration;
    public SceneChunkGeneration sceneChunkGeneration;
    public GameObject[] cameras;


    private void Start()
    {
        instance = this;
        state = GetComponent<GameStateInit>();
        state.Construct();
    }
    private void Update()
    {
        state.UpdateState();
    }
    public void ChangeState(GameState s)
    {
        state.Destruct();
        state = s;
        state.Construct();
    }

    public void ChangeCamera(GameCamera c)
    {
        foreach (GameObject go in cameras)
        {
            go.SetActive(false); // sets all cameras to false
        }
        cameras[(int)c].SetActive(true); // sets the defined Enum camera to active
    }
}
