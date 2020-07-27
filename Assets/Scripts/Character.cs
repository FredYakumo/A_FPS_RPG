using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace A_FPS_RPG
{
    [RequireComponent(typeof(Animator))]
    public class Character : MonoBehaviour
    {
        const float RaycastDistance = 0.1f;
        const float Gravity = 10f;

        private readonly int MoveStateInt = Animator.StringToHash("Move");
        private readonly int JumpStateInt = Animator.StringToHash("Jump");

        private Animator animator;
        private Vector2 moveVector = Vector2.zero;
        private bool isJump = false;
        private bool onGround = true;
        private float destJumpHeight = 0f;
        private float fallVelocity = 0f;

        public CharacterAttribute _Attribute;
        public CharacterSize _Size;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {

            if (moveVector != Vector2.zero)
            {
                var move = Vector3.right * moveVector.x * _Attribute.moveSpeed * Time.fixedDeltaTime;
                float moveDist = move.magnitude;

                RaycastHit hit;
                if (Physics.CapsuleCast(transform.position + Vector3.up * (_Size.height - _Size.radius / 2),
                    transform.position + Vector3.up * (_Size.radius / 2),
                    _Size.radius, move, out hit, moveDist))
                {
                    transform.position += move * (hit.distance / moveDist);
                }
                else
                    transform.position += move;

            }

            JumpProcess();
            GravityProcess();
            GrandCheck();
        }

        private void GrandCheck()
        {
            if (isJump)
            {
                onGround = false;
                return;
            }
            RaycastHit hit;
            onGround = Physics.SphereCast(transform.position + Vector3.up * (_Size.radius / 2 + 0.1f),
                _Size.radius, Vector3.down, out hit, RaycastDistance);
        }

        private void JumpProcess()
        {
            float jumpMove = _Attribute.jumpHight * Time.fixedDeltaTime / _Attribute.jumpTimeDuration;
            if (isJump)
            {
                // reach maxium jump height
                if (transform.position.y + jumpMove >= destJumpHeight)
                {
                    var p = transform.position;
                    p.y = destJumpHeight;
                    transform.position = p;
                    isJump = false;
                }
                else
                {
                    RaycastHit hit;
                    if (Physics.SphereCast(transform.position + Vector3.up * ( _Size.height - _Size.radius / 2), _Size.radius,
                        Vector3.up, out hit, Mathf.Max(jumpMove, RaycastDistance)))
                    {
                        // hit the top
                        isJump = false;
                        return;
                    }
                    transform.position += Vector3.up * jumpMove;
                }
            }
        }

        private void GravityProcess()
        {
            
            float fallMove = fallVelocity * Time.fixedDeltaTime;

            if (!onGround)
            {
                RaycastHit hit;
                if (Physics.SphereCast(transform.position + Vector3.up * (_Size.radius / 2), _Size.radius,
                            Vector3.down, out hit, Mathf.Max(fallMove, RaycastDistance)))
                {
                    animator.SetBool("Jump", false);
                    var p = transform.position;
                    p.y = hit.point.y;
                    transform.position = p;

                    fallVelocity = 0.0f;
                    onGround = true;
                }
                else
                {
                    transform.position += Vector3.down * fallMove;
                    fallVelocity += Gravity * Time.fixedDeltaTime;
                }
            }
        }

        public void OnMovement(InputAction.CallbackContext obj)
        {
            moveVector = obj.ReadValue<Vector2>();

            animator.SetFloat(MoveStateInt, moveVector.x);
        }

        public void OnJump(InputAction.CallbackContext obj)
        {
            if (isJump || !onGround)
                return;

            animator.SetBool(JumpStateInt, true);
            isJump = true;
            destJumpHeight = transform.position.y + _Attribute.jumpHight;
        }
    }
}