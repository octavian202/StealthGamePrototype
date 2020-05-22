using UnityEngine;

namespace Drone
{
    public class DroneManager : MonoBehaviour
    {
        private bool droneIsActivated = false;
        private GameObject drona;
        public Transform droneTransform;
        private Vector3 playerPosition;

        private void Start()
        {
            drona = GameObject.Find("Drona");
            droneTransform = drona.transform;
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.G)) return;

            if (droneIsActivated)
            {
                droneTransform.position = new Vector3(0f, 100f, 0f);
                droneIsActivated = false;
                return;
            }

            var playerTransform = GameObject.Find("Player").transform;
            playerPosition = playerTransform.position;
            var playerForward = playerTransform.forward;
            playerForward.y = 0f;

            droneIsActivated = true;
            droneTransform.position = playerPosition + playerForward * 2f;
        }
    }
}