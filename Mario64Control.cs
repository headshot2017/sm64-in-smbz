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
using static MelonLoader.MelonLogger;

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
            Vector2 groundPositionViaRaycast = GetGroundPositionViaRaycast();
            bool IsFacingRight = (bool)GetField("IsFacingRight");
            EffectSprite.Create(groundPositionViaRaycast + new Vector2(0f, 0.2f), EffectSprite.Sprites.DustPuff, IsFacingRight);
            EffectSprite.Create(groundPositionViaRaycast + new Vector2(0f, 0.2f), EffectSprite.Sprites.DustPuff, !IsFacingRight);

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

    private AttackBundle AttBun_UpperPunch
    {
        get
        {
            AttackBundle atk = new AttackBundle
            {
                AnimationName = "UpperPunch",
                OnAnimationStart = delegate
                {
                    SetPlayerState(PlayerStateENUM.Attacking);
                    typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                        new HitBoxDamageParameters
                        {
                            Owner = this,
                            Tag = base.tag,
                            Damage = 1f,
                            HitStun = 0.4f,
                            Launch = new Vector2(2 * FaceDir, 14f),
                            FreezeTime = 0.03f,
                            Priority = BattleCache.PriorityType.Light,
                            HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                            OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A
                        }
                    );
                    base.HitBox_0.transform.localPosition = new Vector2(0.35f, 0.45f);
                    base.HitBox_0.transform.localScale = new Vector2(0.9f, 0.9f);
                    base.HitBox_0.IsActive = false;
                    sm64.SetAnimFrame(0);
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
                if (sm64.marioState.animID != (short)MARIO_ANIM_SINGLE_JUMP)
                    return;

                if (sm64.marioState.animFrame >= 5)
                    atk.OnAnimationEnd();
                if (sm64.marioState.animFrame >= 3)
                    base.HitBox_0.IsActive = true;
            };
            return atk;
        }
    }


    private AttackBundle AttBun_UpperPunchAir
    {
        get
        {
            AttackBundle atk = new AttackBundle
            {
                AnimationName = "UpperPunchAir",
                OnAnimationStart = delegate
                {
                    SetPlayerState(PlayerStateENUM.Attacking);
                    typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                        new HitBoxDamageParameters
                        {
                            Owner = this,
                            Tag = base.tag,
                            Damage = 2f,
                            HitStun = 0.6f,
                            Launch = new Vector2(4*FaceDir, 12f),
                            FreezeTime = 0.03f,
                            Priority = BattleCache.PriorityType.Light,
                            HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                            OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A
                        }
                    );
                    base.HitBox_0.transform.localPosition = new Vector2(0.35f, 0.45f);
                    base.HitBox_0.transform.localScale = new Vector2(0.9f, 0.9f);
                    base.HitBox_0.IsActive = false;
                    sm64.SetAnimFrame(0);
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
                if (sm64.marioState.animID != (short)MARIO_ANIM_SINGLE_JUMP)
                    return;

                if (sm64.marioState.animFrame >= 5)
                    atk.OnAnimationEnd();
                if (sm64.marioState.animFrame >= 3)
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
            base.HitBox_0.IsActive = false;
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
                            Damage = 2f,
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
                    base.HitBox_0.ReinitializeID();
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
                    //if (SaveData.Data.MovementRush_IsEnabled_ViaCriticalStrikes)
                    {
                        CharacterControl targetControl = (CharacterControl)GetType().GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);

                        SMBZGlobals.MovementRushManager.StartNewMovementRush((bool)GetField("IsFacingRight"), new List<CharacterControl> { MyCharacterControl }, new List<CharacterControl> { targetControl });
                    }
                    /*
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
                    */
                }
            };
            bundle.IsCinematicsQueued = true;
            return bundle;
        }
    }

    private AttackBundle AttBun_GroundTwirl
    {
        get
        {
            AttackBundle atk = new AttackBundle
            {
                AnimationName = "GroundTwirl",
                OnAnimationStart = delegate
                {
                    SetPlayerState(PlayerStateENUM.Attacking);
                    base.HitBox_0.transform.localPosition = new Vector2(0f, 0f);
                    base.HitBox_0.transform.localScale = new Vector2(1.25f, 0.6f);
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
                if (sm64.marioState.animID != (short)MARIO_ANIM_TWIRL)
                    return;

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
                            Damage = 1.5f,
                            HitStun = 0.2f,
                            GetLaunch = () => new Vector2(-sm64.marioState.velocity[0] / 4f, 0),
                            FreezeTime = 0.03f,
                            Priority = BattleCache.PriorityType.Medium,
                            HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                            OnHitSoundEffect = SoundCache.ins.Battle_Hit_1A
                        }
                    );
                }

                if (sm64.marioState.forwardVel <= 0)
                    atk.OnAnimationEnd();
            };
            return atk;
        }
    }

    private AttackBundle AttBun_SuperUppercut
    {
        get
        {
            AttackBundle atk = new AttackBundle
            {
                AnimationName = "SuperUppercut",
                OnAnimationStart = delegate
                {
                    SetPlayerState(PlayerStateENUM.Attacking);
                    typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.HitBox_0,
                        new HitBoxDamageParameters
                        {
                            Owner = this,
                            Tag = base.tag,
                            Damage = 5f / 6f,
                            HitStun = 0.65f,
                            Launch = new Vector2(3f * FaceDir, 12f),
                            FreezeTime = 0.03f,
                            Priority = BattleCache.PriorityType.Light,
                            HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                            OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A
                        }
                    );
                    base.HitBox_0.transform.localPosition = new Vector2(0.35f, 0.45f);
                    base.HitBox_0.transform.localScale = new Vector2(0.9f, 0.9f);
                    base.HitBox_0.IsActive = false;
                    Comp_InterplayerCollider.Disable();
                    attackState = 0;
                    attackTimer = 0;
                },
                OnAnimationEnd = delegate
                {
                    SetPlayerState(PlayerStateENUM.Idle);
                    base.HitBox_0.IsActive = false;
                    CurrentAttackData = null;
                    Comp_InterplayerCollider.Enable();
                    Melon<SMBZ_64.Core>.Logger.Msg("ended");
                },
                OnInterrupt = delegate
                {
                    base.HitBox_0.IsActive = false;
                    Comp_InterplayerCollider.Enable();
                }
            };
            atk.OnFixedUpdate = delegate
            {
                if (sm64.marioState.actionState != 2)
                    return;
                else if (sm64.marioState.velocity[1] <= -28)
                {
                    atk.OnAnimationEnd();
                    return;
                }

                base.HitBox_0.IsActive = true;
                attackTimer += Time.fixedDeltaTime;
                if (attackTimer > 0.08f && attackState++ < 5)
                {
                    attackTimer = 0;
                    base.HitBox_0.ReinitializeID();
                }
            };
            return atk;
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
    public float attackTimer;
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
        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_UP_ATTACK, 0), AttBun_UpperPunch);
        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_UP_ATTACK, 1), AttBun_UpperPunchAir);
        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_HEAVY_ATTACK, 0), AttBun_GroundTwirl);
        SM64AttacksActionArg.Add(new ActionKeyPair(ACT_HEAVY_UP_ATTACK, 0), AttBun_SuperUppercut);

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

    protected override void Update_CPU_Thoughts()
    {
        CharacterControl MyCharacterControl = (CharacterControl)GetField("MyCharacterControl");
        AI_Bundle AI = (AI_Bundle)typeof(CharacterControl).GetField("AI", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(MyCharacterControl);

        base.Update_CPU_Thoughts();
        AI_Bundle.DeltaDataModel dd = AI.DeltaData;
        if (!dd.ShouldContinueWithProcessing)
        {
            return;
        }

        if (AI.RethinkCooldown_Movement > 0f)
        {
            AI.RethinkCooldown_Movement -= BattleController.instance.UnscaledDeltaTime;
        }
        else
        {
            if (AI.GetMood() == AI_Bundle.Enum_Mood.Aggressive)
            {
                if (dd.distanceToTarget > 6f && MyCharacterControl.ParticipantDataReference.Energy.GetCurrent() > 100f)
                {
                    AI.PursueIdea = new AI_Bundle.Internal_PursueIdea(UnityEngine.Random.Range(0, 101));
                }
                else if (dd.distanceToTarget > 1f)
                {
                    AI.MovementIdea.Reset(dd.IsTargetToMyRight ? 1 : (-1));
                }
                else
                {
                    AI.MovementIdea.Reset(dd.IsTargetToMyRight ? 1 : (-1), 1f, JustTurn: true);
                }
            }
            else if (AI.GetMood() == AI_Bundle.Enum_Mood.Defensive)
            {
                if (dd.distanceToTarget > 6f)
                {
                    AI.MovementIdea.Reset(dd.IsTargetToMyRight ? 1 : (-1));
                }
                else
                {
                    AI.MovementIdea.Reset(dd.IsTargetToMyRight ? 1 : (-1), 1f, JustTurn: true);
                }
            }
            else if (AI.GetMood() == AI_Bundle.Enum_Mood.Tactical)
            {
                if (dd.distanceToTarget > 1f)
                {
                    AI.MovementIdea.Reset(dd.IsTargetToMyRight ? 1 : (-1));
                }
                else
                {
                    AI.MovementIdea.Reset(dd.IsTargetToMyRight ? 1 : (-1), 1f, JustTurn: true);
                }

                if (dd.vectorToTarget_Absolute.x > 6f)
                {
                    AI.JumpIdea = new AI_Bundle.Internal_JumpIdea(IsFullJump: true, dd.IsTargetToMyRight ? 1 : (-1));
                }
            }

            AI.RethinkCooldown_Movement = AI.GetRethinkCooldown(AI_Bundle.Enum_RethinkCooldownType.Movement);
        }

        if (AI.RethinkCooldown_Guarding > 0f)
        {
            AI.RethinkCooldown_Guarding -= BattleController.instance.UnscaledDeltaTime;
        }

        if (AI.CommandList.ActionQueue.Count > 0)
        {
            AI_CommandList_Update();
        }
        else
        {
            if (base.IsAttacking)
            {
                return;
            }

            if (AI.RethinkCooldown_Attacking > 0f)
            {
                AI.RethinkCooldown_Attacking -= BattleController.instance.UnscaledDeltaTime;
                return;
            }

            if (AI.GetMood() == AI_Bundle.Enum_Mood.Tactical && UnityEngine.Random.Range(0, 2) == 1 && dd.Target.IsGuarding)
            {
                if (base.IsOnGround)
                {
                    if ((dd.IsCriticalHitReady || !dd.TargetIsApproachingMe) ? (0f <= dd.vectorToTarget_Absolute.x && dd.vectorToTarget_Absolute.x <= 3f && 0f <= dd.vectorToTarget_Absolute.y && dd.vectorToTarget_Absolute.y <= 4f) : (1f <= dd.vectorToTarget_Absolute.x && dd.vectorToTarget_Absolute.x <= 5f))
                    {
                        AI.RethinkCooldown_Attacking = AI.GetRethinkCooldown(AI_Bundle.Enum_RethinkCooldownType.Attacking);
                        AI.AttackIdea = new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Down, ZTrigger: true);
                    }
                }
                else if (dd.vectorToTarget_Absolute.x < 5f)
                {
                    AI.RethinkCooldown_Attacking = AI.GetRethinkCooldown(AI_Bundle.Enum_RethinkCooldownType.Attacking);
                    AI.AttackIdea = new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Neutral, ZTrigger: true);
                }
            }

            if (!(AI.RethinkCooldown_Attacking <= 0f))
            {
                return;
            }

            if (dd.IsTargetWithinAltitude)
            {
                if (dd.vectorToTarget_Absolute.x <= 1.5f)
                {
                    if (base.IsOnGround)
                    {
                        AI.RethinkCooldown_Attacking = AI.GetRethinkCooldown(AI_Bundle.Enum_RethinkCooldownType.Attacking);
                        bool flag = false;
                        if (AI.Difficulty == AI_Bundle.Enum_DifficultyLevel.Hard && dd.IsCriticalHitReady && UnityEngine.Random.Range(0, 2) <= 1)
                        {
                            AI.RethinkCooldown_Attacking = AI.GetRethinkCooldown(AI_Bundle.Enum_RethinkCooldownType.Attacking);
                            AI.AttackIdea = new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Down, ZTrigger: true);
                            flag = true;
                        }

                        if (flag)
                        {
                            return;
                        }

                        switch (UnityEngine.Random.Range(1, 5 + (dd.IsCriticalHitReady ? 1 : 0)))
                        {
                            case 1:
                                AI.CommandList.Set(new AI_Bundle.AI_Action[3]
                                {
                                new AI_Bundle.AI_Action(new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Neutral, ZTrigger: false)),
                                new AI_Bundle.AI_Action(new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Neutral, ZTrigger: false)),
                                new AI_Bundle.AI_Action(new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Neutral, ZTrigger: false))
                                }, () => dd.vectorToTarget_Absolute.x > 3f);
                                break;
                            case 2:
                                AI.CommandList.Set(new AI_Bundle.AI_Action[3]
                                {
                                new AI_Bundle.AI_Action(new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Neutral, ZTrigger: false)),
                                new AI_Bundle.AI_Action(new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Neutral, ZTrigger: false)),
                                new AI_Bundle.AI_Action(new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Up, ZTrigger: false))
                                }, () => dd.vectorToTarget_Absolute.x > 5f);
                                if (AI.Difficulty == AI_Bundle.Enum_DifficultyLevel.Hard)
                                {
                                    AI.CommandList.ActionQueue.Add(new AI_Bundle.AI_Action(new AI_Bundle.Internal_JumpIdea(IsFullJump: false, FaceDir)));
                                    AI.CommandList.ActionQueue.Add(new AI_Bundle.AI_Action(new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Up, ZTrigger: true)));
                                }

                                break;
                            case 3:
                                if (AI.GetMood() == AI_Bundle.Enum_Mood.Tactical)
                                {
                                    AI.CommandList.Set(new AI_Bundle.AI_Action[2]
                                    {
                                    new AI_Bundle.AI_Action(new AI_Bundle.Internal_JumpIdea(IsFullJump: false, 0)),
                                    new AI_Bundle.AI_Action(new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Down, ZTrigger: false))
                                    });
                                    break;
                                }

                                AI.CommandList.Set(new AI_Bundle.AI_Action[3]
                                {
                                new AI_Bundle.AI_Action(new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Neutral, ZTrigger: false)),
                                new AI_Bundle.AI_Action(new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Neutral, ZTrigger: false)),
                                new AI_Bundle.AI_Action(new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Down, ZTrigger: false))
                                }, () => dd.vectorToTarget_Absolute.x > 3f);
                                if (AI.Difficulty == AI_Bundle.Enum_DifficultyLevel.Hard && UnityEngine.Random.Range(0, 3) == 0)
                                {
                                    AI.CommandList.ActionQueue.Add(new AI_Bundle.AI_Action(new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Up, ZTrigger: true)));
                                }

                                break;
                            case 4:
                                AI.CommandList.Set(new AI_Bundle.AI_Action[1]
                                {
                                new AI_Bundle.AI_Action(new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Down, ZTrigger: false))
                                }, () => dd.vectorToTarget_Absolute.x > 3f);
                                break;
                            case 5:
                                AI.RethinkCooldown_Attacking = AI.GetRethinkCooldown(AI_Bundle.Enum_RethinkCooldownType.Attacking);
                                AI.AttackIdea = new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Down, ZTrigger: true);
                                break;
                        }
                    }
                    else
                    {
                        AI.RethinkCooldown_Attacking = AI.GetRethinkCooldown(AI_Bundle.Enum_RethinkCooldownType.Attacking);
                        switch (UnityEngine.Random.Range(1, 4))
                        {
                            case 1:
                                AI.AttackIdea = new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Neutral, ZTrigger: false);
                                break;
                            case 2:
                                AI.AttackIdea = new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Down, ZTrigger: false);
                                break;
                            case 3:
                                AI.AttackIdea = new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Neutral, ZTrigger: true);
                                break;
                        }
                    }
                }
                else
                {
                    if (!(dd.vectorToTarget_Absolute.x <= 4f))
                    {
                        return;
                    }

                    if (AI.GetMood() == AI_Bundle.Enum_Mood.Aggressive)
                    {
                        switch (UnityEngine.Random.Range(0, 2))
                        {
                            case 0:
                                if (MyCharacterControl.ParticipantDataReference.Energy.GetCurrent() > 100f)
                                {
                                    AI.PursueIdea = new AI_Bundle.Internal_PursueIdea(0f);
                                }

                                break;
                            case 1:
                                AI.AttackIdea = new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Neutral, ZTrigger: true);
                                break;
                        }
                    }
                    else if (AI.GetMood() == AI_Bundle.Enum_Mood.Tactical)
                    {
                        switch (UnityEngine.Random.Range(0, 2))
                        {
                            case 0:
                                AI.MovementIdea.Duration = 0f;
                                AI.RethinkCooldown_Movement = 1f;
                                AI.CommandList.Set(new AI_Bundle.AI_Action[2]
                                {
                                new AI_Bundle.AI_Action(new AI_Bundle.Internal_JumpIdea(IsFullJump: false, dd.IsTargetToMyRight ? 1 : (-1))),
                                new AI_Bundle.AI_Action(new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Down, ZTrigger: false), UnityEngine.Random.Range(0f, 0.5f))
                                });
                                break;
                            case 1:
                                AI.AttackIdea = new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Neutral, ZTrigger: true);
                                break;
                        }
                    }

                    AI.RethinkCooldown_Attacking = AI.GetRethinkCooldown(AI_Bundle.Enum_RethinkCooldownType.Attacking);
                }
            }
            else if (dd.IsTargetAboveMe && dd.vectorToTarget_Absolute.x < 0.5f)
            {
                Rigidbody2D rigidbody2D = (Rigidbody2D)typeof(BaseCharacter).GetField("Comp_Rigidbody2D", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dd.Target);
                if (rigidbody2D.velocity.y < 0.25f)
                {
                    AI.AttackIdea = new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Up, dd.vectorToTarget.y < 2f);
                    return;
                }

                switch (UnityEngine.Random.Range(1, 3))
                {
                    case 1:
                        AI.RethinkCooldown_Attacking = AI.GetRethinkCooldown(AI_Bundle.Enum_RethinkCooldownType.Attacking);
                        AI.JumpIdea = new AI_Bundle.Internal_JumpIdea(IsFullJump: false, 0);
                        AI.AttackIdea = new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Up, ZTrigger: false);
                        break;
                    case 2:
                        AI.RethinkCooldown_Attacking = AI.GetRethinkCooldown(AI_Bundle.Enum_RethinkCooldownType.Attacking);
                        AI.JumpIdea = null;
                        AI.AttackIdea = new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Up, ZTrigger: true);
                        break;
                }
            }
            else if (dd.IsTargetBelowMe)
            {
                if (dd.vectorToTarget_Absolute.x < 0.25f && dd.vectorToTarget.y > 0.5f)
                {
                    AI.RethinkCooldown_Attacking = AI.GetRethinkCooldown(AI_Bundle.Enum_RethinkCooldownType.Attacking);
                    AI.AttackIdea = new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Down, ZTrigger: false);
                }
                else if (0.5f < dd.vectorToTarget_Absolute.magnitude && dd.vectorToTarget_Absolute.magnitude < 4f)
                {
                    AI.AttackIdea = new AI_Bundle.Internal_AttackIdea(AI_Bundle.Enum_DirectionalInput.Down, ZTrigger: true);
                }
            }
        }
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

        CharacterControl MyCharacterControl = (CharacterControl)GetField("MyCharacterControl");
        float ARS_MoveSpeedBonus = (float)typeof(CharacterControl).GetField("ARS_MoveSpeedBonus", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(MyCharacterControl);
        sm64.SetBonusSpeed(ARS_MoveSpeedBonus*3);

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
            MovementRushManager.MRDataStore activeMovementRush = SMBZGlobals.MovementRushManager.ActiveMovementRush;
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

    public override void PrepareAnAttack(AttackBundle AttackToPrepare, float MinimumPrepTime = 0)
    {
        base.PrepareAnAttack(AttackToPrepare, MinimumPrepTime);

        MovementRushStateENUM movRush =
                (MovementRushStateENUM)GetMovementRushState();

        if (CurrentAttackData != null && CurrentAttackData.OnAnimationStart != null)
            CurrentAttackData.OnAnimationStart();
    }

    public override void PerformAction_Dodge(Vector2? directionOverride = null)
    {
        InterruptAndNullifyPreparedAttack();
        typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(HitBox_0, null);
        AttackBundle bundle = new AttackBundle
        {
            AnimationNameHash = ASN_MR_Dodge,
            OnAnimationStart = delegate
            {
                SetMovementRushState(MovementRushStateENUM.IsDodging);
                SetPlayerState(PlayerStateENUM.Attacking);
                SetField("IsIntangible", true);
                SetField("DragOverride", GetField("DodgeDrag"));
                SetGravityOverride(0f);
                OnMovementRush_Dodge();
                movRushTimer = 0;
                OnAttackAnimation_QueueCustom(null);
                Comp_InterplayerCollider.Disable();
                sm64.SetAction(ACT_TWIRLING, 2);
            },
            OnInterrupt = delegate
            {
                SetField("IsIntangible", false);
                MovementRush_SetDefaultGravityAndDrag();
                SetMovementRushState(MovementRushStateENUM.Idle);
                Comp_InterplayerCollider.Enable();
            },
            OnAnimationEnd = delegate
            {
                SetField("IsIntangible", false);
                MovementRush_SetDefaultGravityAndDrag();
                SetMovementRushState(MovementRushStateENUM.Idle);
                SetPlayerState(PlayerStateENUM.Idle);
                CurrentAttackData = null;
                Comp_InterplayerCollider.Enable();
                sm64.SetAction(ACT_FREEFALL);
                //c.PlayAnimationIfNotAlready(ASN_MR_Air_Idle);
            },
            OnFixedUpdate = delegate
            {
                movRushTimer += Time.fixedDeltaTime;
                if (movRushTimer > 0.55f)
                {
                    OnAttackAnimation_QueueCustom(null);
                    CurrentAttackData.OnAnimationEnd();
                }
            }
        };
        bundle.SetCustomeQueue(delegate
        {
            if (bundle.CustomQueueCallCount == 0)
            {
                float DodgeSpeed = (float)GetField("DodgeSpeed");
                if (!directionOverride.HasValue)
                {
                    /*
                    bool flag = (IsCPUControlled ? (MyCharacterControl.AI.MovementIdea.VerticalDirection < 0) : (MyCharacterControl.Button_Down.IsBuffered || MyCharacterControl.Button_Down.IsHeld));
                    bool flag2 = (IsCPUControlled ? (0 < MyCharacterControl.AI.MovementIdea.VerticalDirection) : (MyCharacterControl.Button_Up.IsBuffered || MyCharacterControl.Button_Up.IsHeld));
                    bool flag3 = (IsCPUControlled ? (MyCharacterControl.AI.MovementIdea.HorizontalDirection < 0) : (MyCharacterControl.Button_Left.IsBuffered || MyCharacterControl.Button_Left.IsHeld));
                    bool flag4 = (IsCPUControlled ? (0 < MyCharacterControl.AI.MovementIdea.HorizontalDirection) : (MyCharacterControl.Button_Right.IsBuffered || MyCharacterControl.Button_Right.IsHeld));
                    */
                    CharacterControl MyCharacterControl = (CharacterControl)GetField("MyCharacterControl");
                    bool flag = (MyCharacterControl.Button_Down.IsBuffered || MyCharacterControl.Button_Down.IsHeld);
                    bool flag2 = (MyCharacterControl.Button_Up.IsBuffered || MyCharacterControl.Button_Up.IsHeld);
                    bool flag3 = (MyCharacterControl.Button_Left.IsBuffered || MyCharacterControl.Button_Left.IsHeld);
                    bool flag4 = (MyCharacterControl.Button_Right.IsBuffered || MyCharacterControl.Button_Right.IsHeld);
                    if (!flag && !flag3 && !flag4 && !flag2)
                    {
                        SetVelocity((float)FaceDir * DodgeSpeed, 0f);
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

                        SetVelocity(vector.normalized * DodgeSpeed);
                    }
                }
                else
                {
                    SetVelocity(directionOverride.Value * DodgeSpeed);
                }
            }
            else if (bundle.CustomQueueCallCount == 1)
            {
                SetField("IsIntangible", false);
            }
        }, 2);
        PrepareAnAttack(bundle);
    }

    public override void PerformAction_Engage(CharacterControl targetCharacterControl)
    {
        InterruptAndNullifyPreparedAttack();
        SetField("StrikeTarget", targetCharacterControl);
        SetPlayerState(PlayerStateENUM.Attacking);
        SetMovementRushState(MovementRushStateENUM.IsEngaging);
        Comp_InterplayerCollider.Disable();
        AttackBundle attackToPrepare = new AttackBundle
        {
            AnimationNameHash = ASN_MR_Strike_Approach,
            OnInterrupt = delegate
            {
                SetField("StrikeTarget", null);
                ResetRotation();
                SetMovementRushState(MovementRushStateENUM.Idle);
                Comp_InterplayerCollider.Enable();
            },
            OnAnimationStart = delegate
            {
                sm64.SetAction(ACT_SPAWN_SPIN_AIRBORNE, 1);
            }
        };

        CharacterControl StrikeTarget = (CharacterControl)GetField("StrikeTarget");
        float StrikeSpeed = (float)GetField("StrikeSpeed");

        attackToPrepare.CustomDataList.Add(false);
        attackToPrepare.CustomDataList.Add(false);
        attackToPrepare.CustomDataList.Add(false);
        attackToPrepare.CustomDataList.Add(false);
        attackToPrepare.OnFixedUpdate = delegate
        {
            if (!(GetField("StrikeTarget") == null))
            {
                Vector3 vector4 = StrikeTarget.CharacterGO.transform.position - transform.position;
                Vector3 normalized = vector4.normalized;
                _ = normalized * StrikeSpeed;
                int num = 1;
                float num2 = StrikeSpeed * BattleController.instance.ActorFixedDeltaTime;
                if (vector4.magnitude < (float)num + num2)
                {
                    attackToPrepare.CustomDataList[0] = true;
                    attackToPrepare.CustomDataList[1] = normalized;
                    SetVelocity(0f, 0f);
                }
            }
        };
        attackToPrepare.OnUpdate = delegate
        {
            if (StrikeTarget == null)
            {
                PerformAction_Whiff();
            }
            else
            {
                Vector3 vector = (StrikeTarget.CharacterGO.transform.position - transform.position).normalized * StrikeSpeed;
                SetVelocity(vector);
                if ((bool)attackToPrepare.CustomDataList[0])
                {
                    bool targetIntangible = (bool)typeof(BaseCharacter).GetField("IsIntangible", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(StrikeTarget.CharacterGO);
                    MovementRushStateENUM targetMRState = (MovementRushStateENUM)typeof(BaseCharacter).GetField("MovementRushState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(StrikeTarget.CharacterGO);
                    MethodInfo InvokeOnClashMethod = typeof(MovementRushManager.MRDataStore).GetMethod("InvokeOnClash", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(CharacterControl), typeof(CharacterControl) }, null);
                    MethodInfo InvokeOnDodgeMethod = typeof(MovementRushManager.MRDataStore).GetMethod("InvokeOnDodge", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(CharacterControl), typeof(CharacterControl) }, null);

                    Vector2 vector2 = (Vector3)attackToPrepare.CustomDataList[1];
                    Vector2 vector3 = (Vector2)transform.position + vector2 * 0.5f;
                    StrikeTarget.CharacterGO.transform.position = new Vector3(vector3.x, vector3.y, transform.position.z);
                    SetVelocity(vector2 * GetVelocity().magnitude);
                    if (targetIntangible)
                    {
                        StrikeTarget.CharacterGO.OnMovementRush_DodgeSuccess();
                        StrikeTarget.CharacterGO.PerformAction_Dodge(vector2 * -1f);
                        PerformAction_Whiff();
                        //MRManager.ActiveMovementRush?.InvokeOnDodge(StrikeTarget, (CharacterControl)c.GetField("MyCharacterControl"));
                        InvokeOnDodgeMethod.Invoke(SMBZGlobals.MovementRushManager.ActiveMovementRush, new object[] { StrikeTarget, (CharacterControl)GetField("MyCharacterControl") });
                    }
                    else if (targetMRState == MovementRushStateENUM.IsEngaging)
                    {
                        EffectSprite.Create((transform.position + (Vector3)vector3) * 0.5f, EffectSprite.Sprites.HitsparkBlock);
                        SoundCache.ins.PlaySound(SoundCache.ins.Battle_Reflect);
                        SMBZGlobals.CameraManager.SetShake(0.15f);
                        SMBZGlobals.Intensity.IncreaseBy(20f);
                        if (SMBZGlobals.Intensity.GetAmount() >= 100f && !(bool)GetField("IsBursting"))
                        {
                            SMBZGlobals.ClashAndBurstManager.BeginBurst((transform.position + StrikeTarget.transform.position) * 0.5f, (CharacterControl)GetField("MyCharacterControl"), StrikeTarget, 1, BurstDataStore.VictoryStrikeENUM.MovementRushFinisher);
                        }

                        StrikeTarget.CharacterGO.PerformAction_Clash(vector2 * -1f);
                        PerformAction_Clash(vector2);
                        //MRManager.ActiveMovementRush?.InvokeOnClash((CharacterControl)c.GetField("MyCharacterControl"), StrikeTarget);
                        InvokeOnClashMethod.Invoke(SMBZGlobals.MovementRushManager.ActiveMovementRush, new object[] { (CharacterControl)GetField("MyCharacterControl"), StrikeTarget });
                    }
                    else
                    {
                        PerformAction_Strike();
                    }
                }
                else
                {
                    SetField("IsFacingRight", GetVelocity().normalized.x >= 0f);
                    RotateTowardTargetDirection(GetVelocity().normalized);
                    FlipSpriteByFacingDirection();
                    sm64.SetFaceAngle(FaceDir / 2f * -(float)Math.PI);
                }
            }
        };
        PrepareAnAttack(attackToPrepare);
        OnMovementRush_Engage();
    }

    public override void PerformAction_Whiff()
    {
        InterruptAndNullifyPreparedAttack();
        RotateTowardTargetDirection(GetVelocity().normalized);
        typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(HitBox_0, null);
        Comp_InterplayerCollider.Disable();
        PrepareAnAttack(new AttackBundle
        {
            AnimationNameHash = ASN_MR_Strike_Attack,
            OnAnimationStart = delegate
            {
                OnMovementRush_Whiff();
                SetVelocity(GetVelocity().normalized * (float)GetField("StrikeEndSpeed"));
                SetGravityOverride(0f);
                SetField("DragOverride", 1f);
                SetMovementRushState(MovementRushStateENUM.IsWhiffing);
                movRushTimer = 0;
                sm64.SetAction(ACT_CUSTOM_ANIM);
                sm64.SetAnim(MARIO_ANIM_SLIDE_KICK);
                sm64.SetAnimFrame(8);
                sm64.SetFaceAngle(FaceDir / 2f * -(float)Math.PI);
            },
            OnInterrupt = delegate
            {
                ResetRotation();
                MovementRush_SetDefaultGravityAndDrag();
                SetMovementRushState(MovementRushStateENUM.Idle);
                Comp_InterplayerCollider.Enable();
            },
            OnAnimationEnd = delegate
            {
                ResetRotation();
                MovementRush_SetDefaultGravityAndDrag();
                SetMovementRushState(MovementRushStateENUM.Idle);
                SetPlayerState(PlayerStateENUM.Idle);
                PlayAnimationIfNotAlready(ASN_MR_Air_Idle);
                Comp_InterplayerCollider.Enable();
                sm64.SetAction(ACT_FREEFALL);
                CurrentAttackData = null;
            },
            OnFixedUpdate = delegate
            {
                movRushTimer += Time.fixedDeltaTime;
                if (movRushTimer > 0.55f)
                    CurrentAttackData.OnAnimationEnd();
            }
        });
    }

    public override void PerformAction_Clash(Vector2? direction = null)
    {
        float StrikeEndSpeed = (float)GetField("StrikeEndSpeed");

        if (!direction.HasValue)
        {
            direction = GetVelocity().normalized;
        }

        InterruptAndNullifyPreparedAttack();
        SetField("IsFacingRight", direction.Value.x >= 0f);
        RotateTowardTargetDirection(direction.Value);
        FlipSpriteByFacingDirection();
        typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(HitBox_0, null);
        Comp_InterplayerCollider.Disable();
        PrepareAnAttack(new AttackBundle
        {
            AnimationNameHash = ASN_MR_Strike_Attack,
            OnAnimationStart = delegate
            {
                SetVelocity(direction.Value * (0f - StrikeEndSpeed));
                SetGravityOverride(0f);
                SetField("DragOverride", 1f);
                SetMovementRushState(MovementRushStateENUM.IsWhiffing);
                movRushTimer = 0;
                sm64.SetAction(ACT_CUSTOM_ANIM);
                sm64.SetAnim(MARIO_ANIM_SLIDE_KICK);
                sm64.SetAnimFrame(8);
                sm64.SetFaceAngle(FaceDir / 2f * -(float)Math.PI);
            },
            OnInterrupt = delegate
            {
                ResetRotation();
                MovementRush_SetDefaultGravityAndDrag();
                SetMovementRushState(MovementRushStateENUM.Idle);
                Comp_InterplayerCollider.Enable();
            },
            OnAnimationEnd = delegate
            {
                ResetRotation();
                MovementRush_SetDefaultGravityAndDrag();
                SetMovementRushState(MovementRushStateENUM.Idle);
                SetPlayerState(PlayerStateENUM.Idle);
                PlayAnimationIfNotAlready(ASN_MR_Air_Idle);
                Comp_InterplayerCollider.Enable();
                CurrentAttackData = null;
                sm64.SetAction(ACT_FREEFALL);
            },
            OnFixedUpdate = delegate
            {
                movRushTimer += Time.fixedDeltaTime;
                if (movRushTimer > 0.55f)
                    CurrentAttackData.OnAnimationEnd();
            }
        });
        OnMovementRush_Clash();
    }

    public override void PerformAction_Strike()
    {
        FieldInfo IsFacingRightField = GetType().GetField("IsFacingRight", BindingFlags.NonPublic | BindingFlags.Instance);
        float StrikeLaunch = (float)GetField("StrikeLaunch");

        InterruptAndNullifyPreparedAttack();
        IsFacingRightField.SetValue(this, GetVelocity().normalized.x >= 0f);
        RotateTowardTargetDirection(GetVelocity().normalized);
        FlipSpriteByFacingDirection();
        Comp_InterplayerCollider.Disable();
        typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(HitBox_0,
            new HitBoxDamageParameters
            {
                Owner = this,
                Tag = tag,
                Damage = 5f,
                HitStun = 0.5f,
                IsUnblockable = true,
                CanBackAttack = true,
                IsDamageFatal = false,
                FreezeTime = 0.07f,
                Launch = GetVelocity().normalized * StrikeLaunch,
                HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkBlunt),
                OnHitSoundEffect = SoundCache.ins.Battle_Hit_2A,
                CanInitiateBurst = false,
                OnHitCallback = delegate (BaseCharacter target, bool wasBlocked)
                {
                    MovementRushManager.MRDataStore.ParticipantStatus playerStatus = SMBZGlobals.MovementRushManager.FetchActiveRushPlayerData(target);
                    if (playerStatus != null)
                    {
                        FieldInfo StrikeAmmo = typeof(BaseCharacter).GetField("MR_StrikeAmmo", BindingFlags.NonPublic | BindingFlags.Instance);
                        FieldInfo StrikeAmmo_MAX = typeof(BaseCharacter).GetField("MR_StrikeAmmo_MAX", BindingFlags.NonPublic | BindingFlags.Instance);
                        FieldInfo DodgeAmmo = typeof(BaseCharacter).GetField("MR_DodgeAmmo", BindingFlags.NonPublic | BindingFlags.Instance);
                        FieldInfo DodgeAmmo_MAX = typeof(BaseCharacter).GetField("MR_DodgeAmmo_MAX", BindingFlags.NonPublic | BindingFlags.Instance);

                        StrikeAmmo.SetValue(this, StrikeAmmo_MAX.GetValue(this));
                        DodgeAmmo.SetValue(this, DodgeAmmo_MAX.GetValue(this));
                        StrikeAmmo.SetValue(target, StrikeAmmo_MAX.GetValue(target));
                        DodgeAmmo.SetValue(target, DodgeAmmo_MAX.GetValue(target));
                        playerStatus.Health--;
                        if (playerStatus.Health <= 0)
                        {
                            CharacterControl target_MyCharacterControl = (CharacterControl)target.GetType().GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);

                            // WHY is this not public???
                            MethodInfo IsThereOnlyOneTeamRemaining = SMBZGlobals.MovementRushManager.GetType().GetMethod("IsThereOnlyOneTeamRemaining", BindingFlags.NonPublic | BindingFlags.Instance);
                            if ((bool)IsThereOnlyOneTeamRemaining.Invoke(SMBZGlobals.MovementRushManager, null))
                            {
                                CurrentAttackData = null;
                                PerformAction_Finale(target_MyCharacterControl);
                            }
                            else
                            {
                                target.GetType().GetField("IsIntangible", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(target, true);
                                target_MyCharacterControl.InputLockTimer = 4f;
                                target.GetType().GetProperty("HitStun", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(target, 4f);
                                target.Comp_Hurtbox.enabled = false;
                                target.SetVelocity(target.GetVelocity() * 5f);
                                SMBZGlobals.CameraManager.MainTargetGroup.RemoveMember(target_MyCharacterControl.transform);
                                StartCoroutine(target_MyCharacterControl.gameObject.Delayed_Invokation(delegate
                                {
                                    if (target != null)
                                    {
                                        target.Comp_Hurtbox.enabled = true;
                                        target_MyCharacterControl.UnloadCharacterObject();
                                    }
                                }, 1f));
                            }
                        }
                    }
                }
            }
        );
        PrepareAnAttack(new AttackBundle
        {
            AnimationNameHash = ASN_MR_Strike_Attack,
            OnAnimationStart = delegate
            {
                SetVelocity(GetVelocity().normalized * (float)GetField("StrikeEndSpeed"));
                SetGravityOverride(0f);
                SetField("DragOverride", 1f);
                SetMovementRushState(MovementRushStateENUM.IsStriking);
                OnMovementRush_Strike();
                HitBox_0.transform.localPosition = new Vector2(0f, -0.2f);
                HitBox_0.transform.localScale = new Vector2(1.25f, 1f);
                HitBox_0.IsActive = true;
                movRushTimer = 0;
                sm64.SetAction(ACT_CUSTOM_ANIM);
                sm64.SetAnim(MARIO_ANIM_SLIDE_KICK);
                sm64.SetAnimFrame(8);
                sm64.SetFaceAngle(FaceDir / 2f * -(float)Math.PI);
            },
            OnClashCallback = delegate
            {
                PerformAction_Clash();
            },
            OnInterrupt = delegate
            {
                MovementRush_SetDefaultGravityAndDrag();
                ResetRotation();
                SetMovementRushState(MovementRushStateENUM.Idle);
                Comp_InterplayerCollider.Enable();
            },
            OnAnimationEnd = delegate
            {
                MovementRush_SetDefaultGravityAndDrag();
                ResetRotation();
                SetMovementRushState(MovementRushStateENUM.Idle);
                SetPlayerState(PlayerStateENUM.Idle);
                PlayAnimationIfNotAlready(ASN_MR_Air_Idle);
                Comp_InterplayerCollider.Enable();
                CurrentAttackData = null;
                sm64.SetAction(ACT_FREEFALL);
            },
            OnFixedUpdate = delegate
            {
                movRushTimer += Time.fixedDeltaTime;
                if (movRushTimer > 0.55f)
                    CurrentAttackData.OnAnimationEnd();
            }
        });
    }

    public override void PerformAction_Finale(CharacterControl target)
    {
        float GroundPositionY = (float)SMBZGlobals.BackgroundManager.GetType().GetField("GroundPositionY", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(SMBZGlobals.BackgroundManager);
        MethodInfo AirRushCollider_SetActive = SMBZGlobals.BackgroundManager.GetType().GetMethod("AirRushCollider_SetActive", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(bool) }, null);

        Vector2 velocity = new Vector2(5 * FaceDir, Mathf.Lerp(15f, 5f, (transform.position.y - GroundPositionY) / 15f));
        SetVelocity(velocity + new Vector2(0, 1.5f));
        SetGravityOverride(0.1f);
        SetField("DragOverride", 1f);
        target.CharacterGO.transform.position = transform.position + new Vector3(FaceDir, 0f);
        target.CharacterGO.SetHitsunWithAnimation(3f, transitionDirectlyIntoHurtAnimation: false);
        target.CharacterGO.SetGravityOverride(0.1f);
        typeof(BaseCharacter).GetField("DragOverride", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(target.CharacterGO, 1f);
        target.CharacterGO.SetVelocity(velocity);
        SoundCache.ins.PlaySound(SoundCache.ins.Battle_Hit_2A);
        EffectSprite.Create(target.transform.position, EffectSprite.Sprites.HitsparkBlunt, SMBZGlobals.MovementRushManager.ActiveMovementRush.IsDirectionOfRushRight);
        SetField("IgnoreClashTimer", 5f);
        ResetRotation();
        SetField("IsIntangible", true);
        SetPlayerState(PlayerStateENUM.Cinematic_NoInput);
        SetMovementRushState(MovementRushStateENUM.IsFinale);
        AirRushCollider_SetActive.Invoke(SMBZGlobals.BackgroundManager, new object[] { false });
        PrepareAnAttack(new AttackBundle
        {
            AnimationName = "Finale",
            OnAnimationStart = delegate
            {
                SetMovementRushState(MovementRushStateENUM.IsFinale);
                typeof(HitBox).GetField("DamageProperties", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(HitBox_0,
                    new HitBoxDamageParameters
                    {
                        Owner = this,
                        Tag = tag,
                        Damage = 15f,
                        HitStun = 0.75f,
                        IsUnblockable = true,
                        FreezeTime = 0.15f,
                        Launch = new Vector2(10 * FaceDir, -20f),
                        HitSpark = new EffectSprite.Parameters(EffectSprite.Sprites.HitsparkHeavy),
                        OnHitSoundEffect = SoundCache.ins.Battle_Hit_3B,
                        CanInitiateBurst = false,
                        OnHitCallback = delegate (BaseCharacter target, bool wasBlocked)
                        {
                            CharacterControl MyCharacterControl = (CharacterControl)GetField("MyCharacterControl");
                            CharacterControl target_MyCharacterControl = (CharacterControl)typeof(BaseCharacter).GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);
                            SMBZGlobals.MovementRushManager.EndMovementRush(MyCharacterControl, target_MyCharacterControl);
                        }
                    }
                );
                HitBox_0.transform.localPosition = new Vector2(0.5f, -0.6f);
                HitBox_0.transform.localScale = new Vector2(1.25f, 1f);
                HitBox_0.IsActive = false;
                movRushTimer = 0;
                sm64.SetAction(ACT_SPAWN_SPIN_AIRBORNE, 1);
            },
            OnInterrupt = delegate
            {
                SetField("IgnoreClashTimer", 0f);
                MovementRush_SetDefaultGravityAndDrag();
                SetField("IsIntangible", false);
                SetMovementRushState(MovementRushStateENUM.Idle);
            },
            OnAnimationEnd = delegate
            {
                SetField("IgnoreClashTimer", 0f);
                MovementRush_SetDefaultGravityAndDrag();
                SetField("IsIntangible", false);
                SetMovementRushState(MovementRushStateENUM.Idle);
                SetPlayerState(PlayerStateENUM.Idle);
                //PlayAnimationIfNotAlready(c.ASN_Idle);
            },
            OnFixedUpdate = delegate
            {
                movRushTimer += Time.fixedDeltaTime;
                if (movRushTimer > 0.9f && sm64.marioState.action != (uint)ACT_JUMP_KICK)
                {
                    sm64.SetAction(ACT_JUMP_KICK, 1);
                    sm64.SetAngle((float)Math.PI / 6, sm64.marioState.faceAngle, 0);
                }
                if (movRushTimer > 1f)
                    HitBox_0.IsActive = true;
            }
        });
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

            if (HasContactGroundThisFrame && PursueData.StartupCountdown <= 0f)
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
                SoundCache.ins.PlaySound((PursueData.ChargePower >= 100f) ? SoundCache.ins.Battle_Zoom : SoundCache.ins.Battle_Leap_DBZ);
                PlaySoundForVoice(SMBZ_64.Core.Mario64cc.sounds["hahaa"]);
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
            if (HasContactGroundThisFrame && Comp_Rigidbody2D.velocity.y < -1f && Mathf.Abs(Comp_Rigidbody2D.velocity.x) < 3f)
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

    public override void OnBurstEnd()
    {
        base.OnBurstEnd();

        if (Comp_Rigidbody2D.gravityScale == 1)
        {
            // burst tie
            sm64.SetAction(ACT_FREEFALL);
        }
        else
        {
            // burst lose/win
            sm64.SetAction(ACT_CUSTOM_ANIM);
            sm64.SetAnim(MARIO_ANIM_GENERAL_FALL);
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
                    bool IsFacingRight = (bool)GetField("IsFacingRight");
                    CharacterControl MyCharacterControl = (CharacterControl)GetField("MyCharacterControl");
                    CharacterControl targetControl = (CharacterControl)GetType().GetField("MyCharacterControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(t);

                    if (victoryStrikeType == BurstDataStore.VictoryStrikeENUM.MovementRushStarter && SaveData.Data.MovementRush_IsEnabled_ViaCriticalStrikes && t != null && !t_IsNPC && t.IsHurt)
                    {
                        SMBZGlobals.MovementRushManager.StartNewMovementRush(IsFacingRight, new List<CharacterControl> { MyCharacterControl }, new List<CharacterControl> { targetControl });
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

    protected override void Perform_Grounded_UpAttack()
    {
        if (sm64 == null || sm64.marioState.action == (uint)ACT_UP_ATTACK) return;

        sm64.SetAction(ACT_UP_ATTACK);
        PrepareAnAttack(AttBun_UpperPunch);
    }

    protected override void Perform_Grounded_DownAttack()
    {
        if (sm64 == null) return;

        CharacterControl MyCharacterControl = (CharacterControl)GetField("MyCharacterControl");
        if (MyCharacterControl.Button_Left.IsHeld || MyCharacterControl.Button_Right.IsHeld)
        {
            sm64.SetAction(ACT_SLIDE_KICK);
            PrepareAnAttack(AttBun_SlideKick);
            return;
        }

        sm64.SetAction(ACT_PUNCHING, 9);
    }

    protected override void Perform_Grounded_NeutralSpecial()
    {
        if (sm64 == null) return;

        sm64.SetAction(ACT_HEAVY_ATTACK);
        PrepareAnAttack(AttBun_GroundTwirl);
    }

    protected override void Perform_Grounded_UpSpecial()
    {
        if (sm64 == null) return;

        sm64.SetAction(ACT_HEAVY_UP_ATTACK);
        PrepareAnAttack(AttBun_SuperUppercut);
    }

    protected override void Perform_Grounded_DownSpecial()
    {
        CharacterControl MyCharacterControl = (CharacterControl)GetField("MyCharacterControl");
        if (SMBZGlobals.Intensity.IsCriticalHitReady(MyCharacterControl.ParticipantDataReference.ParticipantIndex))
        {
            SMBZGlobals.Intensity.UseCriticalStrike(MyCharacterControl.ParticipantDataReference.ParticipantIndex);
            PrepareAnAttack(AttBun_CriticalPunch);
        }
        else
        {
            PrepareAnAttack(AttBun_StarDancePunch);
        }
    }

    protected override void Perform_Aerial_UpAttack()
    {
        if (sm64 == null || sm64.marioState.action == (uint)ACT_UP_ATTACK) return;

        sm64.SetAction(ACT_UP_ATTACK, 1);
        PrepareAnAttack(AttBun_UpperPunchAir);
    }

    protected override void Perform_Aerial_DownAttack()
    {
        if (sm64 == null || sm64.marioState.action == (uint)ACT_AERIAL_DOWN_ATTACK) return;

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


    protected override void Perform_Aerial_UpSpecial()
    {
        if (sm64 == null) return;

        sm64.SetAction(ACT_HEAVY_UP_ATTACK);
        PrepareAnAttack(AttBun_SuperUppercut);
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
