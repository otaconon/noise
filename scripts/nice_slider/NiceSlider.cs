using Godot;
using System;

public partial class NiceSlider : Control
{
  [Export]
  private double _minValue;

  [Export]
  private double _maxValue;

  [Export]
  private double _step;
  
  public Slider Slider { get; private set; }

  public override void _Ready()
  {
    Slider = GetNode<Slider>("SliderBox/Slider");
    Slider.MinValue = _minValue;
    Slider.MaxValue = _maxValue;
    Slider.Step = _step;
  }
}
