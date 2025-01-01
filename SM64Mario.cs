using UnityEngine;
using MelonLoader;

namespace LibSM64
{
    public class SM64Mario : MonoBehaviour
    {
        Material material = null;

        SM64InputProvider inputProvider;

        Vector3[][] positionBuffers;
        Vector3[][] normalBuffers;
        Vector3[] lerpPositionBuffer;
        Vector3[] lerpNormalBuffer;
        Vector3[] colorBuffer;
        Color[] colorBufferColors;
        Vector2[] uvBuffer;
        int buffIndex;
        Interop.SM64MarioState[] states;

        GameObject marioRendererObject;
        MeshRenderer renderer;
        Mesh marioMesh;
        int marioId;

        float tick;
        public CharacterControl smbzChar;
        public Action<SM64Mario> changeActionCallback = null;
        public Action<SM64Mario> advanceAnimFrameCallback = null;

        public bool spawned { get { return marioId != -1; } }

        public Interop.SM64MarioState marioState { get { return states[buffIndex]; } }

        void OnEnable()
        {
            var initPos = transform.position;
            marioId = Interop.MarioCreate( new Vector3( -initPos.x, initPos.y, initPos.z ) * Interop.SCALE_FACTOR );

            if (marioId == -1)
            {
                Melon<SMBZ_64.Core>.Logger.Msg($"Failed to spawn Mario at {-initPos.x},{initPos.y},{initPos.z}");
                throw new System.Exception($"Failed to spawn Mario at {-initPos.x},{initPos.y},{initPos.z}");
            }
            Melon<SMBZ_64.Core>.Logger.Msg($"Spawned Mario {marioId} at {-initPos.x},{initPos.y},{initPos.z}");

            inputProvider = GetComponent<SM64InputProvider>();
            if (inputProvider == null)
                throw new System.Exception("Need to add an input provider component to Mario");

            marioRendererObject = new GameObject("MARIO");
            marioRendererObject.hideFlags |= HideFlags.HideInHierarchy;

            renderer = marioRendererObject.AddComponent<MeshRenderer>();
            var meshFilter = marioRendererObject.AddComponent<MeshFilter>();

            states = new Interop.SM64MarioState[2] {
                new Interop.SM64MarioState(),
                new Interop.SM64MarioState()
            };

            marioRendererObject.transform.localScale = new Vector3( -1, 1, 1 ) / Interop.SCALE_FACTOR;
            marioRendererObject.transform.localPosition = Vector3.zero;

            lerpPositionBuffer = new Vector3[3 * Interop.SM64_GEO_MAX_TRIANGLES];
            lerpNormalBuffer = new Vector3[3 * Interop.SM64_GEO_MAX_TRIANGLES];
            positionBuffers = new Vector3[][] { new Vector3[3 * Interop.SM64_GEO_MAX_TRIANGLES], new Vector3[3 * Interop.SM64_GEO_MAX_TRIANGLES] };
            normalBuffers = new Vector3[][] { new Vector3[3 * Interop.SM64_GEO_MAX_TRIANGLES], new Vector3[3 * Interop.SM64_GEO_MAX_TRIANGLES] };
            colorBuffer = new Vector3[3 * Interop.SM64_GEO_MAX_TRIANGLES];
            colorBufferColors = new Color[3 * Interop.SM64_GEO_MAX_TRIANGLES];
            uvBuffer = new Vector2[3 * Interop.SM64_GEO_MAX_TRIANGLES];

            marioMesh = new Mesh();
            marioMesh.vertices = lerpPositionBuffer;
            marioMesh.triangles = Enumerable.Range(0, 3*Interop.SM64_GEO_MAX_TRIANGLES).ToArray();
            meshFilter.sharedMesh = marioMesh;

            tick = 0f;
        }

        void OnDisable()
        {
            Melon<SMBZ_64.Core>.Logger.Msg($"Disabled Mario {marioId}");

            if ( marioRendererObject != null )
            {
                Destroy( marioRendererObject );
                marioRendererObject = null;
            }

            if( Interop.isGlobalInit )
            {
                Interop.MarioDelete( marioId );
            }
        }

        public void SetMaterial(Material m)
        {
            material = m;
            renderer.sharedMaterial = m;
            renderer.sharedMaterial.mainTexture = Interop.marioTexture;
        }

        public void SetPosition(Vector3 position, Vector3 offset)
        {
            Interop.MarioSetPosition(marioId, position+offset);
            transform.position = position + offset;
            marioRendererObject.transform.position = position+offset;
        }

        public void SetVelocity(Vector3 vel)
        {
            Interop.MarioSetVelocity(marioId, vel);
        }

        public void SetForwardVelocity(float vel)
        {
            Interop.MarioSetForwardVelocity(marioId, vel);
        }

