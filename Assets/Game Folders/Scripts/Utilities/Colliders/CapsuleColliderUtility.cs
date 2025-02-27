using System;
using UnityEngine;

namespace MovementSystem
{

    [Serializable]
    public class CapsuleColliderUtility
    {

        public CapsuleColliderData CapsuleColliderData { get; private set; }

        [field :SerializeField] public DefaultColliderData DefaultColliderData { get; private set; }

        [field : SerializeField] public SlopeData SlopeData { get; private set; }
        public void Initialize(GameObject gameObject)
        {

            if (CapsuleColliderData!= null)
            { 
                return; 
            }

            CapsuleColliderData = new CapsuleColliderData();

            CapsuleColliderData.Initialize(gameObject);

        }

        public void CalculateCapsuleColliderDimensions()
        {
            SetCapsuleColliderRadius(DefaultColliderData.Radius);
            SetCapsuleColliderHeight(DefaultColliderData.Height * (1f - SlopeData.StepHeightPercentage));

            RecalculateCapsuleColliderCenter();

            float halfColliderHeight = CapsuleColliderData.CapsuleCollider.height / 2f;

            if (halfColliderHeight < CapsuleColliderData.CapsuleCollider.radius) 
            {

                SetCapsuleColliderRadius(halfColliderHeight);
            }

            CapsuleColliderData.UpdateColliderData();


        }

        

        private void SetCapsuleColliderRadius(float radius)
        {
            CapsuleColliderData.CapsuleCollider.radius = radius;      
                
        }

        private void SetCapsuleColliderHeight(float height)
        {
            CapsuleColliderData.CapsuleCollider.height = height;

        }

        private void RecalculateCapsuleColliderCenter()
        {
            float colliderHeightDifference = DefaultColliderData.Height - CapsuleColliderData.CapsuleCollider.height;

            Vector3 newColliderCenter = new Vector3(0f, DefaultColliderData.CenterY + (colliderHeightDifference / 2f),0f);

            CapsuleColliderData.CapsuleCollider.center = newColliderCenter;


        }
    }
}
