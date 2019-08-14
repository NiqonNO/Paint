using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RenderTextureCreator
{
    public static RenderTexture CreateRenderTexture(Color initialColor, int width, int height, int depth)
    {
        RenderTexture ttr = new RenderTexture(width, height, depth);
        ttr.enableRandomWrite = true;
        RenderTexture.active = ttr;
        GL.Clear(true, true, initialColor);
        RenderTexture.active = null;

        return ttr;
    }
}