        public void TakeDamage(uint damage, uint subtype, Vector3 pos)
        {
            Interop.MarioTakeDamage(marioId, damage, subtype, pos);
        }

        public void SetHealth(ushort health)
        {
            Interop.MarioSetHealth(marioId, health);
        }

        public void Kill()
        {
            Interop.MarioKill(marioId);
        }

        public void SetFaceAngle(float angle)
        {
            Interop.MarioSetFaceAngle(marioId, angle);
        }

        public void SetAction(uint action)
        {
            Interop.MarioSetAction(marioId, action);
        }

        public void SetAction(uint action, uint actionArg)
        {
            Interop.MarioSetAction(marioId, action, actionArg);
        }

        public void SetActionTimer(uint actionTimer)
        {
            Interop.MarioSetActionTimer(marioId, actionTimer);
        }

        public void SetAnim(SM64Constants.MarioAnimID animID)
        {
            Interop.MarioSetAnim(marioId, animID);
        }

        public void SetAnimFrame(short frame)
        {
            Interop.MarioSetAnimFrame(marioId, frame);
        }

        public void contextFixedUpdate()
        {
            uint oldAction = marioState.action;
            uint oldActionArg = marioState.actionArg;
            short oldFrame = marioState.animFrame;

            tick += Time.fixedDeltaTime;
            while (tick >= 1 / 30f)
            {
                var inputs = new Interop.SM64MarioInputs();
                var look = inputProvider.GetCameraLookDirection();
                look.y = 0;
                look = look.normalized;

                var joystick = inputProvider.GetJoystickAxes();

                inputs.camLookX = -look.x;
                inputs.camLookZ = look.z;
                inputs.stickX = joystick.x;
                inputs.stickY = -joystick.y;
                inputs.buttonA = inputProvider.GetButtonHeld(SM64InputProvider.Button.Jump) ? (byte)1 : (byte)0;
                inputs.buttonB = inputProvider.GetButtonHeld(SM64InputProvider.Button.Kick) ? (byte)1 : (byte)0;
                inputs.buttonZ = inputProvider.GetButtonHeld(SM64InputProvider.Button.Stomp) ? (byte)1 : (byte)0;

                Interop.MarioTick(marioId, inputs, ref states[buffIndex], positionBuffers[buffIndex], normalBuffers[buffIndex], colorBuffer, uvBuffer);

                for (int i = 0; i < 3*Interop.SM64_GEO_MAX_TRIANGLES; ++i)
                {
                    positionBuffers[buffIndex][i] -= new Vector3(states[buffIndex].position[0], states[buffIndex].position[1], states[buffIndex].position[2]);
                    colorBufferColors[i] = new Color(colorBuffer[i].x, colorBuffer[i].y, colorBuffer[i].z, 1);
                    if (uvBuffer[i].x == 1f && uvBuffer[i].y == 1f)
                    {
                        Color32 col32 = new Color32((byte)(colorBuffer[i].x * 255), (byte)(colorBuffer[i].y * 255), (byte)(colorBuffer[i].z * 255), 1);
                        for (int j = 0; j < Interop.defaultColors.Length; j++)
                        {
                            if (Interop.defaultColors[j].r == col32.r && Interop.defaultColors[j].g == col32.g && Interop.defaultColors[j].b == col32.b)
                            {
                                uvBuffer[i].x = (1024 - 16) / 1024f;
                                uvBuffer[i].y = (j * 10 + 5) / 64f;
                                break;
                            }
                        }
                    }
                }

                marioMesh.colors = colorBufferColors;
                marioMesh.uv = uvBuffer;

                buffIndex = 1 - buffIndex;

                tick -= 1/30f;
            }

            if (changeActionCallback != null && (oldAction != marioState.action || oldActionArg != marioState.actionArg))
            {
                changeActionCallback(this);
            }
            if (advanceAnimFrameCallback != null && oldFrame != marioState.animFrame)
            {
                advanceAnimFrameCallback(this);
            }
        }

        public void contextUpdate()
        {
            //float t = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
            float t = tick / (1 / 30f);
            int j = 1 - buffIndex;

            for( int i = 0; i < lerpPositionBuffer.Length; ++i )
            {
                lerpPositionBuffer[i] = Vector3.LerpUnclamped( positionBuffers[buffIndex][i], positionBuffers[j][i], t );
                lerpNormalBuffer[i] = Vector3.LerpUnclamped( normalBuffers[buffIndex][i], normalBuffers[j][i], t );
            }

            transform.position = Vector3.LerpUnclamped( states[buffIndex].unityPosition, states[j].unityPosition, t );
            marioRendererObject.transform.position = transform.position;

            marioMesh.vertices = lerpPositionBuffer;
            marioMesh.normals = lerpNormalBuffer;

            marioMesh.RecalculateBounds();
            marioMesh.RecalculateTangents();
        }
    }
}