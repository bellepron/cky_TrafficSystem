using UnityEngine;

namespace FCG
{
    public class Crosswalker : MonoBehaviour
    {
        Collider[] _crosswalkColliders;
        public Crosswalk OnCrosswalk;
        public LayerMask crosswalkLayerMask;
        public bool IsRunning;

        private void Awake()
        {
            _crosswalkColliders = new Collider[1];
        }

        private void FixedUpdate()
        {
            var scale = transform.localScale;
            var crosswalkCount = Physics.OverlapBoxNonAlloc(transform.position, scale / 2, _crosswalkColliders, Quaternion.identity, crosswalkLayerMask);

            if (_crosswalkColliders[0] == null)
            {
                OnCrosswalk = null;
                IsRunning = false;
            }
            else
            {
                OnCrosswalk = _crosswalkColliders[0].GetComponent<Crosswalk>();
                OnCrosswalk.PedestrianHere(); // Veee yaþýyor?

                if (!OnCrosswalk.PedestrianGreen && !IsRunning)
                {
                    IsRunning = true;
                }

                _crosswalkColliders[0] = null;
            }
        }

        void OnDrawGizmos()
        {
            if (OnCrosswalk == null)
            {
                Gizmos.color = Color.blue;
            }
            else
            {
                if (OnCrosswalk.PedestrianGreen)
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.magenta;
                }
            }

            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, transform.localScale);
            //Gizmos.matrix = Matrix4x4.identity;
        }

    }
}