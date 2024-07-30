using DD.Core;
using UnityEditor;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public SceneAsset sceneToLoad;
    private TextMesh textMesh;

    private void Start()
    {
        textMesh = GetComponentInChildren<TextMesh>();
        // transform.rotation = Camera.main.transform.rotation;
        // textMesh.text = sceneToLoad.name;
    }

    private void OnMouseDown()
    {
        LevelLoader.Instance.LoadScene(sceneToLoad.name);
    }

}
