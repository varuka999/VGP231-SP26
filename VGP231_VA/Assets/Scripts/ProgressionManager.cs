using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(0)]
public class ProgressionManager : MonoBehaviour
{
    private static ProgressionManager _instance;
    public static ProgressionManager Instance { get { return _instance; } }

    [SerializeField] private UnityEvent[] progressionEvents;
    private int progressionIndex = 0;

    [Header("DEBUG ONLY")]
    [SerializeField] private int startAtProgressionIndex;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        if(startAtProgressionIndex > 0)
        {
            GoToProgression(startAtProgressionIndex);
        }
    }

    public void IncrementProgression()
    {
        progressionEvents[progressionIndex]?.Invoke();
        ++progressionIndex;
        //Debug.Log($"Incremented Progression Index: {progressionIndex}");
    }

    private void GoToProgression(int GoToProgressionIndex)
    {
        for (int i = 0; i < GoToProgressionIndex; ++i)
        {
            progressionEvents[i].Invoke();
            ++progressionIndex;
        }
    }
}
