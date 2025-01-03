using UnityEngine;

namespace cky.TrafficSystem
{
    public class TrafficCarChecker : MonoBehaviour
    {
        public bool CheckForTrafficCar(Vector3 position, float radius, LayerMask layersToBeAffected)
        {
            Collider[] colliders = Physics.OverlapSphere(position, radius, layersToBeAffected);

            if (colliders.Length > 0)
            {
                //// For Test
                //for (int i = 0;i<colliders.Length;i++)
                //{
                //    Debug.Log(colliders[i].name);
                //}
                return true;
            }

            return false;
        }
    }
}
