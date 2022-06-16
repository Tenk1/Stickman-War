using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

namespace Autohand.Demo
{
    public class bazooka : MonoBehaviour
    {
        // Start is called before the first frame update

        public Transform spawnPoint;
        public GameObject missile;
        public GameObject currentMissile;

        public Teleporter hand;
        public XRNode role;
        public CommonButton button;

        public SoundHandler SFXHandler;
        
        InputDevice device;
        List<InputDevice> devices;


        public float speed = 20f;

        void Start()
        {
            //Load(); 
            devices = new List<InputDevice>();

        }

        // Update is called once per frame

        void FixedUpdate()
        {
            InputDevices.GetDevicesAtXRNode(role, devices);
            if (devices.Count > 0)
                device = devices[0];

            if (device != null && device.isValid)
            {
                //Sets hand fingers wrap
                if (device.TryGetFeatureValue(XRHandControllerLink.GetCommonButton(button), out bool LoadButton))
                {
                    if (currentMissile == null && LoadButton)
                        Load();
                }
            }
        }
        void Update()
        {
         
        }

        public void Load()
        {
            (SFXHandler.GetComponent(typeof(SoundHandler)) as SoundHandler).PlaySound(AudioSound.Load, transform.position);
            GameObject missileInstance = Instantiate(missile, spawnPoint.position, spawnPoint.rotation);
            missileInstance.transform.parent = spawnPoint;
            currentMissile = missileInstance;
            (currentMissile.GetComponent(typeof(missile)) as missile).instantiateSFX(SFXHandler);
            Rigidbody rig_m = currentMissile.GetComponent<Rigidbody>();
            rig_m.isKinematic = true;
        }

        public void Launch()
        {
            if(currentMissile != null)
            {
                (SFXHandler.GetComponent(typeof(SoundHandler)) as SoundHandler).PlaySound(AudioSound.Bazooka, transform.position);
            }

            if (currentMissile != null && GameManager.Instance.m_GameState == GameState.gamePlay || SceneManager.GetActiveScene().name == "Menu")
            {
                Rigidbody rig_m = currentMissile.GetComponent<Rigidbody>();

                currentMissile.transform.parent = null;
                currentMissile = null;
                rig_m.isKinematic = false;
                rig_m.AddForce(-spawnPoint.forward * speed, ForceMode.Impulse);
            }

          //  Invoke("Load", 2f);
        }

    }
}
