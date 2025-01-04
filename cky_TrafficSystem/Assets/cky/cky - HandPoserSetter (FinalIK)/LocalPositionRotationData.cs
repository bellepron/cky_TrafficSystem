using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LocalPositionRotation
{
    public Vector3 localPosition;
    public Quaternion localRotation;

    public LocalPositionRotation(Vector3 localPosition, Quaternion localRotation)
    {
        this.localPosition = localPosition;
        this.localRotation = localRotation;
    }
}

[CreateAssetMenu(fileName = "Local Position Rotation Data", menuName = "Datas/Local Position Rotation Data")]
public class LocalPositionRotationData : ScriptableObject
{
    public List<LocalPositionRotation> localPositionRotations = new List<LocalPositionRotation>();
}