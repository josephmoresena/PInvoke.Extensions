﻿namespace Rxmxnx.PInvoke;

public partial class CString : IComparable, IComparable<String>, IComparable<CString>
{
    /// <inheritdoc/>
    public Int32 CompareTo(Object? other)
        => other is null ? 1 : other is String str ? this.CompareTo(str) : other is CString cstr ? this.CompareTo(cstr) :
        throw new ArgumentException("Object must be of type CString.", nameof(other));

    /// <inheritdoc/>
    public Int32 CompareTo(String? other) => other is not String otherNotNull ? 1 : TextCompare(this, otherNotNull);

    /// <inheritdoc/>
    public Int32 CompareTo(CString? other) => other is not CString otherNotNull ? 1 : TextCompare(this, otherNotNull);

    /// <summary>
    /// Compares two specified <see cref="CString"/> objects using the specified rules, and returns an integer that indicates their
    /// relative position in the sort order.
    /// </summary>
    /// <param name="cstrA">The first <see cref="CString"/> to compare.</param>
    /// <param name="cstrB">The second <see cref="CString"/> to compare.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term><description>Condition</description>
    /// </listheader>
    /// <item>
    /// <term>Less than zero</term>
    /// <description><paramref name="cstrA"/> precedes <paramref name="cstrB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Zero</term>
    /// <description><paramref name="cstrA"/> is in the same position as <paramref name="cstrB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="cstrA"/> follows <paramref name="cstrB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    public static Int32 Compare(CString? cstrA, CString? cstrB)
        => cstrA is null && cstrA is null ? 0 : cstrA is null ? 1 : cstrA is null ? -1 : TextCompare(cstrA, cstrA);

    /// <summary>
    /// Compares two specified <see cref="CString"/> objects using the specified rules, and returns an integer that indicates their
    /// relative position in the sort order.
    /// </summary>
    /// <param name="cstrA">The first <see cref="CString"/> to compare.</param>
    /// <param name="cstrB">The second <see cref="CString"/> to compare.</param>
    /// <param name="ignoreCase">
    /// <see langword="true"/> to ignore case during the comparision; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>
    /// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term><description>Condition</description>
    /// </listheader>
    /// <item>
    /// <term>Less than zero</term>
    /// <description><paramref name="cstrA"/> precedes <paramref name="cstrB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Zero</term>
    /// <description><paramref name="cstrA"/> is in the same position as <paramref name="cstrB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="cstrA"/> follows <paramref name="cstrB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    public static Int32 Compare(CString? cstrA, CString? cstrB, Boolean ignoreCase)
        => cstrA is null && cstrB is null ? 0 : cstrA is null ? 1 : cstrB is null ? -1 :
        TextCompare(cstrA, cstrB, ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);

    /// <summary>
    /// Compares two specified <see cref="CString"/> objects using the specified rules, and returns an integer that indicates their
    /// relative position in the sort order.
    /// </summary>
    /// <param name="cstrA">The first <see cref="CString"/> to compare.</param>
    /// <param name="cstrB">The second <see cref="CString"/> to compare.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term><description>Condition</description>
    /// </listheader>
    /// <item>
    /// <term>Less than zero</term>
    /// <description><paramref name="cstrA"/> precedes <paramref name="cstrB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Zero</term>
    /// <description><paramref name="cstrA"/> is in the same position as <paramref name="cstrB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="cstrA"/> follows <paramref name="cstrB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    public static Int32 Compare(CString? cstrA, CString? cstrB, StringComparison comparisonType)
        => cstrA is null && cstrB is null ? 0 : cstrA is null ? 1 : cstrB is null ? -1 : TextCompare(cstrA, cstrB, comparisonType);

    /// <summary>
    /// Compares two specified texts using the specified rules, and returns an integer that indicates their
    /// relative position in the sort order.
    /// </summary>
    /// <param name="textA">The first text to compare.</param>
    /// <param name="textB">The second text to compare.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term><description>Condition</description>
    /// </listheader>
    /// <item>
    /// <term>Less than zero</term>
    /// <description><paramref name="textA"/> precedes <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Zero</term>
    /// <description><paramref name="textA"/> is in the same position as <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="textA"/> follows <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    public static Int32 Compare(CString? textA, String? textB)
        => textA is null && textB is null ? 0 : textA is null ? 1 : textB is null ? -1 : TextCompare(textA, textB);

    /// <summary>
    /// Compares two specified texts using the specified rules, and returns an integer that indicates their
    /// relative position in the sort order.
    /// </summary>
    /// <param name="textA">The first text to compare.</param>
    /// <param name="textB">The second text to compare.</param>
    /// <param name="ignoreCase">
    /// <see langword="true"/> to ignore case during the comparision; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>
    /// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term><description>Condition</description>
    /// </listheader>
    /// <item>
    /// <term>Less than zero</term>
    /// <description><paramref name="textA"/> precedes <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Zero</term>
    /// <description><paramref name="textA"/> is in the same position as <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="textA"/> follows <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    public static Int32 Compare(CString? textA, String? textB, Boolean ignoreCase)
        => textA is null && textB is null ? 0 : textA is null ? 1 : textB is null ? -1 :
        TextCompare(textA, textB, ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);

    /// <summary>
    /// Compares two specified texts using the specified rules, and returns an integer that indicates their
    /// relative position in the sort order.
    /// </summary>
    /// <param name="textA">The first text to compare.</param>
    /// <param name="textB">The second text to compare.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term><description>Condition</description>
    /// </listheader>
    /// <item>
    /// <term>Less than zero</term>
    /// <description><paramref name="textA"/> precedes <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Zero</term>
    /// <description><paramref name="textA"/> is in the same position as <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="textA"/> follows <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    public static Int32 Compare(CString? textA, String? textB, StringComparison comparisonType)
        => textA is null && textB is null ? 0 : textA is null ? 1 : textB is null ? -1 : TextCompare(textA, textB, comparisonType);
}

