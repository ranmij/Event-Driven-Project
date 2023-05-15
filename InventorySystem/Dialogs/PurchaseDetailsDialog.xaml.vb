Imports System.Data

Public Class PurchaseDetailsDialog
    Private _id As String
    Public Sub New(parent As Window, id As String)

        InitializeComponent()
        Dim dataTable As DataTable = GetOrderById(id)
        Debug.WriteLine("ID: " & id)
        CustomerNameTextBox.Text = dataTable.Rows(0).Item(0)
        OrderedDateTextBox.Text = dataTable.Rows(0).Item(5)
        ProductCodeTextBox.Text = dataTable.Rows(0).Item(1)
        ProductNameTextBox.Text = dataTable.Rows(0).Item(2)
        ProductPriceTextBox.Text = dataTable.Rows(0).Item(2)

        Me.Height = parent.MinHeight - 100
        Me.Width = parent.MinWidth - 100
        _id = id
    End Sub

    Private Sub PurchaseDetailsDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim dataTable As DataTable = GetOrderById(_id)
        Debug.WriteLine(dataTable.Rows(0).Item(7))
        CustomerNameTextBox.Text = dataTable.Rows(0).Item(0)
        ProductCodeTextBox.Text = dataTable.Rows(0).Item(1)
        ProductNameTextBox.Text = dataTable.Rows(0).Item(2)
        ProductPriceTextBox.Text = dataTable.Rows(0).Item(3)
        QuantityTextBox.Text = dataTable.Rows(0).Item(4)
        OrderedDateTextBox.Text = dataTable.Rows(0).Item(5)
        TotalPriceTextBox.Text = dataTable.Rows(0).Item(6)
        CustomerImage.Source = New BitmapImage(New Uri("pack://application:,,," & dataTable.Rows(0).Item(7)))
    End Sub
End Class
