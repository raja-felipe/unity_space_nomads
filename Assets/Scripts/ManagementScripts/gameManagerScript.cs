using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.AI.Navigation;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Object = UnityEngine.Object;
using Random = System.Random;

public class gameManagerScript : MonoBehaviour
{
    
    public static GunData[] Guns
    {
        get { return manager.gunDatas;}
    }
    public GunData[] gunDatas;
    public static EnemyData[] Enemies
    {
        get { return manager.enemyDatas;}
    }
    public EnemyData[] enemyDatas;
    public static buildableObjectScript[] Buildings
    {
        get { return manager.buildingDatas;}
    }
    public buildableObjectScript[] buildingDatas;
    // this is a set used for the navigation dynamic baking.
    public IDictionary<int, NavMeshBuildSource> currentNavmeshModiferVolumes = new Dictionary<int, NavMeshBuildSource>();
    
    public static gameManagerScript manager;
    public Transform[] spawns;
    public Transform[] teleporterSpawns;
    public PortalScript[] createdSpawns;
    public PortalScript teleporterPrefab;
    public GooScript gooObject;
    public bool doSpawns = true;
    public int EnemyCountTotal = 0;
    public ObjectiveScript[] objectives;
    public LayerMask buildableLayers; // layers that can be built on
    public LayerMask ShootThroughLayers; // layers that can be shot through
    
    // Time Variables
    private float timeAtLastPause = 0f;

    public GameObject[] buildingHologramPrefabs;
    public GameObject[] buildingHolograms;
    // Need this to change the scene
    [SerializeField] private SceneTransition sceneTransition;
    [SerializeField] private float targetTime = 300f; // 5 minutes in seconds
    [Serializable] public struct EnemyTypeCount{
        public EnemyData type;
        public int count;
    }
    [SerializeField] public EnemyTypeCount[] EnemyPoolList;
    public Dictionary<String,List<EnemyControlScript>> EnemyPool;
    public float timeTillNextWave;
    // Need this to adjust the game volume
    [SerializeField] public InGameVolumeManager inGameVolumeManager;
    void Awake()
    {
        buildingHolograms = new GameObject[buildingHologramPrefabs.Length];
        for (int i = 0; i < buildingHologramPrefabs.Length; i++)
        {
            GameObject holoPrefab = buildingHologramPrefabs[i];
            GameObject newHologram = Instantiate(holoPrefab, Vector3.zero, quaternion.identity);
            buildingHolograms[i] = newHologram;
            newHologram.SetActive(false);
            
        }

        createdSpawns = new PortalScript[teleporterSpawns.Length];
        manager = this;
        // Set the guns
        SetStartingGuns();
    }
    void Start()
    {
        Time.timeScale = 1;
        //DontDestroyOnLoad(this.gameObject);
        if(doSpawns){
            createPool();
            StartCoroutine(spawnRoutine());
        }
        
        // Initialize all the scores list
        /*GlobalSceneManager.DamageTaken = new int[(int)(targetTime)];
        GlobalSceneManager.DamageDealt = new int[(int)(targetTime)];
        GlobalSceneManager.Shots = new int[(int)(targetTime)];
        GlobalSceneManager.ShotsHit = new int[(int)(targetTime)];
        GlobalSceneManager.Accuracy = new int[(int)(targetTime)];
        GlobalSceneManager.BuildingsMade = new int[(int)(targetTime)];*/

        addAllNavMeshMods();
    }

    /// <summary>
    /// Finds all navmesh modifiers and adds them to the list. Very expensive.
    /// </summary>
    public void addAllNavMeshMods()
    {
        
        foreach (NavMeshModifierVolume navModVolume in Object.FindObjectsByType<NavMeshModifierVolume>(FindObjectsSortMode.None))
        {
            addToNavMesh(navModVolume);
        }
    }
    /// <summary>
    /// Adds a navmesh modifer volume to the baking list, so that it is baked dynamically.
    /// Note that re-adding a volume will update its data in the list.
    /// </summary>
    public void addToNavMesh(NavMeshModifierVolume targetVolume)
    {
        NavMeshBuildSource newBuildSource = new NavMeshBuildSource();
        newBuildSource.shape = NavMeshBuildSourceShape.ModifierBox;
        newBuildSource.area = targetVolume.area;
        newBuildSource.component = targetVolume;
        newBuildSource.generateLinks = false;
        newBuildSource.sourceObject = targetVolume.gameObject;
        //Lots of confusing transformations below. Essentially this gets the correct size,position and rotation of the cube in the scene. 
        Transform targTrans = targetVolume.gameObject.transform;
        newBuildSource.size = Vector3.Scale(targetVolume.size, targTrans.lossyScale);
        newBuildSource.transform = Matrix4x4.TRS(targTrans.position + Vector3.Scale(targetVolume.center , targTrans.lossyScale), targTrans.rotation, new Vector3(1,1,1f)); //targetVolume.gameObject.transform.localToWorldMatrix);
        currentNavmeshModiferVolumes[targetVolume.GetHashCode()] = newBuildSource;
    }

