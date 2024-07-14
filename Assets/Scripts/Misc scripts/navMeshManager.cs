using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class navMeshManager : MonoBehaviour
{
    public static navMeshManager meshManager;
    // Start is called before the first frame update
    NavMeshSurface currentNavMeshSurface;
    public LayerMask bakedLayers;
    public int defaultArea;
    public Bounds bounds;
    public bool buildNextFrame = false;
    public List<int> allAreas;
    public List<int> flyOverAreas; //areas that can be flown over.;
    public List<int> walkOverAreas; //areas that can be flown over.
    public LayerMask ShortBlockingLayers;
    public LayerMask TallBlockingLayers;
    public LayerMask BuildingLayers;

    void Awake()
    {
        meshManager = this;
        foreach (buildableObjectScript building in gameManagerScript.Buildings)
        {
            if (building.thisModifier != null)
            {
                NavMesh.SetAreaCost(building.thisModifier.area, Mathf.Max(building.maxHealth, 1));
                if (building.isTall)
                {
                    TallBlockingLayers |= 1 << building.gameObject.layer;
                    flyOverAreas.Append(building.thisModifier.area);
                }

                if (!building.groundBuilding)
                {
                    ShortBlockingLayers |= 1 << building.gameObject.layer;
                    walkOverAreas.Append(building.thisModifier.area);
                }
            
                allAreas.Append(building.thisModifier.area);
            }

            BuildingLayers |= 1 << building.gameObject.layer;
        }
    }
    void Start()
    {
        currentNavMeshSurface = this.GetComponent<NavMeshSurface>();
        NavMesh.pathfindingIterationsPerFrame = 1000;
        //rebuildMesh();
        buildNextFrame = false;
        rebuildMesh();
        StartCoroutine(meshBuildingRoutine());
    }

    IEnumerator meshBuildingRoutine()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (buildNextFrame)
            {
                rebuildMesh();
                buildNextFrame = false;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// rebuilds the current mesh dynamically, including all navmesh modifer volumes aswell.
    /// </summary>
    public void rebuildMesh()
    {
        gameManagerScript.manager.addAllNavMeshMods();
        Bounds newb = bounds;
        newb.center += this.transform.position;
        List<NavMeshBuildSource> results = new List<NavMeshBuildSource>();
        List<NavMeshBuildMarkup> navMeshBuildMarkups = new List<NavMeshBuildMarkup>();
        NavMeshBuilder.CollectSources(bounds,bakedLayers,NavMeshCollectGeometry.PhysicsColliders,defaultArea,navMeshBuildMarkups, results);
        results.AddRange(gameManagerScript.manager.currentNavmeshModiferVolumes.Values);
        NavMeshBuildSettings buildSettings = currentNavMeshSurface.GetBuildSettings();
        NavMeshBuilder.UpdateNavMeshData(currentNavMeshSurface.navMeshData,buildSettings,results,bounds);
    }

    public static void queueRebake()
    {
        meshManager.buildNextFrame = true;
    }
}
