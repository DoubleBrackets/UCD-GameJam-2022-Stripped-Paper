using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class QuadLadderTesselate : AssetPostprocessor
{
    private float lineDist = 0.1f;
    private int vWidth = 20;
    private int vHeight = 20;

    void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
    {
        foreach(var sprite in sprites)
        {
            //QuadTesselate(sprite);
            //PerformLadderTesselation(sprite);
        }
    }

    private void QuadTesselate(Sprite targetSprite)
    {
        if (targetSprite == null) return;
        Vector2 size = targetSprite.rect.size;
        Vector2[] newVerts = new Vector2[vWidth * vHeight];

        int counter = 0;
        for(int y = 0;y < vHeight;y++)
        {
            float yT = (float)y / (vHeight - 1);
            float yPos = yT * size.y;
            if (y == 0)
                yPos += 1;
            else if (y == vHeight - 1)
                yPos -= 1;
            for(int x = 0;x < vWidth;x++)
            {
                float xT = (float)x / (vWidth - 1);
                float xPos = xT * size.x;
                newVerts[counter] = new Vector2(xPos, yPos);
                counter++;
            }
        }
        // tris
        int i = 0;
        ushort[] tris = new ushort[6 * (vWidth - 1)*(vHeight - 1)];
        for (int y = 0; y < vHeight - 1; y++)
        {
            for (int x = 0; x < vWidth - 1; x++)
            {
                int vishort = i + y;
                // 0, 2, 1 and 2 3 1
                tris[6 * i] = (ushort)vishort;
                tris[6 * i + 1] = (ushort)(vishort + vWidth);
                tris[6 * i + 2] = (ushort)(vishort + 1);

                tris[6 * i + 3] = (ushort)(vishort + vWidth);
                tris[6 * i + 4] = (ushort)(vishort + vWidth + 1);
                tris[6 * i + 5] = (ushort)(vishort + 1);
                i++;
            }
        }
        targetSprite.OverrideGeometry(newVerts, tris);
    }

    private void PerformLadderTesselation(Sprite targetSprite)
    {
        if (targetSprite == null) return;
        Vector2 size = targetSprite.rect.size;
        Vector2 worldSize = size / targetSprite.pixelsPerUnit;
        int lineCount = 100;
        Vector2[] newVerts = new Vector2[lineCount * 2];
        for(int i = 0;i < lineCount; i++)
        {
            float t = (float)i / (lineCount - 1);
            float miniOffset = (i % 4 == 0) ? 1f : 0f;
            newVerts[2*i] = new Vector2(miniOffset, t * size.y);
            newVerts[2*i+1] = new Vector2(size.x - miniOffset, t * size.y);
        }
        //tris
        ushort[] tris = new ushort[6 * (lineCount - 1)];
        for(int i = 0;i < tris.Length/6;i++)
        {
            int vishort = (2 * i);
            // 0, 2, 1 and 2 3 1
            tris[6*i] = (ushort)vishort;
            tris[6 * i + 1] = (ushort)(vishort+2);
            tris[6 * i + 2] = (ushort)(vishort+1);

            tris[6 * i + 3] = (ushort)(vishort+2);
            tris[6 * i + 4] = (ushort)(vishort+3);
            tris[6 * i + 5] = (ushort)(vishort+1);
        }
        Debug.Log(newVerts.Length);
        targetSprite.OverrideGeometry(newVerts, tris);
    }
}
