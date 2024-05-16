using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PointCloudRenderer : MonoBehaviour
{
    private float _middlePosition;
    private Texture2D _texColor;
    private Texture2D _texPosScale;
    public VisualEffect _vfx;
    public DataPointsRenderer DPR;
    
    private uint _resolution = 2048;

    public float particalSize;
    private bool _toUpdate;
    private uint _particalCount = 0;

    private void Start()
    {
        _vfx = GetComponent<VisualEffect>();
        _middlePosition = DPR.RederingArea / 2;
    }

    private void Update()
    {
        if (_toUpdate)
        {
            _toUpdate = false;
            
            _vfx.Reinit();
            _vfx.SetUInt(Shader.PropertyToID("ParticalCount"), _particalCount);
            _vfx.SetTexture(Shader.PropertyToID("TexColor"), _texColor);
            _vfx.SetTexture(Shader.PropertyToID("TexPosScale"), _texPosScale);
            _vfx.SetUInt(Shader.PropertyToID("Resolution"), _resolution);
        }
    }

    public void SetParticals(Vector3[] positions, float[] scales, Color[] colors)
    {
        _texColor = new Texture2D(positions.Length > (int)_resolution ? (int)_resolution : positions.Length,
            Mathf.Clamp(positions.Length / (int)_resolution, 1, (int)_resolution), TextureFormat.RGBAFloat, false);
        _texPosScale = new Texture2D(positions.Length > (int)_resolution ? (int)_resolution : positions.Length,
            Mathf.Clamp(positions.Length / (int)_resolution, 1, (int)_resolution), TextureFormat.RGBAFloat, false);

        int texWidht = _texColor.width;
        int texHeight = _texColor.height;

        for (int y = 0; y < texHeight; y++)
        {
            for (int x = 0; x < texWidht; x++)
            {
                int index = x + y * texWidht;
                _texColor.SetPixel(x,y, colors[index]);
                var data = new Color(positions[index].x -_middlePosition, positions[index].y -_middlePosition , positions[index].z -_middlePosition, scales[index] * particalSize);
                _texPosScale.SetPixel(x, y, data);
            }
        }
        
        _texColor.Apply();
        _texPosScale.Apply();
        _particalCount = (uint)positions.Length;
        _toUpdate = true;
    }
}
