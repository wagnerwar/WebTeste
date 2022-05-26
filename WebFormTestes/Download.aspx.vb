Imports System.Configuration
Public Class Download
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim caminhoPasta As String = ConfigurationManager.AppSettings("CaminhoDiretorio")
            Dim fileName As String = "olherite.pdf"
            Dim filePath As String = String.Format("{0}{1}", caminhoPasta, fileName)
            Dim extensao As String = GetExtensao(filePath)
            Dim mimeType As String = GetMimeTypeByExtensao(filePath)
            Response.AddHeader("Content-Disposition", String.Format("attachment; filename=dosagem{0}", extensao))
            Response.ContentType = mimeType
            Response.TransmitFile(filePath)
            Response.End()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function GetMimeTypeByExtensao(ByVal caminhoArquivo As String) As String
        Dim extensao As String = GetExtensao(caminhoArquivo)
        Dim mimeType As String = String.Empty
        If extensao = ".gif" Then
            mimeType = "image/gif"
        ElseIf extensao = ".jpg" Then
            mimeType = "image/jpeg"
        ElseIf extensao = ".png" Then
            mimeType = "image/png"
        ElseIf extensao = ".doc" Then
            mimeType = "application/msword"
        ElseIf extensao = ".pdf" Then
            mimeType = "application/pdf"
        End If
        Return mimeType
    End Function
    Private Function GetExtensao(ByVal caminhoArquivo As String) As String
        Dim ext As String = System.IO.Path.GetExtension(caminhoArquivo).ToLower()
        Return ext
    End Function

End Class