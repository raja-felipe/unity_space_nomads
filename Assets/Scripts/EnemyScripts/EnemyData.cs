using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
[RequireComponent(typeof(AudioSource))]
public abstract class EnemyData : MonoBehaviour
{
    /// <summary>
    /// This script is a generalised script that will be used to describe unique functionality of each enemy
    /// </summary>
    public string enemyName;
    public Sprite sprite;
    public float heightAbove = 0;
    public float maxHealth = 100;
    public float attackSpeed = 1;
    public float attackRange = 1.2f;
    public float stopDistance = 1;
    public float speed = 5;
    public float damage = 10;
    public int cost = 1;
    public float checkBlockersDelay = 0.1f;
    public bool isRangedEnemy = true;
    public float newTargetUpdateDelay = 5f;
    public bool isFlying = false;
    public float attackSpeedReduction = 0; // 1 = 100%, 0 = 0% speed reduction.
    public LayerMask attackblockingLayers; //layers of things that block attack when ranged.
    public int resourcesOnKill = 0;
    public ParticleSystem explosionOnDeath;
    public ParticleSystem attackParticle;
    public Transform attackFromPosition;
    public float explosionOnDeathSize = 1f;
    public float defaultAudioVolume = 1f;
    [Serializable] public struct animationMethod
    {
        public string name;
        public int priority;
        public AnimationClip animation;
        public Animation animationHolder;
    }
    [SerializeField] public animationMethod[] animationList;
    public Dictionary<Animation,int> lastUsedPriorityDict =  new Dictionary<Animation,int>();
    public Dictionary<string,animationMethod> animationDict =  new Dictionary<string, animationMethod>();
    public AudioSource thisAudioSource;
    [Serializable] public struct audioMethod
    {
        public string name;
        public AudioClip audio;
    }
    [SerializeField] public audioMethod[] audioList;
    public Dictionary<string,AudioClip> audioDict =  new Dictionary<string, AudioClip>();
    public abstract Vector3 getNewTarget(EnemyControlScript enemyScript);

    public abstract GameObject getNewAttackTarget(EnemyControlScript enemyScript);

    public virtual Vector3 getAttackPosition(EnemyControlScript enemyScript, GameObject target)
    {
        return target.transform.position;
    }
    public  virtual void whenHit<sourceClass>(sourceClass source, float amount)
    {
        return;
    }
    public (bool, GameObject) getAttackTargetObstructions(EnemyControlScript enemyScript)
    {
        // raycast from current position to next target node.
        Vector3[] corners = new Vector3[2];
        enemyScript.agent.path.GetCornersNonAlloc(corners);
        RaycastHit hit = new RaycastHit();
        LayerMask blockingLayers = 0;
        if (!isFlying)
        {
            blockingLayers = navMeshManager.meshManager.ShortBlockingLayers;
        }
        else
        {
            blockingLayers = navMeshManager.meshManager.TallBlockingLayers;
        }

        if (Physics.Raycast(corners[0], (corners[1] - corners[0]).normalized, out hit, Mathf.Min(this.attackRange,(corners[1] - corners[0]).magnitude), blockingLayers))
        {
            if (hit.collider.TryGetComponent<buildableObjectScript>(out buildableObjectScript hitScript))
            {
                return (true, hit.collider.gameObject);
            }
            return (false,null);
        }
        else
        {
            return (false, null);
        }
    }
    public virtual void onDeath(EnemyControlScript enemyScript)
    {
        if (!GlobalSceneManager.EnemyImageDict.ContainsKey(enemyName))
        {
            GlobalSceneManager.EnemyImageDict.TryAdd(enemyName, sprite);
        }
        GlobalSceneManager.AddEnemiesKilled(enemyName);
        return;
    }

    public virtual void onKill(EnemyControlScript enemyScript,EnemyCanHit killedObject)
    {
        getAttackTargetObstructions(enemyScript);
    }

    public  virtual void onSpawn(EnemyControlScript enemyScript)
    {
        return;
    }
    public virtual void attack(GameObject target,EnemyControlScript thisEnemy)
    {
        return;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        thisAudioSource = this.GetComponent<AudioSource>();
        foreach (animationMethod method in animationList)
        {
            method.animationHolder.AddClip(method.animation,method.animation.name);
            animationDict.Add(method.name,method);
            if (!lastUsedPriorityDict.ContainsKey(method.animationHolder))
            {
                lastUsedPriorityDict.Add(method.animationHolder,-1);
            }
        }
        foreach (audioMethod method in audioList)
        {
            audioDict.Add(method.name,method.audio);
        }
    }
    

    // call is used for animations. call("animation_name") will start the animation based on the name given.
    public void call(string targetAnim)
    {
        call(targetAnim,false);
    }

    void callAudio(string AudioName)
    {
        if (audioDict.ContainsKey(AudioName))
        {
            thisAudioSource.PlayOneShot(audioDict[AudioName]);
        }
    }
    void call(string targetAnim, bool overridePriority)
    {
        callAudio(targetAnim);
        if (!animationDict.ContainsKey(targetAnim))
        {
            //Debug.Log("MISSING ANIMATION CALLED " + targetAnim + " on enemy " + gameObject.name );
            return;
        }
        animationMethod targMethod = animationDict[targetAnim];
        bool doAnimation = false;
        int currentPriority = lastUsedPriorityDict[targMethod.animationHolder];
        doAnimation |= (targMethod.priority >= currentPriority);
        doAnimation |= overridePriority;
        doAnimation |= !targMethod.animationHolder.isPlaying;
        
        // if the previous animation is finished, or this one has a higher priority.
        if (doAnimation)
        {
            if (targMethod.priority == currentPriority)
            {
                if (targMethod.animation.name != targMethod.animationHolder.clip.name)
                {
                    targMethod.animationHolder.PlayQueued(targMethod.animation.name);
                }
                else
                {
                    
                    targMethod.animationHolder.Stop();
                    
                    targMethod.animationHolder.Play();
                }
            }
            else
            {
                targMethod.animationHolder.Stop();
                targMethod.animationHolder.clip = targMethod.animation;
                targMethod.animationHolder.Play();
            }
            lastUsedPriorityDict[targMethod.animationHolder] = targMethod.priority;
        }
    }
}
