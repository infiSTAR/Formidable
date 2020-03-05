using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formidable
{

    public static class Settings
    {

        public static readonly string ApplicationName = "Formidable";
        public static readonly string TargetGameObject = "Application (Main Client)";

        public static readonly string DataDirectoryPath = @"D:\Temp\Formidable\data";

        public static readonly string SpecialLootItemsFilePath = Path.Combine(DataDirectoryPath, "special_loot_items.txt");
        public static readonly string FormidableAssetBundleFilePath = Path.Combine(DataDirectoryPath, "formidable_asset_bundle");

        public static readonly string ShaderFilePath = "assets/outline.shader";

        static Settings()
        {

        }

    }

}
