// Code Used from Workshop 6

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SceneManager))]
public class GlobalSceneManager : MonoBehaviour
{
    // Constants to keep 
    public const string Tag = "GlobalSceneManager";
    public const string LevelPrototypeSceneName = "Level Prototype";
    public const string MenuSceneName = "MainMenuScene";
    public const string GameSceneName = "GameScene";
    public const string SetttingSceneName = "SettingsScene";
    public const string RoomCreactionName = "RoomCreaction";
    public const string VictorySceneName = "VictoryScene";
    public const string GunSelectionSceneName = "GunSelectionScene";
    public const string MapReDoSceneName = "MapReDo";
    public static Dictionary<string,KeyCode> inputLabelList = new Dictionary<string, KeyCode>();
    public static string FailText = "YOU FAILED";
    // Gun Selection
    public static int PrimaryGunIndex = 0;
    public static int SecondaryGunIndex = 1;
    public static int ToolGunIndex = 2;
    public static GunData[] SelectedGuns;
    public static GunData ToolGun;
    // Maps how many kills for each enemy type
    public static Dictionary<string, Sprite> GunImgsDict = new Dictionary<string, Sprite>();
    public static Dictionary<string, int> KillCounts = new Dictionary<string, int>();
    public static Dictionary<string, Sprite> EnemyImageDict = new Dictionary<string, Sprite>();
    public static Dictionary<string, int> TotalShots = new Dictionary<string, int>();
    public static Dictionary<string, float> ShotsHit = new Dictionary<string, float>();
    public static Dictionary<string, float> DamageDealt = new Dictionary<string, float>();
    public static int Retries = 0;
    public static float DamageTaken = 0;
    public static int BuildingsMade = 0;
    public static int GrenadesUsed = 0;
    public static string EndTime = "05:00";
    public static float EndTimeRatio = 1f;
    public static string EndTimePercent = "100%";
    public static bool firstTime = true;
    // public static int Accuracy = 0;
    
    // Use this to set different options
    public static float VolumeDefault = 0.7f;
    public static float VolumeMin = 0.0f;
    public static float VolumeMax = 1.0f;

    public static float GunVolume;
    public static float MusicVolume;
    public static float EnemyVolume;
    public static float DialogueVolume;
    
    public static float GunVolumeMult = 0.7f;
    public static float MusicVolumeMult = 0.5f;
    public static float EnemyVolumeMult = 0.4f;
    public static float DialogueVolumeMult = 1.7f;
    
    public static float SelectedSensitivity;
    public static float SensitivityDefault = 1f;
    public static float SensitivityMin = 0.0f;
    public static float SensitivityMax = 3.0f;
    
    public static float Healing;
    public static float HealingDefault = 1.0f;
    public static float HealingMin = 0.0f;
    public static float HealingMax = 2.0f;
    
    public static int DifficultySetting = 2;
    public static GlobalSceneManager manager;
    public AudioClip[] mainMenuMusic;
    public AudioClip[] inGameMusic;
    public AudioSource thisAudioSource;
    public bool playingMenuMusic = false;
    public bool playingInGameMusic = false;
    public Coroutine currentMusicRoutine;
    
    // Default control static variable labels
    public static string Forward = "Forward";
    public static KeyCode ForwardDefault = KeyCode.W;
    public static string Backward = "Backward";
    public static KeyCode BackwardDefault = KeyCode.S;
    public static string Left = "Left";
    public static KeyCode LeftDefault = KeyCode.A;
    public static string Right = "Right";
    public static KeyCode RightDefault = KeyCode.D;
    public static string Fire = "Fire";
    public static KeyCode FireDefault = KeyCode.Mouse0;
    public static string AltFire = "AltFire";
    public static KeyCode AltFireDefault = KeyCode.Mouse1;
    public static string Primary = "Primary";
    public static KeyCode PrimaryDefault = KeyCode.Alpha1;
    public static string Secondary = "Secondary";
    public static KeyCode SecondaryDefault = KeyCode.Alpha2;
    public static string Tool = "Tool";
    public static KeyCode ToolDefault = KeyCode.Alpha3;
    public static string Grenade = "Grenade";
    public static KeyCode GrenadeDefault = KeyCode.Alpha4;
    public static string Jump = "Jump";
    public static KeyCode JumpDefault = KeyCode.Space;
    public static string Menu = "Menu";
    public static KeyCode MenuDefault = KeyCode.Tab;
    public static string Crouch = "Crouch";
    public static KeyCode CrouchDefault = KeyCode.LeftControl;
    public static string Sprint = "Sprint";
    public static KeyCode SprintDefault = KeyCode.LeftShift;
    public static string Interact = "Interact";
    public static KeyCode InteractDefault = KeyCode.E;
    public static string Reload = "Reload";
    public static KeyCode ReloadDefault = KeyCode.R;

