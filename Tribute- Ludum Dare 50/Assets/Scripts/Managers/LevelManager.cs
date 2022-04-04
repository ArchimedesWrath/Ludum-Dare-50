using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instnace;
    public Animator Transition;
    public float TransitionTime = 1f;

    private void Awake()
    {
        if (Instnace == null)
        {
            Instnace = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void QuitGame() => Debug.Log("Game Ended");

    public void ToggleOptions() => Debug.Log("Options Selected");

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadPreviousLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // Player animation
        Transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(TransitionTime);

        // Load scene
        SceneManager.LoadScene(levelIndex);
        Transition.SetTrigger("End");
    }
}
