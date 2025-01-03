using System.Collections.Generic;
using UnityEngine;

namespace FCG.Pedestrian.Matrix
{
    public class P_MatrixCell : MonoBehaviour
    {
        [Space(5)]
        public int I;
        public int J;
        public List<P_MatrixCell> neighbours = new List<P_MatrixCell>();

        [Space(5)]
        public List<WpDataSpawnPedestrian> spawnPoints = new List<WpDataSpawnPedestrian>();

        bool _active;



        public void Open()
        {
            _active = true;
            //Debug.Log($"Opened - {I}:{J}");
        }

        public void Close()
        {
            _active = false;
            //Debug.Log($"Closed - {I}:{J}");
        }



        #region Gizmos

        private void OnDrawGizmos()
        {
            Gizmos.color = _active ? Color.yellow : Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(transform.localScale.x, 0f, transform.localScale.z));
        }

        #endregion
    }
}