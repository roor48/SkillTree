[System.Serializable]
public class BigNumber
{
    [UnityEngine.SerializeField] private double mantissa;  // 가수 (1.0 ~ 9.999)
    [UnityEngine.SerializeField] private long exponent;    // 지수

    public BigNumber()
    {
        mantissa = 0.0;
        exponent = 0;
    }
    public BigNumber(double value)
    {
        mantissa = value;
        exponent = 0;
        Normalize();
    }
    public BigNumber(double mantissa, long exponent)
    {
        this.mantissa = mantissa;
        this.exponent = exponent;
        Normalize();
    }

    private void Normalize()
    {
        if (mantissa == 0.0)
        {
            exponent = 0;
            return;
        }

        // mantissa를 1.0~9.999 범위로 조정
        // 예: mantissa=1234567 → log10(1234567) ≈ 6.09 → floor(6.09) = 6
        //     → 10^6으로 나눔 → mantissa=1.234567, exponent+=6
        int adjustment = (int)System.Math.Floor(System.Math.Log10(System.Math.Abs(mantissa)));
        
        if (adjustment != 0)
        {
            mantissa /= System.Math.Pow(10, adjustment);
            exponent += adjustment;
        }
    }
    
    public override string ToString()
    {
        // 1e3 미만은 일반 숫자로 표시
        if (exponent < 3)
        {
            return (mantissa * System.Math.Pow(10, exponent)).ToString("F0");
        }

        // 1e93 미만: 단위 사용
        // mantissa는 이미 1.0~9.999 범위로 정규화됨
        if (exponent < 93)
        {
            // exponent를 3의 배수로 조정
            long exponentRemainder = exponent % 3;
            long exponentBase = exponent - exponentRemainder;
            
            NumberUnit unit = GetUnit(exponentBase);
            double displayValue = mantissa * System.Math.Pow(10, exponentRemainder);
            return $"{displayValue:F2}{GetUnitSymbol(unit)}";
        }

        // 1e93 이상: e표기법, 지수를 단위로 표현
        // 예: mantissa=5.5, exponent=12345 → 5.5e12.34K
        return $"{mantissa:F2}e{FormatExponent(exponent)}";
    }

    /// <summary>
    /// 지수를 단위로 표현
    /// 예: exp=12345 → 12.34K (12345를 12.34 * 1000으로 표현)
    /// </summary>
    /// <param name="exp">지수</param>
    /// <returns>string</returns>
    private string FormatExponent(long exp)
    {
        // 지수가 1000 미만이면 그냥 숫자로
        if (exp < 1000)
        {
            return exp.ToString();
        }
        
        // exp의 자릿수 계산
        int expDigits = (int)System.Math.Floor(System.Math.Log10(exp));
        
        // expDigits를 3의 배수로 조정
        long unitExponent = expDigits - (expDigits % 3);
        double adjustedExp = exp / System.Math.Pow(10, unitExponent);
        
        NumberUnit unit = GetUnit(unitExponent);
        return $"{adjustedExp:F2}{GetUnitSymbol(unit)}";
    }

    private NumberUnit GetUnit(long exp)
    {
        return (NumberUnit)((exp / 3) - 1);
    }
    private string GetUnitSymbol(NumberUnit unit)
    {
        return unit switch
        {
            NumberUnit.K => "K",
            NumberUnit.M => "M",
            NumberUnit.B => "B",
            NumberUnit.T => "T",
            NumberUnit.Qa => "Qa",
            NumberUnit.Qi => "Qi",
            NumberUnit.Sx => "Sx",
            NumberUnit.Sp => "Sp",
            NumberUnit.Oc => "Oc",
            NumberUnit.No => "No",
            NumberUnit.Dc => "Dc",
            NumberUnit.Udc => "Udc",
            NumberUnit.Ddc => "Ddc",
            NumberUnit.Trdc => "Trdc",
            NumberUnit.Qadc => "Qadc",
            NumberUnit.Qidc => "Qidc",
            NumberUnit.Sxdc => "Sxdc",
            NumberUnit.Spdc => "Spdc",
            NumberUnit.Ocdc => "Ocdc",
            NumberUnit.Nodc => "Nodc",
            NumberUnit.Vg => "Vg",
            NumberUnit.Uvg => "Uvg",
            NumberUnit.Dvg => "Dvg",
            NumberUnit.Tvg => "Tvg",
            NumberUnit.Qavg => "Qavg",
            NumberUnit.Qivg => "Qivg",
            NumberUnit.Sxvg => "Sxvg",
            NumberUnit.Spvg => "Spvg",
            NumberUnit.Ocvg => "Ocvg",
            NumberUnit.Novg => "Novg",
            _ => ""
        };
    }

