using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
                                                    //   public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
                                                    //  public float m_MinSize = 6.5f;                  // The smallest orthographic size the camera can be.
    [HideInInspector] public List<Transform> m_Targets; // All the targets the camera needs to encompass.
    private Camera m_Camera;                        // Used for referencing the camera.
                                                    //   private float m_ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
    private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
    Vector3 m_DesiredPosition;              // The position the camera is moving towards.
    IEnumerable<Vector3> activeTargets;
    Rect bounds = Rect.zero;

    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
        m_DesiredPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        activeTargets = m_Targets.Where(x => x.gameObject.activeSelf).Select(x => x.transform.position);
        if (activeTargets.Count() == 0) return;
        bounds.xMin = activeTargets.Min(x => x.x);
        bounds.yMin = activeTargets.Min(x => x.z);
        bounds.xMax = activeTargets.Max(x => x.x);
        bounds.yMax = activeTargets.Max(x => x.z);
        m_DesiredPosition = new Vector3((bounds.xMax - bounds.xMin) / 2, m_DesiredPosition.y, (bounds.yMax - bounds.yMin) / 2);
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }
}