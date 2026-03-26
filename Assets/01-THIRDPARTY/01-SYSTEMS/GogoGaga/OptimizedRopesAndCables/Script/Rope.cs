using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GogoGaga.OptimizedRopesAndCables
{
    [ExecuteAlways]
    [RequireComponent(typeof(LineRenderer))]
    public class Rope : MonoBehaviour
    {
        public event Action OnPointsChanged;

        [Header("Rope Transforms")]
        [Tooltip("The rope will start at this point")]
        [SerializeField] private Transform startPoint;
        public Transform StartPoint => startPoint;

        [Tooltip("This will move at the center hanging from the rope, like a necklace, for example")]
        [SerializeField] private Transform midPoint;
        public Transform MidPoint => midPoint;

        [Tooltip("The rope will end at this point")]
        [SerializeField] private Transform endPoint;
        public Transform EndPoint => endPoint;

        [Header("Rope Settings")]
        [Tooltip("How many points should the rope have")]
        [Range(2, 100)] public int linePoints = 10;

        [Tooltip("Stiffness of the simulation (physics speed)")]
        public float stiffness = 350f;

        [Tooltip("Damping of the simulation")]
        public float damping = 15f;

        [Tooltip("Physical length of the rope")]
        public float ropeLength = 15;

        public float ropeWidth = 0.1f;

        // ---------------------------------------------------------
        // NUEVA SECCIÓN: DIRECCIONALIDAD
        // ---------------------------------------------------------
        [Header("Directionality (Stiffness Rotation)")]
        [Tooltip("Empuja la cuerda en la dirección Z del StartPoint. Úsalo para que la cuerda salga recta.")]
        public float startTangentForce = 0f;

        [Tooltip("Empuja la cuerda en la dirección Z del EndPoint (Agarrador). Úsalo para que la cuerda siga la rotación de tu mano.")]
        public float endTangentForce = 0f;
        // ---------------------------------------------------------

        [Header("Rational Bezier Weight Control")]
        [Range(1, 15)] public float midPointWeight = 1f;
        private const float StartPointWeight = 1f;
        private const float EndPointWeight = 1f;

        [Header("Midpoint Position")]
        [Range(0.25f, 0.75f)] public float midPointPosition = 0.5f;

        public Vector3 midPointOffset = Vector3.zero;

        private Vector3 currentValue;
        private Vector3 currentVelocity;
        private Vector3 targetValue;
        public Vector3 otherPhysicsFactors { get; set; }
        private const float valueThreshold = 0.01f;
        private const float velocityThreshold = 0.01f;

        private LineRenderer lineRenderer;
        private bool isFirstFrame = true;

        // Variables para detectar cambios (Optimization)
        private Vector3 prevStartPointPosition;
        private Quaternion prevStartPointRotation;
        private Vector3 prevEndPointPosition;
        private Quaternion prevEndPointRotation;

        private float prevMidPointPosition;
        private float prevMidPointWeight;
        private Vector3 prevMidPointOffset;
        private float prevLineQuality;
        private float prevRopeWidth;
        private float prevstiffness;
        private float prevDampness;
        private float prevRopeLength;
        private float prevStartTangent;
        private float prevEndTangent;

        public bool IsPrefab => gameObject.scene.rootCount == 0;
        
        private void Start()
        {
            InitializeLineRenderer();
            if (AreEndPointsValid())
            {
                currentValue = GetMidPoint();
                targetValue = currentValue;
                currentVelocity = Vector3.zero;
                SetSplinePoint();
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                InitializeLineRenderer();
                if (AreEndPointsValid())
                {
                    RecalculateRope();
                    SimulatePhysics();
                }
                else
                {
                    lineRenderer.positionCount = 0;
                }
            }
        }

        private void InitializeLineRenderer()
        {
            if (!lineRenderer)
            {
                lineRenderer = GetComponent<LineRenderer>();
            }

            lineRenderer.startWidth = ropeWidth;
            lineRenderer.endWidth = ropeWidth;
        }

        private void Update()
        {
            if (IsPrefab) return;

            if (AreEndPointsValid())
            {
                SetSplinePoint();

                if (!Application.isPlaying && (IsPointsMovedOrRotated() || IsRopeSettingsChanged()))
                {
                    SimulatePhysics();
                    NotifyPointsChanged();
                }

                // Update previous values
                prevStartPointPosition = startPoint.position;
                prevStartPointRotation = startPoint.rotation;
                prevEndPointPosition = endPoint.position;
                prevEndPointRotation = endPoint.rotation;

                prevMidPointPosition = midPointPosition;
                prevMidPointWeight = midPointWeight;
                prevMidPointOffset = midPointOffset;

                prevLineQuality = linePoints;
                prevRopeWidth = ropeWidth;
                prevstiffness = stiffness;
                prevDampness = damping;
                prevRopeLength = ropeLength;
                prevStartTangent = startTangentForce;
                prevEndTangent = endTangentForce;
            }
        }

        private bool AreEndPointsValid()
        {
            return startPoint != null && endPoint != null;
        }

        private void SetSplinePoint()
        {
            if (lineRenderer.positionCount != linePoints + 1)
            {
                lineRenderer.positionCount = linePoints + 1;
            }

            Vector3 mid = GetMidPoint();
            targetValue = mid;
            mid = currentValue;

            if (midPoint != null)
            {
                midPoint.position = GetRationalBezierPoint(startPoint.position, mid, endPoint.position, midPointPosition, StartPointWeight, midPointWeight, EndPointWeight);
            }

            for (int i = 0; i < linePoints; i++)
            {
                Vector3 p = GetRationalBezierPoint(startPoint.position, mid, endPoint.position, i / (float)linePoints, StartPointWeight, midPointWeight, EndPointWeight);
                lineRenderer.SetPosition(i, p);
            }

            lineRenderer.SetPosition(linePoints, endPoint.position);
        }

        private float CalculateYFactorAdjustment(float weight)
        {
            float k = Mathf.Lerp(0.493f, 0.323f, Mathf.InverseLerp(1, 15, weight));
            float w = 1f + k * Mathf.Log(weight);
            return w;
        }

        // LÓGICA MODIFICADA PARA SOPORTAR ROTACIÓN
        private Vector3 GetMidPoint()
        {
            Vector3 startPointPosition = startPoint.position;
            Vector3 endPointPosition = endPoint.position;

            Vector3 midpos = Vector3.Lerp(startPointPosition, endPointPosition, midPointPosition);

            float dist = Vector3.Distance(startPointPosition, endPointPosition);
            float yFactor = (ropeLength - Mathf.Min(dist, ropeLength)) / CalculateYFactorAdjustment(midPointWeight);
            midpos.y -= yFactor;

            // Aplicar fuerza tangencial (Rigidez direccional)
            if (startTangentForce != 0)
            {
                midpos += startPoint.up * startTangentForce;
            }

            if (endTangentForce != 0)
            {
                midpos += endPoint.up * endTangentForce;
            }

            midpos += midPointOffset;
            return midpos;
        }

        private Vector3 GetRationalBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t, float w0, float w1, float w2)
        {
            Vector3 wp0 = w0 * p0;
            Vector3 wp1 = w1 * p1;
            Vector3 wp2 = w2 * p2;

            float denominator = w0 * Mathf.Pow(1 - t, 2) + 2 * w1 * (1 - t) * t + w2 * Mathf.Pow(t, 2);
            Vector3 point = (wp0 * Mathf.Pow(1 - t, 2) + wp1 * 2 * (1 - t) * t + wp2 * Mathf.Pow(t, 2)) / denominator;

            return point;
        }

        public Vector3 GetPointAt(float t)
        {
            if (!AreEndPointsValid()) return Vector3.zero;
            return GetRationalBezierPoint(startPoint.position, currentValue, endPoint.position, t, StartPointWeight, midPointWeight, EndPointWeight);
        }

        private void FixedUpdate()
        {
            if (IsPrefab) return;

            if (AreEndPointsValid())
            {
                if (!isFirstFrame)
                {
                    SimulatePhysics();
                }
                isFirstFrame = false;
            }
        }

        private void SimulatePhysics()
        {
            float dampingFactor = Mathf.Max(0, 1 - damping * Time.fixedDeltaTime);
            Vector3 acceleration = (targetValue - currentValue) * stiffness * Time.fixedDeltaTime;
            currentVelocity = currentVelocity * dampingFactor + acceleration + otherPhysicsFactors;
            currentValue += currentVelocity * Time.fixedDeltaTime;

            if (Vector3.Distance(currentValue, targetValue) < valueThreshold && currentVelocity.magnitude < velocityThreshold)
            {
                currentValue = targetValue;
                currentVelocity = Vector3.zero;
            }
        }

        private void NotifyPointsChanged()
        {
            OnPointsChanged?.Invoke();
        }

        private bool IsPointsMovedOrRotated()
        {
            var startMoved = startPoint.position != prevStartPointPosition;
            var startRotated = startPoint.rotation != prevStartPointRotation;
            var endMoved = endPoint.position != prevEndPointPosition;
            var endRotated = endPoint.rotation != prevEndPointRotation;

            return startMoved || startRotated || endMoved || endRotated;
        }

        private bool IsRopeSettingsChanged()
        {
            return !Mathf.Approximately(linePoints, prevLineQuality) ||
                   !Mathf.Approximately(ropeWidth, prevRopeWidth) ||
                   !Mathf.Approximately(stiffness, prevstiffness) ||
                   !Mathf.Approximately(damping, prevDampness) ||
                   !Mathf.Approximately(ropeLength, prevRopeLength) ||
                   !Mathf.Approximately(startTangentForce, prevStartTangent) ||
                   !Mathf.Approximately(endTangentForce, prevEndTangent);
        }

        // ==================================================================================
        // MÉTODOS PÚBLICOS RESTAURADOS (PARA COMPATIBILIDAD CON OTROS SCRIPTS)
        // ==================================================================================

        public void SetStartPoint(Transform newStartPoint, bool instantAssign = false)
        {
            startPoint = newStartPoint;
            prevStartPointPosition = startPoint == null ? Vector3.zero : startPoint.position;
            prevStartPointRotation = startPoint == null ? Quaternion.identity : startPoint.rotation;

            if (instantAssign || newStartPoint == null)
            {
                RecalculateRope();
            }

            NotifyPointsChanged();
        }

        public void SetMidPoint(Transform newMidPoint, bool instantAssign = false)
        {
            midPoint = newMidPoint;
            prevMidPointPosition = midPoint == null ? 0.5f : midPointPosition;
            
            if (instantAssign || newMidPoint == null)
            {
                RecalculateRope();
            }
            NotifyPointsChanged();
        }
        
        public void SetEndPoint(Transform newEndPoint, bool instantAssign = false)
        {
            endPoint = newEndPoint;
            prevEndPointPosition = endPoint == null ? Vector3.zero : endPoint.position;
            prevEndPointRotation = endPoint == null ? Quaternion.identity : endPoint.rotation;

            if (instantAssign || newEndPoint == null)
            {
                RecalculateRope();
            }

            NotifyPointsChanged();
        }

        public void RecalculateRope()
        {
            if (!AreEndPointsValid())
            {
                lineRenderer.positionCount = 0;
                return;
            }

            currentValue = GetMidPoint();
            targetValue = currentValue;
            currentVelocity = Vector3.zero;
            SetSplinePoint();
        }
    }
}