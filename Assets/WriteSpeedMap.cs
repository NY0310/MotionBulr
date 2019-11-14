using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteSpeedMap : MonoBehaviour {
    static public RenderTexture speedMapRT;
    [SerializeField]
    Material material;
    Matrix4x4 prevMatrix;

    Camera renderCamera;
    // Start is called before the first frame update
    void Start () {
        speedMapRT = new RenderTexture (Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat);
        renderCamera = Camera.main;
    }

    // Update is called once per frame
    void Update () {

        material.SetMatrix ("_PrevMatrix", prevMatrix);
        material.SetMatrix ("_NowMatrix", GetMVPMatrix());
        material.SetTexture ("_SpeedMap", speedMapRT);
        speedMapRT.Create();
        Graphics.Blit (null, speedMapRT, material, 0);
        prevMatrix = GetMVPMatrix ();
    }

    Matrix4x4 GetMVPMatrix () {
        Matrix4x4 m = transform.localToWorldMatrix;
        Matrix4x4 v = renderCamera.worldToCameraMatrix;
        Matrix4x4 p = renderCamera.cameraType == CameraType.SceneView ? GL.GetGPUProjectionMatrix (renderCamera.projectionMatrix, true) : renderCamera.projectionMatrix;
        return p * v * m;
    }
}