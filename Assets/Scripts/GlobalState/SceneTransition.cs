// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022
using UnityEngine;

public class SceneTransition : GlobalSceneManagerClient
{
    public void GotoGameScene(float delay = 0f)
    {
        Debug.Log("LOADING GAME SCENE");
        // Make sure the camera in the Game Scene uses the "NoSkybox" material.
        // Camera.main.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
        // Camera.main.GetComponent<Camera>().backgroundColor = Color.white; // Set the background color if needed.
        StartCoroutine(GlobalSceneManager.manager.GotoScene(GlobalSceneManager.GameSceneName, delay));
        manager.playInGameMusic();
    }

    public void GotoMenuScene(float delay = 0f)
    {
        Debug.Log("LOADING MAIN MENU SCENE");
        if (GlobalSceneManager.SelectedGuns != null) GlobalSceneManager.ResetUpgrades();
        // Camera.main.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        // Camera.main.GetComponent<Camera>().backgroundColor = Color.black; // Set the background color if needed.
        StartCoroutine(GlobalSceneManager.manager.GotoScene(GlobalSceneManager.MenuSceneName, delay));
        GlobalSceneManager.manager.playMenuMusic();
        GlobalSceneManager.ResetDictionariesAndCounts(true);
    }

    public void GoToDeathScene(float delay = 0f)
    {
        Debug.Log("LOADING DEATH SCENE");
        GlobalSceneManager.ResetUpgrades();
        GlobalSceneManager.manager.playInGameMusic();
        StartCoroutine(GlobalSceneManager.manager.GotoScene(GlobalSceneManager.MenuSceneName, delay));
    }

    public void GoToLevelPrototypeScene(float delay = 0f)
    {
        Debug.Log("LOADING PROTOTYPE SCENE");
        StartCoroutine(GlobalSceneManager.manager.GotoScene(GlobalSceneManager.LevelPrototypeSceneName, delay));
    }

    public void GoToRoomCreactionScene(float delay = 0f)
    {
        Debug.Log("ROOM CREACTION SCENE");
        GlobalSceneManager.ResetUpgrades();
        /*Debug.Log(GlobalSceneManager.SelectedGuns[0] != null);
        Debug.Log(GlobalSceneManager.SelectedGuns[1] != null);*/
        StartCoroutine(GlobalSceneManager.manager.GotoScene(GlobalSceneManager.RoomCreactionName, delay));
        /*GlobalSceneManager.ResetDictionariesAndCounts(resetWeapons);
        GlobalSceneManager.Retries++;*/
        GlobalSceneManager.manager.playInGameMusic();
    }
    
    public void GoToMapReDoScene(float delay = 0f)
    {
        Debug.Log("MAP RE DO SCENE");
        if (GlobalSceneManager.SelectedGuns != null) GlobalSceneManager.ResetUpgrades();
        GlobalSceneManager.ResetDictionariesAndCounts(false);
        StartCoroutine(manager.GotoScene(GlobalSceneManager.MapReDoSceneName, delay));
        GlobalSceneManager.manager.playInGameMusic();
    }

    public void GoToVictoryScene(float delay = 0f)
    {
        Debug.Log("GAME WAS WON");
        GlobalSceneManager.ResetUpgrades();
        StartCoroutine(GlobalSceneManager.manager.GotoScene(GlobalSceneManager.VictorySceneName, delay));
        GlobalSceneManager.manager.playInGameMusic();
    }

    public void GoToGunSelectionScene(float delay = 0f) 
    {
        Debug.Log("LOADING GUN SELECTION SCENE");
        if (GlobalSceneManager.SelectedGuns != null) GlobalSceneManager.ResetUpgrades();
        // Camera.main.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        // Camera.main.GetComponent<Camera>().backgroundColor = Color.black; // Set the background color if needed.
        GlobalSceneManager.ResetDictionariesAndCounts(true);
        StartCoroutine(GlobalSceneManager.manager.GotoScene(GlobalSceneManager.GunSelectionSceneName, delay));
        GlobalSceneManager.manager.playMenuMusic();
    }

    // Quit Button
    public void QuitGame()
    {
        Debug.Log("QUITTING GAME");
        Application.Quit();
    }
}
