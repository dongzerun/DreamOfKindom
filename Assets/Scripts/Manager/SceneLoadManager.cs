using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public AssetReference map;
    public AssetReference menu;
    public AssetReference intro;

    public FadePanel fadePanel;

    private AssetReference currentScene;

    private Vector2Int currentRoomVector;
    private Room currentRoom;

    public ObjectEventSO afterRoomLoadedEvent;
    public ObjectEventSO updateRoomEvent;

    private void Awake()
    {
        currentRoomVector = Vector2Int.one * -1;
        //LoadMenu();
        LoadIntro();
    }

    public async void OnLoadRoomEvent(object data)
    {
        if (data is Room)
        {
            currentRoom = (Room)data;
            Debug.Log("load room type is: " + currentRoom.roomData.roomType);

            currentScene = currentRoom.roomData.sceneToLoad;

            currentRoomVector = new(currentRoom.column, currentRoom.line);
        }

        // unload current scene
        await UnLoadSceneTask();
        // load room now
        await LoadSceneTask();

        afterRoomLoadedEvent.RaiseEvent(currentRoom, this);
    }


    private async Awaitable LoadSceneTask()
    {
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        await s.Task;

        if (s.Status == AsyncOperationStatus.Succeeded)
        {
            fadePanel.FadeOut(0.2f);
            //fadePanel.FadeIn(0.4f);
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    private async Awaitable UnLoadSceneTask()
    {
        fadePanel.FadeIn(0.4f);
        //fadePanel.FadeOut(0.2f);
        await Awaitable.WaitForSecondsAsync(0.45f);
        await Awaitable.FromAsyncOperation(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));
    }

    public async void LoadMap(object data)
    {
        Debug.Log("SceneLoadManager load Map now!");
        await UnLoadSceneTask();
        if (currentRoomVector != Vector2Int.one * -1)
        { 
            updateRoomEvent.RaiseEvent(currentRoomVector, this);
        }
        currentScene = map;
        await LoadSceneTask();
    }

    public async void LoadMenu()
    {
        if (currentScene != null)
            await UnLoadSceneTask();

        currentScene = menu;
        await LoadSceneTask();
    }

    public async void LoadIntro()
    {
        if (currentScene != null)
            await UnLoadSceneTask();

        currentScene = intro;
        await LoadSceneTask();
    }
}
