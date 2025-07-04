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

    private void Start()
    {
        UIManager.Ins.mainCanvas = this;

        pauseBtn.onClick.AddListener(() =>
        {
            //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
            UIManager.Ins.OpenUI<PauseCanvas>();
            UIManager.Ins.CloseUI<MainCanvas>();
        });
    }

    public void UpdateInfo(int amountArrow, int time)
    {
        arrowTxt.text = amountArrow.ToString();
        timer.text = FormatTime(time);
        currentTime = time;
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
            timer.text = FormatTime(currentTime);
        }

        isCounting = false;

        Debug.Log("Lose: Time's up!");
    }

    private string FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return $"{minutes:00}:{seconds:00}";
    }
}
