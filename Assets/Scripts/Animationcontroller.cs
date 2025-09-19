using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Animationcontroller : MonoBehaviour
{

    [SerializeField] Charactercontroller chracterController;

    [SerializeField] private Animator CharacterAnimator;
    [SerializeField] private Animator InsectAnimator;

    
    private void Update()
    {
        CharacterMoveAnimation();
    }

    public void CharacterMoveAnimation()
    {
        GameObject Character = chracterController.gameObject;

        if (Character !=null )
        {
            float moveX = chracterController.rb.velocity.x;

            if (Mathf.Abs(moveX) > 0.1f)
            {
                CharacterAnimator.SetFloat("Walking", 1);
                InsectAnimator.SetBool("Insect_Walking", true);
            }
            else
            {
                CharacterAnimator.SetFloat("Walking", 0);
                InsectAnimator.SetBool("Insect_Walking", false);
            }

           

        }
    }
}
