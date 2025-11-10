using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class HashGrid : Node3D
{
  [Export]
  private Mesh _instanceMesh;

  private int _resolution = 16;
  private float _invResolution = 1f/16;

  [Export(PropertyHint.Range, "1,512")]
  public int Resolution {
    get => _resolution;
    set {
      _resolution = value;
      _invResolution = 1f / _resolution;
      UpdateMesh();
    }
  }

  private int _seed = 0;
  [Export]
  public int Seed {
    get => _seed;
    set {
      _seed = value;
      UpdateMesh();
    }
  }
  
  private MultiMeshInstance3D _instanceMultiMesh;
  private uint[] _hashes;

  public override void _Ready()
  {
    int length = _resolution * _resolution;
    var multiMesh = new MultiMesh
    {
      TransformFormat = MultiMesh.TransformFormatEnum.Transform3D,
      UseColors = true,
      InstanceCount = length,
      Mesh = _instanceMesh,
    };
    _instanceMultiMesh = new MultiMeshInstance3D
    {
      Multimesh = multiMesh
    };
    AddChild(_instanceMultiMesh);

    UpdateMesh();
  }
  
  private void ComputeHashes()
  {
    var xxHash = SmallXXHash.Seed(_seed);
    for (int i = 0; i < _resolution; i++)
    {
      for (int j = 0; j < _resolution; j++)
      {
        int idx = i * _resolution + j;
        int v = (int)Math.Floor(_invResolution * idx);
        int u = idx - _resolution * v - _resolution / 2;
        v -= _resolution / 2;

        _hashes[idx] = xxHash.Eat(u).Eat(v);
      }
    }
  }

  private void UpdateMesh()
  {
    int length = _resolution * _resolution;

    _hashes = new uint[length];
    ComputeHashes();

    var multiMesh = new MultiMesh
    {
      TransformFormat = MultiMesh.TransformFormatEnum.Transform3D,
      UseColors = true,
      InstanceCount = length,
      Mesh = _instanceMesh,
    };
    _instanceMultiMesh.Multimesh = multiMesh;

    float spacing = 1.0f;
    for (int i = 0; i < _resolution; i++)
    {
      for (int j = 0; j < _resolution; j++)
      {
        int idx = i * _resolution + j;

        var transform = Transform3D.Identity;
        transform.Origin = new Vector3(
            i * spacing - (_resolution * spacing) / 2.0f,
            (1.0f / 255.0f) * (_hashes[idx] >> 24) - 0.5f,
            j * spacing - (_resolution * spacing) / 2.0f
        );

        multiMesh.SetInstanceTransform(idx, transform);
        var color = 1.0f / 255.0f * new Color(_hashes[idx] & 255, (_hashes[idx] >> 8) & 255, (_hashes[idx] >> 16) & 255);
        multiMesh.SetInstanceColor(idx, color);
      }
    }
  }
}
