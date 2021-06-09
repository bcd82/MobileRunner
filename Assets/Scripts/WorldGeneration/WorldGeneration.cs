using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    
    // Gameplay
    private float chunkSpawnZ; // Z of last chunk 
    private Queue<Chunk> activeChunks = new Queue<Chunk>(); // will contain a Queue of active chunks! has an indexed order?
    private List<Chunk> chunkPool = new List<Chunk>(); // contains a pool of the chunks for re-use ( setting active true/false )

    public int chunksCreated;
    
    // configurable fields
    [SerializeField] private int firstChunkSpawnPosition = -10;
    [SerializeField] private int chunkOnScreen = 5; // detrmines max amount 
    [SerializeField] private float despawnDistance = 5.0f; // how far before despawning

    [SerializeField] private List<GameObject> chunkPrefab; // contains the list of available chunk prefabs
    [SerializeField] private List<GameObject> chunkPrefab_2; // contains the list of available chunk prefabs
    [SerializeField] Transform cameraTransform; //  camera position
    

    
    private void Awake()
    {
        ResetWorld();
    }
    
    private void Start()
    {

        // check if we have an empty chunk prefab list 
        if (chunkPrefab.Count == 0)
        {
            Debug.LogError("no chunks found in world generator. please assign chunks");
            return;
        }
        // try to assign the camera ( if not assigned )
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("camera assigned automatically");
        }

    }

    public void ScanPosition() // checking camera position , gets called from GameStateGame now instead of update here

    {

        float cameraZ = cameraTransform.position.z;
        Chunk lastChunk = activeChunks.Peek(); // gets last element in que
        // checks if (camera z position is >= bigger or equal to Length of the last chunk + despawn length we defined ) {...}
        if (cameraZ >= lastChunk.transform.position.z + lastChunk.chunkLength + despawnDistance)
        {
            SpawnNewChunk();
            DeleteLastChunk();
        }
    }
    private void SpawnNewChunk() // if position is far enough from last chunk spawn new chunk
    {
        List<GameObject> chunksToSpawn;
        chunksToSpawn = chunksCreated < chunkOnScreen ? chunkPrefab : chunkPrefab_2;
        // get a random index for which prefab to spawn
        int randomIndex = Random.Range(0, chunksToSpawn.Count);

        // does it alreaedy exist withing generated prefab pool ? 
        Chunk chunk = chunkPool.Find(x => !x.gameObject.activeSelf && x.name == (chunksToSpawn[randomIndex].name + "(Clone)")); // goes through the list checks which one is not active and returns objects

        // if prefab is not in reuse pool , create a chunk

        if (!chunk)
        {
            GameObject go = Instantiate(chunksToSpawn[randomIndex], transform); // assigning transform as a parent will instantiate the object nested in the object containing this script
            chunk = go.GetComponent<Chunk>(); //gets the Chunk component from the created chunk
        }
        // place object and show it
        chunk.transform.position = new Vector3(0, 0, chunkSpawnZ); // moves chunk to spawn position
        chunkSpawnZ += chunk.chunkLength; // next spawn position
        // store value to reuse in our pool
        activeChunks.Enqueue(chunk); // adds chunk to active chunks pool 
        chunk.ShowChunk(); // sets chunk to active (in chunk script)
        chunksCreated++;

    }
    private void DeleteLastChunk() // disables chunks
    {
        Chunk chunk = activeChunks.Dequeue(); // gets chunk reference for the OLDEST cunk in queue  to be used below, and Removes it from queue!
        chunk.HideChunk();
        chunkPool.Add(chunk); // adds chunk to chunkPool
    }
    public void ResetWorld()
    {
        // reset the chunkSpawn Z 
        chunksCreated = 0;
        FishManager.Instance.ResetFish();

        chunkSpawnZ = firstChunkSpawnPosition;
        // delete all chunk from the list 
        for (int i = activeChunks.Count; i != 0; i--) // goes backwords in list 
        {
            DeleteLastChunk(); // uses function created above
        }
        for (int i = 0; i < chunkOnScreen; i++) // spwans new random chunks
        {
            SpawnNewChunk();
        }

    }// reset the scene
}
