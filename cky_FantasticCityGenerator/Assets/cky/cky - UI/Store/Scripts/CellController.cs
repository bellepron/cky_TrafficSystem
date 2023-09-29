using UnityEngine;

namespace cky.UI.Store
{
    public class CellController : MonoBehaviour
    {
        private Item itemClicked;
        private Item itemDraggedOn;
        private Cell cellClicked;
        private Cell cellDraggedOn;

        private Cell cellPointerCurrentlyOn;

        public void CellPointerCurrentlyOn(Cell cell)
        {
            cellPointerCurrentlyOn = cell;
        }

        public void SetCurrentItem(Cell cell)
        {
            cellClicked = cell;
            itemClicked = cell.Item;
        }

        public void DropItemToCell(Cell cell)
        {
            cellDraggedOn = cell;
            itemDraggedOn = cell.Item;

            if (itemDraggedOn == null)
            {
                itemClicked.transform.SetParent(cellDraggedOn.transform, false);
                cellDraggedOn.ItemChangedEffect();

                Debug.LogWarning($"This item changed cell: {itemClicked.name}");
            }
            else
            {
                if (itemClicked.ItemType == itemDraggedOn.ItemType)
                {
                    itemClicked.transform.SetParent(cellDraggedOn.transform, false);
                    cellDraggedOn.ItemChangedEffect();

                    itemDraggedOn.transform.SetParent(cellClicked.transform, false);
                    cellClicked.ItemChangedEffectWhenDraggedOnCellItemComes();

                    Debug.LogWarning($"These items changed cells: {itemClicked.name} - {itemDraggedOn.name}");
                }
                else
                {
                    Debug.LogWarning($"Different type items!");
                }
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (cellPointerCurrentlyOn != null)
                    if (cellClicked.IsFull && cellClicked != cellPointerCurrentlyOn)
                        DropItemToCell(cellPointerCurrentlyOn);

                if (itemClicked) itemClicked.transform.localPosition = Vector3.zero;
                if (itemDraggedOn) itemDraggedOn.transform.localPosition = Vector3.zero;

                cellClicked = null;
                itemClicked = null;
                cellDraggedOn = null;
                itemDraggedOn = null;
                cellPointerCurrentlyOn = null;
            }

            if (itemClicked != null) itemClicked.transform.position = Input.mousePosition;
        }
    }
}