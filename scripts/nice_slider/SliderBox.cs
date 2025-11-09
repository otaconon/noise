using Godot;
using System;

public partial class SliderBox : Control
{
  private Slider _slider;
  private ValueLabel _valueLabel;

  public override void _Ready()
  {
    _slider = GetNode<Slider>("Slider");
    _valueLabel = GetNode<ValueLabel>("Value");

    _slider.ValueChanged += _valueLabel.OnValueChanged;
  }
}
