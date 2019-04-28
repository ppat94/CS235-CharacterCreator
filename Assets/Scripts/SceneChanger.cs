using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Animator animator;

    private int sceneToLoad;

    public void OnBtnClick()
    {
        FadeToNextScene();
    }

    private void FadeToNextScene()
    {
        FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void FadeToScene(int sceneIndex)
    {
        sceneToLoad = sceneIndex;
        animator.SetTrigger("FadeOut");
    }

    private void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
