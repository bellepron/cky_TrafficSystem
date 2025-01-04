using UnityEditor;
using UnityEngine;

public class HandPoserSetter : MonoBehaviour
{
    public LocalPositionRotationData handPoserData;
    int counter;



    public void Save()
    {
        counter = 0;
        handPoserData.localPositionRotations.Clear();

        SaveChilds(transform);
    }

    private void SaveChilds(Transform parent)
    {
        handPoserData.localPositionRotations.Add(new LocalPositionRotation(parent.localPosition, parent.localRotation));

        counter++;

        foreach (Transform child in parent)
            SaveChilds(child);
    }



    public void Load()
    {
        counter = 0;
        LoadChilds(transform);

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    private void LoadChilds(Transform parent)
    {
        if (counter != 0)
        {
            var data = handPoserData.localPositionRotations[counter];

            parent.transform.localPosition = data.localPosition;
            parent.transform.localRotation = data.localRotation;
        }

        counter++;

        foreach (Transform child in parent)
            LoadChilds(child);
    }
}