    private void Awake()
    {
        manager = this;
        // thisAudioSource = this.GetComponent<AudioSource>();
        // Should not be created if there's already a manager present (at least
        // two total, including ourselves). This allows us to place a game
        // manager in every scene, in case we want to open scenes direct.
        if (GameObject.FindGameObjectsWithTag(Tag).Length > 1)
        {
            Destroy(gameObject);   
        }
        /*inputLabelList.TryAdd("Sprint", KeyCode.LeftShift);
        inputLabelList.TryAdd("Interact", KeyCode.E);
        inputLabelList.TryAdd("Reload", KeyCode.R);*/
        SetDefaultControls();
        // Involves music and volume
        SetDefaultSettings();
        // manager.ApplyDefaultSettings();
        // Make this game object persistent even between scene changes.
        DontDestroyOnLoad(gameObject);

        // Hook into scene loaded events.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    public void Start()
    {
        thisAudioSource = transform.gameObject.GetComponent<AudioSource>();
    }

    public IEnumerator GotoScene(string sceneName, float delay) 
    {
        yield return new WaitForSeconds(delay);

        var asyncLoadOp = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoadOp.isDone)
        {
            yield return null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);

        // Add relevant functions to reset per new scene
    }
    
    // Functions for other settings mapping
    public static void SetDefaultSettings()
    {
        SelectedSensitivity = SensitivityDefault;
        GunVolume = EnemyVolume = MusicVolume = DialogueVolume = VolumeDefault;
        Healing = HealingDefault;
        Debug.Log($"SET VALUES FOR SETTINGS: {SelectedSensitivity} {Healing} {GunVolume} {EnemyVolume} {MusicVolume}");
    }

    public void ApplyDefaultSettings()
    {
        SetMusicVolume();
    }

    public void SetMusicVolume()
    {
        manager.thisAudioSource.volume = MusicVolume;
    }
    
    // Functions to map buttons to settings
    public static void SetDefaultControls()
    {
        // Add all the default controls
        inputLabelList.TryAdd(Forward, ForwardDefault);
        inputLabelList.TryAdd(Backward, BackwardDefault);
        inputLabelList.TryAdd(Left, LeftDefault);
        inputLabelList.TryAdd(Right, RightDefault);
        inputLabelList.TryAdd(Jump, JumpDefault);
        inputLabelList.TryAdd(Crouch, CrouchDefault);
        inputLabelList.TryAdd(Sprint, SprintDefault);
        inputLabelList.TryAdd(Fire, FireDefault);
        inputLabelList.TryAdd(AltFire, AltFireDefault);
        inputLabelList.TryAdd(Reload, ReloadDefault);
        inputLabelList.TryAdd(Primary, PrimaryDefault);
        inputLabelList.TryAdd(Secondary, SecondaryDefault);
        inputLabelList.TryAdd(Tool, ToolDefault);
        inputLabelList.TryAdd(Grenade, GrenadeDefault);
        inputLabelList.TryAdd(Menu, MenuDefault);
        inputLabelList.TryAdd(Interact, InteractDefault);
    }

    public static string CheckIfInControls(KeyCode input)
    {
        foreach (string key in inputLabelList.Keys)
        {
            if (inputLabelList[key].Equals(input)) return key;
        }

        return null;
    }
    
    // Functions to update GlobalSceneManager
    // NOTE: I don't know if I can chuck this in one function and have it run for every update,
    //       or if I will have to put this manually in every fire function
    public static void IncreaseKillCount(EnemyData enemy)
    {
        int value;
        int newVal = KillCounts.TryGetValue(enemy.enemyName, out value) ? value + 1 : 1;
        KillCounts.TryAdd(enemy.enemyName, newVal);
    }

    /*public static void AddEnemyImage(EnemyData enemy, Sprite enemyImg)
    {
        // Only add images in the dict for the first time enemy's encountered
        Sprite sprite;
        if (!EnemyImageDict.TryGetValue(enemy.name, out sprite))
        {
            EnemyImageDict.TryAdd(enemy.name, enemyImg);
        }
    }*/

    public static void AddGunImage(GunData gun)
    {
        Sprite sprite;
        if (!GunImgsDict.TryGetValue(gun.gunName, out sprite))
        {
            GunImgsDict.TryAdd(gun.gunName, gun.gunSprite);
        }
    }

    public static void AddTotalShots(string gunName, int shotCount)
    {
        int value;
        if (!gunName.Equals("ToolGun"))
        {
            TotalShots.TryGetValue(gunName, out value);
            TotalShots[gunName] = value + shotCount;
            //Debug.Log("New Total Shots for "+gunName+": "+TotalShots[gunName]);
        }
        else
        {
            AddBuildingsMade();
        }
    }
    
    public static void AddShotsHit(string gunName)
    {
        float value;
        ShotsHit.TryGetValue(gunName, out value);
        ShotsHit[gunName] = value + 1f;
        //Debug.Log("New Shots Hit for "+gunName+": "+ShotsHit[gunName]);
    }
    
    public static void AddDamageDealt(string gunName, float gunDamage)
    {
        float value;
        if (!gunName.Equals("ToolGun"))
        {
            DamageDealt.TryGetValue(gunName, out value);
            DamageDealt[gunName] = value + gunDamage;
            //Debug.Log("New Damage for "+gunName+": "+DamageDealt[gunName]);
        }
    }

