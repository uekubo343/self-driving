// SerialID: [77a855b2-f53d-4b80-9c94-c40562952b74]
using UnityEngine;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private int index = 0;
    public int Index {
        get { return index; }
        private set { index = value; }
    }

    [SerializeField] private bool isLast = false;
    public bool IsLast {
        get { return isLast; }
        private set { isLast = value; }
    }

    [SerializeField] private Vector3 nextDirection;
    /// <summary>
    /// 次のWaypointの方向
    /// </summary>
    public Vector3 NextDirection {
        get { return nextDirection; }
        private set { nextDirection = value; }
    }

    [SerializeField] private List<Vector3> nextDirections = new List<Vector3>();
    /// <summary>
    /// 次に進む方向のリスト
    /// </summary>
    public List<Vector3> NextDirections {
        get { return nextDirections; }
    }

    [Header("Layer Settings"), SerializeField] private string layerWall = "Wall";
    private string LayerWall => layerWall;

    [SerializeField] private string layerRoad = "Road";
    private string LayerRoad => layerRoad;

    public void SetPosition(int index, bool isLast) {
        Index = index;
        IsLast = isLast;

        transform.localScale = Vector3.one;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, float.MaxValue, LayerMask.GetMask(LayerRoad))) {
            transform.position = hit.point + Vector3.up;
            var rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            transform.rotation = rotation * transform.rotation;
        }
    }

    public (Vector3, Vector3) SetScaleAndRotation() {
        var left = Vector3.zero;
        var right = Vector3.zero;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, float.MaxValue, LayerMask.GetMask(LayerRoad))) {
            transform.position = hit.point + Vector3.up;
            var rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            transform.rotation = rotation * transform.rotation;
        }

        if(Physics.Raycast(transform.position + transform.up, -transform.right, out hit, float.MaxValue, LayerMask.GetMask(LayerWall))) {
            left = hit.point;
        }

        if(Physics.Raycast(transform.position + transform.up, transform.right, out hit, float.MaxValue, LayerMask.GetMask(LayerWall))) {
            right = hit.point;
        }

        var distance = Vector3.Distance(left, right);
        var position = (left + right) / 2;

        transform.position = new Vector3(position.x, transform.position.y, position.z);
        transform.localScale = new Vector3(distance, 2, 1);

        return (left, right);
    }

    public void SetNextDirection(Vector3 nextPosition) {
        NextDirection = Vector3.Normalize(nextPosition - transform.position);
    }

    public void SetNextDirections(List <Vector3> nextPositions) {
        nextDirections.Clear(); // 既存の方向をクリア
        Vector3 currentPosition = transform.position;

        foreach (var nextPosition in nextPositions) {
            nextDirections.Add(Vector3.Normalize(nextPosition - currentPosition));
            currentPosition = nextPosition; // 現在位置からnつ先ではなくnとn+1の間を求める
        }
    }

    void OnDrawGizmosSelected() {
        /*
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position - transform.right * 20);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 20);
        */
    }
}
