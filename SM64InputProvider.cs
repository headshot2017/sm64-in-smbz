using UnityEngine;
using MelonLoader;
using System.Reflection;

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

        public bool overrideInput = false;
        public Vector2 joyOverride;
        public HashSet<Button> buttonOverride = new HashSet<Button>();

        public override Vector3 GetCameraLookDirection()
        {
            return new Vector3(0,0,-1);
        }

        public override Vector2 GetJoystickAxes()
        {
            bool IsCPUControlled = c.ParticipantDataReference.CPUSettings != BattleCache.CPUSettingsENUM.Player;

            if (overrideInput)
                return joyOverride;
            if (c.IsInputLocked)
                return new Vector2(0,0);
            if (IsCPUControlled)
            {
                AI_Bundle AI = (AI_Bundle)c.GetType().GetField("AI", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(c);
                return new Vector2(-AI.MovementIdea.HorizontalDirection, 0);
            }

            return -((c.Button_Left.IsHeld) ? Vector2.left : (c.Button_Right.IsHeld) ? Vector2.right : Vector2.zero);
        }

        public override bool GetButtonHeld(Button button)
        {
            bool IsCPUControlled = c.ParticipantDataReference.CPUSettings != BattleCache.CPUSettingsENUM.Player;
            AI_Bundle AI = (AI_Bundle)c.GetType().GetField("AI", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(c);
            if (IsCPUControlled)

            if (overrideInput)
                return buttonOverride.Contains(button);
            if (c.IsInputLocked)
                return false;

            switch (button)
            {
                case Button.Jump:
                    return c.Button_Jump.IsHeld || (IsCPUControlled && AI.JumpIdea != null && AI.JumpIdea.HasActivated);

                case Button.Kick:
                    return c.Button_A.IsHeld;

                case Button.Stomp:
                    return c.Button_Guard.IsHeld;
            }

            return false;
        }
    }
}