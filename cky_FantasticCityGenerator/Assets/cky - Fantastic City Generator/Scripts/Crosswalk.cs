using System;
using UnityEngine;

namespace FCG
{
    public class Crosswalk : MonoBehaviour
    {
        //[SerializeField] TrafficCar trafficLight;

        public bool CarPass;
        public bool PedestrianGreen;

        bool IsTherePedestrianInHere;

        public Action pRun, pWalk;
        public int howManyPedestrianHereInThisFrame;

        public void PedestrianGreenLight(bool b)
        {
            if (b)
            {
                PedestrianGreen = true;
                CarPass = false;
            }
            else
            {
                PedestrianGreen = false;
            }
        }



        void FixedUpdate()
        {
            IsTherePedestrianInHere = howManyPedestrianHereInThisFrame > 0 ? true : false;

            if (!PedestrianGreen && !CarPass)
            {
                if (!IsTherePedestrianInHere)
                {
                    CarPass = true;
                }
            }

            howManyPedestrianHereInThisFrame = 0;
        }

        public void PedestrianHere()
        {
            howManyPedestrianHereInThisFrame++;
        }

        void OnDrawGizmos()
        {
            if (PedestrianGreen)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                if (IsTherePedestrianInHere)
                {
                    Gizmos.color = Color.magenta;
                }
                else
                {
                    Gizmos.color = Color.blue;
                }
            }

            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, transform.localScale);
            //Gizmos.matrix = Matrix4x4.identity;
        }

    }
}
