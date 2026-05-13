using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager _instance;
    public static ObjectManager Instance { get { return _instance; } }

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

    public void EnableObject(GameObject gameObjectToEnable)
    {
        gameObjectToEnable.SetActive(true);
    }

    public void DisableObject(GameObject gameObjectToDisable)
    {
        gameObjectToDisable.SetActive(false);
    }
}
