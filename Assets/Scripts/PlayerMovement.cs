using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownCamera : MonoBehaviour
{
    public float moveSpeed = 20f;
    
    void Update()
    {
        var kb = Keyboard.current;
        Vector3 move = Vector3.zero;

        if (kb.wKey.isPressed) move += Vector3.forward;
        if (kb.sKey.isPressed) move += Vector3.back;
        if (kb.aKey.isPressed) move += Vector3.left;
        if (kb.dKey.isPressed) move += Vector3.right;

        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }
}
