////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
//	RTSCameraController.cs
//
//	Thank you for purchasing this package. For any questions, please contact me at:
//	
//	packapunchgameassets@gmail.com
// 
// 
//
//
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//----------------------------------------------------------------------------------------------------------------------
// INCLUDES
using UnityEngine;

//----------------------------------------------------------------------------------------------------------------------
// CLASS RTSCameraController

public class RTSCameraController : MonoBehaviour
{
	//------------------------------------------------------------------------------------------------------------------
	// CONSTS / DEFINES
	private const string DEBUG_LOG_PREFIX = "[RTSCameraController] ";

	// DEFAULTS
	private const float REGISTER_DRAG_DOWN_TIME = 0.15f;
	private const float CAMERA_DRAG_SPEED = 2.0f;
	private const float CAMERA_INTERIA_SLOW_DOWN = 2.0f;
	private const float CAMERA_ZOOM_SCROLLWHEEL_SENSITIVITY = 12.0f;
	private const float CAMERA_ZOOM_SPEED = 0.05f;
	private const float CAMERA_MIN_FOV = 20.0f;
	private const float CAMERA_MAX_FOV = 60.0f;
	private const float PINCH_TURN_RATIO = Mathf.PI / 2;
	private const float MIN_TURN_ANGLE = 0;
	private const float PINCH_RATIO = 1;
	private const float CAMERA_ROTATE_SPEED = 35.0f;

	//----------------------------------------------------------------------------------------------------------------------
	// SINGLETON
	private static RTSCameraController _instance;

	//----------------------------------------------------------------------------------------------------------------------
	// CLASSES / STRUCTS
	[System.Serializable]
	public struct CameraBounds
	{
		public float _minX;
		public float _maxX;
		public float _minZ;
		public float _maxZ;
	}

	//----------------------------------------------------------------------------------------------------------------------
	// FIELDS
	[SerializeField] private Transform _cameraRig;
	[SerializeField] private bool _allowZoom;
	[SerializeField] private bool _allowRotate;
	[Header( "Settings" )]
	[SerializeField] private CameraBounds _bounds;
	[Header("Drag")]
	[SerializeField] private float _registerDragDownTime = REGISTER_DRAG_DOWN_TIME;
	[SerializeField] private float _dragSpeed = CAMERA_DRAG_SPEED;
	[SerializeField] private float _ineriaSlowDown = CAMERA_INTERIA_SLOW_DOWN;
	[Header( "Zoom" )]
	[SerializeField] private float _zoomScrollWheelSensitivity = CAMERA_ZOOM_SCROLLWHEEL_SENSITIVITY;
	[SerializeField] private float _zoomSpeed = CAMERA_ZOOM_SPEED;
	[SerializeField] private float _minFov = CAMERA_MIN_FOV;
	[SerializeField] private float _maxFov = CAMERA_MAX_FOV;
	[Header( "Rotate" )]
	[SerializeField] private float _pinchTurnRatio = PINCH_RATIO;
	[SerializeField] private float _pinchMinTurnAngle = MIN_TURN_ANGLE;
	[SerializeField] private float _rotateSpeed = CAMERA_ROTATE_SPEED;
	[SerializeField] private KeyCode _rotateLeftKey;
	[SerializeField] private KeyCode _rotateRightKey;

	private Vector3 _lastMouseGroundPlanePosition;
	private Vector3 _mouseDownPosition;
	private Vector3 _dragMoveDiff;
	private float _deltaMagnitudeDiff;
	private float _cameraInteriaTime;
	private float _registerDragTimer;
	private float _fov;


	private float _turnAngleDelta;
	private float _turnAngle;

	private static Camera _camera;
	private static bool _isDragging;
	private static bool _isZooming;
	private static bool _isAutoMoving;
	private static Vector3 _autoMovePosition;
	private static Vector3 _cameraRigOrigin;
	private static float _lerpDelta;
	private static bool _isOrthographicCamera;

	//----------------------------------------------------------------------------------------------------------------------
	// PROPERTIES
	public static bool _pIsDragging => _isDragging;
	public static bool _pIsZooming => _isZooming;

	//----------------------------------------------------------------------------------------------------------------------
	// ABSTRACTS / VIRTUALS

	//----------------------------------------------------------------------------------------------------------------------
	// EVENTS / DELEGATES

	//----------------------------------------------------------------------------------------------------------------------
	// FUNCTIONS

	//----------------------------------------------------------------------------------------------------------------------
	// ** STATIC