    public static BigNumber operator+(BigNumber a, BigNumber b)
    {
        if (System.Math.Abs(a.exponent - b.exponent) > 15)
            return a.exponent > b.exponent ? a : b;

        if (a.exponent == b.exponent)
            return new BigNumber(a.mantissa + b.mantissa, a.exponent);

        long expDiff = System.Math.Abs(a.exponent - b.exponent);
        double adjustedB = b.mantissa * System.Math.Pow(10, -expDiff);
        return new BigNumber(a.mantissa + adjustedB, a.exponent);
    }
    public static BigNumber operator+(BigNumber a, double scalar)
    {
        BigNumber scalarBig = new BigNumber(scalar);
        return a + scalarBig;
    }
    
    public static BigNumber operator-(BigNumber a, BigNumber b)
    {
        if (System.Math.Abs(a.exponent - b.exponent) > 15)
            return a.exponent > b.exponent ? a : new BigNumber(-b.mantissa, b.exponent);

        if (a.exponent == b.exponent)
            return new BigNumber(a.mantissa - b.mantissa, a.exponent);

        long expDiff = System.Math.Abs(a.exponent - b.exponent);
        double adjustedB = b.mantissa * System.Math.Pow(10, -expDiff);
        return new BigNumber(a.mantissa - adjustedB, a.exponent);
    }

    public static BigNumber operator*(BigNumber a, BigNumber b)
    {
        return new BigNumber(a.mantissa * b.mantissa, a.exponent + b.exponent);
    }
    public static BigNumber operator*(BigNumber a, double scalar)
    {
        return new BigNumber(a.mantissa * scalar, a.exponent);
    }

    public static bool operator>(BigNumber a, BigNumber b)
    {
        if (a.exponent != b.exponent)
            return a.exponent > b.exponent;
        return a.mantissa > b.mantissa;
    }
    public static bool operator>(BigNumber a, double scalar)
    {
        BigNumber bBig = new BigNumber(scalar);
        return a > bBig;
    }

    public static bool operator<(BigNumber a, BigNumber b)
    {
        if (a.exponent != b.exponent)
            return a.exponent < b.exponent;
        return a.mantissa < b.mantissa;
    }
    public static bool operator<(BigNumber a, double scalar)
    {
        BigNumber bBig = new BigNumber(scalar);
        return a < bBig;
    }

    public override bool Equals(object obj)
    {
        if (obj is BigNumber other)
            return this == other;
        return false;
    }

    public override int GetHashCode()
    {
        return mantissa.GetHashCode() ^ exponent.GetHashCode();
    }
}

public enum NumberUnit
{
    K,      //1e3
    M,      //1e6
    B,      //1e9
    T,      //1e12
    Qa,     //1e15
    Qi,     //1e18
    Sx,     //1e21
    Sp,     //1e24
    Oc,     //1e27
    No,     //1e30
    Dc,     //1e33
    Udc,    //1e36
    Ddc,    //1e39
    Trdc,   //1e42
    Qadc,   //1e45
    Qidc,   //1e48
    Sxdc,   //1e51
    Spdc,   //1e54
    Ocdc,   //1e57
    Nodc,   //1e60
    Vg,     //1e63
    Uvg,    //1e66
    Dvg,    //1e69
    Tvg,    //1e72
    Qavg,   //1e75
    Qivg,   //1e78
    Sxvg,   //1e81
    Spvg,   //1e84
    Ocvg,   //1e87
    Novg,   //1e90
}
