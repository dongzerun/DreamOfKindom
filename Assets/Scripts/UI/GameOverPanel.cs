using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverPanel : MonoBehaviour
{
    private Button returnButton;
    public ObjectEventSO loadMenuEvent;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        returnButton = root.Q<Button>("ReturnButton");
        returnButton.clicked += BackToStart;
    }

    private void BackToStart()
    {
        loadMenuEvent.RaiseEvent(null, this);
    }
}
