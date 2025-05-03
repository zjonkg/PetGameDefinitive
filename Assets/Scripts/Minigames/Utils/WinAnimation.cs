using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uitween : MonoBehaviour
{
    [SerializeField]
    GameObject backPanel, homeButton, replayButton,
    star1, star2, star3, score, coins, levelSuccess;
    void Start()
    {
        LeanTween.scale(levelSuccess, new Vector3(1.5f, 1.5f, 1.5f), 2f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(LevelComplete);
        LeanTween.moveLocal(levelSuccess, new Vector3(-30f, 747f, 2f), .7f).setDelay(2f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(levelSuccess, new Vector3(1f, 1f, 1f), 2f).setDelay(1.7f).setEase(LeanTweenType.easeInOutCubic);
    }

    void LevelComplete()
    {

        LeanTween.moveLocal(backPanel, new Vector3(0f, -267f, 0f), 0.7f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc).setOnComplete(StarsAnim);
        LeanTween.scale(homeButton, new Vector3(1f, 1f, 1f), 2f).setDelay(.8f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(replayButton, new Vector3(1f, 1f, 1f), 2f).setDelay(.9f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.alphaCanvas(score.GetComponent<CanvasGroup>(), 1f, 0.5f).setDelay(1.1f);
        LeanTween.alphaCanvas(coins.GetComponent<CanvasGroup>(), 1f, 0.5f).setDelay(1.1f);
    }

    void StarsAnim()
    {
        LeanTween.scale(star1, new Vector3(1f, 1f, 1f), 2f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star2, new Vector3(1f, 1f, 1f), 2f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star3, new Vector3(1f, 1f, 1f), 2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic);

    }


}
