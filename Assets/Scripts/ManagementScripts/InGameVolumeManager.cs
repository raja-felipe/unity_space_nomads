using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameVolumeManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject gunVolumeParent;
    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private string enemyLayer = "Enemy";
    public List<GameObject> enemyObjects;
    void Start()
    {
        SetGunVolume();
        SetEnemyVolume();
        SetDialogueVolume();
    }

    // Update is called once per frame
    void Update()
    {
        SetGunVolume();
        SetEnemyVolume();
        SetDialogueVolume();
    }
    
    // Functions to set the volumes
    private void SetGunVolume()
    {
        SetObjectsVolume(gunVolumeParent);
    }

    /*private void SetEnemyVolume()
    {
        SetObjectsVolume(enemyVolumeParent);
    }*/
    
    
    private void SetObjectsVolume(GameObject parent)
    {
        AudioSource[] gunAudioSources = parent.GetComponentsInChildren<AudioSource>();

        foreach (AudioSource audioSrc in gunAudioSources)
        {
            audioSrc.volume = GlobalSceneManager.GunVolume;
        }
    }

    // Code to find objects in a given layer from scene root taken from:
    // https://discussions.unity.com/t/how-to-find-all-objects-in-specific-layer/30513/7
    private void SetEnemyVolume()
    {
        foreach (GameObject enemy in enemyObjects)
        {
            enemy.GetComponent<AudioSource>().volume = GlobalSceneManager.EnemyVolume;
        }
    }

    private void SetDialogueVolume()
    {
        AudioSource[] dialogueSources = dialogueObject.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource audioSrc in dialogueSources)
        {
            audioSrc.volume = GlobalSceneManager.DialogueVolume;
        }
    }
}
