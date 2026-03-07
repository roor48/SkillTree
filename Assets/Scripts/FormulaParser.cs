using System;

public class FormulaParser
{
    public static ExecuteStrategy GetFormula(int id)
    {
        return id switch
        {
            0 => cps => cps + 1,
            1 => cps => cps * 2,
            2 => cps => cps * 1.2,
            3 => cps => cps.Power(1.2),
            4 => cps => cps + 150,
            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }
    
    public static string GetFormulaString(int id)
    {
        return id switch
        {
            0 => "cash/s + 1",
            1 => "cash/s * 2",
            2 => "cash/s * 1.2",
            3 => "cash/s ^ 1.2",
            4 => "cash/s + 150",
            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }
}
