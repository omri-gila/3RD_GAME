using UnityEngine;

namespace MovementSystem
{
    public class CapsuleColliderData
    {

        public CapsuleCollider CapsuleCollider { get; private set; }    
        public Vector3 ColliderCenterInLocalSpace { get; private set; }

        public void Initialize (GameObject gameObject)
        {
            if (CapsuleCollider != null)
            {
                return;
            }


            CapsuleCollider = gameObject.GetComponent<CapsuleCollider>();
            UpdateColliderData();

        }

        public void UpdateColliderData()
        {
            ColliderCenterInLocalSpace = CapsuleCollider.center;
        }





    }
}
