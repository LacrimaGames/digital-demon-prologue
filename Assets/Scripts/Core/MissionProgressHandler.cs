using System.Collections.Generic;
using UnityEngine;

namespace DD.Core
{
    public class MissionProgressHandler : MonoBehaviour
    {
        private GameObject backToMenuScreen;
        private List<GameObject> missionObjectives = new List<GameObject>();

        public static MissionProgressHandler instance;

        private void Awake()
        {
            // Ensure there is only one instance of LevelLoader
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogWarning("Multiple instances of LevelLoader found. Destroying this instance.");
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (backToMenuScreen == null)
            {
                backToMenuScreen = GameObject.FindGameObjectWithTag("Back To Menu");
            }

            backToMenuScreen.SetActive(false);

            foreach (var item in GameObject.FindGameObjectsWithTag("MissionObjective"))
            {
                missionObjectives.Add(item);
            }

        }
        private void Update()
        {
            if (EnemySpawner.Instance.GetCurrentKillCount() >= EnemySpawner.Instance.GetMaxEnemiesThisMission())
            {
                Debug.Log("You won");
                Destroy(EnemySpawner.Instance);
                backToMenuScreen.SetActive(true);
            }

            if (missionObjectives.Count <= 0)
            {
                Debug.Log("You Lost");
                Destroy(EnemySpawner.Instance);
                backToMenuScreen.SetActive(true);
            }
        }

        public void MissionObjectiveDestroyed(GameObject objective)
        {
            missionObjectives.Remove(objective);
        }

        public void GoBackToMenu()
        {
            LevelLoader.Instance.LoadScene(0);
        }
    }
}


