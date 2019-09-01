# kalexi.Expressions.Utilities
Utilities for working with .NET's expression trees.

[![Build status](https://ci.appveyor.com/api/projects/status/98ldrus7ajbw0nid?svg=true)](https://ci.appveyor.com/project/kalexii/kalexi-expressions-utilities)

## Why?

.NET Expression trees is a powerful, yet not so widely known (what a shame!) mechanism that enables you to build blocks of code dynamically and turn them into delegates for a solid performance gain, as opposed to using .NET Reflection API.
[Read More](http://geekswithblogs.net/Madman/archive/2008/06/27/faster-reflection-using-expression-trees.aspx)

## What

This library contains utility methods that are applicable to scenarios with different type-safety levels.

## Features

- Ability to retrieve PropertyInfo/FieldInfo/MemberInfo/Member from an expression.
- Ability to create getter or setter for reference or value type property or field using either it's `PropertyInfo`/`FieldInfo` or `Func<T, TResult>`. You can use `Func<T,TResult>`, `Func<T, object>` or `Func<object, object>` - whatever your situation is.

For usage example, check the [unit tests](https://github.com/kalexii/kalexi.Expressions.Utilities/blob/master/kalexi.Expressions.Utilities.Tests/ExpressionUtilitiesTests.cs).

Todo:
 - expand on this doc, add examples
 - include documentation and debug symbols into nuget package