    public void removeFromNavMesh(NavMeshModifierVolume targetVolume)
    {
        currentNavmeshModiferVolumes.Remove(targetVolume.GetHashCode());
    }
    public static T[] SubArray<T>(T[] data, int index, int end)
    {
        if (index >= end)
        {
            return new T[0];
        }

        int length = end - index;
        T[] result = new T[length];
        Array.Copy(data, index, result, 0, length);
        return result;
    }
    IEnumerator spawnRoutine()
    {
        DialogueBox dialogueBox = FindObjectOfType<DialogueBox>();
        // Write schedule here.
        Vector3[] spawnPoses = spawns.Select(x => x.position).ToArray();
        Quaternion[] spawnRots = spawns.Select(x => x.rotation).ToArray();
        timeTillNextWave = 25;
        yield return new WaitForSeconds(25);
        timeTillNextWave = 40;
        createEnemys(
            SubArray(spawns,1,4),  new EnemyData[]{enemyDatas[0]}, 15,2f);
        yield return new WaitForSeconds(40);
        timeTillNextWave = 45;
        createEnemys(
            SubArray(spawns,1,4),  new EnemyData[]{enemyDatas[0],enemyDatas[1]}, 15,0.1f);
        yield return new WaitForSeconds(45);
        timeTillNextWave = 35;
        createEnemys(
            SubArray(spawns,1,4),  new EnemyData[]{enemyDatas[0]}, 15,0.5f);
        createEnemys(
            SubArray(spawns,1,4),  new EnemyData[]{enemyDatas[2]}, 5,0.5f);
        createEnemys(
            SubArray(spawns,1,4),  new EnemyData[]{enemyDatas[5]}, 3,0.5f);
        yield return new WaitForSeconds(6);
        createEnemys(
            SubArray(spawns,1,4),  new EnemyData[]{enemyDatas[6]}, 3,0.5f);
        yield return new WaitForSeconds(29);
        timeTillNextWave = 35f;
        createEnemys(
            SubArray(spawns,1,3),  new EnemyData[]{enemyDatas[0]}, 10,1f);
        createEnemys(
            SubArray(spawns,1,3),  new EnemyData[]{enemyDatas[1]}, 10,1f);
        createEnemys(
            SubArray(spawns,3,4),  new EnemyData[]{enemyDatas[4]}, 1);
        createEnemys(
            SubArray(spawns,3,4),  new EnemyData[]{enemyDatas[3]}, 4,0.2f);
        createEnemys(
            SubArray(spawns,1,4),  new EnemyData[]{enemyDatas[2]}, 5,0.5f);
        createEnemys(
            SubArray(spawns,0,1),  new EnemyData[]{enemyDatas[6]}, 3,3f);
        yield return new WaitForSeconds(40);
        timeTillNextWave = 40f;
        //Kill those portals!!
        dialogueBox.PlaySpecialDialogue(0);
        spawnPortalRandomPosition(enemyDatas[0],5);
        spawnPortalRandomPosition(enemyDatas[0],5);
        createEnemys(
            SubArray(spawns,1,4),  new EnemyData[]{enemyDatas[0]}, 20,1f);
        createEnemys(
            SubArray(spawns,1,4),  new EnemyData[]{enemyDatas[1]}, 10,2f);
        createEnemys(
            SubArray(spawns,1,4),  new EnemyData[]{enemyDatas[2]}, 8,3f);
        yield return new WaitForSeconds(45);
        timeTillNextWave = 30f;
        //100 left.
        //Kill those portals!!
        dialogueBox.PlaySpecialDialogue(1);
        spawnPortalRandomPosition(enemyDatas[0],5);
        spawnPortalRandomPosition(enemyDatas[0],5);
        spawnPortalRandomPosition(enemyDatas[2],8);
        spawnPortalRandomPosition(enemyDatas[5],10);
        createEnemys(
            SubArray(spawns,1,4),  new EnemyData[]{enemyDatas[1]}, 10,2f);
        createEnemys(
            SubArray(spawns,1,4),  new EnemyData[]{enemyDatas[0]}, 5,2f);
        createEnemys(
            SubArray(spawns,1,4),  new EnemyData[]{enemyDatas[3]}, 10,2);
        createEnemys(
            SubArray(spawns,1,4),  new EnemyData[]{enemyDatas[6]}, 3,2f);
        yield return new WaitForSeconds(30);
        timeTillNextWave = 50f;
        //Kill those portals!!
        //frenzy!!
        dialogueBox.PlaySpecialDialogue(2);
        spawnPortalRandomPosition(enemyDatas[0],8);
        spawnPortalRandomPosition(enemyDatas[1],8);
        spawnPortalRandomPosition(enemyDatas[2],8);
        spawnPortalRandomPosition(enemyDatas[3],8);
        spawnPortalRandomPosition(enemyDatas[6],8);
        createEnemys(
            SubArray(spawns,1,4),  SubArray(enemyDatas,0,4), 80,0.8f);
        createEnemys(
            SubArray(spawns,1,4),  SubArray(enemyDatas,5,7), 30,0.5f);
        yield return new WaitForSeconds(50);
        yield return null;
    }

