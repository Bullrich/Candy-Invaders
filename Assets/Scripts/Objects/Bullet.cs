using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Manager;
using Game.Systems;
using Game.Interface;
//By @JavierBullrich

namespace Game.Obj {
    [RequireComponent(typeof(BoxCollider2D))]
	public abstract class Bullet : MonoBehaviour, IPoolObject {
        public float speed;
        public LayerMask collisionMasks;
        SystemCalculations calcs = new SystemCalculations();
        public enum MoveDir
        {
            Up,
            Down
        }
        public MoveDir moveDirection;

        public virtual void Update()
        {
            Movement((moveDirection == MoveDir.Up ? Vector2.up : Vector2.down));
        }

        private void Movement(Vector2 moveDir)
        {
            Vector2 amountToMove = moveDir * GameManager.DeltaTime * speed;
            int screenPosPercentage = calcs.FloatToPercentage(Camera.main.WorldToScreenPoint(transform.position).y, Screen.height);
            if (screenPosPercentage < 3 || screenPosPercentage > 97)
                gameObject.SetActive(false);

            transform.Translate(amountToMove);


            VerticalCollisions();
        }
        
        void VerticalCollisions()
        {
            float rayLenght = .1f;
            Debug.DrawLine(transform.position, transform.position + new Vector3(0 , rayLenght), Color.blue);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, rayLenght, collisionMasks);

            if (hit)
            {
                CollisionDetection(hit.transform.gameObject);
            }
        }

        public void CollisionDetection(GameObject hit)
        {
            print(hit.name);
            hit.GetComponent<IDamagable>().ReceiveDamage();
            gameObject.SetActive(false);
        }

        public void SetUp(Vector2 position)
        {
            transform.position = position;
        }

        public object getObject()
        {
            return this;
        }
    }
}