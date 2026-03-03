using System;

public class FormulaParser
{
    public static ExecuteStrategy GetFormula(int id)
    {
        return id switch
        {
            0 => cps => cps + 1,
            1 => cps => cps * 2,
            2 => cps => (long)(cps * 1.2),
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
            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }
}
