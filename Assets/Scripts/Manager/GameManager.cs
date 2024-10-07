using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public MapLayoutSO mapLayout;

    public List<EnemyCharacter> aliveEnemyList = new();

    public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameOverEvent;

    public void UpdateMapLayoutData(object value)
    {
        aliveEnemyList.Clear();

        if (mapLayout.mapRoomDataList.Count == 0)
        {
            return;
        }


        var roomVector = (Vector2Int)value;
        var currentRoom = mapLayout.mapRoomDataList.Find(r => r.column == roomVector.x && r.line == roomVector.y);

        currentRoom.roomState = RoomState.Visited;

        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(r=>r.column == roomVector.x);

        foreach (var room in sameColumnRooms)
        {
            if (room.line != roomVector.y)
                room.roomState = RoomState.Locked;
        }

        foreach (var link in currentRoom.linkTo)
        {
            var linkedRoom = mapLayout.mapRoomDataList.Find(r=>r.column == link.x && r.line == link.y);
            linkedRoom.roomState = RoomState.Attainable;
        }
    }

    public void OnRoomLoadedEvent(object obj)
    {
        var enemies = FindObjectsByType<EnemyCharacter>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        Debug.Log("try to add enemy to aliveEnemyList");
        foreach (var enemy in enemies)
        {
            Debug.Log("add enemy to aliveEnemyList");
            aliveEnemyList.Add(enemy);
        }
    }

    public void OnCharacterDeadEvent(object obj)
    {
        Debug.Log("receive character dead now ");
        if (obj is PlayerCharacter)
        {
            // 发送失败通知
            EventDelayAction(gameOverEvent);
            Debug.Log("player dead event trigger");
            gameOverEvent.RaiseEvent(null, this);
        }

        if (obj is Boss)
        {
            // 发送获胜通知
            EventDelayAction(gameWinEvent);
            Debug.Log("boss dead event trigger");
            gameWinEvent.RaiseEvent(null, this);
        } 
        else  if (obj is EnemyCharacter)
        {
            aliveEnemyList.Remove(obj as EnemyCharacter);
            if (aliveEnemyList.Count == 0)
            {
                // 发送获胜通知
                EventDelayAction(gameWinEvent);
                Debug.Log("enemy dead event trigger");
                gameWinEvent.RaiseEvent(null, this);
            }
        }


    }

    IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("yield event delay action");
        eventSO.RaiseEvent(null, this);
    }

    public void OnNewGameStart()
    { 
        mapLayout.linePositionList.Clear();
        mapLayout.mapRoomDataList.Clear();
    }
}
