using LibSM64;
using MelonLoader;
using System;
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
                Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control attack!");
                GetType().GetField("PlayerState", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, PlayerStateENUM.Attacking);
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
                base.HitBox_0.transform.localPosition = new Vector2(-0.5f, 0);
                base.HitBox_0.transform.localScale = new Vector2(1, 1);
            }
            catch (Exception e)
            {
                Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control attack: {e}");
            }
        }
    };

    private Dictionary<ActionArgKey, AttackBundle> SM64Attacks = new();

    private Animator Comp_Animator;
    private Rigidbody2D Comp_Rigidbody2D;

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
            Comp_Animator = GetComponent<Animator>();
            Comp_Rigidbody2D = GetComponent<Rigidbody2D>();
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
            Comp_Animator.enabled = false;
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

    /*
    public new void PrepareAnAttack(AttackBundle AttackToPrepare, float MinimumPrepTime = 0f)
    {
        CurrentAttackData = AttackToPrepare;
        Melon<SMBZ_64.Core>.Logger.Msg($"get coroutine");
        Coroutine Coroutine_GroundedOnLandingFallback = (Coroutine)GetType().GetProperty("Coroutine_GroundedOnLandingFallback", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
        Melon<SMBZ_64.Core>.Logger.Msg($"check coroutine");
        if (Coroutine_GroundedOnLandingFallback != null)
        {
            Melon<SMBZ_64.Core>.Logger.Msg($"stop coroutine");
            StopCoroutine(Coroutine_GroundedOnLandingFallback);
        }

        Melon<SMBZ_64.Core>.Logger.Msg($"start coroutine");
        GetType().GetProperty("Coroutine_GroundedOnLandingFallback", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, StartCoroutine(GroundedOnLandingFallback()));
    }
    */

    public void OnChangeSM64Action(uint action, uint actionArg)
    {
        ActionArgKey k = new ActionArgKey(action, actionArg);
        Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control action 0x{action:X} {actionArg}");
        base.HitBox_0.IsActive = SM64Attacks.ContainsKey(k);
        if (base.HitBox_0.IsActive)
        {
            PrepareAnAttack(SM64Attacks[k]);
            SM64Attacks[k].OnAnimationStart();
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
