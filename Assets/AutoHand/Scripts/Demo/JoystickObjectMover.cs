using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Autohand.Demo{
    public class JoystickObjectMover : PhysicsGadgetJoystick{
        public Transform move;
        public float speed = 2;
        
        void Update(){
            var axis = GetValue();
            var moveAxis = new Vector3(axis.x*Time.unscaledDeltaTime * speed, 0, axis.y*Time.unscaledDeltaTime * speed);
            move.transform.localPosition += moveAxis;
        }
    }
}
