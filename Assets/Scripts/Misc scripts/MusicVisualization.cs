using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVisualization : MonoBehaviour
{
    public GameObject[] musicBoxes;

    public float smooth = 0.1f;

    private AudioSource currentAudio;

    private float[] audioSample = new float[1024];

    private int[] sampleIndex;

    private float[] previousYScaling;

    // Start is called before the first frame update
    void Start()
    {
        currentAudio = GetComponent<AudioSource>();
        sampleIndex = new int[musicBoxes.Length];
        previousYScaling = new float[musicBoxes.Length];

        for (int i = 0; i < musicBoxes.Length; i++)
        {
            sampleIndex[i] = UnityEngine.Random.Range(0, audioSample.Length);
            previousYScaling[i] = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentAudio.GetSpectrumData(audioSample, 0, FFTWindow.Blackman);
        for (int i = 0; i < musicBoxes.Length; i++)
        {
            float targetScaling = audioSample[sampleIndex[i]] * 110000;

            float lerpScaling = Mathf.Lerp(previousYScaling[i], targetScaling, smooth);

            if (lerpScaling < 50)
            {
                lerpScaling += Random.Range(50, 100);
            }

            else if (lerpScaling > 2000)
            {
                lerpScaling -= Random.Range(400, 800);
            }

            musicBoxes[i].transform.localScale = new Vector3(musicBoxes[i].transform.localScale.x, lerpScaling + 50, musicBoxes[i].transform.localScale.z);

            previousYScaling[i] = lerpScaling;
        }


    }
}
