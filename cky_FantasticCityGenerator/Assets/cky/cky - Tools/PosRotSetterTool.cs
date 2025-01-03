using System.Collections.Generic;
using UnityEngine;

namespace cky.Tools
{
    public class PosRotSetterTool : MonoBehaviour
    {
        [SerializeField] List<Transform> objectTransforms;
        [SerializeField] List<Transform> posRotTransforms;

        [SerializeField] bool useLocalTransforms;



        public void Set()
        {
            if (objectTransforms.Count != posRotTransforms.Count)
            {
                Debug.LogError("Object Transforms and Position/Rotation Transforms lists must be of the same length.");
                return;
            }

            var length = objectTransforms.Count;

            if (!useLocalTransforms)
            {
                for (int i = 0; i < length; i++)
                {
                    objectTransforms[i].SetPositionAndRotation(posRotTransforms[i].position, posRotTransforms[i].rotation);
                }
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    objectTransforms[i].SetLocalPositionAndRotation(posRotTransforms[i].localPosition, posRotTransforms[i].localRotation);
                }
            }
        }
    }
}