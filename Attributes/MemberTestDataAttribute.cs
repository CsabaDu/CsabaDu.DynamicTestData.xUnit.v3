// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.Attributes;

public sealed class MemberTestDataAttribute(string memberName, params object[] arguments)
: MemberTestDataAttributeBase(memberName, arguments);
