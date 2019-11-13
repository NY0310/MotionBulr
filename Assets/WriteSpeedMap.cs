using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteSpeedMap : MonoBehaviour {
    static public RenderTexture speedMapRT;
    [SerializeField]
    Material material;
    Matrix4x4 prevMatrix;
    // Start is called before the first frame update
    void Start () {
        speedMapRT = new RenderTexture (Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat);
    }

    // Update is called once per frame
    void Update () {

        material.SetMatrix ("_PrevMatrix", prevMatrix);
        //material.SetTexture ("_SpeedMap", speedMapRT);
        Graphics.Blit (null, speedMapRT, material, 0);
        prevMatrix = GetMVPMatrix ();
    }

   private void OnRenderImage(RenderTexture source, RenderTexture dest){
        material.SetTexture ("_SpeedMap", speedMapRT);
        Graphics.Blit (source, dest, material, 1);
    }

    Matrix4x4 GetMVPMatrix () {
        Camera renderCamera = Camera.main;
        Matrix4x4 m = GetComponent<Renderer> ().localToWorldMatrix;
        Matrix4x4 v = renderCamera.worldToCameraMatrix;
        Matrix4x4 p = renderCamera.cameraType == CameraType.SceneView ? GL.GetGPUProjectionMatrix (renderCamera.projectionMatrix, true) : renderCamera.projectionMatrix;
        return p * v * m;
    }
}