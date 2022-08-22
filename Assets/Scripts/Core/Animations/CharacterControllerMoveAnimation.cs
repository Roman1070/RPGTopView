using UnityEngine;

public class CharacterControllerMoveAnimation : BaseProgrammableAnimationMono<Vector3>
{
    [SerializeField]
    private CharacterController _characterController;
    public override void Process(float t)
    {
        _characterController.Move((EndValue - StartValue) * _Kurwa.Evaluate(t) * Time.deltaTime);
    }
}
