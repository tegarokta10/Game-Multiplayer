    using UnityEngine;

    public class Mover : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float paddleYLimit = 3.62f;

        public void Move(float direction)
        {
            transform.Translate(Vector2.up * direction * moveSpeed * Time.deltaTime);
            ClampPosition();
        }

        private void ClampPosition()
        {
            transform.position = new Vector2(
                transform.position.x,
                Mathf.Clamp(transform.position.y, -paddleYLimit, paddleYLimit)
            );
        }
    }
