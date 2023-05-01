using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed; 
    private Vector2 direction;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        TakeInput();
        Move();
    }

    private void Move() {
        transform.Translate(direction * speed * Time.deltaTime);
        
        // Lock the z-rotation
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

        if(direction.x != 0 || direction.y != 0) {
            SetAnimationMovement(direction);
        }
        else {
            animator.SetLayerWeight(1, 0); //Prioritize idle when idle
        }

    }

    private void TakeInput() {
        direction = Vector2.zero;

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            direction += Vector2.up;
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            direction += Vector2.left;
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            direction += Vector2.down;
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            direction += Vector2.right;
        }
    }

    private void SetAnimationMovement(Vector2 direction) {
        animator.SetLayerWeight(1, 1); //prioritize walking when walking
        animator.SetFloat("xDir", direction.x);

        if (direction.x > 0) {
        // If direction is positive on x-axis (moving right), flip sprite horizontally
        transform.localScale = new Vector2(-1f, 1f);
        } else if (direction.x < 0) {
        // If direction is negative on x-axis (moving left), revert sprite flip
        transform.localScale = new Vector2(1f, 1f);
        }

        animator.SetFloat("yDir", direction.y);
    }
}
