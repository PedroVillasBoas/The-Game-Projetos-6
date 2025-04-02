using UnityEngine;
using GoodVillageGames.Game.Interfaces;
using System.Collections;

public class MissileReloadHandler : MonoBehaviour, IReloadHandler
{
    public float AttackSpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public Coroutine ReloadCoroutine { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool IsReloading { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void CancelReload()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator Reload()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
