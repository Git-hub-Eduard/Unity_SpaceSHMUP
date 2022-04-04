using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator FinishAnimation;
    public void ScaledTime()
    {
        Time.timeScale = 1f;
        FinishAnimation.SetTrigger("Close");
    }
}
