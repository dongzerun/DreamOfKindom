using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    private bool isPlayerTurn = false;
    private bool isEnemyTurn = false;
    public bool isBattleEnd = true;

    private float timeCounter;
    public float enemyTurnDuration;
    public float playerTurnDuration;

    public PlayerCharacter player;

    public ObjectEventSO playerTurnBegin;
    public ObjectEventSO enemyTurnBegin;
    public ObjectEventSO enemyTurnEnd;

    private void Update()
    {
        if (isBattleEnd)
            return;

        if (isEnemyTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= enemyTurnDuration)
            {
                timeCounter = 0;
                // end enemy turn
                EnemyTurnEnd();
                // start player turn
                isPlayerTurn = true;
                isEnemyTurn = false;
            }
        }

        if (isPlayerTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= playerTurnDuration)
            {
                timeCounter = 0;
                // start player turn
                PlayerTurnBegin();
                isPlayerTurn = false;
            }
        }
    }

    [ContextMenu("Game Start")]
    public void GameStart()
    { 
        isPlayerTurn=true;
        isEnemyTurn = false;
        isBattleEnd = false;
        timeCounter = 0;
    }

    public void PlayerTurnBegin() 
    {
        playerTurnBegin.RaiseEvent(null, this);
    }

    public void EnemyTurnBegin()
    { 
        isEnemyTurn = true;
        enemyTurnBegin.RaiseEvent(null, this);  
    }

    public void EnemyTurnEnd()
    {
        isEnemyTurn = false;
        enemyTurnEnd.RaiseEvent(null, this);
    }

    public void OnLoadRoomEvent(object data)
    {
        Room currentRoom = (Room)data;

        switch (currentRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnmy:
            case RoomType.Boss:
                GameStart();
                player.gameObject.SetActive(true);
                break;

            case RoomType.Shop:
            case RoomType.Treasure:
            case RoomType.RestRoom:
                player.gameObject.SetActive(true);
                player.GetComponent<PlayerAnimation>().SetSleepAnimation();
                break;
            default:
                break;
        }
    }

    public void StopTurnBaseSystem(object obj) 
    {
        isBattleEnd = true;
        player.gameObject.SetActive(false);
    }

    public void OnNewGameStart(object obj)
    { 
        player.OnNewGameStart(obj);
    }
}
