using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int id;

    [SerializeField] private int amountArrow;
    [SerializeField] private int timer;

    public static Level currentLevel;
    public bool canShoot = true;

    public bool isTouch;
    private bool startedCountdown;

    private void Start()
    {
        currentLevel = this;
        UIManager.Ins.mainCanvas.UpdateInfo(amountArrow, timer);
    }


    private void Update()
    {
        if (!LevelManager.Ins.isWin) return;

        if (id == LevelManager.Ins.curMapID &&
            !LevelManager.Ins.mapSO.mapList[LevelManager.Ins.curMapID].isWon)
        {
            LevelManager.Ins.mapSO.mapList[LevelManager.Ins.curMapID].isWon = true;
            SaveWinState(LevelManager.Ins.curMapID);
            Debug.Log("Map " + LevelManager.Ins.curMapID + " is won.");
            LevelManager.Ins.curMap++;
        }

        SetCurMap();
    }

    private void SetCurMap()
    {
        PlayerPrefs.SetInt("CurrentMap", LevelManager.Ins.curMap);
        PlayerPrefs.Save();
    }

    private void SaveWinState(int mapIndex)
    {
        string key = "MapWin_" + mapIndex;
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
        LevelManager.Ins.mapSO.LoadWinStates();
    }

    public void DecreaseArrow()
    {
        amountArrow--;

        if (amountArrow <= 0)
        {
            amountArrow = 0;
            canShoot = false;
            Debug.Log("Run Out Of Arrow");
            // Có thể xử lý thua tại đây nếu cần
        }

        UIManager.Ins.mainCanvas.UpdateInfo(amountArrow, timer);
    }

}
