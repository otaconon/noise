using Godot;
using System;

public readonly struct SmallXXHash {

  private const uint _primeA = 0b10011110001101110111100110110001;
  private const uint _primeB = 0b10000101111010111100101001110111;
  private const uint _primeC = 0b11000010101100101010111000111101;
  private const uint _primeD = 0b00100111110101001110101100101111;
  private const uint _primeE = 0b00010110010101100110011110110001;

  private readonly uint _accumulator;


  public SmallXXHash(uint accumulator) {
    _accumulator = accumulator;
  }

  public static SmallXXHash Seed(int seed) => (uint)seed + _primeE;

  public SmallXXHash Eat (int data) => RotateLeft(_accumulator + (uint)data * _primeC, 17) * _primeD;
  public SmallXXHash Eat(byte data) => RotateLeft(_accumulator + data * _primeE, 11) * _primeA;

  public static implicit operator SmallXXHash (uint accumulator) =>	new SmallXXHash(accumulator);

  public static implicit operator uint(SmallXXHash hash) {
    uint avalanche = hash._accumulator;
		avalanche ^= avalanche >> 15;
		avalanche *= _primeB;
		avalanche ^= avalanche >> 13;
		avalanche *= _primeC;
		avalanche ^= avalanche >> 16;
		return avalanche;
  }

  private static uint RotateLeft (uint data, int steps) => (data << steps) | (data >> 32 - steps);
}
