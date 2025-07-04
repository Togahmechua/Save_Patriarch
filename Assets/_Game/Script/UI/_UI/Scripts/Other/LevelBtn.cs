using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtn : MonoBehaviour
{
    public int id;
    public Image img;
    public Sprite[] spr;
    public TextMeshProUGUI txt;
    public Button btn;

    [SerializeField] private Animator anim;

    private void Start()
    {
        btn.onClick.AddListener(LoadLevel);
    }

    private void LoadLevel()
    {
        //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
        UIManager.Ins.TransitionUI<ChangeUICanvas, ChooseLevelCanvas>(0.6f,
           () =>
           {
               UIManager.Ins.OpenUI<MainCanvas>();
               LevelManager.Ins.LoadMapByID(id);
           });
    }

    public void PlayAnim()
    {
        anim.Play(CacheString.TAG_LVBTN);
    }
}
