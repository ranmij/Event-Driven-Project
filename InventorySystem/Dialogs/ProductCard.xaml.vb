Option Strict On
' This is for me
Imports System.Data

Public Class ProductCard
    Public Sub New(id As String)
        InitializeComponent()

        Dim datatable As DataTable = GetProductByID(id)

        Dim propertyObject As New PropertyContainer With {
            .CARD_TITLE = datatable.Rows(0).Item(0).ToString,
            .CARD_DESC = datatable.Rows(0).Item(1).ToString,
            .IMAGE_PATH = datatable.Rows(0).Item(2).ToString
        }
        Me.DataContext = propertyObject
    End Sub
End Class


