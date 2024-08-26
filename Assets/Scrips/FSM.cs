using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    IDLE = 0,
    ROTATING = 1, //El codigo esta un poco mal ya que pude resolver como quiero la rotacion, pero al momento de querer implenetar el movimiento entre dos puntos y vuleva a pasar al estado de rotacion evade el regreso
    WAITING = 2,  // se como soluciuonarlo pero mejor quiero preguntar como funciona bien el sistema de listas para que el codigo se sienta mas entendible, entre otras dudas.
    RETURNING = 3,
    WALKING = 4 
}

public class FSM : MonoBehaviour
{
    protected States myState;

    [Header("Configuración de Rotación")]
    public float idleTime = 1.0f;
    public float rotationDuration = 2.0f;
    public float rotationAngle = 90.0f;
    public float waitTimeAfterRotation = 1.0f;
    public float returnDuration = 2.0f;

    [Header("Configuración de Movimiento")]
    public Transform pointA; // Punto A
    public Transform pointB; // Punto B
    public float walkSpeed = 2.0f; // Velocidad de movimiento
    public float waitTimeAtPoints = 1.0f; // Tiempo de espera en cada punto

    private float stateStartTime;
    private float initialRotation;
    private float targetRotation;
    private bool isReturning;
    private bool movingToPointB;

    private void Start()
    {
        myState = States.IDLE;
        stateStartTime = Time.time;
        initialRotation = transform.rotation.eulerAngles.y;
        targetRotation = initialRotation;
    }

    private void Update()
    {
        switch (myState)
        {
            case States.IDLE:
                HandleIdle();
                break;
            case States.ROTATING:
                HandleRotating();
                break;
            case States.WAITING:
                HandleWaiting();
                break;
            case States.RETURNING:
                HandleReturning();
                break;
            case States.WALKING:
                HandleWalking();
                break;
        }
    }

    private void HandleIdle()
    {
        if (Time.time - stateStartTime >= idleTime)
        {
            targetRotation = initialRotation + rotationAngle;
            ChangeState(States.ROTATING);
        }
    }

    private void HandleRotating()
    {
        float rotationProgress = Mathf.Clamp01((Time.time - stateStartTime) / rotationDuration);
        transform.rotation = Quaternion.Euler(0, Mathf.Lerp(initialRotation, targetRotation, rotationProgress), 0);

        if (rotationProgress >= 1.0f)
        {
            ChangeState(States.WAITING);
        }
    }

    private void HandleWaiting()
    {
        if (Time.time - stateStartTime >= waitTimeAfterRotation)
        {
            ChangeState(States.WALKING);
        }
    }

    private void HandleReturning()
    {
        float returnProgress = Mathf.Clamp01((Time.time - stateStartTime) / returnDuration);
        transform.rotation = Quaternion.Euler(0, Mathf.Lerp(targetRotation, initialRotation, returnProgress), 0);

        if (returnProgress >= 1.0f)
        {
            ChangeState(States.IDLE);
        }
    }

    private void HandleWalking()
    {
        Transform targetPoint = movingToPointB ? pointB : pointA;
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPoint.position);

        // Mover el objeto hacia el punto de destino
        transform.position += direction * walkSpeed * Time.deltaTime;

        // Si ha llegado al punto, esperar un momento y cambiar de dirección
        if (distance <= 0.1f)
        {
            if (Time.time - stateStartTime >= waitTimeAtPoints)
            {
                movingToPointB = !movingToPointB; // Cambiar el destino
                stateStartTime = Time.time; // Reiniciar el temporizador de espera
            }
        }
    }

    private void ChangeState(States newState)
    {
        myState = newState;
        stateStartTime = Time.time;

        if (newState == States.IDLE)
        {
            transform.rotation = Quaternion.Euler(0, initialRotation, 0);
        }
        else if (newState == States.WALKING)
        {
            movingToPointB = true; // Comenzar movimiento hacia el punto B
        }
    }
}

//El codigo esta un poco mal ya que pude resolver como quiero la rotacion, pero al momento de querer implenetar el movimiento entre dos puntos y vuleva a pasar al estado de rotacion evade el regreso
// se como soluciuonarlo pero mejor quiero preguntar como funciona bien el sistema de listas para que el codigo se sienta mas entendible.