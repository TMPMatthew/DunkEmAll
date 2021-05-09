using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;

namespace RootMotion.Demos
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class Rival : MonoBehaviour
    {
        private PhysicMaterial zeroFrictionMaterial;
        private PhysicMaterial highFrictionMaterial;
        protected Rigidbody r;
        protected const float half = 0.5f;
        protected float originalHeight;
        protected Vector3 originalCenter;
        protected CapsuleCollider capsule;





        // Animation state
        public struct AnimState
        {
            public Vector3 moveDirection; // the forward speed
            public bool onGround; // is the character grounded
            public float yVelocity; // y velocity of the character
        }

        Vector3 moveDirection;
        private Animator animator;
        private RaycastHit hit;
        private Vector3 moveDirectionVelocity;
        private Vector3 fixedDeltaPosition;
        private Quaternion fixedDeltaRotation = Quaternion.identity;
        private bool fixedFrame;
        private Vector3 gravity;
        private Vector3 verticalVelocity;

        [Header("Movement")]
        public bool smoothPhysics = true; // If true, will use interpolation to smooth out the fixed time step.
        public float smoothAccelerationTime = 0.2f; // The smooth acceleration of the speed of the character (using Vector3.SmoothDamp)
        public float linearAccelerationSpeed = 3f; // The linear acceleration of the speed of the character (using Vector3.MoveTowards)
        public float platformFriction = 7f;                 // the acceleration of adapting the velocities of moving platforms
        public float groundStickyEffect = 4f;               // power of 'stick to ground' effect - prevents bumping down slopes.
        public float maxVerticalVelocityOnGround = 3f;      // the maximum y velocity while the character is grounded
        public float velocityToGroundTangentWeight = 0f;    // the weight of rotating character velocity vector to the ground tangent

        [Header("Rotation")]
        public float turnSpeed = 5f;                    // additional turn speed added when the player is moving (added to animation root rotation)
        public float stationaryTurnSpeedMlp = 1f;           // additional turn speed added when the player is stationary (added to animation root rotation)



        public Transform moveTarget;
        public float stoppingDistance = 0.5f;
        public float stoppingThreshold = 1.5f;
        private float moveSpeed = 0.5f;
        // Input state
        public struct State
        {
            public Vector3 move;
            public Vector3 lookPos;
        }

        public State state = new State();           // The current state of the user input

        Transform cam;


        [Header("Puppet")]

        public PropRoot propRoot;
        private Vector3 normal;
        private float forwardMlp;

        public BehaviourPuppet puppet { get; private set; }


        void Start()
        {
            capsule = GetComponent<Collider>() as CapsuleCollider;
            r = GetComponent<Rigidbody>();

            // Store the collider volume
            originalHeight = capsule.height;
            originalCenter = capsule.center;

            // Physics materials
            zeroFrictionMaterial = new PhysicMaterial();
            zeroFrictionMaterial.dynamicFriction = 0f;
            zeroFrictionMaterial.staticFriction = 0f;
            zeroFrictionMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
            zeroFrictionMaterial.bounciness = 0f;
            zeroFrictionMaterial.bounceCombine = PhysicMaterialCombine.Minimum;

            highFrictionMaterial = new PhysicMaterial();

            // Making sure rigidbody rotation is fixed
            r.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            animator = GetComponent<Animator>();
            if (animator == null) animator = transform.GetChild(0).GetComponent<Animator>();

            cam = Camera.main.transform;
            puppet = transform.parent.GetComponentInChildren<BehaviourPuppet>();
            CameraFollow.instance.rivalsPos.Add(this.transform);
        }

        void OnAnimatorMove()
        {
            Move(animator.deltaPosition, animator.deltaRotation);
        }


        // Update is called once per frame
        void Update()
        {
            // read inputs
            Vector3 direction = moveTarget.position - transform.position;
            float distance = direction.magnitude;

            Vector3 normal = transform.up;
            Vector3.OrthoNormalize(ref normal, ref direction);

            float sD = state.move != Vector3.zero ? stoppingDistance : stoppingDistance * stoppingThreshold;

            state.move = distance > sD ? direction * moveSpeed : Vector3.zero;
            state.lookPos = moveTarget.position;


            //animState.moveDirection = GetMoveDirection();
        }

        private void FixedUpdate()
        {
            // Move
            MoveFixed(fixedDeltaPosition);
            fixedDeltaPosition = Vector3.zero;

            r.MoveRotation(transform.rotation * fixedDeltaRotation);
            fixedDeltaRotation = Quaternion.identity;

            Rotate();

            // Friction
            if (state.move == Vector3.zero) HighFriction();
            else ZeroFriction();

            bool stopSlide = state.move == Vector3.zero && r.velocity.magnitude < 0.5f;

            if (stopSlide)
            {
                r.useGravity = false;
                r.velocity = Vector3.zero;
            }
        }

        // Rotate a rigidbody around a point and axis by angle
        void RigidbodyRotateAround(Vector3 point, Vector3 axis, float angle)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle, axis);
            Vector3 d = transform.position - point;
            r.MovePosition(point + rotation * d);
            r.MoveRotation(rotation * transform.rotation);
        }

        // Scale the capsule collider to 'mlp' of the initial value
        void ScaleCapsule(float mlp)
        {
            if (capsule.height != originalHeight * mlp)
            {
                capsule.height = Mathf.MoveTowards(capsule.height, originalHeight * mlp, Time.deltaTime * 4);
                capsule.center = Vector3.MoveTowards(capsule.center, originalCenter * mlp, Time.deltaTime * 2);
            }
        }

        public float GetAngleFromForward(Vector3 worldDirection)
        {
            Vector3 local = transform.InverseTransformDirection(worldDirection);
            return Mathf.Atan2(local.x, local.z) * Mathf.Rad2Deg;
        }
        // Set the collider to high friction material
        void HighFriction()
        {
            capsule.material = highFrictionMaterial;
        }

        // Set the collider to zero friction material
        void ZeroFriction()
        {
            capsule.material = zeroFrictionMaterial;
        }

        //---------------------------CHARACTER BASE ENDS--------------------------

        private void MoveFixed(Vector3 deltaPosition)
        {
            Vector3 velocity = deltaPosition / Time.deltaTime;

            // Add velocity of the rigidbody the character is standing on

                // Rotate velocity to ground tangent
            if (velocityToGroundTangentWeight > 0f)
            {
                Quaternion rotation = Quaternion.FromToRotation(transform.up, normal);
                velocity = Quaternion.Lerp(Quaternion.identity, rotation, velocityToGroundTangentWeight) * velocity;
            }

            // Vertical velocity
            Vector3 horizontalVelocity = V3Tools.ExtractHorizontal(velocity, gravity, 1f);

            r.velocity = horizontalVelocity;

            // Dampering forward speed on the slopes (Not working since Unity 2017.2)
            //float slopeDamper = !onGround? 1f: GetSlopeDamper(-deltaPosition / Time.deltaTime, normal);
            //forwardMlp = Mathf.Lerp(forwardMlp, slopeDamper, Time.deltaTime * 5f);
            forwardMlp = 1f;
        }

        private Vector3 GetMoveDirection()
        {
            moveDirection = Vector3.SmoothDamp(moveDirection, new Vector3(0f, 0f, state.move.magnitude), ref moveDirectionVelocity, smoothAccelerationTime);
            moveDirection = Vector3.MoveTowards(moveDirection, new Vector3(0f, 0f, state.move.magnitude), Time.deltaTime * linearAccelerationSpeed);
            return moveDirection * forwardMlp;
        }

        private Vector3 GetForwardDirection()
        {
            bool isMoving = state.move != Vector3.zero;

            if (isMoving) return state.move;
            return transform.forward;


        }

        public void Move(Vector3 deltaPosition, Quaternion deltaRotation)
        {
            // Disable movement while the puppet is not balanced or getting up.
            if (puppet.state != BehaviourPuppet.State.Puppet)
            {
                state.move = Vector3.zero;
                return;
            }

            fixedDeltaPosition += deltaPosition;
            fixedDeltaRotation *= deltaRotation;
        }

        void Rotate()
        {
            // Disable rotation while the puppet is not balanced or getting up.
            if (puppet.state != BehaviourPuppet.State.Puppet)
            {
                return;
            }

            float angle = GetAngleFromForward(GetForwardDirection());

            if (state.move == Vector3.zero) angle *= (1.01f - (Mathf.Abs(angle) / 180f)) * stationaryTurnSpeedMlp;

            // Rotating the character
            //RigidbodyRotateAround(characterAnimation.GetPivotPoint(), transform.up, angle * Time.deltaTime * turnSpeed);
            r.MoveRotation(Quaternion.AngleAxis(angle * Time.deltaTime * 5, transform.up) * r.rotation);
        }
    }
}