#if COM_UNITY_MODULES_PHYSICS2D || COM_UNITY_MODULES_PHYSICS
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace TestProject.RuntimeTests
{
    public class NetworkTransformRigidBodyTestComponent : NetworkTransform
    {
        public enum AuthorityModes
        {
            Server,
            Owner
        }

        public AuthorityModes AuthorityMode;

        protected override bool OnIsServerAuthoritative()
        {
            return AuthorityMode == AuthorityModes.Server;
        }
    }

#if COM_UNITY_MODULES_PHYSICS2D
    public class NetworkRigidbody2DTestComponent : NetworkRigidbody2D
    {
        public bool WasKinematicBeforeSpawn;

        protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
        {
            WasKinematicBeforeSpawn = GetComponent<Rigidbody2D>().isKinematic;
            base.OnNetworkPreSpawn(ref networkManager);
        }
    }
#endif


#if COM_UNITY_MODULES_PHYSICS
    public class NetworkRigidbodyTestComponent : NetworkRigidbody
    {
        public bool WasKinematicBeforeSpawn;
        protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
        {
            WasKinematicBeforeSpawn = GetComponent<Rigidbody>().isKinematic;
            base.OnNetworkPreSpawn(ref networkManager);
        }
    }
#endif

}
#endif
