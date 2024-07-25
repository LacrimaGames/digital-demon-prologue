using DD.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    public SceneAsset sceneToLoad;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(LoadLevel);
    }

    void LoadLevel()
    {
        LevelLoader.Instance.LoadScene(sceneToLoad.name);
    }

}
