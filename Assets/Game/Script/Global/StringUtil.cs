public static class StringUtil
{
    public static string FormatValue(this float value)
    {
        return $"{value:#,##0.00}";
    }

    public static string FormatValue(this int value)
    {
        return $"{value:#,##0}";
    }

    public static string FormatValue(this long value)
    {
        return $"{value:#,##0}";
    }

    public static string FormatValueK(this long value)
    {
        if (value < 10000)
        {
            return value.FormatValue();
        }

        if (value < 1000000)
        {
            return (value / 1000f).FormatValue() + "K";
        }

        if (value < 1000000000)
        {
            return (value / 1000000f).FormatValue() + "M";
        }

        return (value / 1000000000f).FormatValue() + "B";
    }

    public static string FormatValueK(this int value)
    {
        if (value < 10000)
        {
            return value.FormatValue();
        }

        if (value < 1000000)
        {
            return (value / 1000f).FormatValue() + "K";
        }

        if (value < 1000000000)
        {
            return (value / 1000000f).FormatValue() + "M";
        }

        return (value / 1000000000f).FormatValue() + "B";
    }
}