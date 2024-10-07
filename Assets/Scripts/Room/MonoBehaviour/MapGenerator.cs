using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public MapConfigSO mapConfig;
    public Room roomPrefab;
    public LineRenderer linePrefab;
    public float border;

    public MapLayoutSO mapLayout;

    private float screenHeight;
    private float screenWidth;
    private float columnWidth;
    private Vector3 generatePoint;
    private List<Room> rooms = new List<Room>();
    private List<LineRenderer> lines = new List<LineRenderer>();
    public List<RoomDataSO> roomDataList = new List<RoomDataSO>();
    private Dictionary<RoomType, RoomDataSO> roomDataDict = new Dictionary<RoomType, RoomDataSO>();

    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = Camera.main.aspect * screenHeight;
        columnWidth = screenWidth / (mapConfig.roomBlueprints.Count);

        foreach (var roomData in roomDataList)
        {
            Debug.Log("awake add " + roomData.roomType + " to roomDataDict");
            roomDataDict.Add(roomData.roomType, roomData);
        }
    }

    private void Start()
    {
        // CreateMap();
    }

    private void OnEnable()
    {
        if (mapLayout.mapRoomDataList.Count > 0)
        {
            LoadMap();
        }
        else
        {
            CreateMap();
        }
    }

    public void CreateMap()
    {
        List<Room> previousColumnRooms = new List<Room>();

        for (int column = 0; column < mapConfig.roomBlueprints.Count; column++)
        { 
            var blueprint = mapConfig.roomBlueprints[column];
            var amount = Random.Range(blueprint.min, blueprint.max);
            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);
            generatePoint = new Vector3 (-screenWidth/2 + border + column * columnWidth, startHeight, 0);
            var newPos = generatePoint;
            if (column == mapConfig.roomBlueprints.Count - 1)
            {
                newPos.x = screenWidth / 2 - border * 2;
            }

            var roomGapY = screenHeight / (amount + 1);

            List<Room> currentColumnRooms = new List<Room>();

            for (int i = 0; i < amount; i++)
            {
                if (column != 0 && column != mapConfig.roomBlueprints.Count-1)
                {
                    newPos.x = generatePoint.x + Random.Range(-border / 2, border / 2);
                }

                newPos.y = startHeight - roomGapY * i;
                var room = Instantiate(roomPrefab, newPos, Quaternion.identity, transform);
                if (column == 0)
                {
                    room.roomState = RoomState.Attainable;
                }
                else
                {
                    room.roomState = RoomState.Locked;
                }

                RoomType roomType = getRandomRoomType(mapConfig.roomBlueprints[column].roomType);
                RoomDataSO roomData = getRoomDataByType(roomType);

                room.SetupRoom(column, i, roomData);
                rooms.Add(room);
                currentColumnRooms.Add(room);
            }

            if (previousColumnRooms.Count > 0)
            {
                createConnections(previousColumnRooms, currentColumnRooms);
            }

            previousColumnRooms = currentColumnRooms;
        }

        SaveMap();
    }

    private void createConnections(List<Room> column1, List<Room> column2)
    {
        HashSet<Room> connectedColumn2Rooms = new HashSet<Room>();
        
        foreach (Room room in column1)
        {
            var targetRoom = connectToRandomRoom(room, column2, true);
            connectedColumn2Rooms.Add(targetRoom);
        }

        foreach (Room room in column2)
        { 
            if (!connectedColumn2Rooms.Contains(room))
            {
                connectToRandomRoom(room, column1, false);
            }
        }
    }

    private Room connectToRandomRoom(Room room, List<Room> column2, bool forward)
    {
        Room targetRoom;
        targetRoom = column2[Random.Range(0, column2.Count)];

        if (forward)
        {
            room.linkedTo.Add(new Vector2Int(targetRoom.column, targetRoom.line));
        }
        else
        {
            targetRoom.linkedTo.Add(new Vector2Int(room.column, room.line));
        }

        var line = Instantiate(linePrefab, transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);
        lines.Add(line);
        return targetRoom;
    }

    [ContextMenu(itemName:"ReGenerateRoom")]
    public void Reset()
    {
        foreach (var room in rooms)
        { 
            Destroy(room.gameObject);
        }
        rooms.Clear();

        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }
        lines.Clear();
    
        CreateMap();
    }

    private RoomDataSO getRoomDataByType(RoomType tp) 
    {
        return roomDataDict[tp];   
    }

    private RoomType getRandomRoomType(RoomType flags)
    {
        string[] typs = flags.ToString().Split(',');

        string randomType = typs[Random.Range(0, typs.Length)];

        RoomType result = (RoomType) System.Enum.Parse(typeof(RoomType), randomType);
        return result;
    }

    private void SaveMap()
    {
        mapLayout.mapRoomDataList = new();

        for (int i = 0; i < rooms.Count; i++)
        {
            var room = new MapRoomData()
            {
                x = rooms[i].transform.position.x,
                y = rooms[i].transform.position.y,
                column = rooms[i].column,
                line = rooms[i].line,
                roomData = rooms[i].roomData,
                roomState = rooms[i].roomState,
                linkTo = rooms[i].linkedTo,
            };
            mapLayout.mapRoomDataList.Add(room);
        }

        mapLayout.linePositionList = new();

        for (int i = 0; i < lines.Count; i++)
        {
            var line = new LinePosition()
            { 
                startPos = new SerializeVector3(lines[i].GetPosition(0)),
                endPos = new SerializeVector3(lines[i].GetPosition(1)),
            };
            mapLayout.linePositionList.Add(line);
        }
    }

    private void LoadMap()
    {
        rooms.Clear();
        for (int i = 0; i < mapLayout.mapRoomDataList.Count; i++)
        { 
            var mrd = mapLayout.mapRoomDataList[i];
            var newPos = new Vector3(mrd.x, mrd.y, 0);
            var newRoom = Instantiate(roomPrefab, newPos, Quaternion.identity, transform);
            newRoom.roomState = mrd.roomState;
            newRoom.SetupRoom(mrd.column, mrd.line, mrd.roomData);
            newRoom.linkedTo = mrd.linkTo;
            rooms.Add(newRoom);
        }

        lines.Clear();
        for (int i = 0; i < mapLayout.linePositionList.Count; i++)
        { 
            var lp = mapLayout.linePositionList[i];
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, lp.startPos.ToVector3());
            line.SetPosition(1, lp.endPos.ToVector3());
            lines.Add(line);
        }
    }
}
