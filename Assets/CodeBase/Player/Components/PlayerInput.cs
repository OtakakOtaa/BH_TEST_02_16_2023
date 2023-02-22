using System;
using UnityEngine;

namespace CodeBase.Player.Components
{
    public class PlayerInput
    {
        public event Action LmbPressed;
        
        public Vector2 MousePosition => new (Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); 
        public Vector2 DirectionMovement => CalculateDirectionMovement();
        public bool IsMoving => CalculateDirectionMovement().magnitude != 0f;
            
        public void UpdateState()
        {
            if(Input.GetMouseButtonDown(0)) LmbPressed?.Invoke();
        }
        
        private Vector2 CalculateDirectionMovement()
            => new ( Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
    }
}