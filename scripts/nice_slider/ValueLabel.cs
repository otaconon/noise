using Godot;
using System;

public partial class ValueLabel : Label
{
  public void OnValueChanged(double value)
  {
    Text = value.ToString();
  }
}
