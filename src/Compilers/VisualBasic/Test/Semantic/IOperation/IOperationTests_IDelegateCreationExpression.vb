﻿' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.CodeAnalysis.Test.Utilities
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax
Imports Roslyn.Test.Utilities

Namespace Microsoft.CodeAnalysis.VisualBasic.UnitTests.Semantics

    Partial Public Class IOperationTests
        Inherits SemanticModelTestBase

#Region "Lambda Expressions"

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitLambdaConversion()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action = Sub() Console.WriteLine("")'BIND:"Dim a As Action = Sub() Console.WriteLine("")"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement) (Syntax: 'Dim a As Ac ... iteLine("")')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action, IsImplicit) (Syntax: 'Sub() Conso ... iteLine("")')
        Target: 
          IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Sub() Conso ... iteLine("")')
            IBlockStatement (3 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... iteLine("")')
              IBlockStatement (1 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... iteLine("")')
                IExpressionStatement (OperationKind.ExpressionStatement) (Syntax: 'Console.WriteLine("")')
                  Expression: 
                    IInvocationExpression (Sub System.Console.WriteLine(value As System.String)) (OperationKind.InvocationExpression, Type: System.Void) (Syntax: 'Console.WriteLine("")')
                      Instance Receiver: 
                        null
                      Arguments(1):
                          IArgument (ArgumentKind.Explicit, Matching Parameter: value) (OperationKind.Argument) (Syntax: '""')
                            ILiteralExpression (OperationKind.LiteralExpression, Type: System.String, Constant: "") (Syntax: '""')
                            InConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
                            OutConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
              ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Sub() Conso ... iteLine("")')
                Statement: 
                  null
              IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Sub() Conso ... iteLine("")')
                ReturnedValue: 
                  null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitLambdaConversion_JustInitializerReturnsOnlyLambda()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action = Sub() Console.WriteLine("")'BIND:"Sub() Console.WriteLine("")"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Sub() Conso ... iteLine("")')
  IBlockStatement (3 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... iteLine("")')
    IBlockStatement (1 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... iteLine("")')
      IExpressionStatement (OperationKind.ExpressionStatement) (Syntax: 'Console.WriteLine("")')
        Expression: 
          IInvocationExpression (Sub System.Console.WriteLine(value As System.String)) (OperationKind.InvocationExpression, Type: System.Void) (Syntax: 'Console.WriteLine("")')
            Instance Receiver: 
              null
            Arguments(1):
                IArgument (ArgumentKind.Explicit, Matching Parameter: value) (OperationKind.Argument) (Syntax: '""')
                  ILiteralExpression (OperationKind.LiteralExpression, Type: System.String, Constant: "") (Syntax: '""')
                  InConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
                  OutConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
    ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Sub() Conso ... iteLine("")')
      Statement: 
        null
    IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Sub() Conso ... iteLine("")')
      ReturnedValue: 
        null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of SingleLineLambdaExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitLambdaConversion_DisallowedArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action = Sub(i As Integer) Console.WriteLine("")'BIND:"Dim a As Action = Sub(i As Integer) Console.WriteLine("")"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement, IsInvalid) (Syntax: 'Dim a As Ac ... iteLine("")')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... iteLine("")')
        Target: 
          IAnonymousFunctionExpression (Symbol: Sub (i As System.Int32)) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub(i As In ... iteLine("")')
            IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... iteLine("")')
              IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... iteLine("")')
                IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine("")')
                  Expression: 
                    IInvocationExpression (Sub System.Console.WriteLine(value As System.String)) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine("")')
                      Instance Receiver: 
                        null
                      Arguments(1):
                          IArgument (ArgumentKind.Explicit, Matching Parameter: value) (OperationKind.Argument, IsInvalid) (Syntax: '""')
                            ILiteralExpression (OperationKind.LiteralExpression, Type: System.String, Constant: "", IsInvalid) (Syntax: '""')
                            InConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
                            OutConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
              ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... iteLine("")')
                Statement: 
                  null
              IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... iteLine("")')
                ReturnedValue: 
                  null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36670: Nested sub does not have a signature that is compatible with delegate 'Action'.
        Dim a As Action = Sub(i As Integer) Console.WriteLine("")'BIND:"Dim a As Action = Sub(i As Integer) Console.WriteLine("")"
                          ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value
            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitLambdaConversion_InvalidArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Class C1
    End Class
    Sub Method1()
        Dim a As Action(Of String) = Sub(c1 As C1) Console.WriteLine("")'BIND:"Dim a As Action(Of String) = Sub(c1 As C1) Console.WriteLine("")"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement, IsInvalid) (Syntax: 'Dim a As Ac ... iteLine("")')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action(Of System.String)
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String), IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
        Target: 
          IAnonymousFunctionExpression (Symbol: Sub (c1 As M1.C1)) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub(c1 As C ... iteLine("")')
            IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
              IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
                IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine("")')
                  Expression: 
                    IInvocationExpression (Sub System.Console.WriteLine(value As System.String)) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine("")')
                      Instance Receiver: 
                        null
                      Arguments(1):
                          IArgument (ArgumentKind.Explicit, Matching Parameter: value) (OperationKind.Argument, IsInvalid) (Syntax: '""')
                            ILiteralExpression (OperationKind.LiteralExpression, Type: System.String, Constant: "", IsInvalid) (Syntax: '""')
                            InConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
                            OutConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
              ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
                Statement: 
                  null
              IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
                ReturnedValue: 
                  null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36670: Nested sub does not have a signature that is compatible with delegate 'Action(Of String)'.
        Dim a As Action(Of String) = Sub(c1 As C1) Console.WriteLine("")'BIND:"Dim a As Action(Of String) = Sub(c1 As C1) Console.WriteLine("")"
                                     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitLambdaConversion_InvalidReturnType()
            Dim source = <![CDATA[
Option Strict Off
Imports System
Module M1
    Sub Method1()
        Dim a As Func(Of String) = Function() New NonExistant()'BIND:"Dim a As Func(Of String) = Function() New NonExistant()"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement, IsInvalid) (Syntax: 'Dim a As Fu ... nExistant()')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Func(Of System.String)
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.String), IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
        Target: 
          IAnonymousFunctionExpression (Symbol: Function () As System.String) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Function()  ... nExistant()')
            IBlockStatement (3 statements, 1 locals) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
              Locals: Local_1: <anonymous local> As System.String
              IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'New NonExistant()')
                ReturnedValue: 
                  IConversionExpression (Implicit, TryCast: False, Unchecked) (OperationKind.ConversionExpression, Type: System.String, IsInvalid, IsImplicit) (Syntax: 'New NonExistant()')
                    Conversion: CommonConversion (Exists: False, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
                    Operand: 
                      IInvalidExpression (OperationKind.InvalidExpression, Type: NonExistant, IsInvalid) (Syntax: 'New NonExistant()')
                        Children(0)
              ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
                Statement: 
                  null
              IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
                ReturnedValue: 
                  ILocalReferenceExpression:  (OperationKind.LocalReferenceExpression, Type: System.String, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30002: Type 'NonExistant' is not defined.
        Dim a As Func(Of String) = Function() New NonExistant()'BIND:"Dim a As Func(Of String) = Function() New NonExistant()"
                                                  ~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitLambdaConversion_DisallowedReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Func(Of String) = Function() 1'BIND:"Dim a As Func(Of String) = Function() 1"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement, IsInvalid) (Syntax: 'Dim a As Fu ... unction() 1')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Func(Of System.String)
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.String), IsInvalid, IsImplicit) (Syntax: 'Function() 1')
        Target: 
          IAnonymousFunctionExpression (Symbol: Function () As System.String) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Function() 1')
            IBlockStatement (3 statements, 1 locals) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
              Locals: Local_1: <anonymous local> As System.String
              IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: '1')
                ReturnedValue: 
                  IConversionExpression (Implicit, TryCast: False, Unchecked) (OperationKind.ConversionExpression, Type: System.String, IsInvalid, IsImplicit) (Syntax: '1')
                    Conversion: CommonConversion (Exists: True, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
                    Operand: 
                      ILiteralExpression (OperationKind.LiteralExpression, Type: System.Int32, Constant: 1, IsInvalid) (Syntax: '1')
              ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
                Statement: 
                  null
              IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
                ReturnedValue: 
                  ILocalReferenceExpression:  (OperationKind.LocalReferenceExpression, Type: System.String, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30512: Option Strict On disallows implicit conversions from 'Integer' to 'String'.
        Dim a As Func(Of String) = Function() 1'BIND:"Dim a As Func(Of String) = Function() 1"
                                              ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitLambdaConversion_ReturnRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action = Function() 1'BIND:"Dim a As Action = Function() 1"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement) (Syntax: 'Dim a As Ac ... unction() 1')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action, IsImplicit) (Syntax: 'Function() 1')
        Target: 
          IAnonymousFunctionExpression (Symbol: Function () As System.Int32) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Function() 1')
            IBlockStatement (3 statements, 1 locals) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Function() 1')
              Locals: Local_1: <anonymous local> As System.Int32
              IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: '1')
                ReturnedValue: 
                  ILiteralExpression (OperationKind.LiteralExpression, Type: System.Int32, Constant: 1) (Syntax: '1')
              ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Function() 1')
                Statement: 
                  null
              IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Function() 1')
                ReturnedValue: 
                  ILocalReferenceExpression:  (OperationKind.LocalReferenceExpression, Type: System.Int32, IsImplicit) (Syntax: 'Function() 1')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitLambdaExpression_RelaxationOfArgument()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action(Of String) = Sub(o As Object) Console.WriteLine(o)'BIND:"Dim a As Action(Of String) = Sub(o As Object) Console.WriteLine(o)"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement) (Syntax: 'Dim a As Ac ... riteLine(o)')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action(Of System.String)
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String), IsImplicit) (Syntax: 'Sub(o As Ob ... riteLine(o)')
        Target: 
          IAnonymousFunctionExpression (Symbol: Sub (o As System.Object)) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Sub(o As Ob ... riteLine(o)')
            IBlockStatement (3 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub(o As Ob ... riteLine(o)')
              IBlockStatement (1 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub(o As Ob ... riteLine(o)')
                IExpressionStatement (OperationKind.ExpressionStatement) (Syntax: 'Console.WriteLine(o)')
                  Expression: 
                    IInvocationExpression (Sub System.Console.WriteLine(value As System.Object)) (OperationKind.InvocationExpression, Type: System.Void) (Syntax: 'Console.WriteLine(o)')
                      Instance Receiver: 
                        null
                      Arguments(1):
                          IArgument (ArgumentKind.Explicit, Matching Parameter: value) (OperationKind.Argument) (Syntax: 'o')
                            IParameterReferenceExpression: o (OperationKind.ParameterReferenceExpression, Type: System.Object) (Syntax: 'o')
                            InConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
                            OutConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
              ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Sub(o As Ob ... riteLine(o)')
                Statement: 
                  null
              IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Sub(o As Ob ... riteLine(o)')
                ReturnedValue: 
                  null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeLambdaConversion()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Main()
        Dim a As Action = CType(Sub() Console.WriteLine(), Action)'BIND:"CType(Sub() Console.WriteLine(), Action)"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'CType(Sub() ... (), Action)')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Sub() Conso ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeLambdaConversion_DisallowedArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Main()
        Dim a As Action = CType(Sub(i As Integer) Console.WriteLine(), Action)'BIND:"CType(Sub(i As Integer) Console.WriteLine(), Action)"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action, IsInvalid) (Syntax: 'CType(Sub(i ... (), Action)')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub (i As System.Int32)) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub(i As In ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36670: Nested sub does not have a signature that is compatible with delegate 'Action'.
        Dim a As Action = CType(Sub(i As Integer) Console.WriteLine(), Action)'BIND:"CType(Sub(i As Integer) Console.WriteLine(), Action)"
                                ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeLambdaConversion_InvalidArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Class C1
    End Class
    Sub Method1()
        Dim a As Action(Of String) = CType(Sub(c1 As C1) Console.WriteLine(""), Action(Of String))'BIND:"CType(Sub(c1 As C1) Console.WriteLine(""), Action(Of String))"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String), IsInvalid) (Syntax: 'CType(Sub(c ... Of String))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub (c1 As M1.C1)) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub(c1 As C ... iteLine("")')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
          IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine("")')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine(value As System.String)) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine("")')
                Instance Receiver: 
                  null
                Arguments(1):
                    IArgument (ArgumentKind.Explicit, Matching Parameter: value) (OperationKind.Argument, IsInvalid) (Syntax: '""')
                      ILiteralExpression (OperationKind.LiteralExpression, Type: System.String, Constant: "", IsInvalid) (Syntax: '""')
                      InConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
                      OutConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36670: Nested sub does not have a signature that is compatible with delegate 'Action(Of String)'.
        Dim a As Action(Of String) = CType(Sub(c1 As C1) Console.WriteLine(""), Action(Of String))'BIND:"CType(Sub(c1 As C1) Console.WriteLine(""), Action(Of String))"
                                           ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeLambdaConversion_DisallowedReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Main()
        Dim a As Func(Of String) = CType(Function() 1, Func(Of String))'BIND:"CType(Function() 1, Func(Of String))"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.String), IsInvalid) (Syntax: 'CType(Funct ... Of String))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Function () As System.String) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Function() 1')
      IBlockStatement (3 statements, 1 locals) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
        Locals: Local_1: <anonymous local> As System.String
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: '1')
          ReturnedValue: 
            IConversionExpression (Implicit, TryCast: False, Unchecked) (OperationKind.ConversionExpression, Type: System.String, IsInvalid, IsImplicit) (Syntax: '1')
              Conversion: CommonConversion (Exists: True, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
              Operand: 
                ILiteralExpression (OperationKind.LiteralExpression, Type: System.Int32, Constant: 1, IsInvalid) (Syntax: '1')
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
          ReturnedValue: 
            ILocalReferenceExpression:  (OperationKind.LocalReferenceExpression, Type: System.String, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30512: Option Strict On disallows implicit conversions from 'Integer' to 'String'.
        Dim a As Func(Of String) = CType(Function() 1, Func(Of String))'BIND:"CType(Function() 1, Func(Of String))"
                                                    ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeLambdaConversion_InvalidReturnType()
            Dim source = <![CDATA[
Option Strict Off
Imports System
Module M1
    Sub Main()
        Dim a As Func(Of String) = CType(Function() New NonExistant(), Func(Of String)) 'BIND:"CType(Function() New NonExistant(), Func(Of String))"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.String), IsInvalid) (Syntax: 'CType(Funct ... Of String))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Function () As System.String) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Function()  ... nExistant()')
      IBlockStatement (3 statements, 1 locals) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
        Locals: Local_1: <anonymous local> As System.String
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'New NonExistant()')
          ReturnedValue: 
            IConversionExpression (Implicit, TryCast: False, Unchecked) (OperationKind.ConversionExpression, Type: System.String, IsInvalid, IsImplicit) (Syntax: 'New NonExistant()')
              Conversion: CommonConversion (Exists: False, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
              Operand: 
                IInvalidExpression (OperationKind.InvalidExpression, Type: NonExistant, IsInvalid) (Syntax: 'New NonExistant()')
                  Children(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
          ReturnedValue: 
            ILocalReferenceExpression:  (OperationKind.LocalReferenceExpression, Type: System.String, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30002: Type 'NonExistant' is not defined.
        Dim a As Func(Of String) = CType(Function() New NonExistant(), Func(Of String)) 'BIND:"CType(Function() New NonExistant(), Func(Of String))"
                                                        ~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeLambdaConversion_InvalidVariableType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Action(Of Object) = CType(Sub(i As Integer) Console.WriteLine(), Action(Of Integer))'BIND:"Dim a As Action(Of Object) = CType(Sub(i As Integer) Console.WriteLine(), Action(Of Integer))"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement, IsInvalid) (Syntax: 'Dim a As Ac ... f Integer))')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action(Of System.Object)
    Initializer: 
      IConversionExpression (Implicit, TryCast: False, Unchecked) (OperationKind.ConversionExpression, Type: System.Action(Of System.Object), IsInvalid, IsImplicit) (Syntax: 'CType(Sub(i ... f Integer))')
        Conversion: CommonConversion (Exists: False, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
        Operand: 
          IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.Int32), IsInvalid) (Syntax: 'CType(Sub(i ... f Integer))')
            Target: 
              IAnonymousFunctionExpression (Symbol: Sub (i As System.Int32)) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub(i As In ... WriteLine()')
                IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
                  IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
                    IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine()')
                      Expression: 
                        IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine()')
                          Instance Receiver: 
                            null
                          Arguments(0)
                  ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
                    Statement: 
                      null
                  IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
                    ReturnedValue: 
                      null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36755: 'Action(Of Integer)' cannot be converted to 'Action(Of Object)' because 'Object' is not derived from 'Integer', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        Dim a As Action(Of Object) = CType(Sub(i As Integer) Console.WriteLine(), Action(Of Integer))'BIND:"Dim a As Action(Of Object) = CType(Sub(i As Integer) Console.WriteLine(), Action(Of Integer))"
                                     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeLambdaConversion_ReturnRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Main()
        Dim a As Func(Of Object) = CType(Function() 1, Func(Of Object))'BIND:"CType(Function() 1, Func(Of Object))"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.Object)) (Syntax: 'CType(Funct ... Of Object))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Function () As System.Object) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Function() 1')
      IBlockStatement (3 statements, 1 locals) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Function() 1')
        Locals: Local_1: <anonymous local> As System.Object
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: '1')
          ReturnedValue: 
            IConversionExpression (Implicit, TryCast: False, Unchecked) (OperationKind.ConversionExpression, Type: System.Object, IsImplicit) (Syntax: '1')
              Conversion: CommonConversion (Exists: True, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
              Operand: 
                ILiteralExpression (OperationKind.LiteralExpression, Type: System.Int32, Constant: 1) (Syntax: '1')
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Function() 1')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Function() 1')
          ReturnedValue: 
            ILocalReferenceExpression:  (OperationKind.LocalReferenceExpression, Type: System.Object, IsImplicit) (Syntax: 'Function() 1')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeLambdaConversion_ArgumentRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Main()
        Dim a As Action(Of Object) = CType(Sub() Console.WriteLine(), Action(Of Object))'BIND:"CType(Sub() Console.WriteLine(), Action(Of Object))"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.Object)) (Syntax: 'CType(Sub() ... Of Object))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Sub() Conso ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeMethodBinding()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Action = CType(AddressOf M1, Action)'BIND:"CType(AddressOf M1, Action)"
    End Sub

    Sub M1()
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'CType(Addre ... M1, Action)')
  Target: 
    IMethodReferenceExpression: Sub Program.M1() (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf M1')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsImplicit) (Syntax: 'M1')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeMethodBinding_InvalidVariableType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Action(Of Object) = CType(AddressOf M1, Action(Of Integer))'BIND:"Dim a As Action(Of Object) = CType(AddressOf M1, Action(Of Integer))"
    End Sub

    Sub M1(i As Integer)
    End Sub
End Module
]]>.Value

            ' Explicitly verifying the entire tree here to ensure that the top level initializer statement is actually an IConversion, and not
            ' a delegate creation
            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement, IsInvalid) (Syntax: 'Dim a As Ac ... f Integer))')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action(Of System.Object)
    Initializer: 
      IConversionExpression (Implicit, TryCast: False, Unchecked) (OperationKind.ConversionExpression, Type: System.Action(Of System.Object), IsInvalid, IsImplicit) (Syntax: 'CType(Addre ... f Integer))')
        Conversion: CommonConversion (Exists: False, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
        Operand: 
          IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.Int32), IsInvalid) (Syntax: 'CType(Addre ... f Integer))')
            Target: 
              IMethodReferenceExpression: Sub Program.M1(i As System.Int32) (OperationKind.MethodReferenceExpression, Type: null, IsInvalid) (Syntax: 'AddressOf M1')
                Instance Receiver: 
                  IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsInvalid, IsImplicit) (Syntax: 'M1')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36755: 'Action(Of Integer)' cannot be converted to 'Action(Of Object)' because 'Object' is not derived from 'Integer', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        Dim a As Action(Of Object) = CType(AddressOf M1, Action(Of Integer))'BIND:"Dim a As Action(Of Object) = CType(AddressOf M1, Action(Of Integer))"
                                     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastLambdaConversion()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Action = DirectCast(Sub() Console.WriteLine(), Action)'BIND:"DirectCast(Sub() Console.WriteLine(), Action)"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'DirectCast( ... (), Action)')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Sub() Conso ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastLambdaConversion_DisallowedArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Action = DirectCast(Sub(i As Integer) Console.WriteLine(), Action)'BIND:"DirectCast(Sub(i As Integer) Console.WriteLine(), Action)"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action, IsInvalid) (Syntax: 'DirectCast( ... (), Action)')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub (i As System.Int32)) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub(i As In ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36670: Nested sub does not have a signature that is compatible with delegate 'Action'.
        Dim a As Action = DirectCast(Sub(i As Integer) Console.WriteLine(), Action)'BIND:"DirectCast(Sub(i As Integer) Console.WriteLine(), Action)"
                                     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastLambdaConversion_InvalidArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Class C1
    End Class
    Sub Method1()
        Dim a As Action(Of String) = DirectCast(Sub(c1 As C1) Console.WriteLine(""), Action(Of String))'BIND:"DirectCast(Sub(c1 As C1) Console.WriteLine(""), Action(Of String))"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String), IsInvalid) (Syntax: 'DirectCast( ... Of String))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub (c1 As M1.C1)) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub(c1 As C ... iteLine("")')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
          IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine("")')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine(value As System.String)) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine("")')
                Instance Receiver: 
                  null
                Arguments(1):
                    IArgument (ArgumentKind.Explicit, Matching Parameter: value) (OperationKind.Argument, IsInvalid) (Syntax: '""')
                      ILiteralExpression (OperationKind.LiteralExpression, Type: System.String, Constant: "", IsInvalid) (Syntax: '""')
                      InConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
                      OutConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36670: Nested sub does not have a signature that is compatible with delegate 'Action(Of String)'.
        Dim a As Action(Of String) = DirectCast(Sub(c1 As C1) Console.WriteLine(""), Action(Of String))'BIND:"DirectCast(Sub(c1 As C1) Console.WriteLine(""), Action(Of String))"
                                                ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastLambdaConversion_DisallowedReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Func(Of String) = DirectCast(Function() 1, Func(Of String))'BIND:"DirectCast(Function() 1, Func(Of String))"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.String), IsInvalid) (Syntax: 'DirectCast( ... Of String))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Function () As System.String) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Function() 1')
      IBlockStatement (3 statements, 1 locals) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
        Locals: Local_1: <anonymous local> As System.String
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: '1')
          ReturnedValue: 
            IConversionExpression (Implicit, TryCast: False, Unchecked) (OperationKind.ConversionExpression, Type: System.String, IsInvalid, IsImplicit) (Syntax: '1')
              Conversion: CommonConversion (Exists: True, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
              Operand: 
                ILiteralExpression (OperationKind.LiteralExpression, Type: System.Int32, Constant: 1, IsInvalid) (Syntax: '1')
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
          ReturnedValue: 
            ILocalReferenceExpression:  (OperationKind.LocalReferenceExpression, Type: System.String, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30512: Option Strict On disallows implicit conversions from 'Integer' to 'String'.
        Dim a As Func(Of String) = DirectCast(Function() 1, Func(Of String))'BIND:"DirectCast(Function() 1, Func(Of String))"
                                                         ~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastLambdaConversion_InvalidReturnType()
            Dim source = <![CDATA[
Option Strict Off
Imports System

Module Program
    Sub Main()
        Dim a As Func(Of String) = DirectCast(Function() New NonExistant(), Func(Of String)) 'BIND:"DirectCast(Function() New NonExistant(), Func(Of String))"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.String), IsInvalid) (Syntax: 'DirectCast( ... Of String))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Function () As System.String) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Function()  ... nExistant()')
      IBlockStatement (3 statements, 1 locals) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
        Locals: Local_1: <anonymous local> As System.String
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'New NonExistant()')
          ReturnedValue: 
            IConversionExpression (Implicit, TryCast: False, Unchecked) (OperationKind.ConversionExpression, Type: System.String, IsInvalid, IsImplicit) (Syntax: 'New NonExistant()')
              Conversion: CommonConversion (Exists: False, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
              Operand: 
                IInvalidExpression (OperationKind.InvalidExpression, Type: NonExistant, IsInvalid) (Syntax: 'New NonExistant()')
                  Children(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
          ReturnedValue: 
            ILocalReferenceExpression:  (OperationKind.LocalReferenceExpression, Type: System.String, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30002: Type 'NonExistant' is not defined.
        Dim a As Func(Of String) = DirectCast(Function() New NonExistant(), Func(Of String)) 'BIND:"DirectCast(Function() New NonExistant(), Func(Of String))"
                                                             ~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastLambdaConversion_InvalidVariableType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Func(Of String) = DirectCast(Function() 1, Func(Of Integer))'BIND:"DirectCast(Function() 1, Func(Of Integer))"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.Int32), IsInvalid) (Syntax: 'DirectCast( ... f Integer))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Function () As System.Int32) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Function() 1')
      IBlockStatement (3 statements, 1 locals) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
        Locals: Local_1: <anonymous local> As System.Int32
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: '1')
          ReturnedValue: 
            ILiteralExpression (OperationKind.LiteralExpression, Type: System.Int32, Constant: 1, IsInvalid) (Syntax: '1')
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
          ReturnedValue: 
            ILocalReferenceExpression:  (OperationKind.LocalReferenceExpression, Type: System.Int32, IsInvalid, IsImplicit) (Syntax: 'Function() 1')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36754: 'Func(Of Integer)' cannot be converted to 'Func(Of String)' because 'Integer' is not derived from 'String', as required for the 'Out' generic parameter 'TResult' in 'Delegate Function Func(Of Out TResult)() As TResult'.
        Dim a As Func(Of String) = DirectCast(Function() 1, Func(Of Integer))'BIND:"DirectCast(Function() 1, Func(Of Integer))"
                                   ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastLambdaConversion_ReturnRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Func(Of Object) = DirectCast(Function() 1, Func(Of Object))'BIND:"DirectCast(Function() 1, Func(Of Object))"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.Object)) (Syntax: 'DirectCast( ... Of Object))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Function () As System.Object) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Function() 1')
      IBlockStatement (3 statements, 1 locals) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Function() 1')
        Locals: Local_1: <anonymous local> As System.Object
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: '1')
          ReturnedValue: 
            IConversionExpression (Implicit, TryCast: False, Unchecked) (OperationKind.ConversionExpression, Type: System.Object, IsImplicit) (Syntax: '1')
              Conversion: CommonConversion (Exists: True, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
              Operand: 
                ILiteralExpression (OperationKind.LiteralExpression, Type: System.Int32, Constant: 1) (Syntax: '1')
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Function() 1')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Function() 1')
          ReturnedValue: 
            ILocalReferenceExpression:  (OperationKind.LocalReferenceExpression, Type: System.Object, IsImplicit) (Syntax: 'Function() 1')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastLambdaConversion_ArgumentRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Action(Of Object) = DirectCast(Sub() Console.WriteLine(), Action(Of Object))'BIND:"DirectCast(Sub() Console.WriteLine(), Action(Of Object))"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.Object)) (Syntax: 'DirectCast( ... Of Object))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Sub() Conso ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastLambdaConversion()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Action = TryCast(Sub() Console.WriteLine(), Action)'BIND:"TryCast(Sub() Console.WriteLine(), Action)"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'TryCast(Sub ... (), Action)')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Sub() Conso ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastLambdaConversion_DisallowedArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Action = TryCast(Sub(i As Integer) Console.WriteLine(), Action)'BIND:"TryCast(Sub(i As Integer) Console.WriteLine(), Action)"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action, IsInvalid) (Syntax: 'TryCast(Sub ... (), Action)')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub (i As System.Int32)) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub(i As In ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36670: Nested sub does not have a signature that is compatible with delegate 'Action'.
        Dim a As Action = TryCast(Sub(i As Integer) Console.WriteLine(), Action)'BIND:"TryCast(Sub(i As Integer) Console.WriteLine(), Action)"
                                  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastLambdaConversion_InvalidArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Class C1
    End Class
    Sub Method1()
        Dim a As Action(Of String) = TryCast(Sub(c1 As C1) Console.WriteLine(""), Action(Of String))'BIND:"TryCast(Sub(c1 As C1) Console.WriteLine(""), Action(Of String))"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String), IsInvalid) (Syntax: 'TryCast(Sub ... Of String))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub (c1 As M1.C1)) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub(c1 As C ... iteLine("")')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
          IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine("")')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine(value As System.String)) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine("")')
                Instance Receiver: 
                  null
                Arguments(1):
                    IArgument (ArgumentKind.Explicit, Matching Parameter: value) (OperationKind.Argument, IsInvalid) (Syntax: '""')
                      ILiteralExpression (OperationKind.LiteralExpression, Type: System.String, Constant: "", IsInvalid) (Syntax: '""')
                      InConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
                      OutConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36670: Nested sub does not have a signature that is compatible with delegate 'Action(Of String)'.
        Dim a As Action(Of String) = TryCast(Sub(c1 As C1) Console.WriteLine(""), Action(Of String))'BIND:"TryCast(Sub(c1 As C1) Console.WriteLine(""), Action(Of String))"
                                             ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastLambdaConversion_DisallowedReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Func(Of Object) = TryCast(Sub() Console.WriteLine(), Func(Of Object))'BIND:"TryCast(Sub() Console.WriteLine(), Func(Of Object))"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.Object), IsInvalid) (Syntax: 'TryCast(Sub ... Of Object))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub() Conso ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36670: Nested sub does not have a signature that is compatible with delegate 'Func(Of Object)'.
        Dim a As Func(Of Object) = TryCast(Sub() Console.WriteLine(), Func(Of Object))'BIND:"TryCast(Sub() Console.WriteLine(), Func(Of Object))"
                                           ~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastLambdaConversion_InvalidReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Func(Of Object) = TryCast(Function() New NonExistant(), Func(Of Object)) 'BIND:"TryCast(Function() New NonExistant(), Func(Of Object))"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.Object), IsInvalid) (Syntax: 'TryCast(Fun ... Of Object))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Function () As System.Object) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Function()  ... nExistant()')
      IBlockStatement (3 statements, 1 locals) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
        Locals: Local_1: <anonymous local> As System.Object
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'New NonExistant()')
          ReturnedValue: 
            IConversionExpression (Implicit, TryCast: False, Unchecked) (OperationKind.ConversionExpression, Type: System.Object, IsInvalid, IsImplicit) (Syntax: 'New NonExistant()')
              Conversion: CommonConversion (Exists: False, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
              Operand: 
                IInvalidExpression (OperationKind.InvalidExpression, Type: NonExistant, IsInvalid) (Syntax: 'New NonExistant()')
                  Children(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
          ReturnedValue: 
            ILocalReferenceExpression:  (OperationKind.LocalReferenceExpression, Type: System.Object, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30002: Type 'NonExistant' is not defined.
        Dim a As Func(Of Object) = TryCast(Function() New NonExistant(), Func(Of Object)) 'BIND:"TryCast(Function() New NonExistant(), Func(Of Object))"
                                                          ~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastLambdaConversion_InvalidVariableType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Action(Of String) = TryCast(Sub(i As Integer) Console.WriteLine(), Action(Of Integer))'BIND:"TryCast(Sub(i As Integer) Console.WriteLine(), Action(Of Integer))"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.Int32), IsInvalid) (Syntax: 'TryCast(Sub ... f Integer))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub (i As System.Int32)) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub(i As In ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36755: 'Action(Of Integer)' cannot be converted to 'Action(Of String)' because 'String' is not derived from 'Integer', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        Dim a As Action(Of String) = TryCast(Sub(i As Integer) Console.WriteLine(), Action(Of Integer))'BIND:"TryCast(Sub(i As Integer) Console.WriteLine(), Action(Of Integer))"
                                     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastLambdaConversion_ArgumentRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Action(Of Object) = TryCast(Sub() Console.WriteLine(), Action(Of Object))'BIND:"TryCast(Sub() Console.WriteLine(), Action(Of Object))"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.Object)) (Syntax: 'TryCast(Sub ... Of Object))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Sub() Conso ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastLambdaConversion_ReturnRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub Main()
        Dim a As Action = TryCast(Function() 1, Action)'BIND:"TryCast(Function() 1, Action)"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'TryCast(Fun ...  1, Action)')
  Target: 
    IAnonymousFunctionExpression (Symbol: Function () As System.Int32) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Function() 1')
      IBlockStatement (3 statements, 1 locals) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Function() 1')
        Locals: Local_1: <anonymous local> As System.Int32
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: '1')
          ReturnedValue: 
            ILiteralExpression (OperationKind.LiteralExpression, Type: System.Int32, Constant: 1) (Syntax: '1')
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Function() 1')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Function() 1')
          ReturnedValue: 
            ILocalReferenceExpression:  (OperationKind.LocalReferenceExpression, Type: System.Int32, IsImplicit) (Syntax: 'Function() 1')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationLambdaArgument()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action = New Action(Sub() Console.WriteLine())'BIND:"New Action(Sub() Console.WriteLine())"
    End Sub

