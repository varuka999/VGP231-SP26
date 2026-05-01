using UnityEngine;

public enum Rooms
{
    Hall,
    Kitchen
}

[DefaultExecutionOrder(-14)]
public class RoomManager : MonoBehaviour
{
    private static RoomManager _instance;
    public static RoomManager Instance { get { return _instance; } }

    private Rooms currentRoom;
    bool[] roomsTravelled = new bool[System.Enum.GetValues(typeof(Rooms)).Length];

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SetCurrentRoom(int roomIndex)
    {
        currentRoom = (Rooms)roomIndex;
        roomsTravelled[roomIndex] = true;

        MusicManager.Instance.PlayRoomAmbience(roomIndex);
    }
}
