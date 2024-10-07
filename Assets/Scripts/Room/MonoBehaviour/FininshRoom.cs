using UnityEngine;

public class FininshRoom : MonoBehaviour
{
    public ObjectEventSO loadMapEvent;

    private void OnMouseDown()
    {
        loadMapEvent.RaiseEvent(null, this);
    }
}
