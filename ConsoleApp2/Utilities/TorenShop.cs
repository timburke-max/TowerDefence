using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefence.Utilities;

internal class TorenShop
{
    private static int basicStock = 0;
    private static int advancedStock = 0;
    private static bool initialized = false;

    public static void SaveStock(int punten)
    {
        basicStock = punten / 100;
        advancedStock = Math.Max(0, (punten - 50) / 150);
        initialized = true;
    }

    public static void SetStock(int basic, int advanced)
    {
        basicStock = Math.Max(0, basic);
        advancedStock = Math.Max(0, advanced);
        initialized = true;
    }

    public static bool HasStock(TorenPlacement.TorenType type)
    {
        return GetStock(type) > 0;
    }

    public static void DecrementStock(TorenPlacement.TorenType type)
    {
        if (type == TorenPlacement.TorenType.Basic && basicStock > 0) basicStock--;
        if (type == TorenPlacement.TorenType.Advanced && advancedStock > 0) advancedStock--;
    }

    public static void AddStock(TorenPlacement.TorenType type, int amount)
    {
        if (amount <= 0) return;
        if (type == TorenPlacement.TorenType.Basic) basicStock += amount;
        else advancedStock += amount;
    }

    public static int GetStock(TorenPlacement.TorenType type)
    {
        return type == TorenPlacement.TorenType.Basic ? basicStock : advancedStock;
    }

    public static bool IsInitialized()
    {
        return initialized;
    }
}
