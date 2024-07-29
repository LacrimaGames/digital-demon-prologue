using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DD.Core
{
    public class LevelLoader : MonoBehaviour
    {
        public static LevelLoader Instance { get; private set; }

        public List<GameObject> levels = new List<GameObject>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        // Load a scene by build index
        public void LoadScene(int sceneBuildIndex)
        {
            SceneManager.LoadScene(sceneBuildIndex);
        }

        public void LevelCleared(int level, bool state)
        {
            if(state == true && levels.Count >= level)
            {
                levels[level].SetActive(true);
            }
        }

    }

}

