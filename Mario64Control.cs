using LibSM64;
using MelonLoader;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using static SM64Constants.Action;
using static SM64Constants.MarioAnimID;
using ActionKeyPair = System.Collections.Generic.KeyValuePair<SM64Constants.Action, uint>;
using AnimKeyPair = System.Collections.Generic.KeyValuePair<SM64Constants.MarioAnimID, short>;

public class Mario64Control : BaseCharacter
{
    public SM64Mario sm64 = null;
    public SM64InputSMBZG sm64input = null;

    public MovementRushStateENUM LastMovementRushState;

    private AttackBundle AttBun_Punch1 => new AttackBundle
    {
        AnimationName = "Punch1",
        OnAnimationStart = delegate
        {
            SetPlayerState(PlayerStateENUM.Attacking);
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                new HitBoxDamageParameters
                {
                    Owner = this,
                    Tag = base.tag,
                    Damage = 1f,
                    HitStun = 0.25f,
                    BlockStun = 0.25f,
                    Launch = new Vector2(2 * FaceDir, 0),
                    FreezeTime = 0.03f,
                    Priority = BattleCache.PriorityType.Light,
                    HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                    OnHitSoundEffect = SoundCache.ins.Battle_Hit_1A
                }
            );
            base.HitBox_0.transform.localPosition = new Vector2(0.7f, 0);
            base.HitBox_0.transform.localScale = new Vector2(0.8f, 0.8f);
        }
    };

    private AttackBundle AttBun_Punch2 => new AttackBundle
    {
        AnimationName = "Punch2",
        OnAnimationStart = delegate
        {
            SetPlayerState(PlayerStateENUM.Attacking);
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                new HitBoxDamageParameters
                {
                    Owner = this,
                    Tag = base.tag,
                    Damage = 1f,
                    HitStun = 0.25f,
                    BlockStun = 0.25f,
                    Launch = new Vector2(2 * FaceDir, 0),
                    FreezeTime = 0.07f,
                    Priority = BattleCache.PriorityType.Light,
                    HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                    OnHitSoundEffect = SoundCache.ins.Battle_Hit_1A
                }
            );
            base.HitBox_0.transform.localPosition = new Vector2(0.7f, 0);
            base.HitBox_0.transform.localScale = new Vector2(0.8f, 0.8f);
        }
    };

    private AttackBundle AttBun_GroundKick => new AttackBundle
    {
        AnimationName = "GroundKick",
        OnAnimationStart = delegate
        {
            SetPlayerState(PlayerStateENUM.Attacking);
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                new HitBoxDamageParameters
                {
                    Owner = this,
                    Tag = base.tag,
                    Damage = 2f,
                    HitStun = 0.65f,
                    Launch = new Vector2(6 * FaceDir, 2),
                    FreezeTime = 0.15f,
                    Priority = BattleCache.PriorityType.Medium,
                    HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                    OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A
                }
            );
            base.HitBox_0.transform.localPosition = new Vector2(0.7f, 0);
            base.HitBox_0.transform.localScale = new Vector2(0.8f, 0.8f);
            base.HitBox_0.IsActive = true;
        }
    };

    private AttackBundle AttBun_GroundPoundAir => new AttackBundle
    {
        AnimationName = "GroundPoundAir",
        OnAnimationStart = delegate
        {
            SetPlayerState(PlayerStateENUM.Attacking);
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                new HitBoxDamageParameters
                {
                    Owner = this,
                    Tag = base.tag,
                    Damage = 3f,
                    HitStun = 0.5f,
                    GetLaunch = () => new Vector2(0f, Mathf.Lerp(-5f, -15f, (base.transform.position.y - GetGroundPositionViaRaycast().y) / 5f)),
                    FreezeTime = 0.15f,
                    Priority = BattleCache.PriorityType.Medium,
                    HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                    OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A
                }
            );
            base.HitBox_0.transform.localPosition = new Vector2(0, -0.3f);
            base.HitBox_0.transform.localScale = new Vector2(0.8f, 0.4f);
            Comp_InterplayerCollider.Disable();
        }
    };

    private AttackBundle AttBun_GroundPound => new AttackBundle
    {
        AnimationName = "GroundPound",
        OnAnimationStart = delegate
        {
            SetPlayerState(PlayerStateENUM.Attacking);
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                new HitBoxDamageParameters
                {
                    Owner = this,
                    Tag = base.tag,
                    Damage = 3.5f,
                    HitStun = 0.6f,
                    BlockStun = 0.1f,
                    Launch = new Vector2(3f, 10f),
                    BlockedLaunch = new Vector2(3f, 0f),
                    IsLaunchPositionBased = true,
                    FreezeTime = 0.15f,
                    Priority = BattleCache.PriorityType.Light,
                    HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                    OnHitSoundEffect = SoundCache.ins.Battle_Hit_3A
                }
            );
            base.HitBox_0.transform.localPosition = new Vector2(0, -0.3f);
            base.HitBox_0.transform.localScale = new Vector2(1f, 0.5f);
            Comp_InterplayerCollider.Enable();
        }
    };

    private AttackBundle AttBun_AirKick => new AttackBundle
    {
        AnimationName = "AirKick",
        OnAnimationStart = delegate
        {
            SetPlayerState(PlayerStateENUM.Attacking);
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                new HitBoxDamageParameters
                {
                    Owner = this,
                    Tag = base.tag,
                    Damage = 3.5f,
                    HitStun = 0.5f,
                    Launch = new Vector2(8 * FaceDir, 4),
                    FreezeTime = 0.03f,
                    Priority = BattleCache.PriorityType.Medium,
                    HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                    OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A
                }
            );
            base.HitBox_0.transform.localPosition = new Vector2(0.4f, 0.5f);
            base.HitBox_0.transform.localScale = new Vector2(0.8f, 0.9f);
            base.HitBox_0.IsActive = true;
        }
    };

    private AttackBundle AttBun_BreakDanceFront => new AttackBundle
    {
        AnimationName = "BreakDanceFront",
        OnAnimationStart = delegate
        {
            SetPlayerState(PlayerStateENUM.Attacking);
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                new HitBoxDamageParameters
                {
                    Owner = this,
                    Tag = base.tag,
                    Damage = 2f,
                    HitStun = 0.5f,
                    Launch = new Vector2(7f, 8f),
                    BlockedLaunch = new Vector2(3f, 0f),
                    IsLaunchPositionBased = true,
                    FreezeTime = 0.03f,
                    Priority = BattleCache.PriorityType.Light,
                    IsUnblockable = true,
                    HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                    OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A
                }
            );
            base.HitBox_0.transform.localPosition = new Vector2(0.5f, -0.3f);
            base.HitBox_0.transform.localScale = new Vector2(0.7f, 0.4f);
            base.HitBox_0.IsActive = true;
        }
    };

    private AttackBundle AttBun_BreakDanceBack => new AttackBundle
    {
        AnimationName = "BreakDanceBack",
        OnAnimationStart = delegate
        {
            base.HitBox_0.transform.localPosition = new Vector2(-0.2f, -0.3f);
            base.HitBox_0.transform.localScale = new Vector2(0.7f, 0.4f);
        }
    };
    
    private AttackBundle AttBun_Dive => new AttackBundle
    {
        AnimationName = "Dive",
        OnAnimationStart = delegate
        {
            SetPlayerState(PlayerStateENUM.Attacking);
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                new HitBoxDamageParameters
                {
                    Owner = this,
                    Tag = base.tag,
                    Damage = 2.5f,
                    HitStun = 0.6f,
                    GetLaunch = () => new Vector2(-sm64.marioState.velocity[0]/3.2f, sm64.marioState.velocity[1]/3),
                    FreezeTime = 0.1f,
                    Priority = BattleCache.PriorityType.Medium,
                    HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                    OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A
                }
            );
            base.HitBox_0.transform.localPosition = new Vector2(0.3f, 0f);
            base.HitBox_0.transform.localScale = new Vector2(0.6f, 0.6f);
        }
    };

    private AttackBundle AttBun_DiveSlide => new AttackBundle
    {
        AnimationName = "DiveSlide",
        OnAnimationStart = delegate
        {
            SetPlayerState(PlayerStateENUM.Attacking);
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                new HitBoxDamageParameters
                {
                    Owner = this,
                    Tag = base.tag,
                    Damage = 2f,
                    HitStun = 0.5f,
                    GetLaunch = () => new Vector2(-sm64.marioState.velocity[0]/3, 4),
                    FreezeTime = 0.05f,
                    Priority = BattleCache.PriorityType.Light,
                    HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                    OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A
                }
            );
            base.HitBox_0.transform.localPosition = new Vector2(0.3f, 0f);
            base.HitBox_0.transform.localScale = new Vector2(0.6f, 0.6f);
        }
    };

    private AttackBundle AttBun_SlideKick => new AttackBundle
    {
        AnimationName = "SlideKick",
        OnAnimationStart = delegate
        {
            SetPlayerState(PlayerStateENUM.Attacking);
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                new HitBoxDamageParameters
                {
                    Owner = this,
                    Tag = base.tag,
                    Damage = 2.5f,
                    HitStun = 0.7f,
                    GetLaunch = () => new Vector2(-sm64.marioState.velocity[0] / 3, 0),
                    FreezeTime = 0.1f,
                    Priority = BattleCache.PriorityType.Light,
                    HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                    OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A
                }
            );
            base.HitBox_0.transform.localPosition = new Vector2(0f, -0.5f);
            base.HitBox_0.transform.localScale = new Vector2(0.9f, 0.6f);
        }
    };

    private AttackBundle AttBun_SlideKickSlide => new AttackBundle
    {
        AnimationName = "SlideKickSlide",
        OnAnimationStart = delegate
        {
            SetPlayerState(PlayerStateENUM.Attacking);
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                new HitBoxDamageParameters
                {
                    Owner = this,
                    Tag = base.tag,
                    Damage = 2f,
                    HitStun = 0.5f,
                    GetLaunch = () => new Vector2(-sm64.marioState.velocity[0] / 3, 3),
                    FreezeTime = 0.05f,
                    Priority = BattleCache.PriorityType.Light,
                    HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                    OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A
                }
            );
            base.HitBox_0.transform.localPosition = new Vector2(0f, -0.5f);
            base.HitBox_0.transform.localScale = new Vector2(0.9f, 0.6f);
        }
    };

    private AttackBundle AttBun_Twirl => new AttackBundle
    {
        AnimationName = "Twirl",
        OnAnimationStart = delegate
        {
            SetPlayerState(PlayerStateENUM.Attacking);
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                new HitBoxDamageParameters
                {
                    Owner = this,
                    Tag = base.tag,
                    Damage = 2f,
                    HitStun = 0.5f,
                    GetLaunch = () => new Vector2(-sm64.marioState.velocity[0] / 3, 3),
                    FreezeTime = 0.05f,
                    Priority = BattleCache.PriorityType.Medium,
                    HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                    OnHitSoundEffect = SoundCache.ins.Battle_Hit_1A
                }
            );
            base.HitBox_0.transform.localPosition = new Vector2(0f, -0.2f);
            base.HitBox_0.transform.localScale = new Vector2(0.9f, 0.6f);
            base.HitBox_0.IsActive = true;
        },
        OnFixedUpdate = delegate
        {
            Melon<SMBZ_64.Core>.Logger.Msg($"mario {sm64.marioState.faceAngle}");
        }
    };

    private AttackBundle AttBun_AttackEnd => new AttackBundle
    {
        AnimationName = "AttackEnd",
        OnAnimationStart = delegate
        {
            SetPlayerState(PlayerStateENUM.Idle);
            base.HitBox_0.IsActive = false;
        }
    };

    private Dictionary<ActionKeyPair, AttackBundle> SM64AttacksActionArg = new();
    private Dictionary<AnimKeyPair, AttackBundle> SM64AttacksAnimFrame = new();

    private Animator Comp_Animator;
    private Rigidbody2D Comp_Rigidbody2D;

    protected override void Awake()
    {
        Melon<SMBZ_64.Core>.Logger.Msg("Mario64Control Awake");
        Comp_InterplayerCollider = gameObject.transform.GetChild(2).gameObject.GetComponent<InterplayerCollider>();
        Comp_InterplayerCollider.GetType().GetField("MyCharacter", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Comp_InterplayerCollider, this);
        Comp_InterplayerCollider.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(2.8f, 1f);
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
        Comp_Animator = GetComponent<Animator>();
        Comp_Rigidbody2D = GetComponent<Rigidbody2D>();
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
        SM64AttacksActionArg.Clear();
        SM64AttacksAnimFrame.Clear();

        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_PUNCHING, 2), AttBun_Punch1);
        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_PUNCHING, 5), AttBun_Punch2);
        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_MOVE_PUNCHING, 2), AttBun_Punch1);
        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_MOVE_PUNCHING, 5), AttBun_Punch2);

        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_GROUND_POUND, 0), AttBun_GroundPoundAir);
        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_GROUND_POUND_LAND, 0), AttBun_GroundPound);

        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_DIVE, 0), AttBun_Dive);
        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_DIVE, 1), AttBun_Dive);
        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_DIVE_SLIDE, 0), AttBun_DiveSlide);

        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_SLIDE_KICK, 0), AttBun_SlideKick);
        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_SLIDE_KICK_SLIDE, 0), AttBun_SlideKickSlide);

        SM64AttacksAnimFrame.Add(new AnimKeyPair(MARIO_ANIM_GROUND_KICK, 0), AttBun_GroundKick);
        SM64AttacksAnimFrame.Add(new AnimKeyPair(MARIO_ANIM_GROUND_KICK, 4), AttBun_AttackEnd);
        SM64AttacksAnimFrame.Add(new AnimKeyPair(MARIO_ANIM_AIR_KICK, 0), AttBun_AirKick);
        SM64AttacksAnimFrame.Add(new AnimKeyPair(MARIO_ANIM_AIR_KICK, 4), AttBun_AttackEnd);

        SM64AttacksAnimFrame.Add(new AnimKeyPair(MARIO_ANIM_BREAKDANCE, 1), AttBun_BreakDanceFront);
        SM64AttacksAnimFrame.Add(new AnimKeyPair(MARIO_ANIM_BREAKDANCE, 9), AttBun_BreakDanceBack);
        SM64AttacksAnimFrame.Add(new AnimKeyPair(MARIO_ANIM_BREAKDANCE, 18), AttBun_AttackEnd);
    }

    public void SetPlayerState(PlayerStateENUM state)
    {
        GetType().GetField("PlayerState", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, state);
    }

    public PlayerStateENUM GetPlayerState()
    {
        return (PlayerStateENUM)GetType().GetField("PlayerState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
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

        if (sm64.marioState.action == (uint)ACT_GROUND_POUND)
            base.HitBox_0.IsActive = (sm64.marioState.velocity[1] < -50f);

        if (sm64.marioState.action == (uint)ACT_CUSTOM_ANIM)
        {
            bool IsBursting = (bool)typeof(BaseCharacter).GetField("IsBursting", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            if (IsBursting)
            {
                sm64.SetAngle(transform.rotation.eulerAngles.z / 180 * (float)Math.PI * -FaceDir, sm64.marioState.faceAngle, 0);
                if (sm64.marioState.animFrame >= sm64.marioState.animLoopEnd - 2)
                {
                    if (sm64.marioState.animID == (uint)MARIO_ANIM_FIRST_PUNCH)
                        sm64.SetAnim(MARIO_ANIM_SECOND_PUNCH);
                    else if (sm64.marioState.animID == (uint)MARIO_ANIM_SECOND_PUNCH)
                        sm64.SetAnim(MARIO_ANIM_GROUND_KICK);
                }
                else if (sm64.marioState.animID == (uint)MARIO_ANIM_GROUND_KICK && sm64.marioState.animFrame >= 3)
                {
                    sm64.SetAnim(MARIO_ANIM_FIRST_PUNCH);
                }
            }
        }

        MovementRushStateENUM movRush =
            (MovementRushStateENUM)typeof(BaseCharacter).GetField("MovementRushState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);

        if (LastMovementRushState != MovementRushStateENUM.Inactive && LastMovementRushState != MovementRushStateENUM.Idle && movRush != LastMovementRushState)
        {
            if (CurrentAttackData != null && CurrentAttackData.OnAnimationEnd != null)
            {
                Melon<SMBZ_64.Core>.Logger.Msg($"OnAnimationEnd {movRush} {LastMovementRushState} {CurrentAttackData.OnAnimationStart_HasExecuted}");
                CurrentAttackData.OnAnimationEnd();
            }
        }

        LastMovementRushState = movRush;

        sm64input.overrideInput = IsPursuing;
    }

    public override void UpdateSpriteSortOrder(int value)
    {
        // empty
    }

    [HarmonyPatch(typeof(BaseCharacter), "PrepareAnAttack", new Type[] { typeof(AttackBundle), typeof(float) })]
    private static class Patch
    {
        private static void Postfix(BaseCharacter __instance)
        {
            if (__instance.GetType() != typeof(Mario64Control))
                return;

            Mario64Control c = (Mario64Control)__instance;
            MovementRushStateENUM movRush =
                (MovementRushStateENUM)typeof(BaseCharacter).GetField("MovementRushState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(c);

            c.CurrentAttackData.OnAnimationStart();
            Melon<SMBZ_64.Core>.Logger.Msg($"OnAnimationStart {c.CurrentAttackData.AnimationName} {c.CurrentAttackData.AnimationNameHash == c.ASN_MR_Dodge} {c.GetPlayerState()} {movRush} {c.LastMovementRushState} {c.CurrentAttackData.OnAnimationStart_HasExecuted}");
        }
    }

    protected override void Update_General()
    {
        base.Update_General();
        Update_ReadAttackInput();
    }

    protected override void Update_Pursue()
    {
        FieldInfo PursueDataField = GetType().GetField("PursueData", BindingFlags.NonPublic | BindingFlags.Instance);
        PursueBundle PursueData = (PursueBundle)PursueDataField.GetValue(this);
        FieldInfo isChargingField = typeof(PursueBundle).GetField("isCharging", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo IsFacingRightField = GetType().GetField("IsFacingRight", BindingFlags.NonPublic | BindingFlags.Instance);
        MethodInfo PlaySoundMethod = typeof(SoundCache).GetMethod("PlaySound", BindingFlags.NonPublic | BindingFlags.Instance, null, new[]{typeof(AudioClip), typeof(float), typeof(bool), typeof(float?), typeof(float), typeof(bool)}, null);
        bool IsFrozen = (bool)GetType().GetField("IsFrozen", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);

        if (IsFrozen || PursueData == null)
        {
            return;
        }

        sm64input.joyOverride = -((bool)IsFacingRightField.GetValue(this) ? Vector2.right : Vector2.left);
        if (sm64.marioState.action != (uint)ACT_WALKING)
            sm64.SetAction(ACT_WALKING);

        if (PursueData.Target == null)
        {
            PursueData.Target = FindClosestTarget();
        }
        
        PursueData.StartupCountdown = Mathf.Clamp(PursueData.StartupCountdown - Time.deltaTime, 0f, float.MaxValue);
        PursueData.PursueCountdown = Mathf.Clamp(PursueData.PursueCountdown - Time.deltaTime, 0f, float.MaxValue);
        if (PursueData.IsPreping)
        {
            if (CurrentAttackData == null)
            {
                //Comp_Animator.Play(base.IsOnGround ? Animations.PrePursue : Animations.PrePursueRoll, -1, 0f);
            }

            if ((bool)isChargingField.GetValue(PursueData))
            {
                sm64.SetForwardVelocity(0);
            }

            if (ContactGround && PursueData.StartupCountdown <= 0f)
            {
                PursueData.StartupCountdown = 0.15f;
            }

            if (PursueData.StartupCountdown <= 0f && (!(bool)isChargingField.GetValue(PursueData) || ((bool)isChargingField.GetValue(PursueData) && PursueData.ChargePower >= 100f)) && base.IsOnGround)
            {
                PursueData.PursueCountdown = 10f;
                PursueData.IsPursuing = true;
                PursueData.IsPreping = false;
                isChargingField.SetValue(PursueData, false);
                SetPlayerState(PlayerStateENUM.Pursuing);
                GetType().GetField("ComboSwingCounter", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, 0);
                float num = Helpers.Vector2ToDegreeAngle_180(base.transform.position, PursueData.Target.transform.position);
                IsFacingRightField.SetValue(this, -90f <= num && num <= 90f);
                PursueData.Direction = ((bool)IsFacingRightField.GetValue(this) ? Vector2.right : Vector2.left);
                sm64.SetForwardVelocity(15);
                sm64.SetFaceAngle( ((bool)IsFacingRightField.GetValue(this) ? -0.5f : 0.5f) * (float)Math.PI);
                sm64input.joyOverride = -PursueData.Direction;
                PlaySoundMethod.Invoke(SoundCache.ins, new object[]{(PursueData.ChargePower >= 100f) ? SoundCache.ins.Battle_Zoom : SoundCache.ins.Battle_Leap_DBZ, 1f, true, null, 1f, false});
                EffectSprite.Create(groundCheck.position, EffectSprite.Sprites.DustPuff, (bool)IsFacingRightField.GetValue(this));
                OnPursueStart();
            }
        }
        else if (PursueData.Target == null)
        {
            StartCoroutine(OnPursueMiss());
        }
        else if (PursueData.IsPursuing)
        {
            if (PursueData.IsHoming)
            {
                Vector3 vector = PursueData.Target.transform.position - base.transform.position;
                PursueData.Direction = vector / vector.magnitude;
            }

            bool num2 = Vector2.Distance(base.transform.position, PursueData.Target.transform.position) < 1.2f;
            bool flag = ((bool)IsFacingRightField.GetValue(this) ? (PursueData.Target.transform.position.x + 1f < base.transform.position.x) : (base.transform.position.x < PursueData.Target.transform.position.x - 1f));
            if (ContactGround && Comp_Rigidbody2D.velocity.y < -1f && Mathf.Abs(Comp_Rigidbody2D.velocity.x) < 3f)
            {
                PursueData.Direction = new Vector2((float)((PursueData.Direction.x > 0f) ? 1 : (-1)) * (Mathf.Abs(PursueData.Direction.x) + Mathf.Abs(PursueData.Direction.y)), 0f);
            }

            if (num2)
            {
                float value = MaxMoveSpeed.GetValue();
                SetVelocity(PursueData.Direction.x * value, Mathf.Clamp(Comp_Rigidbody2D.velocity.y, 0f - value, value));
                StartCoroutine(OnPursueContact());
            }
            else if (PursueData.PursueCountdown == 0f || flag)
            {
                StartCoroutine(OnPursueMiss());
            }
            else
            {
                SetVelocity(PursueData.Direction * PursueData.Speed);
            }
        }

        if (PursueDataField.GetValue(this) != null)
            PursueDataField.SetValue(this, PursueData);
    }

    protected override System.Collections.IEnumerator OnPursueMiss()
    {
        return base.OnPursueMiss();
    }

    protected override System.Collections.IEnumerator OnPursueContact()
    {
        yield return new WaitForEndOfFrame();

        FieldInfo PursueDataField = GetType().GetField("PursueData", BindingFlags.NonPublic | BindingFlags.Instance);
        PursueBundle PursueData = (PursueBundle)PursueDataField.GetValue(this);

        if (PursueData == null)
        {
            yield break;
        }

        if (PursueData.Target != null)
        {
            base.transform.position = new Vector3(PursueData.Target.transform.position.x + (float)base.FaceDir * -1.25f, base.transform.position.y, base.transform.position.z);
        }

        float a = 0.75f;
        float b = 1.5f;
        float a2 = 3f;
        float b2 = 6f;
        bool isFullPower = PursueData.ChargePower >= 100f;
        typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
            new HitBoxDamageParameters
            {
                Owner = this,
                Tag = base.tag,
                Damage = Mathf.Lerp(a2, b2, PursueData.ChargePower / 100f),
                HitStun = Mathf.Lerp(a, b, PursueData.ChargePower / 100f),
                Launch = new Vector2(2*FaceDir, 6f),
                FreezeTime = (isFullPower ? 0f : 0.07f),
                Priority = ((!isFullPower) ? BattleCache.PriorityType.Light : BattleCache.PriorityType.Heavy),
                IsUnblockable = isFullPower,
                HitSpark = new EffectSprite.Parameters
                {
                    SpriteHash = (isFullPower ? EffectSprite.Sprites.HitsparkHeavy : EffectSprite.Sprites.HitsparkBlunt)
                },
                OnHitSoundEffect = (isFullPower ? SoundCache.ins.Battle_Hit_3A : SoundCache.ins.Battle_Hit_2A)
            }
        );
        AttackBundle atk = new AttackBundle
        {
            AnimationName = "PursuePunch",
            OnAnimationStart = delegate
            {
                base.HitBox_0.transform.localPosition = new Vector2(0.8f, 0.2f);
                base.HitBox_0.transform.localScale = new Vector2(0.9f, 0.9f);
                base.HitBox_0.IsActive = true;
            },
            OnHit = delegate (BaseCharacter target, bool wasBlocked)
            {
                float BlockStun = (float)target.GetType().GetField("BlockStun", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);
                if (!(target == null) && (target.IsHurt || !(BlockStun <= 0f)))
                {
                    if (isFullPower)
                    {
                        BattleController.instance.Cinematic_SlowMotion(0.25f);
                    }
                }
            }
        };

        PursueDataField.SetValue(this, null);

        sm64.SetAction(ACT_STAR_DANCE_EXIT, 1); // arg 1: stationary_ground_step()
        sm64.SetAnim(MARIO_ANIM_STAR_DANCE);
        sm64.SetAnimFrame(39);
        sm64.SetActionTimer(40);
        sm64input.overrideInput = false;

        SetPlayerState(PlayerStateENUM.Attacking);
        PrepareAnAttack(atk);
    }

    public override void OnBurstBegin(BurstDataStore burstData)
    {
        base.OnBurstBegin(burstData);
        base.HitBox_0.IsActive = false;

        if (sm64 == null) return;
        sm64.SetAction(ACT_CUSTOM_ANIM);
        sm64.SetAnim(MARIO_ANIM_FIRST_PUNCH);
    }

    [HarmonyPatch(typeof(BaseCharacter), "OnBurstEnd")]
    private static class OnBurstEndPatch
    {
        private static void Postfix(BaseCharacter __instance)
        {
            if (__instance.GetType() != typeof(Mario64Control))
                return;

            Mario64Control c = (Mario64Control)__instance;
            if (c.Comp_Rigidbody2D.gravityScale == 1)
            {
                // burst tie
                c.sm64.SetAction(ACT_FREEFALL);
            }
            else
            {
                // burst lose/win
                c.sm64.SetAction(ACT_CUSTOM_ANIM);
                c.sm64.SetAnim(MARIO_ANIM_GENERAL_FALL);
            }
        }
    }

    public override System.Collections.IEnumerator OnBurst_Victory(CharacterControl target, BurstDataStore.VictoryStrikeENUM victoryStrikeType = BurstDataStore.VictoryStrikeENUM.General)
    {
        ResetGravity();
        target.CharacterGO.ResetGravity();
        typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
            new HitBoxDamageParameters
            {
                Owner = this,
                Tag = base.tag,
                Damage = 8f,
                HitStun = 1.25f,
                IsUnblockable = true,
                Launch = new Vector2(10 * base.FaceDir, 1f),
                FreezeTime = 0.15f,
                Priority = BattleCache.PriorityType.Heavy,
                HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkHeavy),
                OnHitSoundEffect = SoundCache.ins.Battle_Hit_3A,
                OnHitCallback = delegate (BaseCharacter t, bool b)
                {
                    bool t_IsNPC = (t != null) ? (bool)t.GetType().GetField("IsNPC", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(t) : false;
                    MovementRushManager manager = (MovementRushManager)typeof(BattleController).GetField("MovementRushManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);
                    bool IsFacingRight = (bool)GetType().GetField("IsFacingRight", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
                    CharacterControl MyCharacterControl = (CharacterControl)GetType().GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
                    CharacterControl targetControl = (CharacterControl)GetType().GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(t);

                    if (victoryStrikeType == BurstDataStore.VictoryStrikeENUM.MovementRushStarter && SaveData.Data.MovementRush_IsEnabled_ViaCriticalStrikes && t != null && !t_IsNPC && t.IsHurt)
                    {
                        manager.StartNewMovementRush(IsFacingRight, new List<CharacterControl> { MyCharacterControl }, new List<CharacterControl> { targetControl });
                    }

                    sm64.SetAction(ACT_FREEFALL);
                    sm64.SetForwardVelocity(35);
                }
            }
        );
        AttackBundle atk = new AttackBundle
        {
            AnimationName = "BurstVictoryStrike",
            OnAnimationStart = delegate
            {
                sm64.SetAction(ACT_CUSTOM_ANIM_TO_ACTION, 2); // arg 2: perform_air_step()
                sm64.SetActionArg2((uint)ACT_FREEFALL);
                sm64.SetAnim(MARIO_ANIM_SLIDE_KICK);
                sm64.SetAnimFrame(4);
                sm64.SetVelocity(new Vector3(-35 * base.FaceDir, 0, 0));
                base.HitBox_0.transform.localPosition = new Vector2(0.1f, -0.1f);
                base.HitBox_0.transform.localScale = new Vector2(0.5f, 0.9f);
                base.HitBox_0.IsActive = true;
            }
        };
        PrepareAnAttack(atk);

        base.OnBurst_Victory(target);
        yield return null;
    }

    private void Perform_PeaceSignTaunt()
    {
        if (sm64 == null) return;

        AttackBundle atk = new AttackBundle
        {
            AnimationName = "ACT_STAR_DANCE_EXIT",
            OnAnimationStart = delegate
            {
                SetPlayerState(PlayerStateENUM.Attacking);
                sm64.SetAction(ACT_STAR_DANCE_EXIT);
                sm64.SetAnim(MARIO_ANIM_STAR_DANCE);
                sm64.SetAnimFrame(35);
                sm64.SetActionTimer(36);
            },
            OnAnimationEnd = delegate
            {
                SetPlayerState(PlayerStateENUM.Idle);
            }
        };
        PrepareAnAttack(atk);
    }

    protected override void Perform_Grounded_NeutralTaunt()
    {
        Perform_PeaceSignTaunt();
    }

    protected override void Perform_Grounded_UpTaunt()
    {
        Perform_PeaceSignTaunt();
    }

    protected override void Perform_Grounded_DownTaunt()
    {
        Perform_PeaceSignTaunt();
    }

    protected override void Perform_Aerial_NeutralSpecial()
    {
        if (sm64 == null) return;

        sm64.SetAction(ACT_TWIRLING);
        PrepareAnAttack(AttBun_Twirl);
    }


    public void OnChangeSM64Action(SM64Constants.Action action, uint actionArg)
    {
        ActionKeyPair k = new ActionKeyPair(action, actionArg);
        Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control action {action} {actionArg}");
        if (SM64AttacksActionArg.ContainsKey(k))
        {
            PrepareAnAttack(SM64AttacksActionArg[k]);
            base.HitBox_0.IsActive = true;
        }
        else
        {
            if (CurrentAttackData != null && CurrentAttackData.OnAnimationEnd != null && action.ToString() != CurrentAttackData.AnimationName)
                CurrentAttackData.OnAnimationEnd();

            if (action != ACT_CUSTOM_ANIM && action != ACT_CUSTOM_ANIM_TO_ACTION)
            {
                if (!IsPursuing)
                    SetPlayerState(PlayerStateENUM.Idle);

                base.HitBox_0.IsActive = false;
            }
        }
    }

    public void OnMarioAdvanceAnimFrame(SM64Constants.MarioAnimID animID, short animFrame)
    {
        if (sm64.marioState.action == (uint)ACT_CUSTOM_ANIM || sm64.marioState.action == (uint)ACT_CUSTOM_ANIM_TO_ACTION)
            return;

        AnimKeyPair k = new AnimKeyPair(animID, animFrame);
        Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control anim {animID} {animFrame}");
        if (SM64AttacksAnimFrame.ContainsKey(k))
        {
            PrepareAnAttack(SM64AttacksAnimFrame[k]);
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

    public override void Hurt(ProjectileHitBox AttackingHitBox)
    {
        base.Hurt(AttackingHitBox);
        if (!IsHurt || sm64 == null)
            return;

        // so ProjectileHitBox.DamageProperties is public, but HitBox.DamageProperties isn't?? what?
        HitBoxDamageParameters_Projectile damageProperties = AttackingHitBox.DamageProperties;

        Vector2 to = ((damageProperties.Owner == null) ? AttackingHitBox.transform.position : damageProperties.Owner.transform.position);
        sm64.TakeDamage((uint)(damageProperties.Damage * 3), 0, new Vector3(to.x, to.y, -1));
    }

    public override void OnDeath()
    {
        base.OnDeath();
        Interop.PlaySound(SM64Constants.SOUND_MARIO_WAAAOOOW);

        if (sm64 == null) return;
        sm64.Kill();
    }
}