End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'New Action( ... riteLine())')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Sub() Conso ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of ObjectCreationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationLambdaArgument_MultipleArgumentsToConstructor()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action = New Action(Sub() Console.WriteLine(), 1)'BIND:"Dim a As Action = New Action(Sub() Console.WriteLine(), 1)"
    End Sub

End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement, IsInvalid) (Syntax: 'Dim a As Ac ... eLine(), 1)')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action
    Initializer: 
      IInvalidExpression (OperationKind.InvalidExpression, Type: System.Action, IsInvalid) (Syntax: 'New Action( ... eLine(), 1)')
        Children(2):
            IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: Sub <generated method>(), IsInvalid, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
              Target: 
                IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub() Conso ... WriteLine()')
                  IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
                    IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
                      IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine()')
                        Expression: 
                          IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine()')
                            Instance Receiver: 
                              null
                            Arguments(0)
                    ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
                      Statement: 
                        null
                    IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
                      ReturnedValue: 
                        null
            ILiteralExpression (OperationKind.LiteralExpression, Type: System.Int32, Constant: 1, IsInvalid) (Syntax: '1')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC32008: Delegate 'Action' requires an 'AddressOf' expression or lambda expression as the only argument to its constructor.
        Dim a As Action = New Action(Sub() Console.WriteLine(), 1)'BIND:"Dim a As Action = New Action(Sub() Console.WriteLine(), 1)"
                                    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationLambdaArgument_ArgumentRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action(Of String) = New Action(Of String)(Sub() Console.WriteLine())'BIND:"New Action(Of String)(Sub() Console.WriteLine())"
    End Sub

