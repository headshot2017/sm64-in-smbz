using LibSM64;
using MelonLoader;
using System.Reflection;
using UnityEngine;

public class Mario64Control : BaseCharacter
{
    public SM64Mario sm64 = null;
    public SM64InputSMBZG sm64input = null;

    protected override void Awake()
    {
        Melon<SMBZ_64.Core>.Logger.Msg("Mario64Control Awake");
        Comp_InterplayerCollider = gameObject.AddComponent<InterplayerCollider>();
        Comp_InterplayerCollider.gameObject.AddComponent<CapsuleCollider2D>();
        AdditionalCharacterSpriteList = new List<SpriteRenderer>();
        SupportingSpriteList = new List<SpriteRenderer>();




        try
        {
            base.Awake();
        }
        catch (Exception e)
        {
            Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control Awake: {e}");
        }



        Melon<SMBZ_64.Core>.Logger.Msg("Mario64Control Awakened");
        AerialAcceleration = 0f;
        GroundedAcceleration = 0f;
        GroundedDrag = 0f;
        HopPower = 0f;
        JumpChargeMax = 0f;
        Pursue_Speed = 25f;
        Pursue_StartupDelay = 0.1f;

        Type type = typeof(Mario64Control);
        FieldInfo EnergyMaxField = type.GetField("EnergyMax", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo EnergyStartField = type.GetField("EnergyStart", BindingFlags.NonPublic | BindingFlags.Instance);
        EnergyMaxField.SetValue(this, 200f);
        EnergyStartField.SetValue(this, 100f);
    }

    protected override void Start()
    {
        Melon<SMBZ_64.Core>.Logger.Msg("Mario64Control Start");
        try
        {
            base.Start();
            base.HitBox_0 = base.transform.Find("HitBox_0").GetComponent<HitBox>();
            base.HitBox_0.tag = base.tag;
            SoundEffect_Jump = null;
        }
        catch (Exception e)
        {
            Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control Start: {e}");
        }
        Melon<SMBZ_64.Core>.Logger.Msg("Mario64Control Started");
    }

    protected override void Update()
    {
        try
        {
            base.Update();
        }
        catch (Exception e)
        {
            Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control: {e}");
        }
    }

    public override void UpdateSpriteSortOrder(int value)
    {
        // empty
    }

    public void OnChangeSM64Action(uint action, uint actionArg)
    {

    }

    public override void Hurt(HitBox AttackingHitBox)
    {
        base.Hurt(AttackingHitBox);

        if (!IsHurt || sm64 == null)
            return;

        Type type = typeof(HitBox);
        FieldInfo damagePropField = type.GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance);
        HitBoxDamageParameters damageProperties = (HitBoxDamageParameters)damagePropField.GetValue(AttackingHitBox);

        Vector2 to = ((damageProperties.Owner == null) ? AttackingHitBox.transform.position : damageProperties.Owner.transform.position);
        sm64.TakeDamage((uint)(damageProperties.Damage * 3), 0, new Vector3(to.x, to.y, -1));
    }

    public override void OnDeath()
    {
        Melon<SMBZ_64.Core>.Logger.Msg("Mario64Control OnDeath");
        base.OnDeath();

        if (sm64 == null) return;
        sm64.Kill();
        Interop.PlaySound(SM64Constants.SOUND_MARIO_WAAAOOOW);
    }
}
