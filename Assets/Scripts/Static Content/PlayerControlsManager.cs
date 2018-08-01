using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsManager
{
    // Keyboard Controls
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";
    public const string JumpKeyboard = "space";
    public const string DashKeyboard = "q";

    // Mouse Controls
    public const string MouseX = "Mouse X";
    public const string MouseY = "Mouse Y";

    // Combat Animation Names
    public const string FirstAttack = "Attack0";
    public const string SecondAttack = "Attack1";
    public const string ThirdAttack = "Attack2";

    // Movement Animation Names;
    public const string IdleAnimation = "Idle";
    public const string RunAnimation = "Run";
    public const string FallAnimation = "Fall";

    // Animator Values
    public const string MoveParam = "Move";
    public const string AttackParam = "Attack";
    public const string FireParam = "Fire Arc";
    public const string JumpParam = "Jump";
    public const string FallParam = "Falling";
    public const string HitParam = "Hit";
    public const string DeadParam = "Dead";
}