	//----------------------------------------------------------------------------------------------------------------------
	// ** UNITY
	private void Awake()
	{
		if( _instance != null )
		{
			Debug.LogError( $"{DEBUG_LOG_PREFIX} RTSCameraController instance already exists in the scene! Destroying this object." );
			Destroy( gameObject );
			return;
		}
		
		_instance = this;
		_camera = Camera.main;

		if( _camera == null )
		{
			Debug.LogError( $"{DEBUG_LOG_PREFIX} Main Camera not found. Please set Scene Camera's Tag to use 'MainCamera'" );
			return;

		}
		_fov = _maxFov;
	}

	//------------------------------------------------------------------------------------------------------------------
	private void OnDestroy()
	{
		_instance = null;
	}

	//------------------------------------------------------------------------------------------------------------------
	private void LateUpdate()
	{
		#if UNITY_EDITOR || UNITY_STANDALONE
		UpdateMouseInput();
		#else
		UpdateTouchInput();
		#endif

		if( _dragMoveDiff != Vector3.zero && !_isDragging )
		{
			_dragMoveDiff = Vector3.Lerp( _dragMoveDiff, Vector3.zero, _ineriaSlowDown * Time.deltaTime );
			_cameraRig.Translate( _dragMoveDiff * _dragSpeed, Space.World );
		}

		ClampCameraPosition();
	}

	//----------------------------------------------------------------------------------------------------------------------
	// ** PUBLIC

	//----------------------------------------------------------------------------------------------------------------------
	// ** PROTECTED

	//----------------------------------------------------------------------------------------------------------------------
	// ** PRIVATE
	private void UpdateMouseInput()
	{
		if( Input.GetMouseButtonUp( 0 ) )
		{
			_isDragging = false;
		}

		if( Input.GetMouseButtonDown( 0 ) )
		{
			_lastMouseGroundPlanePosition = _mouseDownPosition = MouseToGroundPlane( Input.mousePosition );
			_dragMoveDiff = Vector3.zero;
			_cameraInteriaTime = 0.0f;
			_registerDragTimer = 0.0f;
		}

		if( Input.GetMouseButton( 0 ) )
		{
			UpdateCameraDrag( Input.mousePosition );
		}

		UpdateCameraRotateKeyboard();
		UpdateZoom();
	}

	//----------------------------------------------------------------------------------------------------------------------
	private void UpdateTouchInput()
	{
		if( Input.touchCount == 1 )
		{
			Touch touch1 = Input.touches[0];

			if( touch1.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled )
			{
				_isDragging = false;
			}

			if( touch1.phase == TouchPhase.Began )
			{
				_lastMouseGroundPlanePosition = _mouseDownPosition = MouseToGroundPlane( touch1.position );
				_dragMoveDiff = Vector3.zero;
				_cameraInteriaTime = 0.0f;
				_registerDragTimer = 0.0f;
			}

			if( touch1.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Stationary )
			{
				UpdateCameraDrag( touch1.position );
			}
		}
		else
		{
			UpdateCameraRotateTouch();
			UpdateZoom();
		}
	}

	//----------------------------------------------------------------------------------------------------------------------
	private Vector3 MouseToGroundPlane( Vector3 inputPosition )
	{
		if( _camera == null )
			return Vector3.zero;

		Ray mouseRay = _camera.ScreenPointToRay( inputPosition );

		if( mouseRay.direction.y >= 0 )
		{
			return Vector3.zero;
		}

		float rayLength = ( mouseRay.origin.y / mouseRay.direction.y );
		return mouseRay.origin - ( mouseRay.direction * rayLength );
	}

	//----------------------------------------------------------------------------------------------------------------------
	private void UpdateCameraDrag( Vector3 inputPosition )
	{
		_registerDragTimer += Time.deltaTime;
		_isDragging = _registerDragTimer >= _registerDragDownTime;

		if( !_isDragging )
		{
			Vector3 hitPos = _lastMouseGroundPlanePosition = MouseToGroundPlane( inputPosition );

			if( ( _mouseDownPosition - hitPos ).sqrMagnitude >= 10.0f )
			{
				_registerDragTimer = _registerDragDownTime;
			}
		}

		if( _isDragging )
		{
			Vector3 hitPos = MouseToGroundPlane( inputPosition );

			Vector3 dir = _lastMouseGroundPlanePosition - hitPos;

			_dragMoveDiff = Vector3.Lerp( _dragMoveDiff, dir, 5 * Time.deltaTime );

			if( _dragMoveDiff != Vector3.zero )
				_cameraRig.Translate( _dragMoveDiff * _dragSpeed, Space.World );

			_lastMouseGroundPlanePosition = hitPos + ( _dragMoveDiff * _dragSpeed * Time.deltaTime );
		}
	}

