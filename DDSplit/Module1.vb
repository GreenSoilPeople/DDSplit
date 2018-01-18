Imports System.IO

Module Module1

    Private InputFile As String
    Private OutputDir As String
    Private OutputFileBase As String
    Private OutputFileTVA As String

    Sub Main(args As String())

        If ParseArgs(args) <> 0 Then Exit Sub

        Dim lines As String() = File.ReadAllLines(InputFile)

        For Each line As String In lines

            Dim items As String() = line.Split(";")

            items(4) = items(4).Trim()

            Dim base As Double = Math.Round(items(4) / 1.19, 2)
            Dim tva As Double = items(4) - base

            items(4) = base

            File.AppendAllLines(OutputFileBase, {String.Join(";", items)})

            items(4) = tva

            File.AppendAllLines(OutputFileTVA, {String.Join(";", items)})
        Next

    End Sub

    Private Function ParseArgs(args As String()) As Integer
        If args.Count < 2 Then
            Console.WriteLine("Usage: conDD <InputFile> <OutputDir>")
            Return 1
        Else
            InputFile = args(0)
            OutputDir = args(1)
        End If

        If Not File.Exists(InputFile) Then
            Console.WriteLine($"File '{InputFile}' does not exist")
            Return 1
        End If

        Dim InputFileName As String = Path.GetFileName(InputFile)
        Dim Extension As String = Path.GetExtension(InputFile)

        Dim m1 = Text.RegularExpressions.Regex.Match(InputFile, "directdebit(\d+)")

        If Not String.IsNullOrWhiteSpace(m1.Value) Then
            Dim day As String = m1.Groups(1).Value.Substring(6, 2)
            Dim month As String = m1.Groups(1).Value.Substring(4, 2)
            Dim year As String = m1.Groups(1).Value.Substring(0, 4)
            Dim suffix As String = m1.Groups(1).Value.Substring(8)

            OutputFileBase = $"{OutputDir}\{day}_{month}_{year}_{suffix}_baza{Extension}"
            OutputFileTVA = $"{OutputDir}\{day}_{month}_{year}_{suffix}_TVA{Extension}"

            If File.Exists(OutputFileBase) Then File.Delete(OutputFileBase)
            If File.Exists(OutputFileTVA) Then File.Delete(OutputFileTVA)

        End If

        Return 0
    End Function

End Module