End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String)) (Syntax: 'New Action( ... riteLine())')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Sub() Conso ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of ObjectCreationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationLambdaArgument_ReturnRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action = New Action(Function() 1)'BIND:"New Action(Function() 1)"
    End Sub

End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'New Action(Function() 1)')
  Target: 
    IAnonymousFunctionExpression (Symbol: Function () As System.Int32) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Function() 1')
      IBlockStatement (3 statements, 1 locals) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Function() 1')
        Locals: Local_1: <anonymous local> As System.Int32
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: '1')
          ReturnedValue: 
            ILiteralExpression (OperationKind.LiteralExpression, Type: System.Int32, Constant: 1) (Syntax: '1')
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Function() 1')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Function() 1')
          ReturnedValue: 
            ILocalReferenceExpression:  (OperationKind.LocalReferenceExpression, Type: System.Int32, IsImplicit) (Syntax: 'Function() 1')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of ObjectCreationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationLambdaArgument_DisallowedArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action(Of Object) = New Action(Of Object)(Sub(i As Integer) Console.WriteLine())'BIND:"New Action(Of Object)(Sub(i As Integer) Console.WriteLine())"
    End Sub

End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.Object), IsInvalid) (Syntax: 'New Action( ... riteLine())')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub (i As System.Int32)) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub(i As In ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(i As In ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30512: Option Strict On disallows implicit conversions from 'Object' to 'Integer'.
        Dim a As Action(Of Object) = New Action(Of Object)(Sub(i As Integer) Console.WriteLine())'BIND:"New Action(Of Object)(Sub(i As Integer) Console.WriteLine())"
                                                           ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of ObjectCreationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationLambdaArgument_InvalidArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Class C1
    End Class
    Sub Method1()
        Dim a As Action(Of String) = New Action(Of String)(Sub(c1 As C1) Console.WriteLine(""))'BIND:"New Action(Of String)(Sub(c1 As C1) Console.WriteLine(""))"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String), IsInvalid) (Syntax: 'New Action( ... teLine(""))')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub (c1 As M1.C1)) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub(c1 As C ... iteLine("")')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
          IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine("")')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine(value As System.String)) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine("")')
                Instance Receiver: 
                  null
                Arguments(1):
                    IArgument (ArgumentKind.Explicit, Matching Parameter: value) (OperationKind.Argument, IsInvalid) (Syntax: '""')
                      ILiteralExpression (OperationKind.LiteralExpression, Type: System.String, Constant: "", IsInvalid) (Syntax: '""')
                      InConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
                      OutConversion: CommonConversion (Exists: True, IsIdentity: True, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub(c1 As C ... iteLine("")')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36670: Nested sub does not have a signature that is compatible with delegate 'Action(Of String)'.
        Dim a As Action(Of String) = New Action(Of String)(Sub(c1 As C1) Console.WriteLine(""))'BIND:"New Action(Of String)(Sub(c1 As C1) Console.WriteLine(""))"
                                                           ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of ObjectCreationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationLambdaArgument_DisallowedReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Func(Of Object) = New Func(Of Object)(Sub() Console.WriteLine())'BIND:"New Func(Of Object)(Sub() Console.WriteLine())"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.Object), IsInvalid) (Syntax: 'New Func(Of ... riteLine())')
  Target: 
    IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Sub() Conso ... WriteLine()')
      IBlockStatement (3 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
        IBlockStatement (1 statements) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          IExpressionStatement (OperationKind.ExpressionStatement, IsInvalid) (Syntax: 'Console.WriteLine()')
            Expression: 
              IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void, IsInvalid) (Syntax: 'Console.WriteLine()')
                Instance Receiver: 
                  null
                Arguments(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
          ReturnedValue: 
            null
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36670: Nested sub does not have a signature that is compatible with delegate 'Func(Of Object)'.
        Dim a As Func(Of Object) = New Func(Of Object)(Sub() Console.WriteLine())'BIND:"New Func(Of Object)(Sub() Console.WriteLine())"
                                                       ~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of ObjectCreationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationLambdaArgument_InvalidReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Func(Of Object) = New Func(Of Object)(Function() New NonExistant())'BIND:"New Func(Of Object)(Function() New NonExistant())"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.Object), IsInvalid) (Syntax: 'New Func(Of ... Existant())')
  Target: 
    IAnonymousFunctionExpression (Symbol: Function () As System.Object) (OperationKind.AnonymousFunctionExpression, Type: null, IsInvalid) (Syntax: 'Function()  ... nExistant()')
      IBlockStatement (3 statements, 1 locals) (OperationKind.BlockStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
        Locals: Local_1: <anonymous local> As System.Object
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'New NonExistant()')
          ReturnedValue: 
            IConversionExpression (Implicit, TryCast: False, Unchecked) (OperationKind.ConversionExpression, Type: System.Object, IsInvalid, IsImplicit) (Syntax: 'New NonExistant()')
              Conversion: CommonConversion (Exists: False, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
              Operand: 
                IInvalidExpression (OperationKind.InvalidExpression, Type: NonExistant, IsInvalid) (Syntax: 'New NonExistant()')
                  Children(0)
        ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
          Statement: 
            null
        IReturnStatement (OperationKind.ReturnStatement, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
          ReturnedValue: 
            ILocalReferenceExpression:  (OperationKind.LocalReferenceExpression, Type: System.Object, IsInvalid, IsImplicit) (Syntax: 'Function()  ... nExistant()')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30002: Type 'NonExistant' is not defined.
        Dim a As Func(Of Object) = New Func(Of Object)(Function() New NonExistant())'BIND:"New Func(Of Object)(Function() New NonExistant())"
                                                                      ~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of ObjectCreationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

#End Region

#Region "AddressOf"

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitAddressOf()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action = AddressOf Method2'BIND:"Dim a As Action = AddressOf Method2"
    End Sub

    Sub Method2()
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement) (Syntax: 'Dim a As Ac ... sOf Method2')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action, IsImplicit) (Syntax: 'AddressOf Method2')
        Target: 
          IMethodReferenceExpression: Sub M1.Method2() (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf Method2')
            Instance Receiver: 
              IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitAddressOf_JustInitializerReturnsOnlyMethodReference()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action = AddressOf Method2'BIND:"AddressOf Method2"
    End Sub

    Sub Method2()
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IMethodReferenceExpression: Sub M1.Method2() (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf Method2')
  Instance Receiver: 
    IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of UnaryExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitAddressOf_WithReceiver()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action = AddressOf o.ToString'BIND:"Dim a As Action = AddressOf o.ToString"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement) (Syntax: 'Dim a As Ac ...  o.ToString')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action, IsImplicit) (Syntax: 'AddressOf o.ToString')
        Target: 
          IMethodReferenceExpression: Function System.Object.ToString() As System.String (IsVirtual) (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf o.ToString')
            Instance Receiver: 
              ILocalReferenceExpression: o (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'o')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitAddressOf_DisallowedArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action = AddressOf Method2'BIND:"Dim a As Action = AddressOf Method2"
    End Sub

    Sub Method2(i As Integer)
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement, IsInvalid) (Syntax: 'Dim a As Ac ... sOf Method2')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action, IsInvalid, IsImplicit) (Syntax: 'AddressOf Method2')
        Target: 
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf Method2')
            Children(1):
                IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'Method2')
                  Children(1):
                      IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsInvalid, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Sub Method2(i As Integer)' does not have a signature compatible with delegate 'Delegate Sub Action()'.
        Dim a As Action = AddressOf Method2'BIND:"Dim a As Action = AddressOf Method2"
                                    ~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitAddressOf_InvalidArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Class C1
    End Class
    Sub Method1()
        Dim a As Action(Of String) = AddressOf Method2'BIND:"Dim a As Action(Of String) = AddressOf Method2"
    End Sub

    Sub Method2(i As C1)
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement, IsInvalid) (Syntax: 'Dim a As Ac ... sOf Method2')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action(Of System.String)
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String), IsInvalid, IsImplicit) (Syntax: 'AddressOf Method2')
        Target: 
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf Method2')
            Children(1):
                IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'Method2')
                  Children(1):
                      IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsInvalid, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Sub Method2(i As M1.C1)' does not have a signature compatible with delegate 'Delegate Sub Action(Of String)(obj As String)'.
        Dim a As Action(Of String) = AddressOf Method2'BIND:"Dim a As Action(Of String) = AddressOf Method2"
                                               ~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitAddressOf_DisallowedReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Func(Of String) = AddressOf Method2 'BIND:"Dim a As Func(Of String) = AddressOf Method2"
    End Sub

    Function Method2() As Integer
        Return 1
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement, IsInvalid) (Syntax: 'Dim a As Fu ... sOf Method2')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Func(Of System.String)
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.String), IsInvalid, IsImplicit) (Syntax: 'AddressOf Method2')
        Target: 
          IMethodReferenceExpression: Function M1.Method2() As System.Int32 (OperationKind.MethodReferenceExpression, Type: null, IsInvalid) (Syntax: 'AddressOf Method2')
            Instance Receiver: 
              IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsInvalid, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36663: Option Strict On does not allow narrowing in implicit type conversions between method 'Public Function Method2() As Integer' and delegate 'Delegate Function Func(Of String)() As String'.
        Dim a As Func(Of String) = AddressOf Method2 'BIND:"Dim a As Func(Of String) = AddressOf Method2"
                                             ~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitAddressOf_InvalidReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Func(Of String) = AddressOf Method2 'BIND:"Dim a As Func(Of String) = AddressOf Method2"
    End Sub

    Function Method2() As NonExistant
        Return New NonExistant
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement, IsInvalid) (Syntax: 'Dim a As Fu ... sOf Method2')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Func(Of System.String)
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.String), IsInvalid, IsImplicit) (Syntax: 'AddressOf Method2')
        Target: 
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf Method2')
            Children(1):
                IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'Method2')
                  Children(1):
                      IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsInvalid, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Function Method2() As NonExistant' does not have a signature compatible with delegate 'Delegate Function Func(Of String)() As String'.
        Dim a As Func(Of String) = AddressOf Method2 'BIND:"Dim a As Func(Of String) = AddressOf Method2"
                                             ~~~~~~~
