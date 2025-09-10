using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

public class ObjectPoolQA_Tests
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
        poolManager.poolSize = 5;
    }

    [Test]
    public void Pool_IsInitializedCorrectly()
    {
        poolManager.Start(); // Inicializar pool manualmente

        Assert.AreEqual(poolManager.poolSize, poolManager.pool.Count, "La cantidad de objetos en el pool no coincide con poolSize.");
        Assert.IsTrue(poolManager.pool.All(o => o != null), "Hay objetos nulos en el pool.");
        Assert.IsTrue(poolManager.pool.All(o => !o.activeInHierarchy), "Todos los objetos deben empezar desactivados.");
    }

    [Test]
    public void GetObject_ActivatesObject()
    {
        poolManager.Start();

        var obj = poolManager.GetObject(Vector3.one, Quaternion.identity, poolGO.transform);

        Assert.IsTrue(obj.activeInHierarchy, "El objeto tomado del pool debe estar activo.");
        Assert.AreEqual(Vector3.one, obj.transform.position, "La posición del objeto no coincide con la solicitada.");
        Assert.AreEqual(poolGO.transform, obj.transform.parent, "El padre del objeto no coincide con el Transform dado.");
    }

    [Test]
    public void ActiveCount_IsCorrect()
    {
        poolManager.Start();

        poolManager.GetObject(Vector3.zero, Quaternion.identity, poolGO.transform);
        poolManager.GetObject(Vector3.one, Quaternion.identity, poolGO.transform);

        Assert.AreEqual(2, poolManager.activeCount, "El contador de objetos activos no es correcto.");
    }

    [TearDown]
    public void Cleanup()
    {
        GameObject.DestroyImmediate(poolGO);
        GameObject.DestroyImmediate(prefab);
    }
}
