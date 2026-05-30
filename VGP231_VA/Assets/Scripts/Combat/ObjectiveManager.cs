using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private static ObjectiveManager instance;
    public static ObjectiveManager Instance { get { return instance; } }

    [SerializeField] private int objectiveCount = 0;
    public int ObjectiveCount => objectiveCount;

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

    public void ResetObjectiveCount()
    {
        objectiveCount = 0;
    }
}
