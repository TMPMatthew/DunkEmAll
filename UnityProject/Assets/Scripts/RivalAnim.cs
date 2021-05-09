using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RootMotion.Demos
{
    [RequireComponent(typeof(Animator))]
    public class RivalAnim : CharacterAnimationBase
    {
        public Rival characterController;
        [SerializeField] float turnSensitivity = 0.2f; // Animator turning sensitivity
        [SerializeField] float turnSpeed = 5f; // Animator turning interpolation speed
        [SerializeField] float runCycleLegOffset = 0.2f; // The offset of leg positions in the running cycle
        [Range(0.1f, 3f)] [SerializeField] float animSpeedMultiplier = 1; // How much the animation of the character will be multiplied by

        protected Animator animator;
        private Vector3 lastForward;
        //private const string groundedDirectional = "Grounded Directional", groundedStrafe = "Grounded Strafe";
        private float deltaAngle;

        protected override void Start()
        {
            base.Start();

            animator = GetComponent<Animator>();

            lastForward = transform.forward;
        }

        public override Vector3 GetPivotPoint()
        {
            return animator.pivotPosition;
        }

        // Is the Animator playing the grounded animations?
        public override bool animationGrounded
        {
            get
            {
                return true;
            }
        }

        // Update the Animator with the current state of the character controller
        protected virtual void Update()
        {
            if (Time.deltaTime == 0f) return;

            animatePhysics = animator.updateMode == AnimatorUpdateMode.AnimatePhysics;


            // Calculate the angular delta in character rotation
            float angle = -GetAngleFromForward(lastForward) - deltaAngle;
            deltaAngle = 0f;
            lastForward = transform.forward;
            angle *= turnSensitivity * 0.01f;
            angle = Mathf.Clamp(angle / Time.deltaTime, -1f, 1f);

            // Update Animator params
            //animator.SetFloat("Turn", Mathf.Lerp(animator.GetFloat("Turn"), angle, Time.deltaTime * turnSpeed));
           
        }

        // Call OnAnimatorMove manually on the character controller because it doesn't have the Animator component
        void OnAnimatorMove()
        {
            // For not using root rotation in Turn value calculation 
            Vector3 f = animator.deltaRotation * Vector3.forward;
            deltaAngle += Mathf.Atan2(f.x, f.z) * Mathf.Rad2Deg;

            characterController.Move(animator.deltaPosition, animator.deltaRotation);
        }
    }
}
    
