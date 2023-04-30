﻿namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
internal sealed record CulturalComparisonTestResult
{
    private static readonly CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

    public CultureInfo Culture { get; private init; }
    public Int32 CaseSensitive { get; private init; }
    public Int32 CaseInsensitive { get; private init; }

    private CulturalComparisonTestResult(CultureInfo culture)
    {
        this.Culture = culture;
    }

    public static CulturalComparisonTestResult Compare(String strA, String strB)
    {
        CultureInfo culture = cultures[Random.Shared.Next(0, cultures.Length)];
        return Compare(culture, strA, strB);
    }
    public static CulturalComparisonTestResult Compare(CultureInfo culture, String strA, String strB)
    {
        Int32 caseInsensitive = String.Compare(strA, strB, true, culture);
        Int32 caseSensitive = String.Compare(strA, strB, false, culture);
        return new(culture)
        {
            CaseInsensitive = caseInsensitive,
            CaseSensitive = caseSensitive,
        };
    }
}
