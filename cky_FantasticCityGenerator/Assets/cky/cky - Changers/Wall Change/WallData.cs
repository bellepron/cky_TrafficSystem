using cky.DataSaving;
using UnityEngine;

namespace cky.Changer.WallChange
{
    [System.Serializable]
    public class WallTypeData
    {
        public WallTypes wallType;

        public WallTypeData(WallTypes wallType)
        {
            this.wallType = wallType;
        }
    }

    [CreateAssetMenu(fileName = "Wall Data", menuName = "Datas/Wall Data")]
    public class WallData : ScriptableObjectSaverAbstract
    {
        public WallTypes defaultWallType = WallTypes.Type1;
        public WallTypeData[] wallTypeDatas;

        public override void SetDefaults()
        {
            foreach (var data in wallTypeDatas)
            {
                data.wallType = defaultWallType;
            }

            Save();
        }
    }
}