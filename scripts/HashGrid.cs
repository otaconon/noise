using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class HashGrid : Node3D
{
  [Export]
  private Mesh _instanceMesh;
  private MultiMeshInstance3D _instanceMultiMesh;

  [Export(PropertyHint.Range, "1,512")]
  private int _resolution = 16;
  private uint[] _hashes;

  private void ComputeHashes()
  {
    for (uint i = 0; i < _resolution; i++)
    {
      for (uint j = 0; j < _resolution; j++)
      {
        _hashes[i * (uint)_resolution + j] = i * (uint)_resolution + j;
      }
    }
  }

  public override void _Ready()
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
    _instanceMultiMesh = new MultiMeshInstance3D
    {
      Multimesh = multiMesh
    };
    AddChild(_instanceMultiMesh);

    float spacing = 1.0f;
    for (int i = 0; i < _resolution; i++)
    {
      for (int j = 0; j < _resolution; j++)
      {
        int index = i * _resolution + j;

        var transform = Transform3D.Identity;
        transform.Origin = new Vector3(
            i * spacing - (_resolution * spacing) / 2.0f,
            0,
            j * spacing - (_resolution * spacing) / 2.0f
        );

        multiMesh.SetInstanceTransform(index, transform);
        float col = (float)_hashes[index] / (float)(length - 1);
        GD.Print(col);
        multiMesh.SetInstanceColor(index, new Color(col, col, col));
      }
    }
  }
}
