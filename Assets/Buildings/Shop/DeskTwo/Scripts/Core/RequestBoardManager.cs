using System;
using System.Collections.Generic;
using UnityEngine;

public class RequestBoardManager : MonoBehaviour
{
    [SerializeField] private List<ShopRequestData> activeRequests = new();

    public event Action OnRequestsChanged;

    public List<ShopRequestData> GetActiveRequests()
    {
        return new List<ShopRequestData>(activeRequests);
    }

    public bool HasRequest(ShopRequestData request)
    {
        return request != null && activeRequests.Contains(request);
    }

    public bool TryAddRequest(ShopRequestData request)
    {
        if (request == null)
            return false;

        if (activeRequests.Contains(request))
            return false;

        activeRequests.Add(request);
        OnRequestsChanged?.Invoke();

        Debug.Log($"Request added: {request.requestTitle}");
        return true;
    }

    public void RemoveRequest(ShopRequestData request)
    {
        if (request == null)
            return;

        if (activeRequests.Remove(request))
        {
            OnRequestsChanged?.Invoke();
            Debug.Log($"Request removed: {request.requestTitle}");
        }
    }
}