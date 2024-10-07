using UnityEngine;

public struct CardTransform
{
    public Vector3 pos;
    public Quaternion rotation;

    public CardTransform(Vector3 vec, Quaternion qua)
    { 
        pos = vec; 
        
        rotation = qua;
    }
}
