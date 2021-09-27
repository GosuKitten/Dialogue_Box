using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundSystem;

[RequireComponent(typeof(CollisionSystem))]
public class PlayerSystem : MonoBehaviour
{
    CollisionSystem colSys;
    Animation anim;
    SoundSystem soundSys;
    SpriteRenderer sprRender;

    public AnimationClip jumpSquishAnim;
    public AnimationClip ceilingSquishAnim;

    public GameObject jumpParticles;
    public GameObject landParticles;

    enum ParticleType { Jump, Land, CeilingHit }

    public delegate void Jump();
    public static event Jump OnJump;

    public delegate void Land();
    public static event Land OnLand;


    public delegate void CeilingHit();
    public static event CeilingHit OnCeilingHit;

    [Header("Ground Movement")]
    public float maxSpeed;
    public float groundInitialSpeed;
    public float groundAcceleration;
    public float groundDeceleration;

    [Header("Air Movement")]
    public float airAcceleration;
    public float airDeceleration;
    public float gravity;
    public float terminalVelocity;

    [Space(10)]
    [Header("Jumping")]
    public float jumpForce;
    public float jumpForceCutoff;
    public float beforeJumpPadding;
    public float ungroundedJumpPadding;

    // internal processing variables
    bool pauseMovement = false;
    bool grounded = false;
    bool xPosAdjusted = false;
    bool yPosAdjusted = false;
    float beforeJumpPaddingTimer;
    float ungroundedJumpPaddingTimer;
    Vector3 velocity;

    private void Start()
    {
        colSys = GetComponent<CollisionSystem>();
        anim = GetComponentInChildren<Animation>();
        soundSys = FindObjectOfType<SoundSystem>();
        sprRender = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        xPosAdjusted = false;
        yPosAdjusted = false;
        ProcessGroundMovement();
        ProcessAirMovement();
        ProcessJumping();

        colSys.CalculateCollision(velocity);
        ProcessCollision();

        ProcessVelocity();
    }

    void ProcessGroundMovement()
    {
        if (Input.GetKey(InputBindings.left))
        {
            if (velocity.x >= 0) velocity.x = -groundInitialSpeed;
            velocity.x = Mathf.Max(velocity.x - (groundAcceleration * Time.deltaTime), -maxSpeed);
            sprRender.flipX = true;
        }
        else if (Input.GetKey(InputBindings.right))
        {
            if (velocity.x <= 0) velocity.x = groundInitialSpeed;
            velocity.x = Mathf.Min(velocity.x + (groundAcceleration * Time.deltaTime), maxSpeed);
            sprRender.flipX = false;
        }
        else
        {
            if (velocity.x < 0) velocity.x = Mathf.Min(velocity.x + (groundDeceleration * Time.deltaTime), 0);
            if (velocity.x > 0) velocity.x = Mathf.Max(velocity.x - (groundDeceleration * Time.deltaTime), 0);
        }
    }
    
    void ProcessAirMovement()
    {
        if (!grounded)
        {
            velocity.y = Mathf.Max(velocity.y - (gravity * Time.deltaTime), -terminalVelocity);
        }
    }

    void ProcessJumping()
    {
        ungroundedJumpPaddingTimer += Time.deltaTime;
        beforeJumpPaddingTimer += Time.deltaTime;

        bool jump = false;
        if (Input.GetKeyDown(InputBindings.space))
        {
            if (grounded)
            {
                jump = true;
            }
            else
            {
                if (ungroundedJumpPaddingTimer <= ungroundedJumpPadding)
                {
                    jump = true;
                }
                else
                {
                    beforeJumpPaddingTimer = 0;
                }
            }
        }

        if (beforeJumpPaddingTimer <= beforeJumpPadding)
        {
            if (grounded) jump = true;
        }

        if (jump)
        {
            velocity.y = jumpForce;
            grounded = false;

            anim.clip = jumpSquishAnim;
            anim.Play();

            soundSys.PlaySound(SoundClip.Jump);
            SpawnParticles(ParticleType.Jump);

            OnJump?.Invoke();
        }

        if (Input.GetKeyUp(InputBindings.space))
        {
            if (velocity.y > 0)
            {
                velocity.y /= 3;
            }
        }
    }

    void ProcessCollision()
    {
        // up collision
        if (colSys.upCol)
        {
            transform.position = new Vector3(transform.position.x, colSys.upColPoint.y - colSys.upOffset.y);
            velocity.y = 0;

            anim.clip = ceilingSquishAnim;
            anim.Play();
            soundSys.PlaySound(SoundClip.Hit);
            SpawnParticles(ParticleType.CeilingHit);
            OnCeilingHit?.Invoke();
        }

        // down collision
        if (colSys.downCol)
        {
            if (!grounded)
            {
                transform.position = new Vector3(transform.position.x, colSys.downColPoint.y - colSys.downOffset.y);
                velocity.y = 0;
                grounded = true;

                anim.clip = jumpSquishAnim;
                anim.Play();


                soundSys.PlaySound(SoundClip.Hit);
                SpawnParticles(ParticleType.Land);
                OnLand?.Invoke();
            }
        }
        else
        {
            if (grounded)
            {
                grounded = false;
                ungroundedJumpPaddingTimer = 0;
            }
        }

        //left collision
        if (colSys.leftCol)
        {
            transform.position = new Vector3(colSys.leftColPoint.x - colSys.leftOffset.x, transform.position.y);
            velocity.x = 0;
        }

        //right collision
        if (colSys.rightCol)
        {
            transform.position = new Vector3(colSys.rightColPoint.x - colSys.rightOffset.x, transform.position.y);
            velocity.x = 0;
        }

        if (colSys.hitEnemy)
        {
            Destroy(this.gameObject);
        }
    }

    void ProcessVelocity()
    {
        transform.position += velocity * Time.deltaTime;
    }

    void SpawnParticles(ParticleType particleType)
    {
        GameObject particles;
        Vector3 offset;
        Color colorAdjust;
        Quaternion rotation;

        switch (particleType)
        {
            case ParticleType.Jump:
                particles = jumpParticles;
                offset = colSys.downOffset;
                colorAdjust = colSys.lastGroundColor;
                rotation = Quaternion.identity;
                break;
            case ParticleType.Land:
                particles = landParticles;
                offset = colSys.downOffset;
                colorAdjust = colSys.lastGroundColor;
                rotation = Quaternion.identity;
                break;
            case ParticleType.CeilingHit:
                particles = landParticles;
                offset = colSys.upOffset;
                colorAdjust = colSys.lastCeilingColor;
                rotation = Quaternion.Euler(0, 0, 180);
                break;
            default:
                return;
        }


        var part = Instantiate(particles, transform.position + offset, rotation).GetComponent<ParticleSystem>();

        var main = part.main;
        var col = part.colorOverLifetime;

        Gradient grad = new Gradient();
        grad.colorKeys = new GradientColorKey[] { new GradientColorKey(colorAdjust, 0), new GradientColorKey(colorAdjust, 1) };
        grad.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 0.75f), new GradientAlphaKey(0, 1) };

        main.startColor = colorAdjust;
        col.color = grad;
    }
}
