using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
[RequireComponent(typeof(Collider))]
public class DebugCollider : MonoBehaviour
{
    public Color gizmoColor = Color.green;
    public bool drawWireBox = true;
    public bool drawIcon = false;
    public bool drawLabel = false;
    public bool autoSizeFromCollider = true;
    public Vector3 manualBoxSize = Vector3.one;

    private Collider cachedCollider;

    private void OnValidate()
    {
        cachedCollider = GetComponent<Collider>();
    }

    private void OnDrawGizmos()
    {
        if (!cachedCollider)
            cachedCollider = GetComponent<Collider>();

        if (!cachedCollider)
        {
            Debug.LogWarning($"{name} has DebugOutline but no Collider.");
            return;
        }

        Gizmos.color = gizmoColor;

        Vector3 size;
        Vector3 center;

        if (autoSizeFromCollider)
        {
            GetColliderBox(out center, out size);
        }
        else
        {
            center = transform.position;
            size = manualBoxSize;
        }

        if (drawWireBox)
            Gizmos.DrawWireCube(center, size);

        if (drawIcon)
            Gizmos.DrawIcon(center, "d_UnityEditor.SceneView.png", true);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (drawLabel)
        {
            GUIStyle style = new GUIStyle
            {
                normal = new GUIStyleState { textColor = gizmoColor },
                fontStyle = FontStyle.Bold
            };

            Handles.Label(transform.position + Vector3.up * 0.5f, $"{name} [{cachedCollider.GetType().Name}]", style);
        }
    }
#endif

    private void GetColliderBox(out Vector3 center, out Vector3 size)
    {
        center = transform.position;
        size = Vector3.one;

        switch (cachedCollider)
        {
            case BoxCollider box:
                center = box.transform.TransformPoint(box.center);
                size = Vector3.Scale(box.size, box.transform.lossyScale);
                break;

            case SphereCollider sphere:
                center = sphere.transform.TransformPoint(sphere.center);
                float dia = sphere.radius * 2f;
                Vector3 scale = sphere.transform.lossyScale;
                size = new Vector3(dia * scale.x, dia * scale.y, dia * scale.z);
                break;

            case CapsuleCollider capsule:
                center = capsule.transform.TransformPoint(capsule.center);
                float radius = capsule.radius;
                float height = capsule.height;

                Vector3 capsuleSize = Vector3.zero;
                switch (capsule.direction)
                {
                    case 0:
                        capsuleSize = new Vector3(height, radius * 2, radius * 2);
                        break;
                    case 1:
                        capsuleSize = new Vector3(radius * 2, height, radius * 2);
                        break;
                    case 2:
                        capsuleSize = new Vector3(radius * 2, radius * 2, height);
                        break;
                }
                size = Vector3.Scale(capsuleSize, capsule.transform.lossyScale);
                break;

            case MeshCollider mesh:
                center = mesh.bounds.center;
                size = mesh.bounds.size;
                break;

            default:
                center = cachedCollider.bounds.center;
                size = cachedCollider.bounds.size;
                break;
        }
    }
}