BC30002: Type 'NonExistant' is not defined.
    Function Method2() As NonExistant
                          ~~~~~~~~~~~
BC30002: Type 'NonExistant' is not defined.
        Return New NonExistant
                   ~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitAddressOf_ReturnRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action = AddressOf Method2 'BIND:"Dim a As Action = AddressOf Method2"
    End Sub

    Function Method2() As Object
        Return 1
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement) (Syntax: 'Dim a As Ac ... sOf Method2')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action, IsImplicit) (Syntax: 'AddressOf Method2')
        Target: 
          IMethodReferenceExpression: Function M1.Method2() As System.Object (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf Method2')
            Instance Receiver: 
              IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitAddressOf_ArgumentRelaxation()
            Dim source = <![CDATA[
Option Strict Off
Imports System
Module M1
    Sub Method1()
        Dim a As Action(Of Integer) = AddressOf Method2'BIND:"Dim a As Action(Of Integer) = AddressOf Method2"
    End Sub

    Sub Method2()
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement) (Syntax: 'Dim a As Ac ... sOf Method2')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action(Of System.Int32)
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.Int32), IsImplicit) (Syntax: 'AddressOf Method2')
        Target: 
          IMethodReferenceExpression: Sub M1.Method2() (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf Method2')
            Instance Receiver: 
              IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitAddressOf_ConvertedToNonDelegateType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Class C1
    End Class
    Sub Method1()
        Dim a As String = AddressOf Method2'BIND:"Dim a As String = AddressOf Method2"
    End Sub

    Sub Method2(i As C1)
    End Sub
End Module]]>.Value

            ' We don't expect a delegate creation here. This is documenting that we still have a conversion expression when the target type
            ' isn't a delegate type
            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement, IsInvalid) (Syntax: 'Dim a As St ... sOf Method2')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.String
    Initializer: 
      IConversionExpression (Implicit, TryCast: False, Unchecked) (OperationKind.ConversionExpression, Type: System.String, IsInvalid, IsImplicit) (Syntax: 'AddressOf Method2')
        Conversion: CommonConversion (Exists: False, IsIdentity: False, IsNumeric: False, IsReference: False, IsUserDefined: False) (MethodSymbol: null)
        Operand: 
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf Method2')
            Children(1):
                IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'Method2')
                  Children(1):
                      IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsInvalid, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30581: 'AddressOf' expression cannot be converted to 'String' because 'String' is not a delegate type.
        Dim a As String = AddressOf Method2'BIND:"Dim a As String = AddressOf Method2"
                          ~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeAddressOf()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim a As Action = CType(AddressOf M1, Action)'BIND:"CType(AddressOf M1, Action)"
    End Sub

    Sub M2()
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'CType(Addre ... M1, Action)')
  Target: 
    IMethodReferenceExpression: Sub Program.M1() (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf M1')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsImplicit) (Syntax: 'M1')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeAddressOf_WithReceiver()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action = CType(AddressOf o.ToString, Action)'BIND:"CType(AddressOf o.ToString, Action)"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'CType(Addre ... ng, Action)')
  Target: 
    IMethodReferenceExpression: Function System.Object.ToString() As System.String (IsVirtual) (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf o.ToString')
      Instance Receiver: 
        ILocalReferenceExpression: o (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'o')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeAddressOf_DisallowedArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action = CType(AddressOf M2, Action)'BIND:"CType(AddressOf M2, Action)"
    End Sub

    Sub M2(i As Integer)
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action, IsInvalid) (Syntax: 'CType(Addre ... M2, Action)')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf M2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'M2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsInvalid, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Sub M2(i As Integer)' does not have a signature compatible with delegate 'Delegate Sub Action()'.
        Dim a As Action = CType(AddressOf M2, Action)'BIND:"CType(AddressOf M2, Action)"
                                          ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeAddressOf_InvalidArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Class C1
    End Class
    Sub Method1()
        Dim a As Action(Of String) = CType(AddressOf Method2, Action(Of String))'BIND:"CType(AddressOf Method2, Action(Of String))"
    End Sub

    Sub Method2(i As C1)
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String), IsInvalid) (Syntax: 'CType(Addre ... Of String))')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf Method2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'Method2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsInvalid, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Sub Method2(i As M1.C1)' does not have a signature compatible with delegate 'Delegate Sub Action(Of String)(obj As String)'.
        Dim a As Action(Of String) = CType(AddressOf Method2, Action(Of String))'BIND:"CType(AddressOf Method2, Action(Of String))"
                                                     ~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeAddressOf_InvalidReturnConversion()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Func(Of String) = CType(AddressOf M2, Func(Of String))'BIND:"CType(AddressOf M2, Func(Of String))"
    End Sub

    Sub M2()
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.String), IsInvalid) (Syntax: 'CType(Addre ... Of String))')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf M2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'M2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsInvalid, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Sub M2()' does not have a signature compatible with delegate 'Delegate Function Func(Of String)() As String'.
        Dim a As Func(Of String) = CType(AddressOf M2, Func(Of String))'BIND:"CType(AddressOf M2, Func(Of String))"
                                                   ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeAddressOf_InvalidVariableType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action(Of String) = CType(AddressOf M2, Action(Of Integer))'BIND:"CType(AddressOf M2, Action(Of Integer))"
    End Sub

    Sub M2(i As Integer)
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.Int32), IsInvalid) (Syntax: 'CType(Addre ... f Integer))')
  Target: 
    IMethodReferenceExpression: Sub Program.M2(i As System.Int32) (OperationKind.MethodReferenceExpression, Type: null, IsInvalid) (Syntax: 'AddressOf M2')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsInvalid, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC36755: 'Action(Of Integer)' cannot be converted to 'Action(Of String)' because 'String' is not derived from 'Integer', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        Dim a As Action(Of String) = CType(AddressOf M2, Action(Of Integer))'BIND:"CType(AddressOf M2, Action(Of Integer))"
                                     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeAddressOf_ReturnRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action = CType(AddressOf M2, Action)'BIND:"CType(AddressOf M2, Action)"
    End Sub

    Function M2() As Integer
        Return 1
    End Function
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'CType(Addre ... M2, Action)')
  Target: 
    IMethodReferenceExpression: Function Program.M2() As System.Int32 (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf M2')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_CTypeAddressOf_ArgumentRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action(Of String) = CType(AddressOf M2, Action(Of String))'BIND:"CType(AddressOf M2, Action(Of String))"
    End Sub

    Sub M2(o As Object)
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String)) (Syntax: 'CType(Addre ... Of String))')
  Target: 
    IMethodReferenceExpression: Sub Program.M2(o As System.Object) (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf M2')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of CTypeExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastAddressOf()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action = DirectCast(AddressOf M2, Action)'BIND:"DirectCast(AddressOf M2, Action)"
    End Sub

    Sub M2()
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'DirectCast( ... M2, Action)')
  Target: 
    IMethodReferenceExpression: Sub Program.M2() (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf M2')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastAddressOf_WithReceiver()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action = DirectCast(AddressOf o.ToString, Action)'BIND:"DirectCast(AddressOf o.ToString, Action)"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'DirectCast( ... ng, Action)')
  Target: 
    IMethodReferenceExpression: Function System.Object.ToString() As System.String (IsVirtual) (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf o.ToString')
      Instance Receiver: 
        ILocalReferenceExpression: o (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'o')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastAddressOf_DisallowedReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Func(Of Object) = DirectCast(AddressOf M2, Func(Of Object))'BIND:"DirectCast(AddressOf M2, Func(Of Object))"
    End Sub

    Sub M2()
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.Object), IsInvalid) (Syntax: 'DirectCast( ... Of Object))')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf M2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'M2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsInvalid, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Sub M2()' does not have a signature compatible with delegate 'Delegate Function Func(Of Object)() As Object'.
        Dim a As Func(Of Object) = DirectCast(AddressOf M2, Func(Of Object))'BIND:"DirectCast(AddressOf M2, Func(Of Object))"
                                                        ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastAddressOf_InvalidReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Func(Of Object) = DirectCast(AddressOf M2, Func(Of Object))'BIND:"DirectCast(AddressOf M2, Func(Of Object))"
    End Sub

    Function M2() As NonExistant
        Return New NonExistant
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.Object), IsInvalid) (Syntax: 'DirectCast( ... Of Object))')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf M2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'M2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsInvalid, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Function M2() As NonExistant' does not have a signature compatible with delegate 'Delegate Function Func(Of Object)() As Object'.
        Dim a As Func(Of Object) = DirectCast(AddressOf M2, Func(Of Object))'BIND:"DirectCast(AddressOf M2, Func(Of Object))"
                                                        ~~
