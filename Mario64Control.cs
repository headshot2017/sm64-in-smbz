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
    public GameObject sm64Obj = null;
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
            base.HitBox_0.IsActive = true;
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
            base.HitBox_0.IsActive = true;
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
            base.HitBox_0.IsActive = true;
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
            base.HitBox_0.IsActive = true;
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
                    GetLaunch = () => new Vector2(-sm64.marioState.velocity[0]/3.2f, sm64.marioState.velocity[1]/3),
                    FreezeTime = 0.1f,
                    Priority = BattleCache.PriorityType.Medium,
                    HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                    OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A
                }
            );
            base.HitBox_0.transform.localPosition = new Vector2(0.3f, 0f);
            base.HitBox_0.transform.localScale = new Vector2(0.6f, 0.6f);
            base.HitBox_0.IsActive = true;
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
            base.HitBox_0.IsActive = true;
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
            base.HitBox_0.IsActive = true;
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
            base.HitBox_0.IsActive = true;
        }
    };

    private AttackBundle AttBun_Twirl => new AttackBundle
    {
        AnimationName = "Twirl",
        OnAnimationStart = delegate
        {
            SetPlayerState(PlayerStateENUM.Attacking);
            base.HitBox_0.transform.localPosition = new Vector2(0f, 0f);
            base.HitBox_0.transform.localScale = new Vector2(1.25f, 0.6f);
            base.HitBox_0.IsActive = true;
        },
        OnUpdate = delegate
        {
            float yaw = sm64.marioState.twirlYaw;

            bool oldActive = base.HitBox_0.IsActive;
            base.HitBox_0.IsActive = (yaw >= 0.5f && yaw <= 2.25f) || (yaw >= -3.04f && yaw <= -0.68f);
            if (base.HitBox_0.IsActive && !oldActive)
            {
                typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                    new HitBoxDamageParameters
                    {
                        Owner = this,
                        Tag = base.tag,
                        Damage = 1f,
                        HitStun = 0.5f,
                        GetLaunch = () => new Vector2(-sm64.marioState.velocity[0] / 3, 0),
                        FreezeTime = 0.05f,
                        Priority = BattleCache.PriorityType.Medium,
                        HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                        OnHitSoundEffect = SoundCache.ins.Battle_Hit_1A
                    }
                );
            }
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
    public float movRushTimer;

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



        Comp_Animator = GetComponent<Animator>();
        Comp_Rigidbody2D = GetComponent<Rigidbody2D>();
        Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control Awakened: {Comp_SpriteRenderer != null} {AdditionalCharacterSpriteList != null}");
        AerialAcceleration = 15f;
        GroundedAcceleration = 30f;
        GroundedDrag = 3f;
        HopPower = 10.5f;
        JumpChargeMax = 0f;
        Pursue_Speed = 25f;
        Pursue_StartupDelay = 0.1f;

        GetType().GetField("EnergyMax", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, 200f);
        GetType().GetField("EnergyStart", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, 100f);
        GetType().GetField("IsFacingRight", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, (base.tag == "Team1"));
    }

    protected override void Start()
    {
        Melon<SMBZ_64.Core>.Logger.Msg("Mario64Control Start");
        try
        {
            base.Start();
            base.HitBox_0 = base.transform.Find("HitBox_0").GetComponent<HitBox>();
            base.HitBox_0.tag = base.tag;
            SoundEffect_Jump = AudioClip.Create("empty", 1, 1, 8000, false);

            sm64Obj = new GameObject("SM64_MARIO");
            sm64Obj.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
            sm64input = sm64Obj.AddComponent<SM64InputSMBZG>();
            sm64input.c = (CharacterControl)GetType().GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            sm64 = sm64Obj.AddComponent<SM64Mario>();
            sm64.smbzChar = sm64input.c;
            sm64.changeActionCallback = SMBZ_64.Core.OnMarioChangeAction;
            sm64.advanceAnimFrameCallback = SMBZ_64.Core.OnMarioAdvanceAnimFrame;
            sm64.SetFaceAngle((float)Math.PI / 2 * (base.tag == "Team1" ? -1 : 1));

            Material[] mats = Resources.FindObjectsOfTypeAll<Material>();
            Material m = null;
            foreach (Material mat in mats)
            {
                if (mat.name != "UISpriteWithHueSaturationContrast")
                    continue;

                m = Material.Instantiate<Material>(mat);
                break;
            }
            Shader[] sh = Resources.FindObjectsOfTypeAll<Shader>();
            foreach (Shader s in sh)
            {
                if (s.name != "Legacy Shaders/VertexLit")
                    continue;

                m.shader = s;
                m.SetColor("_Emission", new Color(0.4f, 0.4f, 0.4f, 1));
                break;
            }
            sm64.SetMaterial(m);

            if (sm64.spawned)
                Melon<SMBZ_64.Core>.Instance.RegisterMario(sm64);
            else
                Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control {base.tag} Failed to spawn Mario");
        }
        catch (Exception e)
        {
            Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control Start: {e}");
        }
        Melon<SMBZ_64.Core>.Logger.Msg("Mario64Control Started");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Melon<SMBZ_64.Core>.Instance.UnregisterMario(sm64);
        GameObject.Destroy(sm64Obj);
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

        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_TWIRL_LAND, 0), AttBun_AttackEnd);

        SM64AttacksAnimFrame.Add(new AnimKeyPair(MARIO_ANIM_GROUND_KICK, 0), AttBun_GroundKick);
        SM64AttacksAnimFrame.Add(new AnimKeyPair(MARIO_ANIM_GROUND_KICK, 4), AttBun_AttackEnd);
        SM64AttacksAnimFrame.Add(new AnimKeyPair(MARIO_ANIM_AIR_KICK, 0), AttBun_AirKick);
        SM64AttacksAnimFrame.Add(new AnimKeyPair(MARIO_ANIM_AIR_KICK, 4), AttBun_AttackEnd);

        SM64AttacksAnimFrame.Add(new AnimKeyPair(MARIO_ANIM_BREAKDANCE, 1), AttBun_BreakDanceFront);
        SM64AttacksAnimFrame.Add(new AnimKeyPair(MARIO_ANIM_BREAKDANCE, 9), AttBun_BreakDanceBack);
        SM64AttacksAnimFrame.Add(new AnimKeyPair(MARIO_ANIM_BREAKDANCE, 18), AttBun_AttackEnd);
    }

    public object GetField(string name)
    {
        return GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
    }

    public void SetField(string name, object value)
    {
        GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, value);
    }

    public void SetPlayerState(PlayerStateENUM state)
    {
        SetField("PlayerState", state);
    }

    public PlayerStateENUM GetPlayerState()
    {
        return (PlayerStateENUM)GetField("PlayerState");
    }

    public void SetMovementRushState(MovementRushStateENUM state)
    {
        SetField("MovementRushState", state);
    }

    public MovementRushStateENUM GetMovementRushState()
    {
        return (MovementRushStateENUM)GetField("MovementRushState");
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

        /*
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
        */

        sm64input.overrideInput = IsPursuing || GetMovementRushState() != MovementRushStateENUM.Inactive;
    }

    public override void UpdateSpriteSortOrder(int value)
    {
        // empty
    }

    protected override void Update_MovementRush()
    {
        base.Update_MovementRush();

        if (GetMovementRushState() != MovementRushStateENUM.Inactive)
        {
            sm64input.buttonOverride.Clear();
            sm64input.joyOverride = Vector2.zero;
        }
    }

    /*
    protected override void Update_MovementRushAnimator()
    {
        MovementRushManager manager = (MovementRushManager)typeof(BattleController).GetField("MovementRushManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);
        bool flag = manager.ActiveMovementRush?.IsDirectionOfRushRight ?? true;
        BattleStateENUM BattleState = (BattleStateENUM)typeof(BattleController).GetField("BattleState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);
        CharacterControl MyCharacterControl = (CharacterControl)GetType().GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);

        if (BattleState == BattleController.BattleStateENUM.MovementRush_Grounded)
        {
            if (!IsOnGround || IsHurt || GetPlayerState() != 0 || GetMovementRushState() != MovementRushStateENUM.Idle)
            {
                return;
            }

            if (flag)
            {
                if (MyCharacterControl.IsInputtingRight)
                {
                    //PlayAnimationIfNotAlready(ASN_MR_Ground_MoveForward, ASN_MR_Ground_Land, ASN_MR_Air_Idle);
                }
                else if (GetCurrentAnimationState() == ASN_MR_Ground_MoveForward)
                {
                    // PlayAnimationIfNotAlready(ASN_MR_Ground_Idle, ASN_MR_Ground_Land);
                }
            }
            else
            {
                if (MyCharacterControl.IsInputtingLeft)
                {
                    //PlayAnimationIfNotAlready(ASN_MR_Ground_MoveForward, ASN_MR_Ground_Land);
                }
                else if (GetCurrentAnimationState() == ASN_MR_Ground_MoveForward)
                {
                    //PlayAnimationIfNotAlready(ASN_MR_Ground_Idle, ASN_MR_Ground_Land);
                }
            }
        }
        else
        {
            if (BattleState != BattleController.BattleStateENUM.MovementRush_Aerial)
            {
                return;
            }

            if (!IsHurt && GetPlayerState() == PlayerStateENUM.Idle && GetMovementRushState() == MovementRushStateENUM.Idle)
            {
                if (GetCurrentAnimationState() == ASN_MR_Air_Idle)
                {
                    if (MyCharacterControl.IsInputtingUp)
                    {
                        PlayAnimationIfNotAlready(ASN_MR_Air_MoveUpward);
                    }
                    else if (MyCharacterControl.IsInputtingDown)
                    {
                        PlayAnimationIfNotAlready(ASN_MR_Air_MoveDownward);
                    }
                    else if (MyCharacterControl.IsInputtingLeft || MyCharacterControl.IsInputtingRight)
                    {
                        PlayAnimationIfNotAlready(ASN_MR_Air_MoveForward);
                    }
                }
                else if (!MyCharacterControl.IsInputtingRight && !MyCharacterControl.IsInputtingLeft && !MyCharacterControl.IsInputtingUp && !MyCharacterControl.IsInputtingDown)
                {
                    PlayAnimationIfNotAlready(ASN_MR_Air_Idle);
                }
                else if (MyCharacterControl.IsInputtingUp)
                {
                    PlayAnimationIfNotAlready(ASN_MR_Air_MoveUpward);
                }
                else if (MyCharacterControl.IsInputtingDown)
                {
                    PlayAnimationIfNotAlready(ASN_MR_Air_MoveDownward);
                }
                else if (MyCharacterControl.IsInputtingLeft || MyCharacterControl.IsInputtingRight)
                {
                    PlayAnimationIfNotAlready(ASN_MR_Air_MoveForward);
                }
            }
        }
    }
    */

    [HarmonyPatch(typeof(BaseCharacter), "PrepareAnAttack", new Type[] { typeof(AttackBundle), typeof(float) })]
    private static class PrepareAnAttackPatch
    {
        private static void Postfix(BaseCharacter __instance)
        {
            if (__instance.GetType() != typeof(Mario64Control))
                return;

            Mario64Control c = (Mario64Control)__instance;
            MovementRushStateENUM movRush =
                (MovementRushStateENUM)typeof(BaseCharacter).GetField("MovementRushState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(c);

            if (c.CurrentAttackData != null && c.CurrentAttackData.OnAnimationStart != null)
                c.CurrentAttackData.OnAnimationStart();
            Melon<SMBZ_64.Core>.Logger.Msg($"OnAnimationStart {c.CurrentAttackData.AnimationName} {c.CurrentAttackData.AnimationNameHash == c.ASN_MR_Dodge} {c.GetPlayerState()} {movRush} {c.LastMovementRushState} {c.CurrentAttackData.OnAnimationStart_HasExecuted}");
        }
    }

    [HarmonyPatch(typeof(BaseCharacter), "PerformAction_Dodge", new Type[] { typeof(Vector2?) })]
    private static class PerformAction_DodgePatch
    {
        private static bool Prefix(BaseCharacter __instance, Vector2? directionOverride = null)
        {
            if (__instance.GetType() != typeof(Mario64Control))
                return true;

            Mario64Control c = (Mario64Control)__instance;
            c.InterruptAndNullifyPreparedAttack();
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c.HitBox_0, null);
            AttackBundle bundle = new AttackBundle
            {
                AnimationNameHash = c.ASN_MR_Dodge,
                OnAnimationStart = delegate
                {
                    c.SetMovementRushState(MovementRushStateENUM.IsDodging);
                    c.SetPlayerState(PlayerStateENUM.Attacking);
                    c.GetType().GetField("IsIntangible", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c, true);
                    c.GetType().GetField("DragOverride", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c,
                        c.GetType().GetField("DodgeDrag", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(c)
                    );
                    c.SetGravityOverride(0f);
                    c.OnMovementRush_Dodge();
                    c.movRushTimer = 0;
                },
                OnInterrupt = delegate
                {
                    c.GetType().GetField("IsIntangible", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c, false);
                    c.MovementRush_SetDefaultGravityAndDrag();
                    c.SetMovementRushState(MovementRushStateENUM.Idle);
                },
                OnAnimationEnd = delegate
                {
                    c.GetType().GetField("IsIntangible", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c, false);
                    c.MovementRush_SetDefaultGravityAndDrag();
                    c.SetMovementRushState(MovementRushStateENUM.Idle);
                    c.SetPlayerState(PlayerStateENUM.Idle);
                    c.CurrentAttackData = null;
                    //c.PlayAnimationIfNotAlready(ASN_MR_Air_Idle);
                },
                OnFixedUpdate = delegate
                {
                    c.movRushTimer += Time.fixedDeltaTime;
                    if (c.movRushTimer > 0.55f)
                        c.CurrentAttackData.OnAnimationEnd();
                }
            };
            bundle.SetCustomeQueue(delegate
            {
                if (bundle.CustomQueueCallCount == 0)
                {
                    float DodgeSpeed = (float)c.GetType().GetField("DodgeSpeed", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(c);
                    if (!directionOverride.HasValue)
                    {
                        /*
                        bool flag = (IsCPUControlled ? (MyCharacterControl.AI.MovementIdea.VerticalDirection < 0) : (MyCharacterControl.Button_Down.IsBuffered || MyCharacterControl.Button_Down.IsHeld));
                        bool flag2 = (IsCPUControlled ? (0 < MyCharacterControl.AI.MovementIdea.VerticalDirection) : (MyCharacterControl.Button_Up.IsBuffered || MyCharacterControl.Button_Up.IsHeld));
                        bool flag3 = (IsCPUControlled ? (MyCharacterControl.AI.MovementIdea.HorizontalDirection < 0) : (MyCharacterControl.Button_Left.IsBuffered || MyCharacterControl.Button_Left.IsHeld));
                        bool flag4 = (IsCPUControlled ? (0 < MyCharacterControl.AI.MovementIdea.HorizontalDirection) : (MyCharacterControl.Button_Right.IsBuffered || MyCharacterControl.Button_Right.IsHeld));
                        */
                        CharacterControl MyCharacterControl = (CharacterControl)c.GetType().GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(c);
                        bool flag = (MyCharacterControl.Button_Down.IsBuffered || MyCharacterControl.Button_Down.IsHeld);
                        bool flag2 = (MyCharacterControl.Button_Up.IsBuffered || MyCharacterControl.Button_Up.IsHeld);
                        bool flag3 = (MyCharacterControl.Button_Left.IsBuffered || MyCharacterControl.Button_Left.IsHeld);
                        bool flag4 = (MyCharacterControl.Button_Right.IsBuffered || MyCharacterControl.Button_Right.IsHeld);
                        if (!flag && !flag3 && !flag4 && !flag2)
                        {
                            c.SetVelocity((float)c.FaceDir * DodgeSpeed, 0f);
                        }
                        else
                        {
                            Vector2 vector = default(Vector2);
                            if (flag2)
                            {
                                vector.y += 1f;
                            }

                            if (flag)
                            {
                                vector.y -= 1f;
                            }

                            if (MyCharacterControl.IsInputtingLeft)
                            {
                                vector.x -= 1f;
                            }

                            if (flag4)
                            {
                                vector.x += 1f;
                            }

                            c.SetVelocity(vector.normalized * DodgeSpeed);
                        }
                    }
                    else
                    {
                        c.SetVelocity(directionOverride.Value * DodgeSpeed);
                    }
                }
                else if (bundle.CustomQueueCallCount == 1)
                {
                    c.GetType().GetField("IsIntangible", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c, false);
                }
            }, 2);
            c.PrepareAnAttack(bundle);

            return false;
        }
    }

    [HarmonyPatch(typeof(BaseCharacter), "PerformAction_Strike")]
    private static class PerformAction_StrikePatch
    {
        private static bool Prefix(BaseCharacter __instance)
        {
            if (__instance.GetType() != typeof(Mario64Control))
                return true;

            Mario64Control c = (Mario64Control)__instance;
            FieldInfo IsFacingRightField = c.GetType().GetField("IsFacingRight", BindingFlags.NonPublic | BindingFlags.Instance);
            float StrikeLaunch = (float)c.GetType().GetField("StrikeLaunch", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(c);
            MovementRushManager manager = (MovementRushManager)typeof(BattleController).GetField("MovementRushManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);

            c.InterruptAndNullifyPreparedAttack();
            IsFacingRightField.SetValue(c, c.GetVelocity().normalized.x >= 0f);
            c.RotateTowardTargetDirection(c.GetVelocity().normalized);
            c.FlipSpriteByFacingDirection();
            c.Comp_InterplayerCollider.Disable();
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c.HitBox_0,
                new HitBoxDamageParameters
                {
                    Owner = c,
                    Tag = c.tag,
                    Damage = 5f,
                    HitStun = 0.5f,
                    IsUnblockable = true,
                    CanBackAttack = true,
                    IsDamageFatal = false,
                    FreezeTime = 0.07f,
                    Launch = c.GetVelocity().normalized * StrikeLaunch,
                    HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                    OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A,
                    CanInitiateBurst = false,
                    OnHitCallback = delegate (BaseCharacter target, bool wasBlocked)
                    {
                        MovementRushManager.MRDataStore.PlayerStatus playerStatus = manager.FetchActiveRushPlayerData(target);
                        if (playerStatus != null)
                        {
                            FieldInfo StrikeAmmo = typeof(BaseCharacter).GetField("MR_StrikeAmmo", BindingFlags.NonPublic | BindingFlags.Instance);
                            FieldInfo StrikeAmmo_MAX = typeof(BaseCharacter).GetField("MR_StrikeAmmo_MAX", BindingFlags.NonPublic | BindingFlags.Instance);
                            FieldInfo DodgeAmmo = typeof(BaseCharacter).GetField("MR_DodgeAmmo", BindingFlags.NonPublic | BindingFlags.Instance);
                            FieldInfo DodgeAmmo_MAX = typeof(BaseCharacter).GetField("MR_DodgeAmmo_MAX", BindingFlags.NonPublic | BindingFlags.Instance);

                            StrikeAmmo.SetValue(c, StrikeAmmo_MAX.GetValue(c));
                            DodgeAmmo.SetValue(c, DodgeAmmo_MAX.GetValue(c));
                            StrikeAmmo.SetValue(target, StrikeAmmo_MAX.GetValue(target));
                            DodgeAmmo.SetValue(target, DodgeAmmo_MAX.GetValue(target));
                            playerStatus.Health--;
                            if (playerStatus.Health <= 0)
                            {
                                CharacterControl target_MyCharacterControl = (CharacterControl)target.GetType().GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);
                                c.CurrentAttackData = null;
                                c.PerformAction_Finale(target_MyCharacterControl);
                            }
                        }
                    }
                }
            );
            c.PrepareAnAttack(new AttackBundle
            {
                AnimationNameHash = c.ASN_MR_Strike_Attack,
                OnAnimationStart = delegate
                {
                    c.SetVelocity(c.GetVelocity().normalized * (float)c.GetField("StrikeEndSpeed"));
                    c.SetGravityOverride(0f);
                    c.SetField("DragOverride", 1f);
                    c.SetMovementRushState(MovementRushStateENUM.IsStriking);
                    c.OnMovementRush_Strike();
                    c.HitBox_0.transform.localPosition = new Vector2(0f, -0.2f);
                    c.HitBox_0.transform.localScale = new Vector2(1.25f, 1f);
                    c.HitBox_0.IsActive = true;
                    c.movRushTimer = 0;
                },
                OnClashCallback = delegate
                {
                    c.PerformAction_Clash();
                },
                OnInterrupt = delegate
                {
                    c.MovementRush_SetDefaultGravityAndDrag();
                    c.ResetRotation();
                    c.SetMovementRushState(MovementRushStateENUM.Idle);
                    c.Comp_InterplayerCollider.Enable();
                },
                OnAnimationEnd = delegate
                {
                    c.MovementRush_SetDefaultGravityAndDrag();
                    c.ResetRotation();
                    c.SetMovementRushState(MovementRushStateENUM.Idle);
                    c.SetPlayerState(PlayerStateENUM.Idle);
                    //PlayAnimationIfNotAlready(ASN_MR_Air_Idle);
                    c.Comp_InterplayerCollider.Enable();
                    c.CurrentAttackData = null;
                },
                OnFixedUpdate = delegate
                {
                    c.movRushTimer += Time.fixedDeltaTime;
                    if (c.movRushTimer > 0.55f)
                        c.CurrentAttackData.OnAnimationEnd();
                }
            });

            return false;
        }
    }

    [HarmonyPatch(typeof(BaseCharacter), "PerformAction_Finale", new Type[] { typeof(CharacterControl) })]
    private static class PerformAction_FinalePatch
    {
        private static bool Prefix(BaseCharacter __instance, CharacterControl target)
        {
            if (__instance.GetType() != typeof(Mario64Control))
                return true;

            Mario64Control c = (Mario64Control)__instance;
            MovementRushManager MRManager = (MovementRushManager)typeof(BattleController).GetField("MovementRushManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);
            BattleBackgroundManager BBManager = (BattleBackgroundManager)typeof(BattleController).GetField("BackgroundManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);
            float GroundPositionY = (float)BBManager.GetType().GetField("GroundPositionY", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BBManager);
            MethodInfo PlaySoundMethod = typeof(SoundCache).GetMethod("PlaySound", BindingFlags.NonPublic | BindingFlags.Instance, null, new[]{typeof(AudioClip), typeof(float), typeof(bool), typeof(float?), typeof(float), typeof(bool)}, null);
            MethodInfo AirRushCollider_SetActive = BBManager.GetType().GetMethod("AirRushCollider_SetActive", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(bool) }, null);

            Vector2 velocity = new Vector2(5 * c.FaceDir, Mathf.Lerp(15f, 5f, (c.transform.position.y - GroundPositionY) / 15f));
            c.SetVelocity(velocity);
            c.SetGravityOverride(0.1f);
            c.SetField("DragOverride", 1f);
            target.CharacterGO.transform.position = c.transform.position + new Vector3(c.FaceDir, 0f);
            target.CharacterGO.SetHitsunWithAnimation(3f, transitionDirectlyIntoHurtAnimation: false);
            target.CharacterGO.SetGravityOverride(0.1f);
            typeof(BaseCharacter).GetField("DragOverride", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(target.CharacterGO, 1f);
            target.CharacterGO.SetVelocity(velocity);
            PlaySoundMethod.Invoke(SoundCache.ins, new object[] { SoundCache.ins.Battle_Hit_2A, 1f, true, null, 1f, false });
            EffectSprite.Create(target.transform.position, EffectSprite.Sprites.HitsparkBlunt, MRManager.ActiveMovementRush.IsDirectionOfRushRight);
            c.SetField("IgnoreClashTimer", 5f);
            c.ResetRotation();
            c.SetField("IsIntangible", true);
            c.SetPlayerState(PlayerStateENUM.Cinematic_NoInput);
            c.SetMovementRushState(MovementRushStateENUM.IsFinale);
            AirRushCollider_SetActive.Invoke(BBManager, new object[] { false });
            c.PrepareAnAttack(new AttackBundle
            {
                AnimationName = "Finale",
                OnAnimationStart = delegate
                {
                    c.SetMovementRushState(MovementRushStateENUM.IsFinale);
                    typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c.HitBox_0,
                        new HitBoxDamageParameters
                        {
                            Owner = c,
                            Tag = c.tag,
                            Damage = 15f,
                            HitStun = 0.75f,
                            IsUnblockable = true,
                            FreezeTime = 0.15f,
                            Launch = new Vector2(10 * c.FaceDir, -20f),
                            HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkHeavy),
                            OnHitSoundEffect = SoundCache.ins.Battle_Hit_3B,
                            CanInitiateBurst = false,
                            OnHitCallback = delegate (BaseCharacter target, bool wasBlocked)
                            {
                                CharacterControl MyCharacterControl = (CharacterControl)typeof(BaseCharacter).GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(c);
                                CharacterControl target_MyCharacterControl = (CharacterControl)typeof(BaseCharacter).GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);
                                MRManager.EndMovementRush(MyCharacterControl, target_MyCharacterControl);
                            }
                        }
                    );
                    c.HitBox_0.transform.localPosition = new Vector2(0.5f, -0.6f);
                    c.HitBox_0.transform.localScale = new Vector2(1.25f, 1f);
                    c.HitBox_0.IsActive = false;
                    c.movRushTimer = 0;
                },
                OnInterrupt = delegate
                {
                    c.SetField("IgnoreClashTimer", 0f);
                    c.MovementRush_SetDefaultGravityAndDrag();
                    c.SetField("IsIntangible", false);
                    c.SetMovementRushState(MovementRushStateENUM.Idle);
                },
                OnAnimationEnd = delegate
                {
                    c.SetField("IgnoreClashTimer", 0f);
                    c.MovementRush_SetDefaultGravityAndDrag();
                    c.SetField("IsIntangible", false);
                    c.SetMovementRushState(MovementRushStateENUM.Idle);
                    c.SetPlayerState(PlayerStateENUM.Idle);
                    //PlayAnimationIfNotAlready(c.ASN_Idle);
                },
                OnFixedUpdate = delegate
                {
                    c.movRushTimer += Time.fixedDeltaTime;
                    if (c.movRushTimer > 1f)
                        c.HitBox_0.IsActive = true;
                }
            });

            return false;
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
                CurrentAttackData = null;
            },
            OnUpdate = delegate
            {
                if (sm64.marioState.action != (uint)ACT_STAR_DANCE_EXIT)
                    this.CurrentAttackData.OnAnimationEnd();
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
        if (sm64.marioState.velocity[1] < 0)
            sm64.SetVelocity(new Vector3(sm64.marioState.velocity[0], 30, 0));
        PrepareAnAttack(AttBun_Twirl);
    }


    public void OnChangeSM64Action(SM64Constants.Action action, uint actionArg)
    {
        ActionKeyPair k = new ActionKeyPair(action, actionArg);
        Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control action {action} {actionArg}");
        if (SM64AttacksActionArg.ContainsKey(k))
        {
            PrepareAnAttack(SM64AttacksActionArg[k]);
        }
        else
        {
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
