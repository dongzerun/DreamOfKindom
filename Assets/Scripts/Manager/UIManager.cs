using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gamePlayPanel;
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
    public GameObject pickCardPanel;
    public GameObject restRoomPanel;

    public void OnLoadRoomEvent(object data)
    {
        Room currentRoom = (Room)data;

        switch (currentRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnmy:
            case RoomType.Boss:
                gamePlayPanel.SetActive(true);
                break;

            case RoomType.Shop:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.RestRoom:
                restRoomPanel.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void HideAllPanels()
    {
        gamePlayPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        restRoomPanel.SetActive(false);
    }

    public void OnGameWinEvent()
    {
        gamePlayPanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }

    public void OnGameOverEvent()
    {
        gamePlayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void OnPickCardEvent()
    {
        pickCardPanel.SetActive(true);
    }

    public void OnFinishedPickCardEvent()
    {
        pickCardPanel.SetActive(false);
    }
}