BC30002: Type 'NonExistant' is not defined.
    Function M2() As NonExistant
                     ~~~~~~~~~~~
BC30002: Type 'NonExistant' is not defined.
        Return New NonExistant
                   ~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastAddressOf_DisallowedArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action = DirectCast(AddressOf M2, Action)'BIND:"DirectCast(AddressOf M2, Action)"
    End Sub

    Sub M2(s As Integer)
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action, IsInvalid) (Syntax: 'DirectCast( ... M2, Action)')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf M2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'M2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsInvalid, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Sub M2(s As Integer)' does not have a signature compatible with delegate 'Delegate Sub Action()'.
        Dim a As Action = DirectCast(AddressOf M2, Action)'BIND:"DirectCast(AddressOf M2, Action)"
                                               ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastAddressOf_InvalidArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Class C1
    End Class
    Sub Method1()
        Dim a As Action(Of String) = DirectCast(AddressOf Method2, Action(Of String))'BIND:"DirectCast(AddressOf Method2, Action(Of String))"
    End Sub

    Sub Method2(i As C1)
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String), IsInvalid) (Syntax: 'DirectCast( ... Of String))')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf Method2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'Method2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsInvalid, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Sub Method2(i As M1.C1)' does not have a signature compatible with delegate 'Delegate Sub Action(Of String)(obj As String)'.
        Dim a As Action(Of String) = DirectCast(AddressOf Method2, Action(Of String))'BIND:"DirectCast(AddressOf Method2, Action(Of String))"
                                                          ~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastAddressOf_InvalidVariableType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action = DirectCast(AddressOf M2, Action(Of String))'BIND:"DirectCast(AddressOf M2, Action(Of String))"
    End Sub

    Sub M2(s As String)
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String), IsInvalid) (Syntax: 'DirectCast( ... Of String))')
  Target: 
    IMethodReferenceExpression: Sub Program.M2(s As System.String) (OperationKind.MethodReferenceExpression, Type: null, IsInvalid) (Syntax: 'AddressOf M2')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsInvalid, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30311: Value of type 'Action(Of String)' cannot be converted to 'Action'.
        Dim a As Action = DirectCast(AddressOf M2, Action(Of String))'BIND:"DirectCast(AddressOf M2, Action(Of String))"
                          ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastAddressOf_ArgumentRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action(Of String) = DirectCast(AddressOf M2, Action(Of String))'BIND:"DirectCast(AddressOf M2, Action(Of String))"
    End Sub

    Sub M2()
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String)) (Syntax: 'DirectCast( ... Of String))')
  Target: 
    IMethodReferenceExpression: Sub Program.M2() (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf M2')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DirectCastAddressOf_ReturnRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action = DirectCast(AddressOf M2, Action)'BIND:"DirectCast(AddressOf M2, Action)"
    End Sub

    Function M2() As Integer
        Return 1
    End Function
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'DirectCast( ... M2, Action)')
  Target: 
    IMethodReferenceExpression: Function Program.M2() As System.Int32 (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf M2')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of DirectCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastAddressOf()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action = TryCast(AddressOf M2, Action)'BIND:"TryCast(AddressOf M2, Action)"
    End Sub

    Sub M2()
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'TryCast(Add ... M2, Action)')
  Target: 
    IMethodReferenceExpression: Sub Program.M2() (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf M2')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastAddressOf_WithReceiver()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action = TryCast(AddressOf o.ToString, Action)'BIND:"TryCast(AddressOf o.ToString, Action)"
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'TryCast(Add ... ng, Action)')
  Target: 
    IMethodReferenceExpression: Function System.Object.ToString() As System.String (IsVirtual) (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf o.ToString')
      Instance Receiver: 
        ILocalReferenceExpression: o (OperationKind.LocalReferenceExpression, Type: System.Object) (Syntax: 'o')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastAddressOf_DisallowedReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Func(Of Object) = TryCast(AddressOf M2, Func(Of Object))'BIND:"TryCast(AddressOf M2, Func(Of Object))"
    End Sub

    Sub M2()
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.Object), IsInvalid) (Syntax: 'TryCast(Add ... Of Object))')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf M2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'M2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsInvalid, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Sub M2()' does not have a signature compatible with delegate 'Delegate Function Func(Of Object)() As Object'.
        Dim a As Func(Of Object) = TryCast(AddressOf M2, Func(Of Object))'BIND:"TryCast(AddressOf M2, Func(Of Object))"
                                                     ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastAddressOf_InvalidReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Func(Of Object) = TryCast(AddressOf M2, Func(Of Object))'BIND:"TryCast(AddressOf M2, Func(Of Object))"
    End Sub

    Function M2() As NonExistant
        Return NonExistant
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.Object), IsInvalid) (Syntax: 'TryCast(Add ... Of Object))')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf M2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'M2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsInvalid, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Function M2() As NonExistant' does not have a signature compatible with delegate 'Delegate Function Func(Of Object)() As Object'.
        Dim a As Func(Of Object) = TryCast(AddressOf M2, Func(Of Object))'BIND:"TryCast(AddressOf M2, Func(Of Object))"
                                                     ~~
