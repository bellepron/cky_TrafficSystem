using cky.DataSaving;
using UnityEngine;

namespace cky.Changer.FloorChange
{
    [System.Serializable]
    public class FloorTypeData
    {
        public FloorTypes floorType;

        public FloorTypeData(FloorTypes floorType)
        {
            this.floorType = floorType;
        }
    }

    [CreateAssetMenu(fileName = "Floor Data", menuName = "Datas/Floor Data")]
    public class FloorData : ScriptableObjectSaverAbstract
    {
        public FloorTypes defaultFloorType = FloorTypes.Closed;
        public FloorTypeData[] floorTypeDatas;

        public override void SetDefaults()
        {
            foreach (var data in floorTypeDatas)
            {
                data.floorType = defaultFloorType;
            }

            Save();
        }
    }
}