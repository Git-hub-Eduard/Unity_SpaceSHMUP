using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelScene : MonoBehaviour
{
    public Animator animator;
    public void NextLevel()
    {
        StartCoroutine(MakeTransition(1));
    }

    IEnumerator MakeTransition(int i)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(i);
    }

    public void QuitLevel()
    {
        StartCoroutine(MakeTransition(0));
    }
}
