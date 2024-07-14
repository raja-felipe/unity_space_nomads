using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(AudioSource))]
public class gunAnimationController : MonoBehaviour
{
    [Serializable] public struct animationMethod
    {
        public string name;
        public int priority;
        public AnimationClip animation;
    }
    [Serializable] public struct audioMethod
    {
        public string name;
        public int priority;
        public AudioClip audio;
    }
    [SerializeField] public animationMethod[] animationList;
    [SerializeField] public audioMethod[] audioList;
    public animationMethod currentAnim;
    public audioMethod currentAudio;
    public float lastAnimationStartTime;
    public float lastAudioStartTime;
    public Animation thisAnimation;
    public AudioSource thisAudioPlayer;
    public Dictionary<string,animationMethod> animationDict =  new Dictionary<string, animationMethod>();
    public Dictionary<string,audioMethod> soundDict =  new Dictionary<string, audioMethod>();
    // Start is called before the first frame update
    void Start()
    {
        thisAnimation = gameObject.GetComponent<Animation>();
        thisAudioPlayer = gameObject.GetComponent<AudioSource>();
        lastAnimationStartTime = Time.time;
        lastAudioStartTime = Time.time;
        foreach (animationMethod method in animationList)
        {
            thisAnimation.AddClip(method.animation,method.animation.name);
            animationDict.Add(method.name,method);
        }
        
        foreach (audioMethod method in audioList)
        {
            soundDict.Add(method.name,method);
        }
    }

    // Update is called once per frame
    public void call(string targetAnim)
    {
        call(targetAnim,false);
    }

    IEnumerator playOnNextFrame(AnimationClip targetAnimationClip)
    {
        thisAnimation.Rewind();
        yield return new WaitForEndOfFrame();
        thisAnimation.clip = targetAnimationClip;
        thisAnimation.Play();
        yield return null;
    }
    void call(string targetCall, bool overridePriority)
    {

        if (animationDict.ContainsKey(targetCall))
        {
            animationMethod targAnimationMethod = animationDict[targetCall];
            // if the previous animation is finished, or this one has a higher priority.
            if (overridePriority || 
                targAnimationMethod.priority >= currentAnim.priority ||
                currentAnim.animation.length + lastAnimationStartTime < Time.time)
            {
                currentAnim = targAnimationMethod;
                StartCoroutine(playOnNextFrame(targAnimationMethod.animation));
                lastAnimationStartTime = Time.time;
            }
        }
        
        if (soundDict.ContainsKey(targetCall))
        {
            audioMethod targAudioMethod = soundDict[targetCall];
            
            // if the previous audio is finished, or this one has a higher priority.
            if (overridePriority || 
                targAudioMethod.priority >= currentAudio.priority ||
                currentAudio.audio.length + lastAudioStartTime > Time.time)
            {
                currentAudio= targAudioMethod;
                thisAudioPlayer.clip = targAudioMethod.audio;
                thisAudioPlayer.Play();
                lastAudioStartTime = Time.time;
            }
        }

        if (!soundDict.ContainsKey(targetCall) & !animationDict.ContainsKey(targetCall))
        {
            Debug.LogWarning("MISSING SOUND AND ANIM CALLED " + targetCall );
            
        }
        
    }
}
