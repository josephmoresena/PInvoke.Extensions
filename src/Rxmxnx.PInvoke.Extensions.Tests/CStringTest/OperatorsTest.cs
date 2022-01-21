﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringTest
{
    [ExcludeFromCodeCoverage]
    public sealed class OperatorsTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void EmptyTest(Boolean empty)
        {
            CString cstr1 = empty ? CString.Empty : default;
            CString cstr2 = empty ? String.Empty : default;
            CString cstr3 = empty ? Encoding.UTF8.GetBytes(String.Empty) : default;
            CString cstr4 = empty ? Array.Empty<Byte>() : default;
            CString cstr5 = new(empty ? TestUtilities.GetPrintableByte() : default, 0);
            CString cstr6 = new((empty ? Array.Empty<Byte>() : default).AsSpan().AsIntPtr(), 0);

            ReadOnlySpan<Byte> span1 = cstr1;
            ReadOnlySpan<Byte> span2 = cstr2;
            ReadOnlySpan<Byte> span3 = cstr3;
            ReadOnlySpan<Byte> span4 = cstr4;
            ReadOnlySpan<Byte> span5 = cstr5;
            ReadOnlySpan<Byte> span6 = cstr6;

            Assert.Equal(!empty, span1.IsEmpty);
            Assert.Equal(!empty, span2.IsEmpty);
            Assert.True(span3.IsEmpty);
            Assert.True(span4.IsEmpty);
            Assert.False(span5.IsEmpty);
            Assert.True(span6.IsEmpty);

            if (empty)
            {
                Assert.NotNull(cstr1);
                Assert.NotNull(cstr2);
                Assert.NotNull(cstr3);
                Assert.NotNull(cstr4);

                Assert.Empty(cstr1.ToString());
                Assert.Empty(cstr2.ToString());
                Assert.Empty(cstr3.ToString());
                Assert.Empty(cstr4.ToString());

                Assert.True(cstr1.IsNullTerminated);
                Assert.True(cstr2.IsNullTerminated);
                Assert.False(cstr3.IsNullTerminated);
                Assert.False(cstr4.IsNullTerminated);

                Assert.Equal(0, cstr1.Length);
                Assert.Equal(0, cstr2.Length);
                Assert.Equal(0, cstr3.Length);
                Assert.Equal(0, cstr4.Length);

                Assert.Equal(1, span1.Length);
                Assert.Equal(1, span2.Length);
                Assert.Equal(0, span3.Length);
                Assert.Equal(0, span4.Length);

                Assert.Equal(default, cstr1[0]);
                Assert.Equal(default, cstr2[0]);
                Assert.Throws<IndexOutOfRangeException>(() => cstr3[0]);
                Assert.Throws<IndexOutOfRangeException>(() => cstr4[0]);
            }
            else
            {
                Assert.Null(cstr1);
                Assert.Null(cstr2);
                Assert.Null(cstr3);
                Assert.Null(cstr4);
            }

            Assert.NotNull(cstr5);
            Assert.NotNull(cstr6);

            Assert.Empty(cstr5.ToString());
            Assert.Empty(cstr6.ToString());

            Assert.True(cstr5.IsNullTerminated);
            Assert.False(cstr6.IsNullTerminated);

            Assert.Equal(0, cstr5.Length);
            Assert.Equal(0, cstr6.Length);

            Assert.Equal(1, span5.Length);
            Assert.Equal(0, span6.Length);

            Assert.Equal(default, cstr5[0]);

            if (empty)
                Assert.Throws<IndexOutOfRangeException>(() => cstr4[0]);
            else
                Assert.Throws<NullReferenceException>(() => cstr4[0]);

            Assert.True(CString.IsNullOrEmpty(cstr1));
            Assert.True(CString.IsNullOrEmpty(cstr2));
            Assert.True(CString.IsNullOrEmpty(cstr3));
            Assert.True(CString.IsNullOrEmpty(cstr4));
            Assert.True(CString.IsNullOrEmpty(cstr5));
            Assert.True(CString.IsNullOrEmpty(cstr6));
        }

        [Fact]
        internal void NormalTest()
        {
            String str1 = TestUtilities.SharedFixture.Create<String>();
            Byte[] byt2 = Encoding.UTF8.GetBytes(TestUtilities.SharedFixture.Create<String>());
            Byte[] byt3 = TestUtilities.AsArray(TestUtilities.GetPrintableByte(), TestUtilities.GetPrintableByte());
            Byte[] byt4 = TestUtilities.AsArray(TestUtilities.GetPrintableByte(), default);
            ReadOnlySpan<Byte> spa5 = TestUtilities.AsArray(TestUtilities.GetPrintableByte(), TestUtilities.GetPrintableByte());
            ReadOnlySpan<Byte> spa6 = TestUtilities.AsArray(TestUtilities.GetPrintableByte(), default);
            Byte ccr7 = TestUtilities.GetPrintableByte();

            Byte[] byt1 = Encoding.UTF8.GetBytes(str1);
            String str2 = Encoding.UTF8.GetString(byt2);
            String str3 = Encoding.UTF8.GetString(byt3);
            String str4 = Encoding.UTF8.GetString(byt4);
            String str5 = Encoding.UTF8.GetString(spa5);
            String str6 = Encoding.UTF8.GetString(spa6);
            String str7 = Encoding.UTF8.GetString(Enumerable.Repeat(ccr7, 3).ToArray());

            CString cstr1 = str1;
            CString cstr2 = byt2;
            CString cstr3 = byt3;
            CString cstr4 = byt4;
            CString cstr5 = new(spa5.AsIntPtr(), spa5.Length);
            CString cstr6 = new(spa6.AsIntPtr(), spa6.Length);
            CString cstr7 = new(ccr7, 3);

            Assert.NotNull(cstr1);
            Assert.NotNull(cstr2);
            Assert.NotNull(cstr3);
            Assert.NotNull(cstr4);
            Assert.NotNull(cstr5);
            Assert.NotNull(cstr6);
            Assert.NotNull(cstr7);

            Assert.Equal(str1, cstr1.ToString());
            Assert.Equal(str1.Length, cstr1.Length);
            Assert.Equal(cstr1.Length + 1, cstr1.AsSpan().Length);
            Assert.True(cstr1.IsNullTerminated);
            AssertIndex(byt1, cstr1);

            Assert.Equal(str2, cstr2.ToString());
            Assert.Equal(str2.Length, cstr2.Length);
            Assert.Equal(cstr2.Length, cstr2.AsSpan().Length);
            Assert.False(cstr2.IsNullTerminated);
            AssertIndex(byt2, cstr2);
            AssertReference(byt2, cstr2, false);
            AssertReference(byt2, (CString)cstr2.Clone(), true);

            Assert.Equal(str3, cstr3.ToString());
            Assert.Equal(str3.Length, cstr3.Length);
            Assert.Equal(cstr3.Length, cstr3.AsSpan().Length);
            Assert.False(cstr3.IsNullTerminated);
            AssertIndex(byt3, cstr3);
            AssertReference(byt3, cstr3, false);
            AssertReference(byt3, (CString)cstr3.Clone(), true);

            Assert.Equal(str4[0..^1], cstr4.ToString());
            Assert.Equal(str4.Length - 1, cstr4.Length);
            Assert.Equal(cstr4.Length + 1, cstr4.AsSpan().Length);
            Assert.True(cstr4.IsNullTerminated);
            AssertIndex(byt4, cstr4);
            AssertReference(byt4, cstr4, false);
            AssertReference(byt4, (CString)cstr4.Clone(), true);

            Assert.Equal(str5, cstr5.ToString());
            Assert.Equal(str5.Length, cstr5.Length);
            Assert.Equal(cstr5.Length, cstr5.AsSpan().Length);
            Assert.False(cstr5.IsNullTerminated);
            AssertIndex(spa5, cstr5);
            AssertReference(spa5, cstr5, false);
            AssertReference(spa5, (CString)cstr5.Clone(), true);

            Assert.Equal(str6[0..^1], cstr6.ToString());
            Assert.Equal(str6.Length - 1, cstr6.Length);
            Assert.Equal(cstr6.Length + 1, cstr6.AsSpan().Length);
            Assert.True(cstr6.IsNullTerminated);
            AssertIndex(spa6, cstr6);
            AssertReference(spa6, cstr6, false);
            AssertReference(spa6, (CString)cstr6.Clone(), true);

            Assert.Equal(str7, cstr7.ToString());
            Assert.Equal(str7.Length, cstr7.Length);
            Assert.Equal(cstr7.Length + 1, cstr7.AsSpan().Length);
            Assert.True(cstr7.IsNullTerminated);
            AssertIndex(cstr7, cstr7);
        }

        private static void AssertIndex(ReadOnlySpan<Byte> span, CString cstr)
        {
            for (Int32 i = 0; i < span.Length; i++)
                Assert.Equal(span[i], cstr[i]);
            Int32 index = 0;
            foreach (Byte c in cstr)
            {
                c.Equals(span[index]);
                index++;
                if (index == cstr.Length)
                    break;
            }
            if (!cstr.IsNullTerminated)
                Assert.Throws<IndexOutOfRangeException>(() => cstr[index]);
            else
                Assert.Equal(default, cstr[index]);
        }

        private static void AssertReference(ReadOnlySpan<Byte> span, CString cstr, Boolean clone)
        {
            Assert.Equal(!clone, span.AsIntPtr().Equals(cstr.AsSpan().AsIntPtr()));
            Assert.Equal(span.Length, cstr.AsSpan().Length);
            Int32 index = 0;
            foreach (ref readonly Byte c in cstr)
            {
                Assert.Equal(c, span[index]);
                Assert.Equal(!clone, Unsafe.AreSame(ref Unsafe.AsRef(c), ref Unsafe.AsRef(span[index])));
                index++;
                if (index == cstr.Length)
                    break;
            }
        }
    }
}