    public void spawnPortalRandomPosition(EnemyData type, float delay)
    {
        List<int> spawnPoses = new List<int>();
        for (int i = 0; i < createdSpawns.Length; i++)
        {
            
            if (createdSpawns[i] != null)
            {
                if (createdSpawns[i].enabled)
                {
                    continue;
                }
            }
            spawnPoses.Add(i);
        }

        Random rnd = new Random();
        if (spawnPoses.Count > 0)
        {
            int randomIndex = rnd.Next(spawnPoses.Count);
            spawnPortal(spawnPoses[randomIndex],type,delay);
        }
    }
    public void spawnPortal(int index,EnemyData type, float delay)
    {
        index = index % teleporterSpawns.Length;
        if (createdSpawns[index] != null)
        {
            if (createdSpawns[index].enabled)
            {
                return;
            }
        }
        createdSpawns[index] = Instantiate(teleporterPrefab, teleporterSpawns[index].position,teleporterSpawns[index].rotation);
        createdSpawns[index].enemyType = type;
        createdSpawns[index].delayBetweenSpawns = delay;
        
    }
    /// <summary>
    /// Create pool creates a bunch of enemies that will be recycled throughout the game.
    /// Higher pool size will slow down game initialisation, but will be better long term.
    /// </summary>
    void createPool()
    {
        
        EnemyPool = new Dictionary<String, List<EnemyControlScript>>();
        for (int i = 0; i < enemyDatas.Length; i++)
        {
            EnemyData type = enemyDatas[i];
            EnemyPool.TryAdd(type.enemyName,new List<EnemyControlScript>());
        }
        for (int i = 0; i < EnemyPoolList.Length; i++)
        {
            EnemyData type = EnemyPoolList[i].type;
            for (int j = 0; j < EnemyPoolList[i].count; j++)
            {
                EnemyControlScript newEnemy = Instantiate(type.gameObject, spawns[0].position, Quaternion.identity)
                    .GetComponent<EnemyControlScript>();
                
                // Now add that enemy to the list of enemies to track the volume
                inGameVolumeManager.enemyObjects.Add(newEnemy.gameObject);
                
                newEnemy.gameObject.name = newEnemy.data.enemyName + " " + EnemyCountTotal++;
                EnemyPool[type.enemyName].Add(newEnemy);
                newEnemy.gameObject.SetActive(false);
            }
        }
        return;
    }
    
    // returns an enemy to the enemypool, for reuse.
    public void returnToPool(EnemyControlScript target)
    {
        EnemyPool[target.data.enemyName].Add(target);
        target.gameObject.SetActive(false);
    }

    /// <summary>
    /// Create enemies creates a bunch of enemies at the given positions.
    /// The parameter lists are cycled through as enemies are spawned.
    /// </summary>
    /// <param name="positions"></param>
    /// list of positions to spawn enemies at.
    /// <param name="rotations"></param>
    /// list of rotations to spawn enemies with.
    /// <param name="types"></param>
    /// list of the type of enemies that are spawned.
    /// <param name="amountOfEnemies"></param>
    /// how many enemies to spawn
    /// <param name="delay"></param>
    /// how much time between each spawn.
    /// <returns></returns>
    /// an array of the spawn enemies. Because of the coroutine, this will be updated slowly.
    
