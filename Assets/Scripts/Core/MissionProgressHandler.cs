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

        public int bonusHealth = 100;


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

        public void ApplyBonuses(RewardButton.RewardTypes rewardType, int amount)
        {
            if (levelCleared)
            {
                switch (rewardType)
                {
                    case RewardButton.RewardTypes.PlayerDamage:
                        GlobalModifiers.instance.playerModifiers.attackDamage += amount;
                        break;
                    case RewardButton.RewardTypes.PlayerAttackSpeed:
                        float playerAttackspeedAmountFromPercentage = 1f / (float)amount;
                        GlobalModifiers.instance.playerModifiers.fireRate += playerAttackspeedAmountFromPercentage;
                        break;
                    case RewardButton.RewardTypes.PlayerGatherSpeed:
                        GlobalModifiers.instance.playerModifiers.gatheringSpeedPerSecond -= amount/20f;
                        break;
                    case RewardButton.RewardTypes.PlayerUnloadSpeed:
                        GlobalModifiers.instance.playerModifiers.unloadSpeedPerSecond -= amount /10f;
                        break;
                    case RewardButton.RewardTypes.PlayerMovementSpeed:
                        GlobalModifiers.instance.playerModifiers.movementSpeed += amount;
                        break;
                    case RewardButton.RewardTypes.PlayerHealth:
                        GlobalModifiers.instance.playerModifiers.health += amount;
                        break;
                    case RewardButton.RewardTypes.TowerDamage:
                        GlobalModifiers.instance.tier1TowerModifiers.damage += amount;
                        GlobalModifiers.instance.tier2TowerModifiers.damage += amount;
                        GlobalModifiers.instance.tier3TowerModifiers.damage += amount;
                        break;
                    case RewardButton.RewardTypes.TowerAttackSpeed:
                        float towerAttackSpeedAmountFromPercentage = 1f / (float)amount;
                        GlobalModifiers.instance.tier1TowerModifiers.fireRate += towerAttackSpeedAmountFromPercentage;
                        GlobalModifiers.instance.tier2TowerModifiers.fireRate += towerAttackSpeedAmountFromPercentage;
                        GlobalModifiers.instance.tier3TowerModifiers.fireRate += towerAttackSpeedAmountFromPercentage;
                        break;
                    case RewardButton.RewardTypes.TowerDetectionRadius:
                        GlobalModifiers.instance.tier1TowerModifiers.detectionRadius += amount;
                        GlobalModifiers.instance.tier2TowerModifiers.detectionRadius += amount;
                        GlobalModifiers.instance.tier3TowerModifiers.detectionRadius += amount;
                        break;
                    case RewardButton.RewardTypes.TowerHealth:
                        GlobalModifiers.instance.tier1TowerModifiers.health += amount;
                        GlobalModifiers.instance.tier2TowerModifiers.health += amount;
                        GlobalModifiers.instance.tier3TowerModifiers.health += amount;
                        break;
                    case RewardButton.RewardTypes.FriendlyAiMovementSpeed:
                        GlobalModifiers.instance.friendlyAIModifiers.movementSpeed += amount;
                        break;
                    case RewardButton.RewardTypes.FriendlyAiGatherSpeed:
                        GlobalModifiers.instance.friendlyAIModifiers.gatheringSpeedPerSecond -= amount / 20f;
                        break;
                    case RewardButton.RewardTypes.FriendlyAiUnloadSpeed:
                        GlobalModifiers.instance.friendlyAIModifiers.unloadSpeedPerSecond -= amount / 10f;
                        break;
                }
            }
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


