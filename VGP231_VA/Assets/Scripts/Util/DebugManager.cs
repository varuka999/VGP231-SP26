using UnityEngine;

public class DebugManager : MonoBehaviour
{
    private static DebugManager instance;
    public static DebugManager Instance { get { return instance; } }

    [SerializeField] private bool isDebug = false;
    public bool IsDebug => isDebug;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
