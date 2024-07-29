using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DD.Core
{
    public class MissionProgressHandler : MonoBehaviour
    {
        private GameObject backToMenuScreen;
        private List<GameObject> missionObjectives = new List<GameObject>();
        private bool objectiveIsBuilt = false;

        public static MissionProgressHandler instance;

        public int level;

        private bool levelCleared = false;


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
            if (EnemySpawner.Instance.GetCurrentKillCount() >= EnemySpawner.Instance.GetMaxEnemiesThisMission() && objectiveIsBuilt)
            {
                Debug.Log("You won");
                Destroy(EnemySpawner.Instance);
                backToMenuScreen.transform.GetChild(2).GetComponent<TMP_Text>().text = "You won!";
                backToMenuScreen.SetActive(true);
                levelCleared = true;
            }

            if (missionObjectives.Count <= 0)
            {
                Debug.Log("You Lost");
                Destroy(EnemySpawner.Instance);
                backToMenuScreen.transform.GetChild(2).GetComponent<TMP_Text>().text = "You lost";
                backToMenuScreen.SetActive(true);
                levelCleared = false;
            }
        }

        public void MissionObjectiveDestroyed(GameObject objective)
        {
            missionObjectives.Remove(objective);
        }

        public void GoBackToMenu()
        {
            LevelLoader.Instance.LevelCleared(level, levelCleared);
            LevelLoader.Instance.LoadScene(0);
        }

        public void FinishObjective()
        {
            objectiveIsBuilt = true;
        }
    }
}


