using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WeldingTestsQA
{
    // Test 1:  que no haya objetos nulos o faltantes
    [Test]
    public void SceneHasNoMissingScripts()
    {
        foreach (var go in GameObject.FindObjectsOfType<GameObject>())
        {
            var components = go.GetComponents<Component>();
            foreach (var c in components)
            {
                Assert.IsNotNull(c, $"Objeto {go.name} tiene un componente faltante.");
            }
        }
    }

    // Test 2:  configuración de FPS mínimo
    [UnityTest]
    public IEnumerator AverageFPS_IsAbove60()
    {
        float elapsed = 0f;
        int frames = 0;

        while (elapsed < 2f) // Prueba rápida de 2 segundos
        {
            yield return null;
            elapsed += Time.deltaTime;
            frames++;
        }

        float averageFPS = frames / elapsed;
        Assert.GreaterOrEqual(averageFPS, 60f, $"FPS promedio demasiado bajo: {averageFPS}");
    }

    // Test 3:   de efectos visuales
    [Test]
    public void WeldingEffects_AreAssigned()
    {
        var weldingEffects = GameObject.FindObjectOfType<WeldingGunOnActivate>();
        if (weldingEffects != null)
        {
            Assert.IsNotNull(weldingEffects.sparkEffect, "Efecto de chispas no asignado.");
            Assert.IsNotNull(weldingEffects.fireEffect, "Efecto de fuego no asignado.");
            Assert.IsNotNull(weldingEffects.spawnPoint, "Spawn point no asignado.");
        }
        else
        {
            Assert.Ignore("No hay WeldingGunOnActivate en la escena, ignora este test.");
        }
    }

    // Test 4:  compatibilidad VR (cámara)
    [Test]
    public void CameraIsSetupForVR()
    {
        var cam = Camera.main;
        Assert.IsNotNull(cam, "No hay cámara principal en la escena.");
        Assert.IsTrue(cam.stereoTargetEye != StereoTargetEyeMask.None, "La cámara no está configurada para VR.");
    }
}
