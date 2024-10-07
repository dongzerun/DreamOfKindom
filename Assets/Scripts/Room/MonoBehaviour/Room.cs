using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int column;
    public int line;

    public RoomDataSO roomData;
    public RoomState roomState;

    private SpriteRenderer spriteRender;

    public ObjectEventSO loadRoomEvent;

    public List<Vector2Int> linkedTo = new();

    private void Awake()
    {
        spriteRender = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Click room now " + roomData.roomType);
        if (roomState == RoomState.Attainable)
        {
            loadRoomEvent.RaiseEvent(this, this);
        }
    }

    public void SetupRoom(int column, int line, RoomDataSO roomData)
    {
        this.column = column;
        this.line = line;
        this.roomData = roomData;

        spriteRender.sprite = roomData.roomIcon;
        spriteRender.color = roomState switch
        {
            RoomState.Locked => new Color(0.5f, 0.5f,0.5f,1f),
            RoomState.Visited => new Color(0.5f, 0.8f, 0.5f, 0.5f),
            RoomState.Attainable => Color.white,
            _ => throw new System.NotImplementedException(),
        };
    }
}
