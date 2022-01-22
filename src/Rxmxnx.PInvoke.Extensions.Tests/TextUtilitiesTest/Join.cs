﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.TextUtilitiesTest
{
    [ExcludeFromCodeCoverage]
    public sealed class JoinAsyncTest
    {
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        internal async Task LocalEmptyTest(Boolean withSeperator, Boolean emptyData)
        {
            Byte separatorByte = withSeperator ? TestUtilities.GetPrintableByte() : default;
            CString separatorCString = withSeperator ? TestUtilities.SharedFixture.Create<CString>() : default;
            IEnumerable<CString> values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : default;

            await EmptyTest(separatorByte, separatorCString, values);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        internal async Task ReferenceEmptyTest(Boolean withSeperator, Boolean emptyData)
        {
            Byte separatorByte = withSeperator ? TestUtilities.GetPrintableByte() : default;
            CString separatorCString = withSeperator ? TestUtilities.SharedFixture.Create<CString>() : default;
            IEnumerable<CString> values = !emptyData ? Enumerable.Repeat(new CString(IntPtr.Zero, default), 3).ToArray() : default;

            await EmptyTest(separatorByte, separatorCString, values);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal async Task LocalNormalTest(Boolean withSeperator)
        {
            Byte separatorByte = withSeperator ? TestUtilities.GetPrintableByte() : default;
            String separatorString = withSeperator ? TestUtilities.SharedFixture.Create<String>() : default;
            CString separatorCString = separatorString;
            String[] strValue = TestUtilities.SharedFixture.Create<String[]>();
            CString[] values = strValue.Select(x => (CString)x).ToArray();
            await NormalTest(separatorByte, separatorString, separatorCString, strValue, values);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal async Task ReferenceNormalTest(Boolean withSeperator)
        {
            Byte separatorByte = withSeperator ? TestUtilities.GetPrintableByte() : default;
            String separatorString = withSeperator ? TestUtilities.SharedFixture.Create<String>() : default;
            CString separatorCString = separatorString;
            String[] strValue = TestUtilities.SharedFixture.Create<String[]>();
            Byte[][] bytes = strValue.Select(x => Encoding.UTF8.GetBytes(x)).ToArray();
            GCHandle[] handles = TestUtilities.Alloc(bytes);
            try
            {
                CString[] values = TestUtilities.CreateCStrings(bytes, handles);
                await NormalTest(separatorByte, separatorString, separatorCString, strValue, values);
            }
            finally
            {
                TestUtilities.Free(handles);
            }
        }

        private static async Task NormalTest(Byte separatorByte, String separatorString, CString separatorCString, String[] strValue, CString[] values)
        {
            String expectedByte = String.Join(Encoding.UTF8.GetString(new[] { separatorByte }), strValue);
            String expectedCString = String.Join(separatorString, strValue);
            Byte[] expectedResultByte = Encoding.UTF8.GetBytes(expectedByte);
            Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

            CString resultByte = await TextUtilities.JoinAsync(separatorByte, values);
            CString resultCString = await TextUtilities.JoinAsync(separatorCString, values);
            String resultCStringByte = Encoding.UTF8.GetString(CString.GetBytes(resultByte)[0..^1]);
            String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

            Assert.Equal(expectedByte, resultCStringByte);
            Assert.Equal(expectedCString, resultCStringCString);
            Assert.Equal(expectedResultByte, CString.GetBytes(resultByte)[0..^1]);
            Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
        }

        private static async Task EmptyTest(byte separatorByte, CString separatorCString, IEnumerable<CString> values)
        {
            CString resultByte = await TextUtilities.JoinAsync(separatorByte, values);
            CString resultCString = await TextUtilities.JoinAsync(separatorCString, values);

            Assert.Null(resultByte);
            Assert.Null(resultCString);
        }
    }
}
