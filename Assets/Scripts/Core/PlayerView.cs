using UnityEngine;
using UnityEngine.AI;

public class PlayerView : MonoBehaviour
{
    public CharacterController Controller;
    public Transform Model;
    public Transform GroundChecker;
    public Transform HandAnchor;
    public Transform SpineAnchor;
    public Transform WeaponsHolder;
    public CharacterControllerMoveAnimation MoveAnim;
    public SurfaceSlider SurfaceSlider;

    private void Start()
    {
        transform.position = new Vector3(30.27f, 0.73f, 117);
    }
}
