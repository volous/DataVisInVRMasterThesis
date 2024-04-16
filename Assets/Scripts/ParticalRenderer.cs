using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[RequireComponent(typeof(ParticleSystem))]
public class ParticalRenderer : MonoBehaviour
{
    [Header("Visuals")] 
    public float size = 1;
    
    [Space]
    [Header("Partical system")]
    public ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] _voxels;
    private bool _voxelsUpdate = false;

    void Start()
    {
        _particleSystem.GetComponent<ParticleSystem>();

        Vector3[] positions = new Vector3[] { new Vector3(0, 0, 0), new Vector3(5, 5, 5) };
        Color[] color = new Color[] { new Color(0, 0, 0), new Color(1, 1, 1) };
        float[] scales = new float[] { 0.1f, 0.2f };
        
        SetVoxels(positions, scales,color);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_voxelsUpdate)
        {
            _particleSystem.SetParticles(_voxels, _voxels.Length);
            _voxelsUpdate = false;
        }
    }

    public void SetVoxels(Vector3[] positions, float[] scales, Color[] colors)
    {
        _voxels = new ParticleSystem.Particle[positions.Length];

        Parallel.For(0, positions.Length, i =>
        {
            ParticleSystem.Particle voxel = new ParticleSystem.Particle
            {
                position = positions[i],
                startSize = scales[i] * size,
                startColor = colors[i]
            };
            _voxels[i] = voxel;
        });

        _voxelsUpdate = true;
    }
}
