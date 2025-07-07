using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LooseCanvas : UICanvas
{
    [Header("===Button===")]
    [SerializeField] private Button collectBtn;
    [SerializeField] private TextMeshProUGUI PoopTxt;
    [SerializeField] private GameObject eff;

    private int earnedPoop;
    private bool isClick;
    private bool isDone;
    private bool flag;

    private Coroutine transitionCoroutine;

    private void OnEnable()
    {
        //AudioManager.Ins.PlaySFX(AudioManager.Ins.win);
        Display();
    }

    private void Start()
    {
        earnedPoop = PlayerPrefs.GetInt("Poop", 0);

        collectBtn.onClick.AddListener(() =>
        {
            if (isClick) return;
            isClick = true;
            collectBtn.interactable = false;

            eff.gameObject.SetActive(true);
        });
    }

    private void Display()
    {
        earnedPoop = PlayerPrefs.GetInt("Poop");
        eff.SetActive(false);
        PoopTxt.text = earnedPoop <= 9 ? "0" + earnedPoop.ToString() : earnedPoop.ToString();

        collectBtn.interactable = true;
        isDone = false;
        isClick = false;
        flag = true;
    }

    #region Use in UIParticle Attractor
    public void GetMoney()
    {
        if (!isDone)
        {
            isDone = true;
            int newEarnedTrophy = earnedPoop + 1;
            PlayerPrefs.SetInt("Poop", newEarnedTrophy);
            PlayerPrefs.Save();

            earnedPoop = PlayerPrefs.GetInt("Poop");
            PoopTxt.text = earnedPoop <= 9 ? "0" + earnedPoop.ToString() : earnedPoop.ToString();
        }
    }

    public void PlaySFX()
    {
        if (!flag)
            return;
        flag = false;
        //AudioManager.Ins.PlaySFX(AudioManager.Ins.coin);
    }

    public void Home()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }

        transitionCoroutine = StartCoroutine(TransitionToUpgrade());
    }

    private IEnumerator TransitionToUpgrade()
    {
        yield return new WaitForSeconds(2f);

        UIManager.Ins.TransitionUI<ChangeUICanvas, LooseCanvas>(0.6f,
            () =>
            {
                Debug.Log("Loose");
                LevelManager.Ins.DespawnMap();
                UIManager.Ins.OpenUI<ChooseLevelCanvas>();
            }
        );
    }

    private void OnDisable()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
            transitionCoroutine = null;
        }
    }
    #endregion
}
