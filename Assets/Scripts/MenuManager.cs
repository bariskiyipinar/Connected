using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    MenuAnimatoinEffect menuAnimatoinEffect;
    public GameObject background;
    private void Start()
    {
        menuAnimatoinEffect = FindAnyObjectByType<MenuAnimatoinEffect>();
    }

    public void StartFNC()
    {
        Destroy(background, 3.5f);
        menuAnimatoinEffect.CircleObject.SetActive(true);
        menuAnimatoinEffect.TransationEffect.Play("BackgroundAnimation");

       
        StartCoroutine(PlayAnimationAndText());
    }

    private IEnumerator PlayAnimationAndText()
    {
    
        float animLength = menuAnimatoinEffect.TransationEffect.GetCurrentAnimatorStateInfo(0).length;

 
        yield return new WaitForSeconds(animLength);

     
        yield return StartCoroutine(menuAnimatoinEffect.PlayText());

     
        SceneManager.LoadScene("basescene");
    }

    public void QuitFNC()
    {
        Application.Quit();
    }
}
