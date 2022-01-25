using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.WorldGeneration
{
    public class WorldGeneration : MonoBehaviour
    {
        //Gameplay
        private int _chunkSpawnZ;
        private Queue<Chunk> activeChunks = new Queue<Chunk>();
        private List<Chunk> chunkPool = new List<Chunk>();
        
        //Confs
        [SerializeField] private int firstChunkSpawnPos = -10;
        [SerializeField] private int chunkOnScreen = 3;
        [SerializeField] private int chunkDespawnDistance;

        [SerializeField] private List<GameObject> chunkPrefabs;
        [SerializeField] private Transform cameraTransform;
        
        
        
        private void Start()
        {
            if (chunkPrefabs.Count == 0)
            {
                Debug.LogError("There is no assigned chunk prefab inside inspector. Please assign them!");
                return;
            }

            if (cameraTransform) return;
            cameraTransform = Camera.main.transform;
            Debug.LogWarning("Camera was not assigned, it assigned automatically.");
        }

        private void Update()
        {
            ScanPosition();
        }

        private void ScanPosition()
        {
            float camZ = cameraTransform.position.z;
            Chunk lastChunk = activeChunks.Peek();
            
            if (camZ >= lastChunk.transform.position.z + lastChunk.chunkLength + chunkDespawnDistance)
            {
                SpawnNewChunk();
                RemoveLastChunk();
            }
        }

        private void SpawnNewChunk()
        {
            // Gets random chunk from list
            var rndChunk = Random.Range(0, chunkPrefabs.Count);

            //Does it already exist in our pool
            Chunk chunk = null;

            //Create a chunk, if we were not able to find one to reuse
            if (!chunk)
            {
                var go = Instantiate(chunkPrefabs[rndChunk], transform);
                chunk = go.GetComponent<Chunk>();
            }
            
            //Place the object and show it, and we add an offset for chunkSpawnZ to create recursive chunks
            chunk.transform.position = new Vector3(0, 0, _chunkSpawnZ);
            _chunkSpawnZ += chunk.chunkLength;

            //Store the value to reuse in our pool
            activeChunks.Enqueue(chunk);
            chunk.Show();


        }

        private void RemoveLastChunk()
        {
            // Takes the oldest chunk and remove from queue
            Chunk chunk = activeChunks.Dequeue();
            chunk.Hide();
            
            // Add to the pool to reuse later.
            chunkPool.Add(chunk);
        }

        private void ResetWorld()
        {
            // Reset the ChunkSpawnZ
            _chunkSpawnZ = firstChunkSpawnPos;

            // Remove every chunk
            for (int i = activeChunks.Count; i != 0; i--) RemoveLastChunk(); 
            
            //Spawn new chunks with given chunk on screen value
            for (int i = 0; i < chunkOnScreen; i++) SpawnNewChunk();
            
            
        }
    }
}