	//----------------------------------------------------------------------------------------------------------------------
	private void CalculateTouchRotation()
	{
		_turnAngle = _turnAngleDelta = 0;

		if( Input.touchCount == 2 )
		{
			Touch touch1 = Input.touches[0];
			Touch touch2 = Input.touches[1];

			if( touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved )
			{
				_turnAngle = Angle( touch1.position, touch2.position );
				float prevTurn = Angle( touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition );
				_turnAngleDelta = Mathf.DeltaAngle( prevTurn, _turnAngle );

				if( Mathf.Abs( _turnAngleDelta ) > _pinchMinTurnAngle )
				{
					_turnAngleDelta *= PINCH_TURN_RATIO;
				}
				else
				{
					_turnAngle = _turnAngleDelta = 0;
				}
			}
		}
	}

	//----------------------------------------------------------------------------------------------------------------------
	private float Angle( Vector2 pos1, Vector2 pos2 )
	{
		Vector2 from = pos2 - pos1;
		Vector2 to = new Vector2( 1, 0 );
		float result = Vector2.Angle( from, to );
		Vector3 cross = Vector3.Cross( from, to );

		if( cross.z > 0 )
		{
			result = 360f - result;
		}
		return result;
	}

	//----------------------------------------------------------------------------------------------------------------------
	private void UpdateCameraRotateTouch()
	{
		if( !_allowRotate )
			return;

		if( Input.touchCount != 2 )
			return;

		Quaternion desiredRotation = _cameraRig.transform.rotation;

		CalculateTouchRotation();

		if( Mathf.Abs( _turnAngleDelta ) > 0 )
		{
			Vector3 rotationDeg = Vector3.zero;
			rotationDeg.y = -_turnAngleDelta;
			desiredRotation *= Quaternion.Euler( rotationDeg );
			_isDragging = true;
		}

		_cameraRig.transform.rotation = desiredRotation;
	}

	//----------------------------------------------------------------------------------------------------------------------
	private void UpdateCameraRotateKeyboard()
	{
		if( !_allowRotate )
			return;

		if( Input.GetKey( _rotateLeftKey ) )
		{
			_cameraRig.transform.Rotate( Vector3.up * _rotateSpeed * Time.deltaTime );
		}
		else if( Input.GetKey( _rotateRightKey ) )
		{
			_cameraRig.transform.Rotate( Vector3.up * -_rotateSpeed * Time.deltaTime );
		}
	}

	//----------------------------------------------------------------------------------------------------------------------
	private void UpdateZoom()
	{
		if( !_allowZoom )
			return;

		#if UNITY_EDITOR || UNITY_STANDALONE

		_fov += Input.GetAxis( "Mouse ScrollWheel" ) * _zoomScrollWheelSensitivity;

		#else
		if( Input.touchCount == 2 )
		{
			_isZooming = true;

			Touch touchZero = Input.GetTouch( 0 );
			Touch touchOne = Input.GetTouch( 1 );

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevTouchDeltaMag = ( touchZeroPrevPos - touchOnePrevPos ).magnitude;
			float touchDeltaMag = ( touchZero.position - touchOne.position ).magnitude;

			_deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
			_fov += _deltaMagnitudeDiff * _zoomSpeed;
		}
		else if( Input.touchCount == 0 )
		{
			_isZooming = false;
		}
		#endif

		_fov = Mathf.Clamp( _fov, _minFov, _maxFov );
		if( _camera.orthographic )
		{
			_camera.orthographicSize = _fov;
		}
		else
		{
			_camera.fieldOfView = _fov;
		}
	}

	//----------------------------------------------------------------------------------------------------------------------
	private void ClampCameraPosition()
	{
		Vector3 mousePositionThisFrame = Input.mousePosition;
				
		_cameraRig.position = new Vector3(
			Mathf.Clamp( _cameraRig.position.x, _bounds._minX, _bounds._maxX ),
			_cameraRig.position.y,
			Mathf.Clamp( _cameraRig.position.z, _bounds._minZ, _bounds._maxZ ) );

		if( _cameraRig.position.z >= _bounds._maxZ || _cameraRig.position.z <= _bounds._minZ )
		{
			_lastMouseGroundPlanePosition = MouseToGroundPlane( mousePositionThisFrame );
			_mouseDownPosition = MouseToGroundPlane( mousePositionThisFrame );
		}

		if( _cameraRig.position.x >= _bounds._minX || _cameraRig.position.x <= _bounds._maxX )
		{
			_lastMouseGroundPlanePosition = MouseToGroundPlane( mousePositionThisFrame );
			_mouseDownPosition = MouseToGroundPlane( mousePositionThisFrame );
		}
	}

	//----------------------------------------------------------------------------------------------------------------------
	// INTERFACE
}

