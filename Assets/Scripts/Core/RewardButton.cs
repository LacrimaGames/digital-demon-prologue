using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace DD.Core
{
    public class RewardButton : MonoBehaviour
    {
        public enum RewardTypes
        {
            PlayerDamage,
            PlayerAttackSpeed,
            PlayerGatherSpeed,
            PlayerUnloadSpeed,
            PlayerMovementSpeed,
            PlayerHealth,
            TowerDamage,
            TowerAttackSpeed,
            TowerDetectionRadius,
            TowerHealth,
            FriendlyAiMovementSpeed,
            FriendlyAiGatherSpeed,
            FriendlyAiUnloadSpeed
        }

        [Serializable]
        public class Rewards
        {
            public RewardTypes reward;
            public int rewardAmount;
        }

        public Rewards rewardThisTurn;
        [Range(1, 3)]
        public int rewardIndex;

        public GameObject backTomenuButton;



        // Start is called before the first frame update
        void Start()
        {
            if (LevelModifier.instance.noRewardsThisTurn)
            {
                backTomenuButton.SetActive(true);
                Destroy(gameObject);
                return;
            }

            switch (rewardIndex)
            {
                case 1:
                    rewardThisTurn = LevelModifier.instance.LoadReward1();
                    break;
                case 2:
                    rewardThisTurn = LevelModifier.instance.LoadReward2();
                    break;
                case 3:
                    rewardThisTurn = LevelModifier.instance.LoadReward3();
                    break;
            }

            backTomenuButton.SetActive(false);
            string rewardText = Regex.Replace(rewardThisTurn.reward.ToString(), "([A-Z])", " $1").Trim();
            string rewardamountText = rewardThisTurn.rewardAmount.ToString(); ;
            if (rewardThisTurn.reward == RewardTypes.PlayerAttackSpeed || rewardThisTurn.reward == RewardTypes.TowerAttackSpeed)
            {
                rewardamountText = rewardThisTurn.rewardAmount.ToString() +"%";
            }
            GetComponentInChildren<TMP_Text>().text = rewardText + " +" + rewardamountText;
        }

        public void ApplyBonuses()
        {
            if (rewardThisTurn.rewardAmount != 0)
            {
                MissionProgressHandler.instance.ApplyBonuses(rewardThisTurn.reward, rewardThisTurn.rewardAmount);
            }
            backTomenuButton.SetActive(true);
        }
    }
}
