using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences = new Queue<string>();
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI DialogueText;
    public Button NextButton;
    public Animator animator;
    public Animator TaskAnimator;
    public Animator CountDownAnimator;

    public TextMeshProUGUI TaskText;
    public string TaskTemplate;

    private void Start()
    {
        //NextButton.gameObject.SetActive(false);
    }

    public void StartNewTask(ItemData itemData)
    {
        // Make the UI pop up on screen
        CountDownAnimator.SetBool("IsOpen", false);
        TaskAnimator.SetBool("IsOpen", true);
        GameManager.Instance.InDialogue = true;
        StartCoroutine(TypeTask(itemData));
    }

    IEnumerator TypeTask(ItemData itemData)
    {
        TaskText.text = "";
        string sentence = TaskTemplate.Replace("ITEM", itemData.ItemName);
        foreach(char letter in sentence)
        {
            TaskText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        // Get rid of UI
        StartCoroutine(EndTask());
    }

    IEnumerator EndTask()
    {
        yield return new WaitForSeconds(1f);

        TaskAnimator.SetBool("IsOpen", false);
        StartCoroutine(StartTimer());
    }

    public void StartDialogue(Dialogue dialogue)
    {
        GameManager.Instance.GamePaused = true;
        GameManager.Instance.InDialogue = true;
        animator.SetBool("IsOpen", true);
        NameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        //NextButton.gameObject.SetActive(false);
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        DialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        NextButton.gameObject.SetActive(true);
    }

    IEnumerator StartTimer()
    {
        CountDownAnimator.SetBool("IsOpen", true);
        yield return new WaitForSeconds(1f);

        GameManager.Instance.InDialogue = false;
    }


    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        GameManager.Instance.GamePaused = false;
        StartCoroutine(StartTimer());
    }
}
