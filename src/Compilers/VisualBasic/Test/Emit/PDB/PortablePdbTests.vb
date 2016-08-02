﻿' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Roslyn.Test.Utilities
Imports System.IO
Imports System.Reflection.Metadata
Imports Microsoft.CodeAnalysis.Emit
Imports Microsoft.CodeAnalysis.Test.Utilities
Imports System.Reflection.PortableExecutable

Namespace Microsoft.CodeAnalysis.VisualBasic.UnitTests.PDB
    Public Class PortablePdbTests
        Inherits BasicTestBase

        <Fact()>
        Public Sub DateTimeConstant()
            Dim source = <compilation>
                             <file>
Imports System

Public Class C
    Public Sub M()
        const dt1 as datetime = #3/01/2016#
        const dt2 as datetime = #10:53:37 AM#
        const dt3 as datetime = #3/01/2016 10:53:37 AM#
    End Sub
End Class
</file>
                         </compilation>

            Dim compilation = CompilationUtils.CreateCompilationWithMscorlibAndVBRuntime(
                source,
                TestOptions.DebugDll)

            Dim pdbStream = New MemoryStream()
            compilation.EmitToArray(EmitOptions.Default.WithDebugInformationFormat(DebugInformationFormat.PortablePdb), pdbStream:=pdbStream)

            Using pdbMetadata As New PinnedMetadata(pdbStream.ToImmutable())
                Dim mdReader = pdbMetadata.Reader

                Assert.Equal(3, mdReader.LocalConstants.Count)

                For Each constantHandle In mdReader.LocalConstants
                    Dim constant = mdReader.GetLocalConstant(constantHandle)
                    Dim sigReader = mdReader.GetBlobReader(constant.Signature)

                    ' DateTime constants are always SignatureTypeCode.ValueType {17}
                    Dim rawTypeCode = sigReader.ReadCompressedInteger()
                    Assert.Equal(17, rawTypeCode)

                    ' DateTime constants are always HandleKind.TypeReference {1}
                    Dim typeHandle = sigReader.ReadTypeHandle()
                    Assert.Equal(HandleKind.TypeReference, typeHandle.Kind)

                    ' DateTime constants are always stored and retrieved with no time zone specification
                    Dim value = sigReader.ReadDateTime()
                    Assert.Equal(DateTimeKind.Unspecified, value.Kind)
                Next
            End Using
        End Sub

        <Fact>
        Public Sub EmbeddedPortablePdb()
            Dim source = "
Imports System

Class C
    Public Shared Sub Main()
        Console.WriteLine()
    End Sub
End Class
"
            Dim c = CreateCompilationWithMscorlib(Parse(source, "foo.vb"), options:=TestOptions.DebugDll)
            Dim peBlob = c.EmitToArray(EmitOptions.Default.WithDebugInformationFormat(DebugInformationFormat.Embedded).WithPdbFilePath("a/b/c/d.pdb"))

            Using peReader = New PEReader(peBlob)
                Dim entries = peReader.ReadDebugDirectory()

                AssertEx.Equal({DebugDirectoryEntryType.CodeView, DebugDirectoryEntryType.EmbeddedPortablePdb}, entries.Select(Function(e) e.Type))

                Dim codeView = entries(0)
                Dim embedded = entries(1)

                ' EmbeddedPortablePdb entry
                Assert.Equal(&H100, embedded.MajorVersion)
                Assert.Equal(&H100, embedded.MinorVersion)
                Assert.Equal(CUInt(0), embedded.Stamp)

                Dim pdbId As BlobContentId
                Using embeddedMetadataProvider = peReader.ReadEmbeddedPortablePdbDebugDirectoryData(embedded)
                    Dim mdReader = embeddedMetadataProvider.GetMetadataReader()
                    AssertEx.Equal({"foo.vb"}, mdReader.Documents.Select(Function(doc) mdReader.GetString(mdReader.GetDocument(doc).Name)))
                    pdbId = New BlobContentId(mdReader.DebugMetadataHeader.Id)
                End Using

                ' CodeView entry:
                Dim codeViewData = peReader.ReadCodeViewDebugDirectoryData(codeView)
                Assert.Equal(&H100, codeView.MajorVersion)
                Assert.Equal(&H504D, codeView.MinorVersion)
                Assert.Equal(pdbId.Stamp, codeView.Stamp)
                Assert.Equal(pdbId.Guid, codeViewData.Guid)
                Assert.Equal("d.pdb", codeViewData.Path)
            End Using
        End Sub

        <Fact>
        Public Sub EmbeddedPortablePdb_Deterministic()
            Dim source = "
Imports System

Class C
    Public Shared Sub Main()
        Console.WriteLine()
    End Sub
End Class
"
            Dim c = CreateCompilationWithMscorlib(Parse(source, "foo.vb"), options:=TestOptions.DebugDll.WithDeterministic(True))
            Dim peBlob = c.EmitToArray(EmitOptions.Default.WithDebugInformationFormat(DebugInformationFormat.Embedded).WithPdbFilePath("a/b/c/d.pdb"))

            Using peReader = New PEReader(peBlob)
                Dim entries = peReader.ReadDebugDirectory()

                AssertEx.Equal({DebugDirectoryEntryType.CodeView, DebugDirectoryEntryType.Reproducible, DebugDirectoryEntryType.EmbeddedPortablePdb}, entries.Select(Function(e) e.Type))

                Dim codeView = entries(0)
                Dim reproducible = entries(1)
                Dim embedded = entries(2)

                ' EmbeddedPortablePdb entry
                Assert.Equal(&H100, embedded.MajorVersion)
                Assert.Equal(&H100, embedded.MinorVersion)
                Assert.Equal(CUInt(0), embedded.Stamp)

                Dim pdbId As BlobContentId
                Using embeddedMetadataProvider = peReader.ReadEmbeddedPortablePdbDebugDirectoryData(embedded)
                    Dim mdReader = embeddedMetadataProvider.GetMetadataReader()
                    AssertEx.Equal({"foo.vb"}, mdReader.Documents.Select(Function(doc) mdReader.GetString(mdReader.GetDocument(doc).Name)))
                    pdbId = New BlobContentId(mdReader.DebugMetadataHeader.Id)
                End Using

                ' CodeView entry:
                Dim codeViewData = peReader.ReadCodeViewDebugDirectoryData(codeView)
                Assert.Equal(&H100, codeView.MajorVersion)
                Assert.Equal(&H504D, codeView.MinorVersion)
                Assert.Equal(pdbId.Stamp, codeView.Stamp)
                Assert.Equal(pdbId.Guid, codeViewData.Guid)
                Assert.Equal("d.pdb", codeViewData.Path)

                ' Reproducible entry
                Assert.Equal(0, reproducible.MajorVersion)
                Assert.Equal(0, reproducible.MinorVersion)
                Assert.Equal(CUInt(0), reproducible.Stamp)
                Assert.Equal(0, reproducible.DataSize)
            End Using
        End Sub

    End Class
End Namespace
