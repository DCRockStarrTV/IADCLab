using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rotacion : MonoBehaviour
{
    public float rotationAngle = 180f; // Ángulo de rotación en grados
    public float waitTime = 3f;         // Tiempo de espera en segundos

    private Quaternion startRotation;
    private Quaternion targetRotation;

    void Start()
    {
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, rotationAngle, 0));
        StartCoroutine(RotateRoutine());
    }

    IEnumerator RotateRoutine()
    {
        while (true)
        {
            // Rote 180 grados
            yield return RotateOverTime(targetRotation, 2f); // Duración del movimiento de rotación (1 segundo en este caso)
            yield return new WaitForSeconds(waitTime);

            // Rote de vuelta a la posición inicial
            yield return RotateOverTime(startRotation, 4f); // Duración del movimiento de rotación (1 segundo en este caso)
            yield return new WaitForSeconds(waitTime);

            // Actualiza las rotaciones para la próxima iteración
            targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, rotationAngle, 0));
        }
    }

    IEnumerator RotateOverTime(Quaternion endRotation, float duration)
    {
        Quaternion startRotation = transform.rotation;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
    }
}
