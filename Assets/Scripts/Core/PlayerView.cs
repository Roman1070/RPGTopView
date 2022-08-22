using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerView : MonoBehaviour
{
    public CharacterController Controller;
    public Transform Model;
    public Transform GroundChecker;
    public Camera Camera;
    public Transform HandAnchor;
    public Transform SpineAnchor;
    public Transform CurrentWeapon;
    public CharacterControllerMoveAnimation MoveAnim;
}
