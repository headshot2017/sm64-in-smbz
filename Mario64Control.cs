using LibSM64;
using MelonLoader;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using static SM64Constants.Action;
using static SM64Constants.MarioAnimID;
using ActionKeyPair = System.Collections.Generic.KeyValuePair<SM64Constants.Action, uint>;
using AnimKeyPair = System.Collections.Generic.KeyValuePair<SM64Constants.MarioAnimID, short>;
using SMBZG;

public class Mario64Control : BaseCharacter
{
    public GameObject sm64Obj = null;
    public SM64Mario sm64 = null;
    public SM64InputSMBZG sm64input = null;

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
            base.HitBox_0.IsActive = false;
            Comp_InterplayerCollider.Disable();
        },
        OnUpdate = delegate
        {
            base.HitBox_0.IsActive = (sm64.marioState.velocity[1] < -50f);
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

    private AttackBundle AttBun_SmackDown
    {
        get
        {
            AttackBundle atk = new AttackBundle
            {
                AnimationName = "SmackDown",
                OnAnimationStart = delegate
                {
                    SetPlayerState(PlayerStateENUM.Attacking);
                    typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                        new HitBoxDamageParameters
                        {
                            Owner = this,
                            Tag = base.tag,
                            Damage = 1.5f,
                            HitStun = 0.5f,
                            Launch = new Vector2(6 * FaceDir, -10f),
                            FreezeTime = 0.03f,
                            Priority = BattleCache.PriorityType.Light,
                            HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                            OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A,
                        }
                    );
                    base.HitBox_0.transform.localPosition = new Vector2(0.7f, -0.3f);
                    base.HitBox_0.transform.localScale = new Vector2(0.7f, 0.5f);
                    base.HitBox_0.IsActive = false;
                },
                OnAnimationEnd = delegate
                {
                    SetPlayerState(PlayerStateENUM.Idle);
                    base.HitBox_0.IsActive = false;
                    CurrentAttackData = null;
                }
            };
            atk.OnUpdate = delegate
            {
                if (sm64.marioState.animFrame >= 17)
                    atk.OnAnimationEnd();
                if (sm64.marioState.animFrame >= 12)
                    base.HitBox_0.IsActive = true;
            };
            return atk;
        }
    }

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
                        Damage = 1.2f,
                        HitStun = 0.5f,
                        GetLaunch = () => new Vector2(-sm64.marioState.velocity[0] / 3.5f, 0),
                        FreezeTime = 0.05f,
                        Priority = BattleCache.PriorityType.Medium,
                        HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                        OnHitSoundEffect = SoundCache.ins.Battle_Hit_1A
                    }
                );
            }
        }
    };

    private AttackBundle AttBun_StarDancePunch
    {
        get
        {
            AttackBundle atk = new AttackBundle
            {
                AnimationName = "StarDancePunch",
                OnAnimationStart = delegate
                {
                    attackState = 0;
                    sm64.SetAction(ACT_STAR_DANCE_EXIT, 1);
                    sm64.SetAnim(MARIO_ANIM_STAR_DANCE);
                    sm64.SetAnimFrame(3);
                    sm64.SetActionTimer(4);
                    SetPlayerState(PlayerStateENUM.Attacking);
                    base.HitBox_0.IsActive = false;
                },
                OnAnimationEnd = delegate
                {
                    sm64.SetAction(ACT_IDLE);
                    SetPlayerState(PlayerStateENUM.Idle);
                    base.HitBox_0.IsActive = false;
                    CurrentAttackData = null;
                }
            };
            atk.OnUpdate = delegate
            {
                if (sm64.marioState.animFrame == 10 && attackState == 0)
                {
                    typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                        new HitBoxDamageParameters
                        {
                            Owner = this,
                            Tag = base.tag,
                            Damage = 1f,
                            HitStun = 1.2f,
                            Launch = Vector2.zero,
                            FreezeTime = 0.05f,
                            Priority = BattleCache.PriorityType.Medium,
                            HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                            OnHitSoundEffect = SoundCache.ins.Battle_Hit_1A
                        }
                    );
                    base.HitBox_0.transform.localPosition = new Vector2(0.7f, 0.15f);
                    base.HitBox_0.transform.localScale = new Vector2(0.7f, 0.7f);
                    base.HitBox_0.IsActive = true;
                    attackState = 1;
                }
                else if (sm64.marioState.animFrame == 15 && attackState == 1)
                {
                    typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                        new HitBoxDamageParameters
                        {
                            Owner = this,
                            Tag = base.tag,
                            Damage = 1f,
                            HitStun = 1.2f,
                            Launch = Vector2.zero,
                            FreezeTime = 0.05f,
                            Priority = BattleCache.PriorityType.Medium,
                            HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                            OnHitSoundEffect = SoundCache.ins.Battle_Hit_1A
                        }
                    );
                    base.HitBox_0.IsActive = true;
                    attackState = 2;
                }
                else if (sm64.marioState.animFrame == 39 && attackState == 2)
                {
                    typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                        new HitBoxDamageParameters
                        {
                            Owner = this,
                            Tag = base.tag,
                            Damage = 3f,
                            HitStun = 0.9f,
                            Launch = new Vector2(8 * FaceDir, 6),
                            FreezeTime = 0.15f,
                            Priority = BattleCache.PriorityType.Heavy,
                            HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkHeavy),
                            OnHitSoundEffect = SoundCache.ins.Battle_Hit_3A
                        }
                    );
                    base.HitBox_0.transform.localPosition = new Vector2(1f, 0.15f);
                    base.HitBox_0.transform.localScale = new Vector2(0.7f, 0.7f);
                    base.HitBox_0.IsActive = true;
                    attackState = 3;
                }

                if ((sm64.marioState.animFrame == 13 && attackState == 1) ||
                    (sm64.marioState.animFrame == 19 && attackState == 2) ||
                    (sm64.marioState.animFrame == 43 && attackState == 3))
                    base.HitBox_0.IsActive = false;

                if (sm64.marioState.animFrame >= 45)
                    atk.OnAnimationEnd();
            };
            return atk;
        }
    }

    private AttackBundle AttBun_CriticalPunch
    {
        get
        {
            AttackBundle bundle = new AttackBundle
            {
                AnimationName = "CriticalPunch",
                OnAnimationStart = delegate
                {
                    SetPlayerState(PlayerStateENUM.Cinematic_NoInput);
                    typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                        new HitBoxDamageParameters
                        {
                            Owner = this,
                            Tag = base.tag,
                            Damage = 10f,
                            Stun = 200f,
                            HitStun = 4f,
                            Intensity = 0f,
                            IsCriticalStrike = true,
                            IsUnblockable = true,
                            Launch = new Vector2(40 * FaceDir, 25f),
                            FreezeTime = 0.3f,
                            Priority = BattleCache.PriorityType.Critical,
                            HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkHeavy),
                            OnHitSoundEffect = SoundCache.ins.Battle_MeleeFlash
                        }
                    );
                    base.HitBox_0.transform.localPosition = new Vector2(1f, 0.15f);
                    base.HitBox_0.transform.localScale = new Vector2(0.7f, 0.7f);
                    base.HitBox_0.IsActive = false;
                    sm64.SetAction(ACT_STAR_DANCE_EXIT, 1);
                    sm64.SetAnim(MARIO_ANIM_STAR_DANCE);
                    sm64.SetAnimFrame(31);
                    sm64.SetActionTimer(32);
                    attackState = 0;
                },
                OnAnimationEnd = delegate
                {
                    sm64.SetAction(ACT_IDLE);
                    SetPlayerState(PlayerStateENUM.Idle);
                    base.HitBox_0.IsActive = false;
                    CurrentAttackData = null;
                },
                CinematicEffects = new List<CinematicEffect>
                {
                    new CinematicEffect
                    {
                        StartupDelay = 0.02f,
                        PauseAndDimDuringStartup = false,
                        Duration = 0.75f,
                        PauseAndDimDuringDuration = true,
                        EffectToCreate = new EffectSprite.Parameters
                        {
                            SpriteHash = EffectSprite.Sprites.CriticalPower
                        }
                    }
                },
                OnCustomQueue = delegate
                {
                    PlaySoundForMelee(SoundCache.ins.Battle_StaffSwish);
                },
                OnClashCallback = delegate
                {
                    HitBoxDamageParameters DamageProperties = (HitBoxDamageParameters)typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(base.HitBox_0);
                    DamageProperties.IsNullified = true;
                }
            };
            bundle.OnUpdate = delegate
            {
                if (sm64.marioState.animFrame == 32 && attackState == 0)
                {
                    attackState = 1;
                    sm64.SetAnimFrame(36);
                    sm64.SetActionTimer(37);
                }

                if (sm64.marioState.animFrame == 37 && attackState == 1)
                {
                    attackState = 2;
                    Interop.PlaySound(SM64Constants.SOUND_MARIO_GROUND_POUND_WAH);
                    base.HitBox_0.IsActive = true;
                }

                if (sm64.marioState.animFrame >= 48)
                    bundle.OnAnimationEnd();
            };
            bundle.OnHit = delegate (BaseCharacter target, bool wasBlocked)
            {
                bool t_IsNPC = (bool)target.GetType().GetField("IsNPC", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);
                if (!(target == null) && !t_IsNPC && target.IsHurt)
                {
                    CharacterControl MyCharacterControl = (CharacterControl)GetField("MyCharacterControl");

                    SetPlayerState(PlayerStateENUM.Cinematic_NoInput);
                    CancelAndRefundPursue();
                    SetField("IsIntangible", true);
                    if (SaveData.Data.MovementRush_IsEnabled_ViaCriticalStrikes)
                    {
                        MovementRushManager MRManager = (MovementRushManager)typeof(BattleController).GetField("MovementRushManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);
                        CharacterControl targetControl = (CharacterControl)GetType().GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);

                        MRManager.StartNewMovementRush((bool)GetField("IsFacingRight"), new List<CharacterControl> { MyCharacterControl }, new List<CharacterControl> { targetControl });
                    }
                    else
                    {
                        BattleCameraManager CamManager = (BattleCameraManager)typeof(BattleController).GetField("CameraManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);

                        Comp_InterplayerCollider.Disable();
                        SetField("IgnoreClashTimer", 1f);
                        CamManager.SetTargetGroup(MyCharacterControl.transform);
                        bundle.OnClashCallback = (bundle.OnInterrupt = (bundle.OnAnimationEnd = delegate
                        {
                            Base_RushProperties RushProperties = new Base_RushProperties();
                            RushProperties.Target = target;
                            SetField("RushProperties", RushProperties);
                            Comp_Animator.Play("Rush_Startup", -1, 0f);
                        }));
                    }
                }
            };
            bundle.IsCinematicsQueued = true;
            return bundle;
        }
    }

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
    public int attackState;

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

        SetField("EnergyMax", 200f);
        SetField("EnergyStart", 100f);
        SetField("IsFacingRight", (base.tag == "Team1"));
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
            sm64input.c = (CharacterControl)GetField("MyCharacterControl");
            sm64 = sm64Obj.AddComponent<SM64Mario>();
            sm64.smbzChar = sm64input.c;
            sm64.changeActionCallback = SMBZ_64.Core.OnMarioChangeAction;
            sm64.advanceAnimFrameCallback = SMBZ_64.Core.OnMarioAdvanceAnimFrame;
            sm64.SetFaceAngle((float)Math.PI / 2 * -FaceDir);

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
        Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control Started");
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

        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_AERIAL_DOWN_ATTACK, 0), AttBun_SmackDown);

        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_TWIRLING, 0), AttBun_Twirl);
        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_TWIRLING, 1), AttBun_Twirl);
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

        if (sm64.marioState.action == (uint)ACT_CUSTOM_ANIM)
        {
            bool IsBursting = (bool)GetField("IsBursting");
            if (IsBursting || GetMovementRushState() != MovementRushStateENUM.Inactive)
            {
                sm64.SetAngle(transform.rotation.eulerAngles.z / 180 * (float)Math.PI * -FaceDir, FaceDir / 2f * -(float)Math.PI, 0);
                if (IsBursting)
                {
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
        }

        sm64input.overrideInput = IsPursuing || GetMovementRushState() != MovementRushStateENUM.Inactive;
        if (GetMovementRushState() != MovementRushStateENUM.Inactive)
        {
            MovementRushManager MRManager = (MovementRushManager)typeof(BattleController).GetField("MovementRushManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);
            MovementRushManager.MRDataStore activeMovementRush = MRManager.ActiveMovementRush;
            if (activeMovementRush.MovementRushType != MovementRushManager.MovementRushTypeENUM.Air)
                sm64input.joyOverride = -Vector2.right * (activeMovementRush.IsDirectionOfRushRight ? 1 : -1);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsHurt && sm64 && (sm64.marioState.action == (uint)ACT_SPAWN_NO_SPIN_AIRBORNE || sm64.marioState.action == (uint)ACT_SPAWN_SPIN_AIRBORNE || sm64.marioState.action == (uint)ACT_SPAWN_NO_SPIN_LANDING || sm64.marioState.action == (uint)ACT_SPAWN_SPIN_LANDING))
            sm64.SetAction(ACT_HARD_BACKWARD_AIR_KB);
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

    public override void OnActivateMovementRush()
    {
        base.OnActivateMovementRush();

        if (sm64 != null)
            sm64.SetFaceAngle(FaceDir / 2f * -(float)Math.PI);
    }

    [HarmonyPatch(typeof(BaseCharacter), "PrepareAnAttack", new Type[] { typeof(AttackBundle), typeof(float) })]
    private static class PrepareAnAttackPatch
    {
        private static void Postfix(BaseCharacter __instance)
        {
            if (__instance.GetType() != typeof(Mario64Control))
                return;

            Mario64Control c = (Mario64Control)__instance;
            MovementRushStateENUM movRush =
                (MovementRushStateENUM)c.GetMovementRushState();

            if (c.CurrentAttackData != null && c.CurrentAttackData.OnAnimationStart != null)
                c.CurrentAttackData.OnAnimationStart();
            Melon<SMBZ_64.Core>.Logger.Msg($"OnAnimationStart {c.CurrentAttackData.AnimationName} {c.CurrentAttackData.AnimationNameHash == c.ASN_MR_Dodge} {c.GetPlayerState()} {movRush} {c.CurrentAttackData.OnAnimationStart_HasExecuted}");
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
                    c.SetField("IsIntangible", true);
                    c.SetField("DragOverride", c.GetField("DodgeDrag"));
                    c.SetGravityOverride(0f);
                    c.OnMovementRush_Dodge();
                    c.movRushTimer = 0;
                    c.OnAttackAnimation_QueueCustom(null);
                    c.Comp_InterplayerCollider.Disable();
                    c.sm64.SetAction(ACT_TWIRLING, 2);
                },
                OnInterrupt = delegate
                {
                    c.SetField("IsIntangible", false);
                    c.MovementRush_SetDefaultGravityAndDrag();
                    c.SetMovementRushState(MovementRushStateENUM.Idle);
                    c.Comp_InterplayerCollider.Enable();
                },
                OnAnimationEnd = delegate
                {
                    c.SetField("IsIntangible", false);
                    c.MovementRush_SetDefaultGravityAndDrag();
                    c.SetMovementRushState(MovementRushStateENUM.Idle);
                    c.SetPlayerState(PlayerStateENUM.Idle);
                    c.CurrentAttackData = null;
                    c.Comp_InterplayerCollider.Enable();
                    c.sm64.SetAction(ACT_FREEFALL);
                    //c.PlayAnimationIfNotAlready(ASN_MR_Air_Idle);
                },
                OnFixedUpdate = delegate
                {
                    c.movRushTimer += Time.fixedDeltaTime;
                    if (c.movRushTimer > 0.55f)
                    {
                        c.OnAttackAnimation_QueueCustom(null);
                        c.CurrentAttackData.OnAnimationEnd();
                    }
                }
            };
            bundle.SetCustomeQueue(delegate
            {
                if (bundle.CustomQueueCallCount == 0)
                {
                    float DodgeSpeed = (float)c.GetField("DodgeSpeed");
                    if (!directionOverride.HasValue)
                    {
                        /*
                        bool flag = (IsCPUControlled ? (MyCharacterControl.AI.MovementIdea.VerticalDirection < 0) : (MyCharacterControl.Button_Down.IsBuffered || MyCharacterControl.Button_Down.IsHeld));
                        bool flag2 = (IsCPUControlled ? (0 < MyCharacterControl.AI.MovementIdea.VerticalDirection) : (MyCharacterControl.Button_Up.IsBuffered || MyCharacterControl.Button_Up.IsHeld));
                        bool flag3 = (IsCPUControlled ? (MyCharacterControl.AI.MovementIdea.HorizontalDirection < 0) : (MyCharacterControl.Button_Left.IsBuffered || MyCharacterControl.Button_Left.IsHeld));
                        bool flag4 = (IsCPUControlled ? (0 < MyCharacterControl.AI.MovementIdea.HorizontalDirection) : (MyCharacterControl.Button_Right.IsBuffered || MyCharacterControl.Button_Right.IsHeld));
                        */
                        CharacterControl MyCharacterControl = (CharacterControl)c.GetField("MyCharacterControl");
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
                    c.SetField("IsIntangible", false);
                }
            }, 2);
            c.PrepareAnAttack(bundle);

            return false;
        }
    }

    [HarmonyPatch(typeof(BaseCharacter), "PerformAction_Engage", new Type[] { typeof(CharacterControl) })]
    private static class PerformAction_EngagePatch
    {
        private static bool Prefix(BaseCharacter __instance, CharacterControl targetCharacterControl)
        {
            if (__instance.GetType() != typeof(Mario64Control))
                return true;

            Mario64Control c = (Mario64Control)__instance;
            c.InterruptAndNullifyPreparedAttack();
            c.SetField("StrikeTarget", targetCharacterControl);
            c.SetPlayerState(PlayerStateENUM.Attacking);
            c.SetMovementRushState(MovementRushStateENUM.IsEngaging);
            c.Comp_InterplayerCollider.Disable();
            AttackBundle attackToPrepare = new AttackBundle
            {
                AnimationNameHash = c.ASN_MR_Strike_Approach,
                OnInterrupt = delegate
                {
                    c.SetField("StrikeTarget", null);
                    c.ResetRotation();
                    c.SetMovementRushState(MovementRushStateENUM.Idle);
                    c.Comp_InterplayerCollider.Enable();
                },
                OnAnimationStart = delegate
                {
                    c.sm64.SetAction(ACT_SPAWN_SPIN_AIRBORNE, 1);
                }
            };

            CharacterControl StrikeTarget = (CharacterControl)c.GetField("StrikeTarget");
            float StrikeSpeed = (float)c.GetField("StrikeSpeed");

            attackToPrepare.CustomDataList.Add(false);
            attackToPrepare.CustomDataList.Add(false);
            attackToPrepare.CustomDataList.Add(false);
            attackToPrepare.CustomDataList.Add(false);
            attackToPrepare.OnFixedUpdate = delegate
            {
                if (!(c.GetField("StrikeTarget") == null))
                {
                    Vector3 vector4 = StrikeTarget.CharacterGO.transform.position - c.transform.position;
                    Vector3 normalized = vector4.normalized;
                    _ = normalized * StrikeSpeed;
                    int num = 1;
                    float num2 = StrikeSpeed * BattleController.instance.ActorFixedDeltaTime;
                    if (vector4.magnitude < (float)num + num2)
                    {
                        attackToPrepare.CustomDataList[0] = true;
                        attackToPrepare.CustomDataList[1] = normalized;
                        c.SetVelocity(0f, 0f);
                    }
                }
            };
            attackToPrepare.OnUpdate = delegate
            {
                if (StrikeTarget == null)
                {
                    c.PerformAction_Whiff();
                }
                else
                {
                    Vector3 vector = (StrikeTarget.CharacterGO.transform.position - c.transform.position).normalized * StrikeSpeed;
                    c.SetVelocity(vector);
                    if ((bool)attackToPrepare.CustomDataList[0])
                    {
                        MovementRushManager MRManager = (MovementRushManager)typeof(BattleController).GetField("MovementRushManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);
                        BattleCameraManager CamManager = (BattleCameraManager)typeof(BattleController).GetField("CameraManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);
                        IntensityDataStore Intensity = (IntensityDataStore)typeof(BattleController).GetField("Intensity", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);
                        ClashAndBurstManager _ClashAndBurstManager = (ClashAndBurstManager)typeof(BattleController).GetField("ClashAndBurstManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(BattleController.instance);
                        bool targetIntangible = (bool)typeof(BaseCharacter).GetField("IsIntangible", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(StrikeTarget.CharacterGO);
                        MovementRushStateENUM targetMRState = (MovementRushStateENUM)typeof(BaseCharacter).GetField("MovementRushState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(StrikeTarget.CharacterGO);
                        MethodInfo InvokeOnClashMethod = typeof(MovementRushManager.MRDataStore).GetMethod("InvokeOnClash", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(CharacterControl), typeof(CharacterControl) }, null);
                        MethodInfo InvokeOnDodgeMethod = typeof(MovementRushManager.MRDataStore).GetMethod("InvokeOnDodge", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(CharacterControl), typeof(CharacterControl) }, null);

                        Vector2 vector2 = (Vector3)attackToPrepare.CustomDataList[1];
                        Vector2 vector3 = (Vector2)c.transform.position + vector2 * 0.5f;
                        StrikeTarget.CharacterGO.transform.position = new Vector3(vector3.x, vector3.y, c.transform.position.z);
                        c.SetVelocity(vector2 * c.GetVelocity().magnitude);
                        if (targetIntangible)
                        {
                            StrikeTarget.CharacterGO.OnMovementRush_DodgeSuccess();
                            StrikeTarget.CharacterGO.PerformAction_Dodge(vector2 * -1f);
                            c.PerformAction_Whiff();
                            //MRManager.ActiveMovementRush?.InvokeOnDodge(StrikeTarget, (CharacterControl)c.GetField("MyCharacterControl"));
                            InvokeOnDodgeMethod.Invoke(MRManager.ActiveMovementRush, new object[] { StrikeTarget, (CharacterControl)c.GetField("MyCharacterControl") });
                        }
                        else if (targetMRState == MovementRushStateENUM.IsEngaging)
                        {
                            MethodInfo PlaySoundMethod = typeof(SoundCache).GetMethod("PlaySound", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(AudioClip), typeof(float), typeof(bool), typeof(float?), typeof(float), typeof(bool) }, null);

                            EffectSprite.Create((c.transform.position + (Vector3)vector3) * 0.5f, EffectSprite.Sprites.HitsparkBlock);
                            PlaySoundMethod.Invoke(SoundCache.ins, new object[] { SoundCache.ins.Battle_Reflect, 1f, true, null, 1f, false });
                            CamManager.SetShake(0.15f);
                            Intensity.IncreaseBy(20f);
                            if (Intensity.GetAmount() >= 100f && !(bool)c.GetField("IsBursting"))
                            {
                                _ClashAndBurstManager.BeginBurst((c.transform.position + StrikeTarget.transform.position) * 0.5f, (CharacterControl)c.GetField("MyCharacterControl"), StrikeTarget, 1, BurstDataStore.VictoryStrikeENUM.MovementRushFinisher);
                            }

                            StrikeTarget.CharacterGO.PerformAction_Clash(vector2 * -1f);
                            c.PerformAction_Clash(vector2);
                            //MRManager.ActiveMovementRush?.InvokeOnClash((CharacterControl)c.GetField("MyCharacterControl"), StrikeTarget);
                            InvokeOnClashMethod.Invoke(MRManager.ActiveMovementRush, new object[] { (CharacterControl)c.GetField("MyCharacterControl"), StrikeTarget });
                        }
                        else
                        {
                            c.PerformAction_Strike();
                        }
                    }
                    else
                    {
                        c.SetField("IsFacingRight", c.GetVelocity().normalized.x >= 0f);
                        c.RotateTowardTargetDirection(c.GetVelocity().normalized);
                        c.FlipSpriteByFacingDirection();
                        c.sm64.SetFaceAngle(c.FaceDir / 2f * -(float)Math.PI);
                    }
                }
            };
            c.PrepareAnAttack(attackToPrepare);
            c.OnMovementRush_Engage();

            return false;
        }
    }

    [HarmonyPatch(typeof(BaseCharacter), "PerformAction_Whiff")]
    private static class PerformAction_WhiffPatch
    {
        private static bool Prefix(BaseCharacter __instance)
        {
            if (__instance.GetType() != typeof(Mario64Control))
                return true;

            Mario64Control c = (Mario64Control)__instance;
            c.InterruptAndNullifyPreparedAttack();
            c.RotateTowardTargetDirection(c.GetVelocity().normalized);
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c.HitBox_0, null);
            c.Comp_InterplayerCollider.Disable();
            c.PrepareAnAttack(new AttackBundle
            {
                AnimationNameHash = c.ASN_MR_Strike_Attack,
                OnAnimationStart = delegate
                {
                    c.OnMovementRush_Whiff();
                    c.SetVelocity(c.GetVelocity().normalized * (float)c.GetField("StrikeEndSpeed"));
                    c.SetGravityOverride(0f);
                    c.SetField("DragOverride", 1f);
                    c.SetMovementRushState(MovementRushStateENUM.IsWhiffing);
                    c.movRushTimer = 0;
                    c.sm64.SetAction(ACT_CUSTOM_ANIM);
                    c.sm64.SetAnim(MARIO_ANIM_SLIDE_KICK);
                    c.sm64.SetAnimFrame(8);
                    c.sm64.SetFaceAngle(c.FaceDir / 2f * -(float)Math.PI);
                },
                OnInterrupt = delegate
                {
                    c.ResetRotation();
                    c.MovementRush_SetDefaultGravityAndDrag();
                    c.SetMovementRushState(MovementRushStateENUM.Idle);
                    c.Comp_InterplayerCollider.Enable();
                },
                OnAnimationEnd = delegate
                {
                    c.ResetRotation();
                    c.MovementRush_SetDefaultGravityAndDrag();
                    c.SetMovementRushState(MovementRushStateENUM.Idle);
                    c.SetPlayerState(PlayerStateENUM.Idle);
                    c.PlayAnimationIfNotAlready(c.ASN_MR_Air_Idle);
                    c.Comp_InterplayerCollider.Enable();
                    c.sm64.SetAction(ACT_FREEFALL);
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

    [HarmonyPatch(typeof(BaseCharacter), "PerformAction_Clash", new Type[] { typeof(Vector2?) })]
    private static class PerformAction_ClashPatch
    {
        private static bool Prefix(BaseCharacter __instance, Vector2? direction = null)
        {
            if (__instance.GetType() != typeof(Mario64Control))
                return true;

            Mario64Control c = (Mario64Control)__instance;
            float StrikeEndSpeed = (float)c.GetField("StrikeEndSpeed");

            if (!direction.HasValue)
            {
                direction = c.GetVelocity().normalized;
            }

            c.InterruptAndNullifyPreparedAttack();
            c.SetField("IsFacingRight", direction.Value.x >= 0f);
            c.RotateTowardTargetDirection(direction.Value);
            c.FlipSpriteByFacingDirection();
            typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(c.HitBox_0, null);
            c.Comp_InterplayerCollider.Disable();
            c.PrepareAnAttack(new AttackBundle
            {
                AnimationNameHash = c.ASN_MR_Strike_Attack,
                OnAnimationStart = delegate
                {
                    c.SetVelocity(direction.Value * (0f - StrikeEndSpeed));
                    c.SetGravityOverride(0f);
                    c.SetField("DragOverride", 1f);
                    c.SetMovementRushState(MovementRushStateENUM.IsWhiffing);
                    c.movRushTimer = 0;
                    c.sm64.SetAction(ACT_CUSTOM_ANIM);
                    c.sm64.SetAnim(MARIO_ANIM_SLIDE_KICK);
                    c.sm64.SetAnimFrame(8);
                    c.sm64.SetFaceAngle(c.FaceDir / 2f * -(float)Math.PI);
                },
                OnInterrupt = delegate
                {
                    c.ResetRotation();
                    c.MovementRush_SetDefaultGravityAndDrag();
                    c.SetMovementRushState(MovementRushStateENUM.Idle);
                    c.Comp_InterplayerCollider.Enable();
                },
                OnAnimationEnd = delegate
                {
                    c.ResetRotation();
                    c.MovementRush_SetDefaultGravityAndDrag();
                    c.SetMovementRushState(MovementRushStateENUM.Idle);
                    c.SetPlayerState(PlayerStateENUM.Idle);
                    c.PlayAnimationIfNotAlready(c.ASN_MR_Air_Idle);
                    c.Comp_InterplayerCollider.Enable();
                    c.CurrentAttackData = null;
                    c.sm64.SetAction(ACT_FREEFALL);
                },
                OnFixedUpdate = delegate
                {
                    c.movRushTimer += Time.fixedDeltaTime;
                    if (c.movRushTimer > 0.55f)
                        c.CurrentAttackData.OnAnimationEnd();
                }
            });
            c.OnMovementRush_Clash();
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
            float StrikeLaunch = (float)c.GetField("StrikeLaunch");
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
                    c.sm64.SetAction(ACT_CUSTOM_ANIM);
                    c.sm64.SetAnim(MARIO_ANIM_SLIDE_KICK);
                    c.sm64.SetAnimFrame(8);
                    c.sm64.SetFaceAngle(c.FaceDir/2f * -(float)Math.PI);
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
                    c.PlayAnimationIfNotAlready(c.ASN_MR_Air_Idle);
                    c.Comp_InterplayerCollider.Enable();
                    c.CurrentAttackData = null;
                    c.sm64.SetAction(ACT_FREEFALL);
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
            c.SetVelocity(velocity + new Vector2(0, 1.5f));
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
                                CharacterControl MyCharacterControl = (CharacterControl)c.GetField("MyCharacterControl");
                                CharacterControl target_MyCharacterControl = (CharacterControl)typeof(BaseCharacter).GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);
                                MRManager.EndMovementRush(MyCharacterControl, target_MyCharacterControl);
                            }
                        }
                    );
                    c.HitBox_0.transform.localPosition = new Vector2(0.5f, -0.6f);
                    c.HitBox_0.transform.localScale = new Vector2(1.25f, 1f);
                    c.HitBox_0.IsActive = false;
                    c.movRushTimer = 0;
                    c.sm64.SetAction(ACT_SPAWN_SPIN_AIRBORNE, 1);
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
                    if (c.movRushTimer > 0.9f && c.sm64.marioState.action != (uint)ACT_JUMP_KICK)
                    {
                        c.sm64.SetAction(ACT_JUMP_KICK, 1);
                        c.sm64.SetAngle((float)Math.PI / 6, c.sm64.marioState.faceAngle, 0);
                    }
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

    protected override void Update_Blocking()
    {
        CharacterControl MyCharacterControl = (CharacterControl)GetField("MyCharacterControl");
        AI_Bundle AI = (AI_Bundle)typeof(CharacterControl).GetField("AI", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(MyCharacterControl);

        if (!MyCharacterControl.IsInputLocked && ((AI.GuardIdea?.IsActive ?? false) || (MyCharacterControl.Button_Guard?.IsHeld ?? false)))
        {
            bool CanGuard =
                sm64.marioState.action == (uint)ACT_START_CROUCHING ||
                sm64.marioState.action == (uint)ACT_CROUCHING ||
                sm64.marioState.action == (uint)ACT_CROUCH_SLIDE ||
                sm64.marioState.action == (uint)ACT_CROUCH_AIR;

            if (CanGuard && (IsIdle || IsGuarding))
            {
                SetPlayerState(PlayerStateENUM.Guarding);
                bool flag = false;
                bool flag2 = false;
                if (AI.GuardIdea != null)
                {
                    flag = AI.GuardIdea.Direction < 0;
                    flag2 = AI.GuardIdea.Direction > 0;
                }

                if (MyCharacterControl.Button_Left != null)
                {
                    flag = MyCharacterControl.Button_Left.IsHeld && !MyCharacterControl.Button_Right.IsHeld;
                    flag2 = !MyCharacterControl.Button_Left.IsHeld && MyCharacterControl.Button_Right.IsHeld;
                }

                if (flag)
                {
                    SetField("IsFacingRight", false);
                }
                else if (flag2)
                {
                    SetField("IsFacingRight", true);
                }

                if (AI.GuardIdea != null && AI.GuardIdea.Duration > 0f)
                {
                    AI.GuardIdea.Duration -= BattleController.instance.DeltaTime;
                }
            }
        }
        else if (IsGuarding)
        {
            float BlockStun = (float)GetField("BlockStun");
            if (BlockStun > 0f)
            {
                SetPlayerState(PlayerStateENUM.Guarding);
            }
            else
            {
                SetPlayerState(PlayerStateENUM.Idle);
            }
        }
    }

    protected override void Update_Pursue()
    {
        FieldInfo PursueDataField = GetType().GetField("PursueData", BindingFlags.NonPublic | BindingFlags.Instance);
        PursueBundle PursueData = (PursueBundle)PursueDataField.GetValue(this);
        FieldInfo isChargingField = typeof(PursueBundle).GetField("isCharging", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo IsFacingRightField = GetType().GetField("IsFacingRight", BindingFlags.NonPublic | BindingFlags.Instance);
        MethodInfo PlaySoundMethod = typeof(SoundCache).GetMethod("PlaySound", BindingFlags.NonPublic | BindingFlags.Instance, null, new[]{typeof(AudioClip), typeof(float), typeof(bool), typeof(float?), typeof(float), typeof(bool)}, null);
        bool IsFrozen = (bool)GetField("IsFrozen");

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
                SetField("ComboSwingCounter", 0);
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

        sm64.SetAction(ACT_STAR_DANCE_EXIT, 1);
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
                    bool IsFacingRight = (bool)GetField("IsFacingRight");
                    CharacterControl MyCharacterControl = (CharacterControl)GetField("MyCharacterControl");
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

    protected override void Perform_Grounded_DownSpecial()
    {
        CharacterControl MyCharacterControl = (CharacterControl)GetField("MyCharacterControl");
        IntensityDataStore Intensity = (IntensityDataStore)typeof(BattleController).GetField("Intensity", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(BattleController.instance);
        if (Intensity.IsCriticalHitReady(MyCharacterControl.PlayerDataReference.PlayerIndex))
        {
            Intensity.UseCriticalStrike(MyCharacterControl.PlayerDataReference.PlayerIndex);
            PrepareAnAttack(AttBun_CriticalPunch);
        }
        else
        {
            PrepareAnAttack(AttBun_StarDancePunch);
        }
    }

    protected override void Perform_Aerial_DownAttack()
    {
        if (sm64 == null) return;

        sm64.SetAction(ACT_AERIAL_DOWN_ATTACK);
        PrepareAnAttack(AttBun_SmackDown);
    }

    protected override void Perform_Aerial_NeutralSpecial()
    {
        if (sm64 == null) return;

        sm64.SetAction(ACT_TWIRLING);
        if (sm64.marioState.velocity[1] < 0)
            sm64.SetVelocity(new Vector3(sm64.marioState.velocity[0], 30, 0));
        PrepareAnAttack(AttBun_Twirl);
    }

    protected override void Perform_Aerial_DownSpecial()
    {
        if (sm64 == null) return;

        sm64.SetAction(ACT_GROUND_POUND);
        PrepareAnAttack(AttBun_GroundPoundAir);
    }


    public void OnChangeSM64Action(SM64Constants.Action action, uint actionArg)
    {
        if (GetMovementRushState() != MovementRushStateENUM.Inactive)
            return;

        ActionKeyPair k = new ActionKeyPair(action, actionArg);
        Melon<SMBZ_64.Core>.Logger.Msg($"Mario64Control action {action} {actionArg}");
        if (SM64AttacksActionArg.ContainsKey(k))
        {
            PrepareAnAttack(SM64AttacksActionArg[k]);
        }
        else
        {
            if (action != ACT_STAR_DANCE_EXIT && action != ACT_CUSTOM_ANIM && action != ACT_CUSTOM_ANIM_TO_ACTION)
            {
                if (!IsPursuing)
                    SetPlayerState(PlayerStateENUM.Idle);

                base.HitBox_0.IsActive = false;
            }
        }
    }

    public void OnMarioAdvanceAnimFrame(SM64Constants.MarioAnimID animID, short animFrame)
    {
        if (GetMovementRushState() != MovementRushStateENUM.Inactive || sm64.marioState.action == (uint)ACT_CUSTOM_ANIM || sm64.marioState.action == (uint)ACT_CUSTOM_ANIM_TO_ACTION)
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
