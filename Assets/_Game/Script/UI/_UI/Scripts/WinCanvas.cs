using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinCanvas : UICanvas
{
    [Header("===Effect===")]
    [SerializeField] private Image roateImg;
    [SerializeField] float speed = 90f;

    [Header("===Button===")]
    [SerializeField] private Button collectBtn;
    [SerializeField] private TextMeshProUGUI TrophyTxt;
    [SerializeField] private GameObject eff;

    private int earnedTrophy;
    private bool isClick;
    private bool isDone;
    private bool flag;

    private Coroutine transitionCoroutine;

    private void OnEnable()
    {
        AudioManager.Ins.PlaySFX(AudioManager.Ins.win);
        Display();
    }

    private void Start()
    {
        earnedTrophy = PlayerPrefs.GetInt("Trophy", 0);

        collectBtn.onClick.AddListener(() =>
        {
            if (isClick) return;
            isClick = true;
            collectBtn.interactable = false;

            eff.gameObject.SetActive(true);
        });
    }

    private void Update()
    {
        roateImg.transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }

    private void Display()
    {
        earnedTrophy = PlayerPrefs.GetInt("Trophy");
        eff.SetActive(false);
        TrophyTxt.text = earnedTrophy <= 9 ? "0" + earnedTrophy.ToString() : earnedTrophy.ToString();

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
            int newEarnedTrophy = earnedTrophy + 1;
            PlayerPrefs.SetInt("Trophy", newEarnedTrophy);
            PlayerPrefs.Save();

            earnedTrophy = PlayerPrefs.GetInt("Trophy");
            TrophyTxt.text = earnedTrophy <= 9 ? "0" + earnedTrophy.ToString() : earnedTrophy.ToString();
        }
    }

    public void PlaySFX()
    {
        if (!flag)
            return;
        flag = false;
        AudioManager.Ins.PlaySFX(AudioManager.Ins.coin);
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
        yield return new WaitForSeconds(4f);

        UIManager.Ins.TransitionUI<ChangeUICanvas, WinCanvas>(0.6f,
            () =>
            {
                Debug.Log("Win");
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