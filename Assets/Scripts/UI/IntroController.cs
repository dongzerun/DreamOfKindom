using System;
using UnityEngine;
using UnityEngine.Playables;

public class IntroController : MonoBehaviour
{
    public PlayableDirector director;

    public ObjectEventSO loadMenuEvent;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.stopped += OnPlayableDirectorStopped;
    }

    private void Update()
    {
        if (director.state != PlayState.Playing)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            director.Stop();
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        loadMenuEvent.RaiseEvent(null, this);
    }
}
