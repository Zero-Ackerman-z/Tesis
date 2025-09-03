using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WeldingTests
{
    // Configura la pistola para las pruebas
    private PistolaSphereCastMerge SetupWeldingGun()
    {
        var go = new GameObject("WeldingGunTest");
        var gun = go.AddComponent<PistolaSphereCastMerge>();
        return gun;
    }

    // Configura los efectos de soldadura
    private WeldingGunOnActivate SetupWeldingEffects()
    {
        var go = new GameObject("WeldingEffectsTest");
        var effects = go.AddComponent<WeldingGunOnActivate>();

        effects.sparkEffect = new GameObject("SparkEffect").AddComponent<ParticleSystem>();
        effects.fireEffect = new GameObject("FireEffect").AddComponent<ParticleSystem>();
        effects.spawnPoint = effects.transform;

        go.AddComponent<XRGrabInteractable>();

        return effects;
    }

    // Test 1: Soldadura activa o no
    [Test]
    public void IsWelding_ReturnsCorrectState()
    {
        var gun = SetupWeldingGun();

        gun.Press = true;
        Assert.IsTrue(gun.IsWelding(), "Debe estar soldando cuando Press es true."); // Assert 1

        gun.Press = false;
        Assert.IsFalse(gun.IsWelding(), "No debe estar soldando cuando Press es false."); // Assert 2

        Assert.IsNotNull(gun, "El objeto de la pistola no debe ser nulo."); // Assert 3

        Object.DestroyImmediate(gun.gameObject);
    }

    // Test 2: Validar voltaje
    [Test]
    public void GetVoltage_ReturnsCorrectValue()
    {
        var gun = SetupWeldingGun();
        gun.voltage = 25.5f;

        Assert.AreEqual(25.5f, gun.GetVoltage(), 0.001f, "El voltaje debe ser 25.5f."); // Assert 1
        Assert.Greater(gun.GetVoltage(), 0f, "El voltaje debe ser mayor que 0."); // Assert 2
        Assert.Less(gun.GetVoltage(), 100f, "El voltaje debe ser menor que 100."); // Assert 3

        Object.DestroyImmediate(gun.gameObject);
    }

    // Test 3: Resultado de soldadura
    [Test]
    public void GetWeldingResult_ReturnsCorrectString()
    {
        var gun = SetupWeldingGun();
        gun.weldingResult = "90% excelente";

        Assert.AreEqual("90% excelente", gun.GetWeldingResult(), "El resultado debe coincidir exactamente."); // Assert 1
        Assert.IsTrue(gun.GetWeldingResult().Contains("excelente"), "Debe contener la palabra 'excelente'."); // Assert 2
        Assert.IsNotEmpty(gun.GetWeldingResult(), "El resultado no debe estar vacío."); // Assert 3

        Object.DestroyImmediate(gun.gameObject);
    }

    // Test 4: Activar efectos al iniciar soldadura
    [UnityTest]
    public IEnumerator StartWelding_ActivatesFireEffect()
    {
        var effects = SetupWeldingEffects();

        effects.StartWelding(new ActivateEventArgs());
        yield return null;

        Assert.IsNotNull(effects.currentFire, "El efecto de fuego no debe ser nulo."); // Assert 1
        Assert.IsTrue(effects.currentFire.isPlaying, "El efecto de fuego debe estar activo."); // Assert 2
        Assert.AreEqual(effects.spawnPoint.position, effects.currentFire.transform.position, "El fuego debe instanciarse en el punto de spawn."); // Assert 3

        Object.DestroyImmediate(effects.gameObject);
    }

    // Test 5: Detener efectos de soldadura
    [UnityTest]
    public IEnumerator StopWelding_DeactivatesEffects_Alternative()
    {
        var effects = SetupWeldingEffects();

        // Activa la soldadura
        effects.StartWelding(new ActivateEventArgs());
        yield return null;

        var fireObject = effects.currentFire?.gameObject;

        Assert.IsNotNull(effects.currentFire, "El fuego debe haberse creado.");           // Assert 1
        Assert.IsTrue(effects.currentFire.isPlaying, "El fuego debería estar activo.");   // Asegura que sí empezó

        // Detiene la soldadura
        effects.StopWelding(new DeactivateEventArgs());
        yield return null;

        // Assert 2: El sistema de partículas fue detenido
        Assert.IsFalse(effects.currentFire.isPlaying, "El fuego debe haberse detenido.");

        // Assert 3: El GameObject del fuego aún existe, pero fue marcado para destruirse (opcionalmente inactivo)
        Assert.IsTrue(fireObject != null && fireObject.activeSelf, "El objeto de fuego debe seguir activo antes de destrucción final.");

        Object.DestroyImmediate(effects.gameObject);
    }


}
