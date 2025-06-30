using System.Collections.Generic;
using UnityEngine;


public class CubeWeldingController : MonoBehaviour
{
    public bool hasAcid = false; // Indica si el cubo ya tiene �cido aplicado
    public bool isWelded = false; // Indica si el cubo ya est� soldado

    public GameObject weldingParticlePrefab; // Prefab de part�culas de soldadura
    public Material weldingMaterial; // Material para el cord
                                     // �n de soldadura
    private List<Vector3> weldPoints = new List<Vector3>(); // Puntos del cord�n de soldadura
    private LineRenderer weldLine; // L�nea que representa el cord�n

    private void Start()
    {
        // Crear el LineRenderer para el cord�n de soldadura
        weldLine = gameObject.AddComponent<LineRenderer>();
        weldLine.startWidth = 0.05f;
        weldLine.endWidth = 0.05f;
        weldLine.material = weldingMaterial;
        weldLine.positionCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si la pistola de soldadura toca el cubo y si el cubo tiene �cido aplicado
        if (!isWelded && hasAcid && other.CompareTag("WeldingGun"))
        {
            Debug.Log("Pistola de soldadura toc� el cubo con �cido.");

            // Buscar otros cubos cercanos con �cido aplicado
            Collider[] nearbyCubes = Physics.OverlapSphere(transform.position, 0.5f);
            foreach (Collider cube in nearbyCubes)
            {
                if (cube.CompareTag("Metal") && cube.gameObject != this.gameObject)
                {
                    CubeWeldingController otherCubeController = cube.GetComponent<CubeWeldingController>();
                    if (otherCubeController != null && otherCubeController.hasAcid && !otherCubeController.isWelded)
                    {
                        Debug.Log("Cubo cercano con �cido encontrado.");

                        // Alinear el cubo con el otro
                        cube.transform.position = transform.position + (cube.transform.position - transform.position).normalized * 0.5f;

                        // Hacer el cubo hijo del otro para soldarlo
                        cube.transform.SetParent(transform);

                        // Desactivar f�sica para que no se separen
                        Rigidbody rb = cube.GetComponent<Rigidbody>();
                        if (rb != null) rb.isKinematic = true;

                        Rigidbody rbThis = GetComponent<Rigidbody>();
                        if (rbThis != null) rbThis.isKinematic = true;

                        // Marcar ambos como soldados
                        isWelded = true;
                        otherCubeController.isWelded = true;

                        // Desactivar la interacci�n
                        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
                        if (grabInteractable != null) grabInteractable.enabled = false;

                        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable otherGrabInteractable = cube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
                        if (otherGrabInteractable != null) otherGrabInteractable.enabled = false;

                        // Agregar los puntos del cord�n de soldadura
                        weldPoints.Add(transform.position);
                        weldPoints.Add(cube.transform.position);

                        // Dibujar el cord�n de soldadura con LineRenderer
                        weldLine.positionCount = weldPoints.Count;
                        weldLine.SetPositions(weldPoints.ToArray());

                        // Crear efecto de part�culas de soldadura
                        if (weldingParticlePrefab != null)
                        {
                            ParticleSystem weldingParticles = Instantiate(weldingParticlePrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                            if (weldingParticles != null)
                            {
                                weldingParticles.Play();
                            }
                            else
                            {
                                Debug.LogError("El prefab de part�culas de soldadura no tiene un componente ParticleSystem.");
                            }
                        }
                        else
                        {
                            Debug.LogError("El prefab de part�culas de soldadura no est� asignado.");
                        }

                        Debug.Log("Cubos soldados con cord�n de soldadura.");
                        break;
                    }
                }
            }
        }
    }

    // M�todo para marcar que el cubo tiene �cido aplicado
    public void ApplyAcid()
    {
        hasAcid = true;
        Debug.Log("�cido aplicado al cubo.");
    }
}