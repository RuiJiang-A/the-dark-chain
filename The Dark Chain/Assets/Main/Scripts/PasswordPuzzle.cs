using Boopoo.Telemetry;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class PasswordPuzzle : MonoBehaviour
{
    private string _inputs;
    [SerializeField] private string _answer;
    [SerializeField] private bool _solved;

    [SerializeField] public UnityEvent PuzzleSolved;

    [UsedImplicitly]
    private void Awake()
    {
        PuzzleSolved.AddListener(PasswordPuzzle_OnPuzzleSolved);
    }

    [UsedImplicitly]
    public void UpdatePassword(string input)
    {
        if (input.Length > _answer.Length) return;
        _inputs = input;
    }

    public void Validate()
    {
        if (_solved) return;
        _solved = _inputs == _answer;
        if (_solved) PuzzleSolved?.Invoke();
    }

    public void PasswordPuzzle_OnPuzzleSolved()
    {
        TelemetryLogger.Log(this, "Puzzle Solved");
    }
}