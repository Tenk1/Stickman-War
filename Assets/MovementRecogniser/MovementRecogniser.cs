using Autohand.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using PDollarGestureRecognizer;
using System.IO;
public class MovementRecogniser : MonoBehaviour
{
    [Header("Hand casting parameters"), Space]
    public Autohand.Hand castHand;
    public Transform movementSource;
    public Autohand.Demo.CommonButton castButton;
    public ParticleSystem particleCast;

    [Header("Hand recognition parameters"), Space]
    public float newPositionTresholdDistance = 0.05f;
    public float recognitionTreshold = 0.90f;
    public bool creationMode = true;
    public string newGestureName;

    [Header("Attacks parameters"), Space]
    public shooterScript BallShooter;
    public swordSpawner SwordsManager;

    private bool isMoving = false;
    private XRNode role;
    private InputDevice device;
    private List<Gesture> trainingSet;
    private List<InputDevice> devices;
    private List<Vector3> positionsList = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        if (castHand.left)
            role = XRNode.LeftHand;
        else
            role = XRNode.RightHand;
        devices = new List<InputDevice>();
        particleCast.Stop();
        trainingSet = new List<Gesture>();

        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach(var item in gestureFiles){
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }

    }

    // Update is called once per frame
    void Update()
    {
        InputDevices.GetDevicesAtXRNode(role, devices);
        if (devices.Count > 0)
            device = devices[0];
        //Grip input
        if (device.TryGetFeatureValue(GetCommonButton(castButton), out bool cast))
        {
            if (cast & isMoving) {
                updateMovement();
            }
            else if (!cast & isMoving)
            {
                endMovement();
            }
            else if (cast && !isMoving)
            {
                startMovement();
            }
        }

        //Debug.Log(positionsList.Count);
    }

    void updateMovement()
    {
        Vector3 lastPosition = positionsList[positionsList.Count - 1];
        if(Vector3.Distance(movementSource.position,lastPosition) > newPositionTresholdDistance)
        {
            positionsList.Add(movementSource.position);
        }
        
    }

    void endMovement()
    {
        isMoving = false;
        particleCast.Stop();

        if (positionsList.Count > 3)
        {
            //Algo reconaissance
            Point[] pointArray = new Point[positionsList.Count];
            for (int i = 0; i < positionsList.Count; i++)
            {
                Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
                pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
            }
            Gesture newGesture = new Gesture(pointArray);

            if (creationMode)
            {
                newGesture.Name = newGestureName;
                trainingSet.Add(newGesture);

                string filename = Application.persistentDataPath + "/" + newGestureName + ".xml";
                GestureIO.WriteGesture(pointArray, newGestureName, filename);
            }
            else
            {
                Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());

                if (result.Score < recognitionTreshold)
                    return;

                if (string.Equals(result.GestureClass.ToString(), "CannonGesture"))
                {
                    BallShooter.shootBall();
                }
                else if (string.Equals(result.GestureClass.ToString(), "SwordGesture"))
                {
                    SwordsManager.createSword();
                }
                else if (string.Equals(result.GestureClass.ToString(), "SwordAttackGesture"))
                {
                    SwordsManager.setOrientationSword();
                }

            }
        }

    }

    void startMovement()
    {
        isMoving = true;
        particleCast.Play();
        positionsList.Clear();
        positionsList.Add(movementSource.position);
    }


    public static InputFeatureUsage<bool> GetCommonButton(CommonButton button)
    {
        if (button == CommonButton.gripButton)
            return CommonUsages.gripButton;
        if (button == CommonButton.menuButton)
            return CommonUsages.menuButton;
        if (button == CommonButton.primary2DAxisClick)
            return CommonUsages.primary2DAxisClick;
        if (button == CommonButton.primary2DAxisTouch)
            return CommonUsages.primary2DAxisTouch;
        if (button == CommonButton.primaryButton)
            return CommonUsages.primaryButton;
        if (button == CommonButton.primaryTouch)
            return CommonUsages.primaryTouch;
        if (button == CommonButton.secondary2DAxisClick)
            return CommonUsages.secondary2DAxisClick;
        if (button == CommonButton.secondary2DAxisTouch)
            return CommonUsages.secondary2DAxisTouch;
        if (button == CommonButton.secondaryButton)
            return CommonUsages.secondaryButton;
        if (button == CommonButton.secondaryTouch)
            return CommonUsages.secondaryTouch;

        return CommonUsages.triggerButton;
    }
}
