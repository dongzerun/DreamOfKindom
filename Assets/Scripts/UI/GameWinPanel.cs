using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameWinPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button pickCardButton;
    private Button backToMapButton;

    public ObjectEventSO loadMapEvent;
    public ObjectEventSO pickCardEvent;

    private void Awake()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        pickCardButton = rootElement.Q<Button>("PickCardButton");
        backToMapButton = rootElement.Q<Button>("BackToMenuButton");

        backToMapButton.clicked += OnBackToMenuButtonClicked;
        pickCardButton.clicked += OnPickCardButtonClicked;
    }

    private void OnPickCardButtonClicked()
    {
        pickCardEvent.RaiseEvent(null, this);
    }

    private void OnBackToMenuButtonClicked()
    {
        loadMapEvent.RaiseEvent(null, this);
    }

    public void OnFinishedPickCardEvent()
    {
        //pickCardButton.SetEnabled(false);
        pickCardButton.style.display = DisplayStyle.None;
    }
}
