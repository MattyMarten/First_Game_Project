using System;
using System.Collections.Generic;

[Serializable]
public class ExpeditionSessionData
{
    public ExpeditionDestinationData selectedDestination;
    public ExpeditionEntryPointData selectedEntryPoint;
    public List<ExpeditionMemberData> selectedMembers = new();
}