using CodeBase.Controllers;
using CodeBase.Fishing.FishingBoxes;
using CodeBase.Infrastructure.Services.ObjectCreator;
using CodeBase.Utils;
using CodeBase.ZonesAndTriggers;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Services.BoatInstance
{
    public class BoatInstanceService : IInitializable
    {
        private readonly Vector3 _boatSpawnPosition = new (-3,0,-3);
        private readonly IObjectCreatorService _creatorService;
        private GameObject _boat;
        private GradeController _gradeController; 
        
        
        public BoatInstanceService(IObjectCreatorService creatorService)
        {
            _creatorService = creatorService;
        }
        public void Initialize()
        {
            CreateBoatSetPosition();
            GetGradeControllerAddEvent();
        }

        private void GetGradeControllerAddEvent()
        {
            _gradeController = Object.FindObjectOfType<GradeController>();
            _gradeController.OnBoatGraded.AddListener(EnabledCarvedBounds);
        }

        private void CreateBoatSetPosition()
        {
            _boat = _creatorService.InstantiateWithRegister(AssetPath.Boat);
            _boat.transform.position = _boatSpawnPosition;
        }

        public Transform GetBoatTransform()
        {
            return _boat.transform;
        }

        public Transform GetActiveBoat()
        {
            foreach (Transform child in _boat.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    return child.transform;
                }
            } 
            return null;
        }
        public int GetActiveChildIndex()
        {
            foreach (Transform child in _boat.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    return child.GetSiblingIndex();
                }
            } 
            return 0;
        }

        public BoatMovement GetBoatMovement()
        {
            foreach (Transform child in _boat.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    return child.gameObject.GetComponent<BoatMovement>();
                }
            } 
            return null;
        }
        public void EnableNormalBounds()
        {
            var boat = GetActiveBoat();
            var normalBounds = boat.transform.GetChild(2);
            var carvedBounds = boat.transform.GetChild(3);
            normalBounds.gameObject.SetActive(true);
            carvedBounds.gameObject.SetActive(false);
        }

        public void EnabledCarvedBounds()
        {
            var boat = GetActiveBoat();
            var normalBounds = boat.transform.GetChild(2);
            var carvedBounds = boat.transform.GetChild(3);
            normalBounds.gameObject.SetActive(false);
            carvedBounds.gameObject.SetActive(true);
        }

        public void DisableBoatMovementComponents()
        {
            var boat = GetActiveBoat();
            boat.gameObject.GetComponent<BoatMovement>().enabled = false;
            boat.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            boat.gameObject.GetComponent<BuoyancyObject>().enabled = false;
        }

        public WheelTriggerZone GetWheelZone()
        {
            var boat = GetActiveBoat();
            var wheel = boat.GetComponentInChildren<WheelTriggerZone>();
            return wheel;
        }
        public FishingTriggerZone GetStartFishingZone()
        {
            var boat = GetActiveBoat();
            var fishingZone = boat.GetComponentInChildren<FishingTriggerZone>(true);
            return fishingZone;
        } 
        
        public CollectingTriggerZone GetCollectingZone()
        {
            var boat = GetActiveBoat();
            var collectZone = boat.GetComponentInChildren<CollectingTriggerZone>(true);
            return collectZone;
        }

        public FishingBoxZone FishingBoxZone()
        {
            var boat = GetActiveBoat(); 
            var boxZone = boat.GetComponentInChildren<FishingBoxZone>();
            return boxZone;
        }
     
        public void DisableUnloadingBoxZone()
        {
            var boat = GetActiveBoat();
            var zone = boat.GetComponentInChildren<UnloadingFishZone>(true);
            zone.gameObject.SetActive(false);
            
            var boxZone = boat.GetComponentInChildren<FishingBoxZone>().GetComponent<Collider>();
            boxZone.enabled = true;
        }
        
        public void EnableUnloadingBoxZone()
        {
            var boat = GetActiveBoat();
            var zone = boat.GetComponentInChildren<UnloadingFishZone>(true);
            zone.gameObject.SetActive(true);

            var boxZone = boat.GetComponentInChildren<FishingBoxZone>().GetComponent<Collider>();
            boxZone.enabled = false;
        }
        
        public void ChangeMeshForInstanceBoat(int onIndex)
        {
            _boat.transform.GetChild(0).gameObject.SetActive(false);
            var active = _boat.transform.GetChild(onIndex);
            active.gameObject.SetActive(true);
        }
        
    }
}
