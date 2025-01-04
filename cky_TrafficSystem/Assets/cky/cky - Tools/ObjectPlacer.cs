using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] Transform startTr;
    [SerializeField] Transform endTr;
    [SerializeField] float padding;
    [SerializeField] Transform[] objects;

    private void OnValidate()
    {
        PlaceObjectsBetween();
    }

    public void PlaceObjectsBetween()
    {
        if (objects.Length < 2) return;

        Vector3 startPosition = startTr.position;
        Vector3 endPosition = endTr.position;

        Vector3 direction = (endPosition - startPosition).normalized;
        float totalDistance = Vector3.Distance(startPosition, endPosition);

        float atualTotalDistance = totalDistance - padding * 2;
        float spacing = atualTotalDistance / (objects.Length - 1);

        var alignStartPosition = startPosition + direction * padding;
        objects[0].position = alignStartPosition;
        objects[^1].position = endPosition - direction * padding;

        for (int i = 1; i < objects.Length - 1; i++)
        {
            objects[i].position = alignStartPosition + i * direction * spacing;
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