    public EnemyControlScript[] createEnemys(Transform[] transforms, EnemyData[] types,int amountOfEnemies, float delay)
    {
        Vector3[] positions = transforms.Select(x => x.position).ToArray();
        Quaternion[] rotations = transforms.Select(x => x.rotation).ToArray();
        EnemyControlScript[] ret = new EnemyControlScript[amountOfEnemies];
        StartCoroutine(createEnemysCoroutine(positions, rotations, types, amountOfEnemies, delay, ret));
        return ret;
    }
    public EnemyControlScript[] createEnemys(Transform[] transforms, EnemyData[] types,int amountOfEnemies)
    {
        return createEnemys(transforms, types, amountOfEnemies, 0);
    }
    //a coroutine that creates enemies at a delay.
    IEnumerator createEnemysCoroutine(Vector3[] positions, Quaternion[] rotations, EnemyData[] types,int amountOfEnemies,float delay, EnemyControlScript[] outEnemies)
    {
        // Write schedule here.
        EnemyData currentType = enemyDatas[0];
        Quaternion currentRotation = Quaternion.identity;
        Vector3 currentPosition = Vector3.zero;
        for (int i = 0; i < amountOfEnemies; i++)
        {
            currentPosition = positions[i % positions.Length];
            currentRotation = rotations[i % rotations.Length];
            currentType = types[i % types.Length];
            outEnemies[i] = createEnemy(currentPosition, currentRotation, currentType);
            if (delay > 0){
                yield return new WaitForSeconds(delay);
            }
        }
        yield return null;
    }
    // create enemy at specified position, rotation, and of specified type.
    public EnemyControlScript createEnemy(Vector3 position, Quaternion rotation, EnemyData type)
    {
        EnemyControlScript newEnemy;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas))
        {
            position = hit.position;
        }
        else
        {
            Debug.LogError("NO NAV MESH AT POSITION");
        }
        
        position.y += type.transform.localScale.y *  type.heightAbove/2;
        // if we have spare enemies in the pool, we can use those. More efficient.
        if (EnemyPool[type.enemyName].Count >0)
        {
            newEnemy = EnemyPool[type.enemyName].First();
            EnemyPool[type.enemyName].RemoveAt(0);
            newEnemy.transform.position = position;
            newEnemy.transform.rotation = rotation;
            newEnemy.gameObject.SetActive(true);
        }
        else
        {
            newEnemy = Instantiate(type, position, rotation).GetComponent<EnemyControlScript>();
            newEnemy.gameObject.name = newEnemy.data.enemyName + " " + EnemyCountTotal++;
            newEnemy.gameObject.SetActive(true);
        }
        return newEnemy;
    }
    // return the closest objective to the location.
    // TODO: Make it usable for multiple objectives.
    public ObjectiveScript closest_objective(Vector3 location)
    {
        float m = Mathf.Infinity;
        ObjectiveScript closest = null;
        foreach (ObjectiveScript obj in objectives)
        {
            if (Vector3.Distance(obj.transform.position, location) < m)
            {
                m = Vector3.Distance(obj.transform.position, location);
                closest = obj;
                
            }
        }
        return closest;
    }
    
    public ObjectiveScript random_objective()
    {
        Random temp = new Random();
        int chosen = temp.Next(objectives.Length);
        return objectives[chosen];
    }
    // Time getters
    public float getTimeAtLastPause()
    {
        return timeAtLastPause;
    }
    
    public float getTargetTime()
    {
        return targetTime;
    }
    
    
    // Update is called once per frame
    void Update()
    {
        // Do this first to check if the game was won
        // Debug.Log("TIME PASSED: " + timeAtLastPause);

        if (Time.timeScale != 0)
        {
            timeAtLastPause += Time.deltaTime;
        }

        if (timeTillNextWave > 0)
        {
            timeTillNextWave -= Time.deltaTime;
        }
        if(timeTillNextWave < 0)
        {
            timeTillNextWave = 0;
        }
        // Check for victory condition
        if (timeAtLastPause >= targetTime)
        {
            // Trigger your victory event or do something here
            Debug.Log("Victory!");
            Cursor.lockState = CursorLockMode.Confined;
            sceneTransition.GoToVictoryScene();
        }

    }

    private void SetStartingGuns()
    {
        if (GlobalSceneManager.SelectedGuns != null)
        {
            gunDatas[GlobalSceneManager.PrimaryGunIndex] =
                GlobalSceneManager.SelectedGuns[GlobalSceneManager.PrimaryGunIndex];
            gunDatas[GlobalSceneManager.SecondaryGunIndex] =
                GlobalSceneManager.SelectedGuns[GlobalSceneManager.SecondaryGunIndex];
            GlobalSceneManager.ToolGun = gunDatas[GlobalSceneManager.ToolGunIndex];
        }
    }
}
