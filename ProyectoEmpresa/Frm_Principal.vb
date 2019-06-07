Public Class Frm_Principal
    Private Sub For1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub AgregarNuevoToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim FAR As New Frm_AgregarRegistros
        FAR.ShowDialog()
    End Sub

    Private Sub ModificarToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim FMR As New Frm_ModificarRegistros
        FMR.ShowDialog()
    End Sub

    Private Sub EliminarToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim FER As New Frm_EliminarRegistros
        FER.ShowDialog()
    End Sub

    Private Sub SalirDelSistemaToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub ListarRegistroToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim Frm As New Frm_ListarRegistros
        Frm_ListarRegistros.Show()
    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub BtnCMMBrowseCSV_Click(sender As Object, e As EventArgs) Handles BtnCMMBrowseCSV.Click
        CDFileCSV.Title = "Seleccione Archivo CSV"
        CDFileCSV.InitialDirectory = My.Application.Info.DirectoryPath
        CDFileCSV.FileName = ""
        CDFileCSV.ShowDialog()
        TextBox_FileName.Text = CDFileCSV.FileName
        'TxtUpdateCSV.Text = System.IO.File.ReadAllText(TxtFileCSV.Text)
        'llenaCMMGrillaUpdate()
    End Sub

    Private Sub BtnCMMOpenFile_Click(sender As Object, e As EventArgs)

    End Sub
End Class
