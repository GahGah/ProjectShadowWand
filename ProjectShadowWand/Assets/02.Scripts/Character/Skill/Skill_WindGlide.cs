using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_WindGlide : Skill
{
    public IEnumerator GlideCoroutine;

    public GameObject windEffect;
    public Animator windAnimator;


    public int windAnimatorTornadoBlend;

    public Skill_WindGlide(PlayerController _p)
    {
        player = _p;
    }

    public override void Init()
    {
        GlideCoroutine = ProcessGlideTimer();
        windEffect.SetActive(false);
    }

    public override void Execute()
    {
        CheckGlideInput();

    }

    public override void PhysicsExecute()
    {
        UpdateCanGlide();
    }

    #region È°°­
    private void CheckGlideInput()
    {
        if (player.canGliding && (player.isFalling || player.isJumping))
        {
            if (InputManager.Instance.buttonMoveJump.wasPressedThisFrame && player.isOtherSkillUse() == false)
            {
                player.isGliding = true;
            }

            if (InputManager.Instance.buttonMoveJump.wasReleasedThisFrame)
            {
                player.isGliding = false;
                if (player.glideGauge != null)
                {
                    player.glideGauge.fillAmount = 0f;

                }
                if (player.GlideCoroutine != null)
                {
                    player.StopCoroutine(player.GlideCoroutine);
                    windAnimator.SetFloat(windAnimatorTornadoBlend, 3f);
                    player.GlideCoroutine = null;
                }
            }
        }
        else
        {
            if (InputManager.Instance.buttonMoveJump.isPressed == false)
            {
                player.isGliding = false;
                if (player.glideGauge != null)
                {
                    player.glideGauge.fillAmount = 0f;

                }
                if (player.GlideCoroutine != null)
                {
                    player.StopCoroutine(player.GlideCoroutine);
                    windAnimator.SetFloat(windAnimatorTornadoBlend, 3f);
                    player.GlideCoroutine = null;
                }

            }
        }
    }

    private void UpdateCanGlide()
    {
        if (player.isGliding == true && player.canGliding == true)
        {
            player.canGliding = false;

        }
    }

    public IEnumerator ProcessGlideTimer()
    {
        float one = 1f;
        float timer = 0f;
        if (player.glideGauge != null)
        {
            player.glideGauge.fillAmount = 1f;

        }

        windEffect.SetActive(true);
        windAnimator.SetFloat(windAnimatorTornadoBlend, 1.5f);


        while (timer < player.glideTime)
        {
            if (player.glideGauge != null)
            {
                player.glideGauge.fillAmount = one - timer / player.glideTime;

            }
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        if (player.glideGauge != null)
        {
            player.glideGauge.fillAmount = 0f;
        }
        windAnimator.SetFloat(windAnimatorTornadoBlend, 3f);
        player.isGliding = false;
    }
    #endregion
}
