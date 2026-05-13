using UnityEngine;
using UnityEngine.Events;

public class ProgressionManager : MonoBehaviour
{
    private static ProgressionManager _instance;
    public static ProgressionManager Instance { get { return _instance; } }

    [SerializeField] private UnityEvent[] progressionEvents;
    private int progressionIndex = 0;

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

    public void IncrementProgression()
    {
        progressionEvents[progressionIndex]?.Invoke();
        ++progressionIndex;
    }
}
