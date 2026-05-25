using System;
using System.Collections.Generic;

[Serializable]
public class ExpeditionDestinationData
{
    public string destinationId;
    public string destinationName;
    public string sceneName;
    public List<ExpeditionEntryPointData> entryPoints = new();
}