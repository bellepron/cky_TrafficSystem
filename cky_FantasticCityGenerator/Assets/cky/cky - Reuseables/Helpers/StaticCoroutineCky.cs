using System.Collections;
using UnityEngine;

namespace cky.Reuseables.Helpers
{
    public static class StaticCoroutineCky
    {
        public class StaticCoroutineMono : MonoBehaviour { }

        private static StaticCoroutineMono _staticCoroutine;

        private static StaticCoroutineMono StaticCorotine
        {
            get
            {
                if (_staticCoroutine == null)
                {
                    GameObject gameObject = new GameObject("Static Coroutine");

                    _staticCoroutine = gameObject.AddComponent<StaticCoroutineMono>();
                }
                return _staticCoroutine;
            }
        }

        public static void Perform(IEnumerator routine)
        {
            StaticCorotine.StartCoroutine(routine);
        }
    }
}