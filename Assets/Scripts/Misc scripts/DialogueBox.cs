using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI textContent;
    public TextMeshProUGUI personName;
    public Dialogue[] dialogueContainer;
    private int index;
    private AudioSource audioPlaying;
    private bool typingFinished = false;
    private Coroutine dialogueCoroutine;
    private Coroutine typingCoroutine;

    public Dialogue[] specialDialogueContainer;
    private float specialDialogueStartTime;
    private bool SpecialDialoguePlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.SetActive(false);
        textContent.text = string.Empty;
        personName.text = string.Empty;
        foreach(Dialogue dialogue in dialogueContainer)
        {
            dialogue.personModel.SetActive(false);
        }

        index = 0;
        dialogueCoroutine = StartCoroutine(DialogueAutoStart());


    }

    public void PlaySpecialDialogue(int dialogueIndex)
    {
        if (dialogueIndex >= 0 && dialogueIndex < specialDialogueContainer.Length)
        {
            SpecialDialoguePlaying = true;
            textContent.text = "";
            
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            if (audioPlaying != null && audioPlaying.isPlaying)
            {
                audioPlaying.Stop();
            }
            if (dialogueCoroutine != null)
            {
                StopCoroutine(dialogueCoroutine);
                dialogueCoroutine = null;
            }
            specialDialogueStartTime = Time.time;
            StartCoroutine(PlaySpecialDialogueFromContainer(dialogueIndex));

        }
    }

    IEnumerator PlaySpecialDialogueFromContainer(int dialogueIndex)
    {
        dialogueBox.SetActive(true);
        personName.text = specialDialogueContainer[dialogueIndex].personName;
        showModel(specialDialogueContainer[dialogueIndex].personModel);

        audioPlaying = specialDialogueContainer[dialogueIndex].singleAudio;
        float audioLength = audioPlaying.clip.length;
        float typingSpeed = audioLength / specialDialogueContainer[dialogueIndex].sentence.Length;
        typingCoroutine = StartCoroutine(typeSentence(specialDialogueContainer[dialogueIndex].sentence, typingSpeed));
        audioPlaying.Play();

        yield return new WaitUntil(() => !audioPlaying.isPlaying);
        yield return new WaitUntil(() => !typingFinished);

        dialogueBox.SetActive(false);
        SpecialDialoguePlaying = false;

        StartCoroutine(DialogueAutoStart());
    }

    IEnumerator DialogueAutoStart()
    {
        while (index < dialogueContainer.Length)
        {
            yield return new WaitForSeconds(dialogueContainer[index].delayBeforePrevious);
            dialogueBox.SetActive(true);
            personName.text = dialogueContainer[index].personName;
            showModel(dialogueContainer[index].personModel);

            audioPlaying = dialogueContainer[index].singleAudio;
            float audioLength = audioPlaying.clip.length;
            float typingSpeed = audioLength / dialogueContainer[index].sentence.Length;
            typingCoroutine =  StartCoroutine(typeSentence(dialogueContainer[index].sentence, typingSpeed));
            audioPlaying.Play();

            yield return new WaitUntil(() => !audioPlaying.isPlaying);
            yield return new WaitUntil(() => !typingFinished);

            yield return new WaitForSeconds(0.5f);
            index++;
            dialogueBox.SetActive(false);
        }
        dialogueCoroutine = null;
    }

    IEnumerator typeSentence(string sentence, float speed)
    {
        typingFinished = true;
        textContent.text = "";

        foreach (char c in sentence.ToCharArray())
        {
            textContent.text += c;
            yield return new WaitForSeconds(speed);
        }
        typingFinished = false;
    }

    void showModel(GameObject personModel)
    {
        foreach (Dialogue dialogue in dialogueContainer)
        {
            dialogue.personModel.SetActive(dialogue.personModel == personModel);
        }

        foreach (Dialogue dialogue in specialDialogueContainer)
        {
            dialogue.personModel.SetActive(dialogue.personModel == personModel);
        }
    }
}