using UnityEngine;
using System.Collections;
using RootMotion.Dynamics;

namespace RootMotion.Demos
{

    public class MuscleDisconnect : MonoBehaviour
    {

        public BehaviourPuppet puppet;
        public Skeleton skeleton;
        public MuscleDisconnectMode disconnectMuscleMode;
        public float unpin = 10f;
        public float force = 10f;
        GameObject mesh;
        GameObject meshCut;
        public GameObject[] connected;
        public GameObject[] eyes;

        private void Start()
        {
            mesh = this.transform.Find("Mesh").gameObject;
            meshCut = mesh.transform.Find("MeshCut").gameObject;

            meshCut.SetActive(false);
        }

        public void Cut()
        {

            var muscle = GetComponent<Collider>().attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>();
            if (muscle != null)
            {
                muscle.puppetMaster.DisconnectMuscleRecursive(muscle.muscleIndex, disconnectMuscleMode);
            }

            foreach (GameObject j in connected)
            {
                j.transform.Find("Mesh").gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
                j.transform.Find("Mesh").gameObject.transform.Find("MeshCut").gameObject.SetActive(true);
            }

            foreach (GameObject j in eyes)
            {
                j.SetActive(false);
            }
            mesh.GetComponent<SkinnedMeshRenderer>().enabled = false;
            meshCut.SetActive(true);

        }


        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Sword") && ContactPoint.velocity > KatanaCut.cutVelocityThreshold)
            {
                var muscle = GetComponent<Collider>().attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>();
                if (muscle != null)
                {
                    muscle.puppetMaster.DisconnectMuscleRecursive(muscle.muscleIndex, disconnectMuscleMode);
                }

                foreach (GameObject j in connected)
                {
                    j.transform.Find("Mesh").gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
                    j.transform.Find("Mesh").gameObject.transform.Find("MeshCut").gameObject.SetActive(true);
                }

                foreach (GameObject j in eyes)
                {
                    j.SetActive(false);
                }
                mesh.GetComponent<SkinnedMeshRenderer>().enabled = false;
                 meshCut.SetActive(true);
            }
        }

    }
}