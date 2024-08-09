using DD.Builder.Buildings;
using UnityEngine;

namespace DD.Builder
{
    public class TowerPrioritySetter : MonoBehaviour
    {
        public void SetPriorityLowestHealthEnemy()
        {
            GetComponent<Tower>().attackPriority = Tower.Priority.LowestHealth;
        }

        public void SetPriorityHighestTierEnemy()
        {
            GetComponent<Tower>().attackPriority = Tower.Priority.HighestTier;

        }

        public void SetPriorityClosestEnemy()
        {
            GetComponent<Tower>().attackPriority = Tower.Priority.Closest;
        }
    }
}
