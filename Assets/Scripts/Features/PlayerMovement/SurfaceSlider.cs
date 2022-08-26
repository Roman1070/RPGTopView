using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceSlider : MonoBehaviour
{
    private Vector3 _normal;
    private Vector3 _offset;
    [SerializeField]
    PlayerView _player;

    private void Update()
    {
        _normal = GetNormal();

        Vector3 directionALongSurface = transform.forward.normalized - Vector3.Dot(transform.forward.normalized, _normal) * _normal;
        Vector3 offset = directionALongSurface  * Time.deltaTime*10;
        _offset = offset;
    }

    private Vector3 GetNormal()
    {
        var point = _player.Model.transform.position + _player.transform.up;

        RaycastHit hits;
        if (Physics.Raycast(point, -_player.Model.transform.up, out hits, 100, LayerMask.GetMask("Ground")))
        {
            return hits.normal;
        }

        return _player.transform.up;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + _offset * 3);
    }
}
