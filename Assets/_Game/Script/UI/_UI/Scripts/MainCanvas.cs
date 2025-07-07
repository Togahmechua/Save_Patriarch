using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : UICanvas
{
    [SerializeField] private Button pauseBtn;
    [SerializeField] private TextMeshProUGUI arrowTxt;
    [SerializeField] private TextMeshProUGUI timer;

    private int currentTime;
    private bool isCounting = false;

    public int CurrentTime => currentTime;

    private void OnEnable()
    {
        isCounting = false;
    }

    private void Start()
    {
        UIManager.Ins.mainCanvas = this;

        pauseBtn.onClick.AddListener(() =>
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
            UIManager.Ins.OpenUI<PauseCanvas>();
            UIManager.Ins.CloseUI<MainCanvas>();
        });
    }

    public void UpdateInfo(int amountArrow, int time)
    {
        currentTime = time;
        UpdateArrow(amountArrow);
        UpdateTimer(currentTime);
    }

    public void StartCountdown()
    {
        if (!isCounting)
        {
            StartCoroutine(CountdownRoutine());
        }
    }

    private IEnumerator CountdownRoutine()
    {
        isCounting = true;

        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;
            UpdateTimer(currentTime);
        }

        isCounting = false;

        // ✅ Khi hết giờ, giả lập "hết đạn"
        if (!LevelManager.Ins.isWin)
        {
            LevelManager.Ins.level.OutOfArrowDueToTimeout();
        }
    }

    public void DecreaseTime(int amount)
    {
        currentTime -= amount;
        if (currentTime <= 0)
        {
            currentTime = 0;
            UpdateTimer(currentTime);

            // ✅ Khi thời gian <= 0, giả lập hết đạn
            if (!LevelManager.Ins.isWin)
            {
                LevelManager.Ins.level.OutOfArrowDueToTimeout();
            }
        }
        else
        {
            UpdateTimer(currentTime);
        }
    }

    public void UpdateArrow(int amountArrow)
    {
        arrowTxt.text = amountArrow.ToString();
    }

    public void UpdateTimer(int time)
    {
        timer.text = FormatTime(time);
    }

    private string FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return $"{minutes:00}:{seconds:00}";
    }
}
