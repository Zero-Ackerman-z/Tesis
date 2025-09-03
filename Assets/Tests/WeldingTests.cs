using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WeldingTests
{
    // Función  configurar la pistola para las pruebas.
    private PistolaSphereCastMerge SetupWeldingGun()
    {
        var go = new GameObject("WeldingGunTest");
        var gun = go.AddComponent<PistolaSphereCastMerge>();
        return gun;
    }

    // Función  configurar  efectos .
    private WeldingGunOnActivate SetupWeldingEffects()
    {
        var go = new GameObject("WeldingEffectsTest");
        var effects = go.AddComponent<WeldingGunOnActivate>();

        // Simular las referencias a ParticleSystem y XRGrabInteractable
        effects.sparkEffect = new GameObject("SparkEffect").AddComponent<ParticleSystem>();
        effects.fireEffect = new GameObject("FireEffect").AddComponent<ParticleSystem>();
        effects.spawnPoint = effects.transform;

        go.AddComponent<XRGrabInteractable>();

        return effects;
    }

    // Pruebas unitarias 

    // Test 1: Soldadura está activa
    [Test]
    public void IsWelding_ReturnsCorrectState()
    {
        var gun = SetupWeldingGun();
        gun.Press = true;
        Assert.IsTrue(gun.IsWelding()); // Assert 1: Verifica que sea true cuando el gatillo está presionado.
        gun.Press = false;
        Assert.IsFalse(gun.IsWelding()); // Assert 2: Verifica que sea false cuando el gatillo no está presionado.
        Object.DestroyImmediate(gun.gameObject);
    }

    // Test 2: Probar el valor de voltaje
    [Test]
    public void GetVoltage_ReturnsCorrectValue()
    {
        var gun = SetupWeldingGun();
        gun.voltage = 25.5f;
        Assert.AreEqual(25.5f, gun.GetVoltage(), 0.001f); // Assert 3: Verifica el valor exacto del voltaje.
        Object.DestroyImmediate(gun.gameObject);
    }

    // Test 3: Probar el resultado de la soldadura
    [Test]
    public void GetWeldingResult_ReturnsCorrectString()
    {
        var gun = SetupWeldingGun();
        gun.weldingResult = "90% excelente";
        Assert.AreEqual("90% excelente", gun.GetWeldingResult()); // Assert 4: Verifica que el string sea el esperado.
        Object.DestroyImmediate(gun.gameObject);
    }

    // Test 4: Probar el inicio de la soldadura
    [UnityTest]
    public IEnumerator StartWelding_ActivatesFireEffect()
    {
        var effects = SetupWeldingEffects();
        // Simular el evento de activación del gatillo
        effects.StartWelding(new ActivateEventArgs());

        yield return null; 

        Assert.IsNotNull(effects.currentFire, "El efecto de fuego no se instanció."); // Assert 5: El efecto de fuego debe existir.
        Assert.IsTrue(effects.currentFire.isPlaying, "El efecto de fuego no está reproduciéndose."); // Assert 6: El efecto debe estar activo.

        Object.DestroyImmediate(effects.gameObject);
    }

    // Test 5:  detención de la soldadura
    [UnityTest]
    public IEnumerator StopWelding_DeactivatesEffects()
    {
        var effects = SetupWeldingEffects();
        effects.StartWelding(new ActivateEventArgs());
        yield return null;

        Assert.IsNotNull(effects.currentFire); // Assert 7: El efecto debe existir antes de detener.

        // Simular la desactivación
        effects.StopWelding(new DeactivateEventArgs());

        yield return new WaitForSeconds(effects.currentFire.main.duration + 0.1f); 

        Assert.IsTrue(effects.currentFire == null); // Assert 8: El efecto de fuego debe ser nulo.

        Object.DestroyImmediate(effects.gameObject);
    }
}