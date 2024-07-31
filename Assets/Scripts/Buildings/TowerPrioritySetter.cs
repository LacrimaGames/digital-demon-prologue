using System;
using System.Collections;
using DD.Builder.Buildings;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace DD.Bilder
{
    public class TowerPrioritySetter : MonoBehaviour
    {
        public GameObject tooltipGroup;
        public GameObject tooltipButton;
        public GameObject lowestHealth;
        public GameObject highestTier;
        public GameObject cloestEnemy;

        public AnimationClip openClip;
        public AnimationClip closeClip;

        public LayerMask tooltipMask;
        public GameObject buttonPreview;

        private GameObject previousPreview;

        private void Start()
        {
            tooltipButton.SetActive(true);
            tooltipButton.transform.rotation = Camera.main.transform.rotation;
            GameObject prefab = null;

            switch (GetComponent<Tower>().attackPriority)
            {
                case Tower.Priority.LowestHealth:
                    prefab = lowestHealth.transform.GetChild(0).gameObject;
                    break;
                case Tower.Priority.HighestTier:
                    prefab = highestTier.transform.GetChild(0).gameObject;
                    break;
                case Tower.Priority.Closest:
                    prefab = cloestEnemy.transform.GetChild(0).gameObject;
                    break;
            }

            previousPreview = Instantiate(prefab, buttonPreview.transform.position, buttonPreview.transform.rotation, buttonPreview.transform);
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tooltipMask))
            {
                if (hit.collider.gameObject == tooltipButton && Input.GetMouseButtonDown(0))
                {
                    if (lowestHealth.activeSelf)
                    {
                        // tooltipGroup.SetActive(false);
                        CloseTooltip();
                    }
                    else
                    {
                        // tooltipButton.GetComponent<Animation>().clip = openClip;
                        tooltipButton.GetComponent<Animation>().Play();
                    }
                }

                if (hit.collider.gameObject == lowestHealth && Input.GetMouseButtonDown(0))
                {
                    GetComponent<Tower>().attackPriority = Tower.Priority.LowestHealth;
                    SetupPreview(lowestHealth);
                    CloseTooltip();
                }
                if (hit.collider.gameObject == highestTier && Input.GetMouseButtonDown(0))
                {
                    GetComponent<Tower>().attackPriority = Tower.Priority.HighestTier;
                    SetupPreview(highestTier);
                    CloseTooltip();
                }
                if (hit.collider.gameObject == cloestEnemy && Input.GetMouseButtonDown(0))
                {
                    GetComponent<Tower>().attackPriority = Tower.Priority.Closest;
                    SetupPreview(cloestEnemy);
                    CloseTooltip();
                }

                if (Input.GetKey(KeyCode.Escape))
                {
                    CloseTooltip();
                }
            }
        }

        private void SetupPreview(GameObject priorityPreview)
        {
            if (previousPreview != null) Destroy(previousPreview);
            GameObject prefab = priorityPreview.transform.GetChild(0).gameObject;
            previousPreview = Instantiate(prefab, buttonPreview.transform.position, buttonPreview.transform.rotation, buttonPreview.transform);
        }

        private void CloseTooltip()
        {
            tooltipButton.GetComponent<Animation>().clip = closeClip;
            tooltipButton.GetComponent<Animation>().Play();
            tooltipButton.GetComponent<Animation>().clip = openClip;
        }
    }
}
