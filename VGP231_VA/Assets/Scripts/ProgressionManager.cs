using UnityEngine;
using UnityEngine.Events;

public class ProgressionManager : MonoBehaviour
{
    private static ProgressionManager _instance;
    public static ProgressionManager Instance { get { return _instance; } }

    [SerializeField] private DelayableUnityEventArray[] progressionEventsDelayArray;
    private int progressionIndex = 0;

    [SerializeField] private bool startWithProgression = false;

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
    }

    private void Start()
    {
        if (startAtProgressionIndex > 0)
        {
            GoToProgression(startAtProgressionIndex);
        }
        else if (startWithProgression)
        {
            IncrementProgression();
        }
    }

    public void IncrementProgression()
    {
        RunDelayableUnityEventAtIndex(progressionIndex);
        ++progressionIndex;
    }

    private void GoToProgression(int GoToProgressionIndex)
    {
        for (int i = 0; i < GoToProgressionIndex; ++i)
        {
            IncrementProgression();
        }
    }

    public void RunDelayableUnityEventAtIndex(int index)
    {
        for(int i = 0; i < progressionEventsDelayArray[index].delaybleUnityEvents.Length; ++i)
        {
            DelayableUnityEventUtility.Invoke(this, progressionEventsDelayArray[index].delaybleUnityEvents[i]);
        }
    }
}
