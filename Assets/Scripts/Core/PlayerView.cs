using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class PlayerView : MonoBehaviour
{
    [Inject]
    private UpdateProvider _updateProvider;

    public CharacterController Controller;
    public Transform Model;
    public Transform GroundChecker;
    public Transform SpineAnchor;
    public Transform HandAnchor;
    public Transform WeaponsHolder;
    public CharacterControllerMoveAnimation MoveAnim;

    private void Start()
    {
        transform.position = new Vector3(30.27f, 1.73f, 117);
    }
}