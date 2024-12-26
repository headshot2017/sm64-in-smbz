using UnityEngine;
using MelonLoader;

namespace LibSM64
{
    public abstract class SM64InputProvider : MonoBehaviour
    {
        public enum Button
        {
            Jump,
            Kick,
            Stomp
        };

        public abstract Vector3 GetCameraLookDirection();
        public abstract Vector2 GetJoystickAxes();
        public abstract bool GetButtonHeld( Button button );
    }

    public class SM64InputSMBZG : SM64InputProvider
    {
        public CharacterControl c = null;

        public override Vector3 GetCameraLookDirection()
        {
            return new Vector3(0,0,-1);
        }

        public override Vector2 GetJoystickAxes()
        {
            if (c.IsInputLocked)
                return new Vector2(0,0);

            return new Vector2(
                (c.Button_Left.IsHeld) ? 1 : (c.Button_Right.IsHeld) ? -1 : 0,
                0
            );
        }

        public override bool GetButtonHeld(Button button)
        {
            if (c.IsInputLocked)
                return false;

            switch (button)
            {
                case Button.Jump:
                    return c.Button_Jump.IsHeld;

                case Button.Kick:
                    return c.Button_A.IsHeld;

                case Button.Stomp:
                    return c.Button_Guard.IsHeld;
            }

            return false;
        }
    }
}