using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;

namespace Maid.Blazor.Generators.AutoFillProperties;
[Generator(LanguageNames.CSharp)]
public class AutoFillPropertiesGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(AutoFillPropertiesGenerator.PostInit);
	}
	public static void PostInit(IncrementalGeneratorPostInitializationContext context)
	{

	}
}
