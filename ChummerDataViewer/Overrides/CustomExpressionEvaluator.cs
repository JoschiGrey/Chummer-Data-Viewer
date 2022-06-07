﻿using CodingSeb.ExpressionEvaluator;

namespace ChummerDataViewer.Overrides;

public class CustomExpressionEvaluator : ExpressionEvaluator
{
    protected override void Init()
    {
        operatorsDictionary.Add("div", ExpressionOperator.Divide);
    }
}