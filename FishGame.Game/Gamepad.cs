using System.Numerics;
using Raylib_cs;

namespace FishGame.Game;

public class Gamepad
{
    public const int DefaultGamepad = 0;

    private readonly GamepadAxis[] Axises = [GamepadAxis.RightY, GamepadAxis.RightX, GamepadAxis.LeftY, GamepadAxis.LeftX];

    private float _deadZone = 0.1f;

    public Vector2 LeftStickDelta { get; private set; }
    public Vector2 RightStickDelta { get; private set; }

    public bool Enabled { get; private set; } = false;

    public void Update()
    {
        if (Raylib.GetGamepadButtonPressed() != 0)
        {
            Enabled = true;
        }
        LeftStickDelta = new(AxisDelta(GamepadAxis.LeftX), AxisDelta(GamepadAxis.LeftY));
        RightStickDelta = new(AxisDelta(GamepadAxis.RightX), AxisDelta(GamepadAxis.RightY));
    }

    /// <summary>
    /// Check axis delta and returns it only if it is outside dead zone.
    /// </summary>
    private float AxisDelta(GamepadAxis axis)
    {
        var delta = Raylib.GetGamepadAxisMovement(DefaultGamepad, axis);
        if (Math.Abs(delta) > _deadZone)
        {
            Enabled = true;
            return delta;
        }
        return 0f;
    }


}
