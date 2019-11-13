using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
     public   Material material;
   private void OnRenderImage(RenderTexture source, RenderTexture dest){
        material.SetTexture ("_SpeedMap", WriteSpeedMap.speedMapRT);
        Graphics.Blit(source, dest, material,1);
    }
}
