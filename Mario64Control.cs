using LibSM64;
using MelonLoader;
using System.Reflection;
using UnityEngine;
using static SM64Constants.MarioAnimID;
using TwoUintPair = System.Collections.Generic.KeyValuePair<uint, uint>;
using AnimKeyPair = System.Collections.Generic.KeyValuePair<SM64Constants.MarioAnimID, short>;

public class Mario64Control : BaseCharacter
{
    public SM64Mario sm64 = null;
    public SM64InputSMBZG sm64input = null;

    public float HitStunStart { get; private set; }

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
            base.HitBox_0.transform.localPosition = new Vector2(0.4f, -0.3f);
            base.HitBox_0.transform.localScale = new Vector2(0.7f, 0.4f);
            base.HitBox_0.IsActive = true;
        }
    };

    private AttackBundle AttBun_BreakDanceBack => new AttackBundle
    {
        AnimationName = "BreakDanceBack",
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
            base.HitBox_0.transform.localPosition = new Vector2(-0.4f, -0.3f);
            base.HitBox_0.transform.localScale = new Vector2(0.7f, 0.4f);
            base.HitBox_0.IsActive = true;
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
                    GetLaunch = () => new Vector2(-sm64.marioState.velocity[0]/3, sm64.marioState.velocity[1]/3),
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
            base.HitBox_0.transform.localPosition = new Vector2(0.3f, -0.5f);
            base.HitBox_0.transform.localScale = new Vector2(0.7f, 0.6f);
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
            base.HitBox_0.transform.localPosition = new Vector2(0.3f, -0.5f);
            base.HitBox_0.transform.localScale = new Vector2(0.7f, 0.6f);
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

    private Dictionary<TwoUintPair, AttackBundle> SM64AttacksActionArg = new();
    private Dictionary<AnimKeyPair, AttackBundle> SM64AttacksAnimFrame = new();

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

        SM64AttacksActionArg.Add(new TwoUintPair(SM64Constants.ACT_PUNCHING, 2), AttBun_Punch1);
        SM64AttacksActionArg.Add(new TwoUintPair(SM64Constants.ACT_PUNCHING, 5), AttBun_Punch2);
        SM64AttacksActionArg.Add(new TwoUintPair(SM64Constants.ACT_MOVE_PUNCHING, 2), AttBun_Punch1);
        SM64AttacksActionArg.Add(new TwoUintPair(SM64Constants.ACT_MOVE_PUNCHING, 5), AttBun_Punch2);

        SM64AttacksActionArg.Add(new TwoUintPair(SM64Constants.ACT_GROUND_POUND, 0), AttBun_GroundPoundAir);
        SM64AttacksActionArg.Add(new TwoUintPair(SM64Constants.ACT_GROUND_POUND_LAND, 0), AttBun_GroundPound);

        SM64AttacksActionArg.Add(new TwoUintPair(SM64Constants.ACT_DIVE, 0), AttBun_Dive);
        SM64AttacksActionArg.Add(new TwoUintPair(SM64Constants.ACT_DIVE, 1), AttBun_Dive);
        SM64AttacksActionArg.Add(new TwoUintPair(SM64Constants.ACT_DIVE_SLIDE, 0), AttBun_DiveSlide);

        SM64AttacksActionArg.Add(new TwoUintPair(SM64Constants.ACT_SLIDE_KICK, 0), AttBun_SlideKick);
        SM64AttacksActionArg.Add(new TwoUintPair(SM64Constants.ACT_SLIDE_KICK_SLIDE, 0), AttBun_SlideKickSlide);

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

        if (sm64.marioState.action == SM64Constants.ACT_GROUND_POUND)
            base.HitBox_0.IsActive = (sm64.marioState.velocity[1] < -50f);
    }

    public override void UpdateSpriteSortOrder(int value)
    {
        // empty
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
        bool IsFrozen = (bool)GetType().GetField("IsFrozen", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
        bool IsFacingRight = (bool)GetType().GetField("IsFacingRight", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);

        if (IsFrozen || PursueData == null)
        {
            return;
        }

        if (PursueData.Target == null)
        {
            PursueData.Target = FindClosestTarget();
        }

        
        /*
        PursueData.StartupCountdown = Mathf.Clamp(PursueData.StartupCountdown - Time.deltaTime, 0f, float.MaxValue);
        PursueData.PursueCountdown = Mathf.Clamp(PursueData.PursueCountdown - Time.deltaTime, 0f, float.MaxValue);
        if (PursueData.IsPreping)
        {
            if (CurrentAttackData == null && !IsCurrentAnimationStateEqualTo(Animations.PrePursue, Animations.PrePursueRoll))
            {
                Comp_Animator.Play(base.IsOnGround ? Animations.PrePursue : Animations.PrePursueRoll, -1, 0f);
            }

            if (ContactGround && PursueData.StartupCountdown <= 0f)
            {
                PursueData.StartupCountdown = 0.15f;
            }

            if (PursueData.StartupCountdown <= 0f && (!PursueData.isCharging || (PursueData.isCharging && PursueData.ChargePower >= 100f)) && base.IsOnGround)
            {
                PursueData.PursueCountdown = 10f;
                PursueData.IsPursuing = true;
                PursueData.IsPreping = false;
                PursueData.isCharging = false;
                PlayerState = PlayerStateENUM.Pursuing;
                ComboSwingCounter = 0;
                float num = Helpers.Vector2ToDegreeAngle_180(base.transform.position, PursueData.Target.transform.position);
                IsFacingRight = -90f <= num && num <= 90f;
                PursueData.Direction = (IsFacingRight ? Vector2.right : Vector2.left);
                SoundCache.ins.PlaySound((PursueData.ChargePower >= 100f) ? SoundCache.ins.Battle_Zoom : SoundCache.ins.Battle_Leap_DBZ);
                Comp_Animator.Play(Animations.Pursue, -1, 0f);
                EffectSprite.Create(groundCheck.position, EffectSprite.Sprites.DustPuff, IsFacingRight);
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
            bool flag = (IsFacingRight ? (PursueData.Target.transform.position.x + 1f < base.transform.position.x) : (base.transform.position.x < PursueData.Target.transform.position.x - 1f));
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
        */

        PursueDataField.SetValue(this, PursueData);
    }

    private void Perform_PeaceSignTaunt()
    {
        if (sm64 == null) return;
        sm64.SetAction(SM64Constants.ACT_STAR_DANCE_EXIT);
        sm64.SetAnim(MARIO_ANIM_STAR_DANCE);
        sm64.SetAnimFrame(35);
        sm64.SetActionTimer(36);
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


    public void OnChangeSM64Action(uint action, uint actionArg)
    {
        TwoUintPair k = new TwoUintPair(action, actionArg);
        Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control action 0x{action:X} {actionArg}");
        if (SM64AttacksActionArg.ContainsKey(k))
        {
            PrepareAnAttack(SM64AttacksActionArg[k]);
            SM64AttacksActionArg[k].OnAnimationStart();
            base.HitBox_0.IsActive = true;
        }
        else
        {
            SetPlayerState(PlayerStateENUM.Idle);
            base.HitBox_0.IsActive = false;
        }
    }

    public void OnMarioAdvanceAnimFrame(int animID, short animFrame)
    {
        AnimKeyPair k = new AnimKeyPair((SM64Constants.MarioAnimID)animID, animFrame);
        Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control anim {(SM64Constants.MarioAnimID)animID} {animFrame}");
        if (SM64AttacksAnimFrame.ContainsKey(k))
        {
            PrepareAnAttack(SM64AttacksAnimFrame[k]);
            SM64AttacksAnimFrame[k].OnAnimationStart();
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

        HitStunStart = (damageProperties.GetHitStun != null) ? damageProperties.GetHitStun() : damageProperties.HitStun;
    }

    public override void OnDeath()
    {
        base.OnDeath();
        Interop.PlaySound(SM64Constants.SOUND_MARIO_WAAAOOOW);

        if (sm64 == null) return;
        sm64.Kill();
    }
}
