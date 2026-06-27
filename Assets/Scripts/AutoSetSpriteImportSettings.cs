using UnityEngine;
using UnityEditor;

public class AutoSetSpriteImportSettings : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        var importer = (TextureImporter)assetImporter;

        if (importer.textureType == TextureImporterType.Sprite)
        {
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.filterMode = FilterMode.Point;
            importer.mipmapEnabled = false;
            importer.spritePixelsPerUnit = 32;
        }
    }
}