using UnityEngine;

public class EntityAnimeHandler
{
    private EntityStateMachine _stateMachine;
    public EntityAnimeHandler(EntityStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public virtual void StartAnimation(int animatorHash)
    {
        _stateMachine.Entity.Animator.SetBool(animatorHash, true);
    }

    public virtual void StopAnimation(int animatorHash)
    {
        _stateMachine.Entity.Animator.SetBool(animatorHash, false);
    }

    public virtual float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
