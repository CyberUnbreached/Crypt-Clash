using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        GameObject volumeObject = GameObject.Find("PostProcessingVolume");

        if (volumeObject == null)
        {
            Debug.LogWarning("[GameManager] PostProcessingVolume GameObject not found in this scene.");
            return;
        }

        Volume volumeInScene = volumeObject.GetComponent<Volume>();
        if (volumeInScene == null)
        {
            Debug.LogWarning("[GameManager] Volume component missing on PostProcessingVolume GameObject.");
            return;
        }

        if (SettingsMenu.Instance != null)
        {
            SettingsMenu.Instance.UpdateSceneReferences(volumeInScene);
        }
        else
        {
            Debug.LogWarning("[GameManager] SettingsMenu.Instance is null.");
        }
    }
}
