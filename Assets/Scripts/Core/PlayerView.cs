using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public CharacterController Controller;
    public Transform Model;
    public Transform GroundChecker;
    public Transform HandAnchor;
    public Transform SpineAnchor;
    public Transform WeaponsHolder;
    public CharacterControllerMoveAnimation MoveAnim;

    private void Start()
    {
        transform.position = new Vector3(25, 3, 68);
    }
}
