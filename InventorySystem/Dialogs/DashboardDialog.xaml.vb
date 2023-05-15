Imports System.Data

Public Class DashboardDialog
    Public Sub New(parent As Window, heading As String, dataGrida As DataTable)
        InitializeComponent()
        HeadingReportTextBox.Text = heading
        ReportDataGridView.ItemsSource = dataGrida.DefaultView

        Me.Width = parent.MinWidth - 100
        Me.Height = parent.MinHeight - 100
    End Sub

    Private Sub SalesDataGridView_AutoGeneratingColumn(sender As Object, e As DataGridAutoGeneratingColumnEventArgs) Handles ReportDataGridView.AutoGeneratingColumn
        ' Check if which header should we hide
        Dim columnsToHide() As String = {"id", "unit_name", "category", "category_id"}
        If columnsToHide.Contains(e.Column.Header.ToString()) Then
            e.Column.Visibility = Visibility.Collapsed
        End If
    End Sub

End Class
