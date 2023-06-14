using System.Collections;
using System.ComponentModel;

using UnityEngine;

public class ChemicalStand : ItemHolder
{
    protected override void Awake()
    {
        base.Awake();

        successConditions.Add(_ => InventoryManager.instance?.currentItem is Item_Chemical);
    }
}
