/*
 * MIT License
 * 
 * Copyright (c) 2025. Csaba Dudas (CsabaDu)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
namespace CsabaDu.DynamicTestData.xUnit.v3.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class NamedMemberDataAttribute(string memberName, params object[] arguments)
: MemberDataAttributeBase(memberName, arguments)
{
    public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
            MethodInfo testMethod,
            DisposalTracker disposalTracker)
    {
        var data = base.GetData(testMethod, disposalTracker).AsTask().Result;

        if (data is not IReadOnlyCollection<TheoryTestDataRow> dataRows)
        {
            return new ValueTask<IReadOnlyCollection<ITheoryDataRow>>(data);
        }

        var namedDataRows = new List<ITheoryDataRow>(dataRows.Count);

        foreach (TheoryTestDataRow item in dataRows)
        {
            namedDataRows.Add(new TheoryTestDataRow(item.TestData, item.ArgsCode)
            {
                TestDisplayName = GetTestDisplayName(testMethod.Name, item.TestData)
            });
        }

        return new ValueTask<IReadOnlyCollection<ITheoryDataRow>>(namedDataRows);
    }
}
