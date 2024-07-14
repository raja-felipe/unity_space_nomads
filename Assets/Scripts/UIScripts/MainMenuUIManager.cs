using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject defaultPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        InitializeCanvases();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Internal Functions to Edit
    // Internal Functions to Edit
    private void SetActiveControls(bool state)
    {
        controlPanel.SetActive(state);
        settingsPanel.SetActive(settingsPanel.activeSelf ? !state : settingsPanel.activeSelf);
        // defaultPanel.SetActive(defaultPanel.activeSelf ? !state : state);
        defaultPanel.SetActive(!state);
    }

    private void SetActiveSettings(bool state)
    {
        settingsPanel.SetActive(state);
        controlPanel.SetActive(controlPanel.activeSelf ? !state : controlPanel.activeSelf);
        // defaultPanel.SetActive(defaultPanel.activeSelf ? !state : state);
        defaultPanel.SetActive(!state);
    }

    private void SetActiveDefault(bool state)
    {
        defaultPanel.SetActive(state);
        controlPanel.SetActive(controlPanel.activeSelf ? !state : controlPanel.activeSelf);
        settingsPanel.SetActive(settingsPanel.activeSelf ? !state : settingsPanel.activeSelf);
    }

    private void InitializeCanvases()
    {
        defaultPanel.SetActive(true);
        controlPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
    
    // External functions to open 
    public void GoToControls()
    {
        SetActiveControls(!controlPanel.activeSelf);
    }

    public void GoToSettings()
    {
        SetActiveSettings(!settingsPanel.activeSelf);
    }

    public void GoToDefault()
    {
        SetActiveDefault(!defaultPanel.activeSelf);
    }
}