BC30002: Type 'NonExistant' is not defined.
    Function M2() As NonExistant
                     ~~~~~~~~~~~
BC30451: 'NonExistant' is not declared. It may be inaccessible due to its protection level.
        Return NonExistant
               ~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastAddressOf_DisallowedArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action = TryCast(AddressOf M2, Action)'BIND:"TryCast(AddressOf M2, Action)"
    End Sub

    Sub M2(s As Integer)
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action, IsInvalid) (Syntax: 'TryCast(Add ... M2, Action)')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf M2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'M2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsInvalid, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Sub M2(s As Integer)' does not have a signature compatible with delegate 'Delegate Sub Action()'.
        Dim a As Action = TryCast(AddressOf M2, Action)'BIND:"TryCast(AddressOf M2, Action)"
                                            ~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastAddressOf_InvalidArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Class C1
    End Class
    Sub Method1()
        Dim a As Action(Of String) = TryCast(AddressOf Method2, Action(Of String))'BIND:"TryCast(AddressOf Method2, Action(Of String))"
    End Sub

    Sub Method2(i As C1)
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String), IsInvalid) (Syntax: 'TryCast(Add ... Of String))')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf Method2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'Method2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsInvalid, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Sub Method2(i As M1.C1)' does not have a signature compatible with delegate 'Delegate Sub Action(Of String)(obj As String)'.
        Dim a As Action(Of String) = TryCast(AddressOf Method2, Action(Of String))'BIND:"TryCast(AddressOf Method2, Action(Of String))"
                                                       ~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastAddressOf_InvalidVariableType()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action = TryCast(AddressOf M2, Action(Of String))'BIND:"TryCast(AddressOf M2, Action(Of String))"
    End Sub

    Sub M2(s As String)
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String), IsInvalid) (Syntax: 'TryCast(Add ... Of String))')
  Target: 
    IMethodReferenceExpression: Sub Program.M2(s As System.String) (OperationKind.MethodReferenceExpression, Type: null, IsInvalid) (Syntax: 'AddressOf M2')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsInvalid, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC30311: Value of type 'Action(Of String)' cannot be converted to 'Action'.
        Dim a As Action = TryCast(AddressOf M2, Action(Of String))'BIND:"TryCast(AddressOf M2, Action(Of String))"
                          ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastAddressOf_ArgumentRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action(Of String) = TryCast(AddressOf M2, Action(Of String))'BIND:"TryCast(AddressOf M2, Action(Of String))"
    End Sub

    Sub M2()
    End Sub
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String)) (Syntax: 'TryCast(Add ... Of String))')
  Target: 
    IMethodReferenceExpression: Sub Program.M2() (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf M2')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_TryCastAddressOf_ReturnRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System

Module Program
    Sub M1()
        Dim o As New Object
        Dim a As Action = TryCast(AddressOf M2, Action)'BIND:"TryCast(AddressOf M2, Action)"
    End Sub

    Function M2() As Integer
        Return 1
    End Function
End Module
]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'TryCast(Add ... M2, Action)')
  Target: 
    IMethodReferenceExpression: Function Program.M2() As System.Int32 (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf M2')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: Program, IsImplicit) (Syntax: 'M2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of TryCastExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationAddressOfArgument()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action = New Action(AddressOf Method2)'BIND:"New Action(AddressOf Method2)"
    End Sub

    Sub Method2()
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'New Action( ... Of Method2)')
  Target: 
    IMethodReferenceExpression: Sub M1.Method2() (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf Method2')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of ObjectCreationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationAdressOfArgument_MultipleArgumentsToConstructor()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action = New Action(AddressOf Method2, 1)'BIND:"Dim a As Action = New Action(AddressOf Method2, 1)"
    End Sub

    Sub Method2()
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement, IsInvalid) (Syntax: 'Dim a As Ac ... Method2, 1)')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As System.Action
    Initializer: 
      IInvalidExpression (OperationKind.InvalidExpression, Type: System.Action, IsInvalid) (Syntax: 'New Action( ... Method2, 1)')
        Children(2):
            IInvalidExpression (OperationKind.InvalidExpression, Type: ?, IsInvalid, IsImplicit) (Syntax: 'AddressOf Method2')
              Children(1):
                  IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf Method2')
                    Children(1):
                        IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'Method2')
                          Children(1):
                              IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsInvalid, IsImplicit) (Syntax: 'Method2')
            ILiteralExpression (OperationKind.LiteralExpression, Type: System.Int32, Constant: 1, IsInvalid) (Syntax: '1')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC32008: Delegate 'Action' requires an 'AddressOf' expression or lambda expression as the only argument to its constructor.
        Dim a As Action = New Action(AddressOf Method2, 1)'BIND:"Dim a As Action = New Action(AddressOf Method2, 1)"
                                    ~~~~~~~~~~~~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationAddressOfArgument_ReturnRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action = New Action(AddressOf Method2)'BIND:"New Action(AddressOf Method2)"
    End Sub

    Function Method2() As Object
        Return 1
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action) (Syntax: 'New Action( ... Of Method2)')
  Target: 
    IMethodReferenceExpression: Function M1.Method2() As System.Object (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf Method2')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of ObjectCreationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationAddressOfArgument_ArgumentRelaxation()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action(Of Integer) = New Action(Of Integer)(AddressOf Method2)'BIND:"New Action(Of Integer)(AddressOf Method2)"
    End Sub

    Sub Method2(o As Object)
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.Int32)) (Syntax: 'New Action( ... Of Method2)')
  Target: 
    IMethodReferenceExpression: Sub M1.Method2(o As System.Object) (OperationKind.MethodReferenceExpression, Type: null) (Syntax: 'AddressOf Method2')
      Instance Receiver: 
        IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of ObjectCreationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationAddressOfArgument_DisallowedArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Action= New Action(AddressOf Method2)'BIND:"New Action(AddressOf Method2)"
    End Sub

    Sub Method2(o As Object)
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action, IsInvalid) (Syntax: 'New Action( ... Of Method2)')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf Method2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'Method2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsInvalid, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Sub Method2(o As Object)' does not have a signature compatible with delegate 'Delegate Sub Action()'.
        Dim a As Action= New Action(AddressOf Method2)'BIND:"New Action(AddressOf Method2)"
                                              ~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of ObjectCreationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationAddressOfArgument_InvalidArgumentType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Class C1
    End Class
    Sub Method1()
        Dim a As Action(Of String) = New Action(Of String)(AddressOf Method2)'BIND:"New Action(Of String)(AddressOf Method2)"
    End Sub

    Sub Method2(i As C1)
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Action(Of System.String), IsInvalid) (Syntax: 'New Action( ... Of Method2)')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf Method2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'Method2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsInvalid, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Sub Method2(i As M1.C1)' does not have a signature compatible with delegate 'Delegate Sub Action(Of String)(obj As String)'.
        Dim a As Action(Of String) = New Action(Of String)(AddressOf Method2)'BIND:"New Action(Of String)(AddressOf Method2)"
                                                                     ~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of ObjectCreationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationAddressOfArgument_DisallowedReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Func(Of String) = New Func(Of String)(AddressOf Method2)'BIND:"New Func(Of String)(AddressOf Method2)"
    End Sub

    Sub Method2()
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.String), IsInvalid) (Syntax: 'New Func(Of ... Of Method2)')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf Method2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'Method2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsInvalid, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Sub Method2()' does not have a signature compatible with delegate 'Delegate Function Func(Of String)() As String'.
        Dim a As Func(Of String) = New Func(Of String)(AddressOf Method2)'BIND:"New Func(Of String)(AddressOf Method2)"
                                                                 ~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of ObjectCreationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_DelegateCreationAddressOfArgument_InvalidReturnType()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a As Func(Of String) = New Func(Of String)(AddressOf Method2)'BIND:"New Func(Of String)(AddressOf Method2)"
    End Sub

    Function Method2() As NonExistant
        Return New NonExistant()
    End Function
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: System.Func(Of System.String), IsInvalid) (Syntax: 'New Func(Of ... Of Method2)')
  Target: 
    IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'AddressOf Method2')
      Children(1):
          IOperation:  (OperationKind.None, IsInvalid) (Syntax: 'Method2')
            Children(1):
                IInstanceReferenceExpression (OperationKind.InstanceReferenceExpression, Type: M1, IsInvalid, IsImplicit) (Syntax: 'Method2')
]]>.Value

            Dim expectedDiagnostics = <![CDATA[
BC31143: Method 'Public Function Method2() As NonExistant' does not have a signature compatible with delegate 'Delegate Function Func(Of String)() As String'.
        Dim a As Func(Of String) = New Func(Of String)(AddressOf Method2)'BIND:"New Func(Of String)(AddressOf Method2)"
                                                                 ~~~~~~~
BC30002: Type 'NonExistant' is not defined.
    Function Method2() As NonExistant
                          ~~~~~~~~~~~
BC30002: Type 'NonExistant' is not defined.
        Return New NonExistant()
                   ~~~~~~~~~~~
]]>.Value

            VerifyOperationTreeAndDiagnosticsForTest(Of ObjectCreationExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub
#End Region

#Region "Anonymous Delegates"

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitAnonymousDelegateConversion()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a = Sub()'BIND:"Dim a = Sub()"
                End Sub
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement) (Syntax: 'Dim a = Sub ... End Sub')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As Sub <generated method>()
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: Sub <generated method>(), IsImplicit) (Syntax: 'Sub()'BIND: ... End Sub')
        Target: 
          IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Sub()'BIND: ... End Sub')
            IBlockStatement (2 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub()'BIND: ... End Sub')
              ILabeledStatement (Label: exit) (OperationKind.LabeledStatement) (Syntax: 'End Sub')
                Statement: 
                  null
              IReturnStatement (OperationKind.ReturnStatement) (Syntax: 'End Sub')
                ReturnedValue: 
                  null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitAnonymousDelegateConversion_JustInitializerReturnsOnlyLambda()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a = Sub()'BIND:"Sub()"
                End Sub
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Sub()'BIND: ... End Sub')
  IBlockStatement (2 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub()'BIND: ... End Sub')
    ILabeledStatement (Label: exit) (OperationKind.LabeledStatement) (Syntax: 'End Sub')
      Statement: 
        null
    IReturnStatement (OperationKind.ReturnStatement) (Syntax: 'End Sub')
      ReturnedValue: 
        null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of MultiLineLambdaExpressionSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

        <CompilerTrait(CompilerFeature.IOperation)>
        <Fact()>
        Public Sub DelegateCreationExpression_ImplicitAnonymousDelegateConversion_SingleLineLambda()
            Dim source = <![CDATA[
Option Strict On
Imports System
Module M1
    Sub Method1()
        Dim a = Sub() Console.WriteLine()'BIND:"Dim a = Sub() Console.WriteLine()"
    End Sub
End Module]]>.Value

            Dim expectedOperationTree = <![CDATA[
IVariableDeclarationStatement (1 declarations) (OperationKind.VariableDeclarationStatement) (Syntax: 'Dim a = Sub ... WriteLine()')
  IVariableDeclaration (1 variables) (OperationKind.VariableDeclaration) (Syntax: 'a')
    Variables: Local_1: a As Sub <generated method>()
    Initializer: 
      IDelegateCreationExpression (OperationKind.DelegateCreationExpression, Type: Sub <generated method>(), IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
        Target: 
          IAnonymousFunctionExpression (Symbol: Sub ()) (OperationKind.AnonymousFunctionExpression, Type: null) (Syntax: 'Sub() Conso ... WriteLine()')
            IBlockStatement (3 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
              IBlockStatement (1 statements) (OperationKind.BlockStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
                IExpressionStatement (OperationKind.ExpressionStatement) (Syntax: 'Console.WriteLine()')
                  Expression: 
                    IInvocationExpression (Sub System.Console.WriteLine()) (OperationKind.InvocationExpression, Type: System.Void) (Syntax: 'Console.WriteLine()')
                      Instance Receiver: 
                        null
                      Arguments(0)
              ILabeledStatement (Label: exit) (OperationKind.LabeledStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
                Statement: 
                  null
              IReturnStatement (OperationKind.ReturnStatement, IsImplicit) (Syntax: 'Sub() Conso ... WriteLine()')
                ReturnedValue: 
                  null
]]>.Value

            Dim expectedDiagnostics = String.Empty

            VerifyOperationTreeAndDiagnosticsForTest(Of LocalDeclarationStatementSyntax)(source, expectedOperationTree, expectedDiagnostics)
        End Sub

#End Region

    End Class
End Namespace
