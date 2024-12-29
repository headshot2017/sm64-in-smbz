using LibSM64;
using MelonLoader;
using System.Reflection;
using UnityEngine;
using ActionArgKey = System.Collections.Generic.KeyValuePair<uint, uint>;

public class Mario64Control : BaseCharacter
{
    public SM64Mario sm64 = null;
    public SM64InputSMBZG sm64input = null;

    private AttackBundle AttBun_Punch1 => new AttackBundle
    {
        AnimationName = "Punch1",
        OnAnimationStart = delegate
        {
            try
            {
                GetType().GetField("PlayerState", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, PlayerStateENUM.Attacking);
                PlaySoundForMelee(SoundCache.ins.Battle_Swish_Light);
                bool IsFacingRight = (bool)GetType().GetField("IsFacingRight", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
                typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                    new HitBoxDamageParameters
                    {
                        Owner = this,
                        Tag = base.tag,
                        Damage = 1f,
                        HitStun = 0.25f,
                        BlockStun = 0.25f,
                        Launch = new Vector2(2 * (IsFacingRight ? 1 : (-1)), 0f),
                        FreezeTime = 0.03f,
                        Priority = BattleCache.PriorityType.Light,
                        HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                        OnHitSoundEffect = SoundCache.ins.Battle_Hit_1A
                    }
                );
            }
            catch (Exception e)
            {
                Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control attack: {e}");
            }
        }
    };

    private Dictionary<ActionArgKey, AttackBundle> SM64Attacks = new();

    protected override void Awake()
    {
        Melon<SMBZ_64.Core>.Logger.Msg("Mario64Control Awake");
        Comp_InterplayerCollider = gameObject.AddComponent<InterplayerCollider>();
        Comp_InterplayerCollider.gameObject.AddComponent<CapsuleCollider2D>();
        AdditionalCharacterSpriteList = new List<SpriteRenderer>();
        SupportingSpriteList = new List<SpriteRenderer>();

        Setup_Attacks();

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

    private void Setup_Attacks()
    {
        SM64Attacks.Clear();
        SM64Attacks.Add(new ActionArgKey(SM64Constants.ACT_PUNCHING, 2), AttBun_Punch1);
        SM64Attacks.Add(new ActionArgKey(SM64Constants.ACT_MOVE_PUNCHING, 2), AttBun_Punch1);
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
        ActionArgKey k = new ActionArgKey(action, actionArg);
        Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control action 0x{action:X} {actionArg}");
        if (SM64Attacks.ContainsKey(k))
        {
            PrepareAnAttack(SM64Attacks[k]);
        }
    }

    public override void Hurt(HitBox AttackingHitBox)
    {
        base.Hurt(AttackingHitBox);

        if (!IsHurt || sm64 == null)
            return;

        HitBoxDamageParameters damageProperties =
            (HitBoxDamageParameters)typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(AttackingHitBox);

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
