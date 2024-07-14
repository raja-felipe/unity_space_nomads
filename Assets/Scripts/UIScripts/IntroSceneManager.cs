using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class IntroSceneManager : MonoBehaviour
{
    private float timeStart;
    private float timeLen = 5f;
    public bool firstSpacePressed = false;
    [SerializeField] private GameObject skipText;
    [SerializeField] private SceneTransition transition;
    [SerializeField] private PlayableDirector director;
    
    // Start is called before the first frame update
    void Start()
    {
        skipText.SetActive(false);
        timeStart = Time.timeSinceLevelLoad;
        director.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > timeStart + timeLen)
        {
            skipText.SetActive(true);
        }

        if (firstSpacePressed)
        {

            if (Input.GetKeyDown(KeyCode.Space) || !director.state.Equals(PlayState.Playing))
            {
                transition.GotoMenuScene();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                firstSpacePressed = true;
                director.Resume();
            }
        }

        // Check to skip the cutscene
    }
}
