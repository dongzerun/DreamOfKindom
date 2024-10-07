using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(BaseEventSO<>))]
public class BaseEventSOEditor<T> : Editor
{
    private BaseEventSO<T> baseEventSO;

    private void OnEnable()
    {
        if (baseEventSO == null)
             baseEventSO = target as BaseEventSO<T>;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var listeners = GetListeners();
        EditorGUILayout.LabelField("Subscribers count: " + listeners.Count);

        foreach (var listener in listeners)
        {
            EditorGUILayout.LabelField(listener.ToString());
        }
    }

    private List<MonoBehaviour> GetListeners()
    {
        List<MonoBehaviour> listeners = new();
        if (baseEventSO == null || baseEventSO.OnEventRaised == null) 
        { 
            return listeners;
        }

        var subscribers = baseEventSO.OnEventRaised.GetInvocationList();
        foreach ( var subscriber in subscribers )
        {
            var obj = subscriber.Target as MonoBehaviour;
            if (!listeners.Contains(obj))
                listeners.Add(obj);
        }
        return listeners;
    }
}
