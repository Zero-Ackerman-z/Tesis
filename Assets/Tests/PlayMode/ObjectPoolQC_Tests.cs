using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ObjectPoolQC_Tests
{
    private GameObject poolGO;
    private ObjectPoolManager poolManager;
    private GameObject prefab;

    [SetUp]
    public void Setup()
    {
        // Crear prefab de prueba
        prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Crear ObjectPoolManager en escena
        poolGO = new GameObject("PoolManagerTest");
        poolManager = poolGO.AddComponent<ObjectPoolManager>();
        poolManager.prefab = prefab;
        poolManager.poolSize = 3; // pequeño para test de reutilización
        poolManager.Start();
    }

    [Test]
    public void GetObject_ReturnsInactiveObject_WhenPoolAvailable()
    {
        // Tomar el primer objeto
        var obj1 = poolManager.GetObject(Vector3.zero, Quaternion.identity, poolGO.transform);
        var obj2 = poolManager.GetObject(Vector3.one, Quaternion.identity, poolGO.transform);

        // Deben ser diferentes y activos
        Assert.AreNotEqual(obj1, obj2, "Los objetos tomados del pool deberían ser diferentes.");
        Assert.IsTrue(obj1.activeInHierarchy, "El primer objeto debería estar activo.");
        Assert.IsTrue(obj2.activeInHierarchy, "El segundo objeto debería estar activo.");
    }

    [Test]
    public void GetObject_ReusesObjects_WhenPoolExhausted()
    {
        // Tomar más objetos que poolSize
        var obj1 = poolManager.GetObject(Vector3.zero, Quaternion.identity, poolGO.transform);
        var obj2 = poolManager.GetObject(Vector3.one, Quaternion.identity, poolGO.transform);
        var obj3 = poolManager.GetObject(Vector3.up, Quaternion.identity, poolGO.transform);
        var obj4 = poolManager.GetObject(Vector3.forward, Quaternion.identity, poolGO.transform); // debería reutilizar

        // Activos correctos
        Assert.IsTrue(obj4.activeInHierarchy, "El objeto reutilizado debería estar activo.");
        Assert.AreEqual(poolManager.poolSize, poolManager.pool.Count, "El pool no debería crear más objetos de los declarados.");
    }

    [Test]
    public void ActiveCount_IsUpdatedCorrectly()
    {
        poolManager.GetObject(Vector3.zero, Quaternion.identity, poolGO.transform);
        poolManager.GetObject(Vector3.one, Quaternion.identity, poolGO.transform);

        Assert.AreEqual(2, poolManager.activeCount, "El contador de objetos activos debe reflejar correctamente los objetos tomados.");
    }

    [TearDown]
    public void Cleanup()
    {
        GameObject.DestroyImmediate(poolGO);
        GameObject.DestroyImmediate(prefab);
    }
}
