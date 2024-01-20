using System.Collections.Generic;
using UnityEngine;

namespace DefaultSetting
{
    [ExcelAsset]
    public class ExImporterScript : ScriptableObject
    {
        public List<ExImporterEntity> Save;
    }
}
