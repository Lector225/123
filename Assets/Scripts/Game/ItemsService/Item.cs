using UnityEngine;

namespace ZombieIo.Items
{
    public abstract class Item : MonoBehaviour
    {
        [SerializeField] private ItemsService.ItemClass itemClass;
        [SerializeField] private float maxFlySpeed = 3.0f;
        
        private bool isMovementToPlayer;
        private Transform playerTarget;
        private float currentFlySpeed = 0.0f;
        private float distanceForPickup = 0.0f;
        
        public bool IsActive => gameObject.activeSelf;
        public ItemsService.ItemClass ItemClass => itemClass;
        
        
        public void Initialize(Vector3 position)
        {
            
            isMovementToPlayer = false;
            transform.position = position;
            gameObject.SetActive(true);
        }
        
        public void SetDistanceForPick(float distanceForPickup)
        {
            this.distanceForPickup = distanceForPickup;
        }

        public void ActivateFlyingToTarget()
        {
            isMovementToPlayer = true;
            currentFlySpeed = 0.0f;
        }

        public void OnUpdate()
        {
            if (isMovementToPlayer)
            {
                if (currentFlySpeed < maxFlySpeed)
                {
                    currentFlySpeed += Time.deltaTime * maxFlySpeed;
                    if (currentFlySpeed > maxFlySpeed)
                        currentFlySpeed = maxFlySpeed;
                }

                Vector3 direction = (playerTarget.position - transform.position).normalized;
                transform.position += direction * currentFlySpeed * Time.deltaTime;
            }
            else
            {
                var distance = Vector3.Distance(playerTarget.position, transform.position);
            }
        }
        
        protected abstract void FlyToTargetComplete();
    }
}