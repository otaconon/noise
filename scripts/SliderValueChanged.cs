using Godot;
using System;

public partial class SliderValueChanged : Label
{
  private void OnValueChanged(float value)
  {
    Text = value.ToString();
  }
}
