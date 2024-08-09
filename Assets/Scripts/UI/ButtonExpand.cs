using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DD.UI
{
    public class ButtonExpand : MonoBehaviour
    {
        public GameObject buttonGroup;
        public GameObject mainButton;

        public AnimationClip openClip;
        public AnimationClip closeClip;

        public LayerMask tooltipMask;
        public GameObject buttonPreview;

        private GameObject previousPreview;

        private bool buttonExpanded = false;

        public List<UnityEvent> Events = new List<UnityEvent>();

        private List<GameObject> buttons = new List<GameObject>();

        private Animation animationPlayer;

        private bool replacePreview = true;


        private void Start()
        {

            if (mainButton == null)
            {
                mainButton = transform.gameObject;
            }

            mainButton.SetActive(true);
            animationPlayer = GetComponent<Animation>();

            animationPlayer.AddClip(openClip, openClip.name);
            animationPlayer.AddClip(closeClip, closeClip.name);
            
            if(buttonGroup == null)
            {
                buttonGroup = transform.GetChild(0).gameObject;
            }

            foreach (Transform button in buttonGroup.transform)
            {
                buttons.Add(button.gameObject);
                button.gameObject.SetActive(false);
            }

            if(buttonPreview == null)
            {
                replacePreview = false;
            }

            if(replacePreview)
            {
                SetupPreview(buttons[0].transform.GetChild(0).gameObject);
            }
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tooltipMask))
            {
                if (hit.collider.gameObject == mainButton && Input.GetMouseButtonDown(0))
                {
                    if (buttonExpanded)
                    {
                        CollapseButton();
                    }
                    else
                    {
                        ExpandButton();
                    }
                }

                if (hit.collider.gameObject == buttons[0] && Input.GetMouseButtonDown(0))
                {
                    Event1();
                    CollapseButton();
                }
                if (hit.collider.gameObject == buttons[1] && Input.GetMouseButtonDown(0)) // There will always be 2 buttons
                {
                    Event2();

                    CollapseButton();
                }
                if (buttons.Count >= 3 && hit.collider.gameObject == buttons[2] && Input.GetMouseButtonDown(0))
                {
                    Event3();
                    CollapseButton();
                }

                if (Input.GetKey(KeyCode.Escape))
                {
                    CollapseButton();
                }
            }
            else if(!Physics.Raycast(ray, out hit, Mathf.Infinity, tooltipMask) && Input.GetMouseButtonDown(0) && buttonExpanded)
            {
                CollapseButton();
            }
        }

        private void SetupPreview(GameObject priorityPreview)
        {
            if(!replacePreview || buttonPreview == null) return;
            if (previousPreview != null) Destroy(previousPreview);
            GameObject prefab = priorityPreview.transform.GetChild(0).gameObject;
            previousPreview = Instantiate(prefab, buttonPreview.transform.position, buttonPreview.transform.rotation, buttonPreview.transform);
        }

        public void ExpandButton()
        {
            buttonExpanded = true;
            animationPlayer.clip = openClip;
            animationPlayer.Play();
        }

        public void CollapseButton()
        {
            buttonExpanded = false;
            animationPlayer.clip = closeClip;
            animationPlayer.Play();
        }

        public void Event1()
        {
            Events[0].Invoke();
            SetupPreview(buttons[0].transform.GetChild(0).gameObject);
        }

        public void Event2()
        {
            Events[1].Invoke();

            SetupPreview(buttons[1].transform.GetChild(0).gameObject);

        }

        public void Event3()
        {
            Events[2].Invoke();
            SetupPreview(buttons[2].transform.GetChild(0).gameObject);
        }
    }
}
