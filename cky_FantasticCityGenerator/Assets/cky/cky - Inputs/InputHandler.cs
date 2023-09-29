using UnityEngine;

namespace cky.Inputs
{
    public class InputHandler : MonoBehaviour
    {
        [field: SerializeField] private LayerMask LayerMask { get; set; }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out raycastHit, 100f, LayerMask))
                {
                    if (raycastHit.transform != null)
                    {
                        Execute(raycastHit.transform, raycastHit.point);
                    }
                }
            }
        }

        private void Execute(Transform clickedObjectTr, Vector3 clickedPosition)
        {
            if (clickedObjectTr.TryGetComponent<IClickable>(out var iClickable))
            {
                iClickable.OnClick(clickedPosition);
            }
        }
    }
}