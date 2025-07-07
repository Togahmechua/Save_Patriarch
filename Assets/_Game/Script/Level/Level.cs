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
    public List<Rope> ropes;

    private void OnEnable()
    {
        Arrow.OnArrowDespawned += OnArrowDespawned;
    }

    private void OnDisable()
    {
        Arrow.OnArrowDespawned -= OnArrowDespawned;
    }


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
            UIManager.Ins.mainCanvas.UpdateArrow(amountArrow);

            Debug.Log("Run Out Of Arrow");
            StartCoroutine(LoseAfterDelay(2f));
            return;
        }

        UIManager.Ins.mainCanvas.UpdateArrow(amountArrow);
    }

    private IEnumerator LoseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UIManager.Ins.CloseUI<MainCanvas>();
        // Chỉ xử lý thua nếu chưa thắng
        if (!LevelManager.Ins.isWin)
        {
            Debug.Log("Lose: Out of arrows and not won after delay!");
            // TODO: Mở UI thua nếu cần, ví dụ:
            yield return new WaitForSeconds(2f);
            UIManager.Ins.OpenUI<LooseCanvas>();
        }
    }

    private void OnArrowDespawned(Arrow arrow)
    {
        // Sau mỗi lần mũi tên biến mất, kiểm tra nếu tất cả dây đã bị cắt
        bool allCut = true;
        foreach (var rope in ropes)
        {
            if (!rope.isCutted)
            {
                allCut = false;
                break;
            }
        }

        if (allCut && !LevelManager.Ins.isWin)
        {
            LevelManager.Ins.isWin = true;
            Debug.Log("Win: All ropes cut!");
            // TODO: Mở UI Win nếu cần
            StartCoroutine(IEWait());
        }
    }

    private IEnumerator IEWait()
    {
        UIManager.Ins.CloseUI<MainCanvas>();

        yield return new WaitForSeconds(2f);
        UIManager.Ins.OpenUI<WinCanvas>();
    }

    public void OutOfArrowDueToTimeout()
    {
        amountArrow = 0;
        canShoot = false;
        UIManager.Ins.mainCanvas.UpdateArrow(amountArrow);
        Debug.Log("Run Out Of Arrow (Timeout)");
        StartCoroutine(LoseAfterDelay(2f));
    }

}
