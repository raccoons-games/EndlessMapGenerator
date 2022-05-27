using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        private void FixedUpdate()
        {
            Vector2 direction = Vector2.zero;
            direction.x = Input.GetAxis("Horizontal");
            direction.y = Input.GetAxis("Vertical");
            Move(direction);
        }
        private void Move(Vector2 direction)
        {
            direction *= Time.fixedDeltaTime * _speed;
            Vector3.ClampMagnitude(direction, _speed);
            Vector3 targetPos = new Vector3();
            targetPos.x += direction.x;
            targetPos.z += direction.y;
            transform.position += targetPos;
        }
    }
}