    public static void AddEnemiesKilled(string enemyName)
    {
        int outVal;
        KillCounts.TryGetValue(enemyName, out outVal);
        KillCounts[enemyName] = outVal + 1;
        //Debug.Log("New Kill Count for "+enemyName+": "+KillCounts[enemyName]);
    }

    public static void AddGrenadesUsed()
    {
        GrenadesUsed++;
    }

    public static void AddDamageTaken(float damageDealt)
    {
        DamageTaken+=damageDealt;
    }

    public static void AddBuildingsMade()
    {
        BuildingsMade++;
        //Debug.Log("New Buildings Made: "+BuildingsMade);
    }
    
    // Other helper functions
    public static void ResetDictionariesAndCounts(bool resetWeapons)
    {
        KillCounts = new Dictionary<string, int>();
        // EnemyImageDict = new Dictionary<string, Sprite>();
        TotalShots = new Dictionary<string, int>();
        ShotsHit = new Dictionary<string, float>();
        DamageDealt = new Dictionary<string, float>();
        EnemyImageDict = new Dictionary<string, Sprite>();
        Retries = 0;
        DamageTaken = 0;
        BuildingsMade = 0;
        GrenadesUsed = 0;
        SelectedGuns = resetWeapons ? new GunData[2] : SelectedGuns;
        SelectedGuns[0] = resetWeapons ? null : SelectedGuns[0];
        SelectedGuns[1] = resetWeapons ? null : SelectedGuns[1];
    }

    public static void InitializeSelectedGuns()
    {
        
    }

    public static void SetEndTime(string finalTime)
    {
        EndTime = finalTime;
    }
    
    // Use this to unapply the gun upgrades for the selected guns
    public static void ResetUpgrades()
    {
        foreach (GunData gunData in SelectedGuns)
        {
            if (gunData != null)
            {
                foreach (GunUpgrade upgrade in gunData.currentUpgrades.Keys)
                {
                    //Debug.Log("UNAPPLYING: "+upgrade.name);
                    //Debug.Log("FROM GUN: "+gunData.gunName);
                    upgrade.unApply(gunData);
                }

                gunData.currentUpgrades = new Dictionary<GunUpgrade, bool>();
            }
        }
        /*if (gameManagerScript.manager != null)
        {
            foreach (GunData currData in gameManagerScript.manager.gunDatas)
            {
                foreach (GunUpgrade upgrade in currData.currentUpgrades.Keys)
                {
                    upgrade.unApply(currData);
                }

                currData.currentUpgrades = new Dictionary<GunUpgrade, bool>();
            }   
        }*/
    }

// Use this to recursively change the layers of objects
    // Taken from: https://discussions.unity.com/t/change-layer-of-child/28667/2
    // NOTE: Really don't know how to fix the recursion error
    public static void RecursiveLayerAssign(GameObject instance, string layer)
    {
        instance.layer = LayerMask.NameToLayer(layer);
        Transform[] childTransforms = instance.GetComponentsInChildren<Transform>(true);
        if (childTransforms != null && childTransforms.Length > 0) {
            foreach (Transform child in childTransforms)
            {
                // Debug.Log("NUMBER IN LAYER: "+instance.GetComponentsInChildren<Transform>(true));
                child.gameObject.layer = LayerMask.NameToLayer(layer);
            }
        }
    }

    public static void playGameMuse()
    {
        manager.playInGameMusic();
    }
    public void playInGameMusic()
    {
        if (playingInGameMusic)
        {
            return;
        }
        playingInGameMusic = true;
        playingMenuMusic = false;
        Shuffle(inGameMusic);
        if (currentMusicRoutine != null)
        {
            StopCoroutine(currentMusicRoutine);

        }
        currentMusicRoutine = StartCoroutine(MusicPlayerCoroutine(inGameMusic));
    }

    public static void playMenuMuse()
    {
        manager.playMenuMusic();
    }
    public void playMenuMusic()
    {
        if (playingMenuMusic)
        {
            return;
        }
        playingMenuMusic = true;
        playingInGameMusic = false;
        Shuffle(mainMenuMusic);
        if (currentMusicRoutine != null)
        {
            StopCoroutine(currentMusicRoutine);

        }
        currentMusicRoutine = StartCoroutine(MusicPlayerCoroutine(mainMenuMusic));
    }

    public IEnumerator MusicPlayerCoroutine(AudioClip[] toLoop)
    {
        int i = 0;
        while (toLoop.Length > 0)
        {
            AudioClip currentClip = toLoop[i];
            thisAudioSource.Stop();
            thisAudioSource.clip = currentClip;
            thisAudioSource.Play();
            yield return new WaitForSeconds(currentClip.length);
            i = (i + 1) % toLoop.Length;
        }

        yield return null;
    }
    
    //shuffle is not my code. https://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net
    public void Shuffle<T> (T[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;
        while (n > 1) 
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
