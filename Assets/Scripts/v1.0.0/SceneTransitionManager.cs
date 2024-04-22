using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneTransitionManager : MonoBehaviour
{

public Animator transition;

public void EnterLevel(string lvlNum) {
    StartCoroutine(SceneTransition(lvlNum));
}

IEnumerator SceneTransition(string lvlNum) {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("Level"+lvlNum);
    }

}
