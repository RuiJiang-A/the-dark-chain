using Boopoo.Telemetry;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialogTextRef = null;
    [SerializeField] private CanvasGroup _canvasGroup = null;
    private Tween _animationProgress;

    [SerializeField] private string[] _temp_safe = null;
    bool _temp_first_time_see_safe = true;
    [SerializeField] private string[] _temp_clue = null;
    int _temp_clue_index = 0;
    bool _temp_first_time_see_clue = true;

    [SerializeField] private string[] _dialogs = null;

    [SerializeField] float _lastHintTime;

    [UsedImplicitly]
    public void DisplayDialog(string dialogText)
    {
        DisplayDialog(dialogText, 3);
    }

    [UsedImplicitly]
    public void DisplayDialog(string dialogText, float waitTimeBeforeFadeOut)
    {
        _animationProgress?.Kill();

        _canvasGroup.gameObject.SetActive(true);
        _dialogTextRef.text = dialogText;

        _animationProgress = _canvasGroup.DOFade(1, 0.1f).OnComplete(() =>
        {
            _animationProgress = _canvasGroup.DOFade(0, 0.1f)
                .SetDelay(waitTimeBeforeFadeOut)
                .OnComplete(() =>
                {
                    _canvasGroup.gameObject.SetActive(false);
                    _animationProgress = null;
                    _canvasGroup.alpha = 1.0f;
                });
        });
    }

    [UsedImplicitly]
    public void ShowTheDialog(int index)
    {
        DisplayDialog(_dialogs[index], 3);
    }

    [UsedImplicitly]
    public void ShowSafe()
    {
        if (Time.time - _lastHintTime < 5.0f && !_temp_first_time_see_safe) return;
        int index = Random.Range(0, _temp_safe.Length);
        DisplayDialog(_temp_safe[index]);
        _lastHintTime = Time.time;
        _temp_first_time_see_safe = false;
    }

    [System.Serializable]
    public struct ShowClueEventData
    {
        public string puzzleName;
        public string clueType;
        public int index;
    }

    [UsedImplicitly]
    public void ShowClue()
    {
        if (Time.time - _lastHintTime < 5.0f && !_temp_first_time_see_clue) return;
        int index = _temp_clue_index;
        DisplayDialog(_temp_clue[index], 5);
        _temp_clue_index++;
        if (_temp_clue_index > _temp_clue.Length) _temp_clue_index = _temp_clue.Length;
        TelemetryLogger.Log(this, "Show Clue", new ShowClueEventData
        {
            puzzleName = "Butterfly #1",
            clueType = "Dialog",
            index = _temp_clue_index,
        });
        _lastHintTime = Time.time;
        _temp_first_time_see_clue = false;
    }
}