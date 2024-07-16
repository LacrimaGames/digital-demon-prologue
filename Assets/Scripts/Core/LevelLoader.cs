using UnityEngine;
using UnityEngine.SceneManagement;

namespace DD.Core
{
    public class LevelLoader : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        // Load a scene by build index
        public void LoadScene(int sceneBuildIndex)
        {
            SceneManager.LoadScene(sceneBuildIndex);
        }
    }
}

