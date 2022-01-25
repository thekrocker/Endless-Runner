using UnityEngine;

namespace _Project.Scripts
{
    public class Chunk : MonoBehaviour
    {
        public int chunkLength = 20;

        public Chunk Show()
        {
            gameObject.SetActive(true);
            return this;
        }
        public Chunk Hide()
        {
            gameObject.SetActive(false);
            return this;
        }

    }
}