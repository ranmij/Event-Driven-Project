' This is for me

Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports System.Data
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSetTableAdapters

Public Class ProductDialog
    Private _DATA_GRID As DataGrid = Nothing
    Private _productAdapter As New productTableAdapter
    Private _productData As ProductData
    Private _parent As Window
    Public Sub New(parent As Window, ByRef datagridview As DataGrid, product_id As String)
        InitializeComponent()
        ' Serves as data source to bind to the user interface Dialog
        Dim datarow As DataTable = ProductDataProperty(product_id)
        Dim dataproperty As New List(Of String)

        With datarow.Rows.Item(0)
            For index = 0 To .ItemArray.Count - 1
                dataproperty.Add(.ItemArray(index))
            Next
        End With
        _productData = New ProductData With {
            .PRODUCT_ID = product_id,
            .PRODUCT_CODE = dataproperty(0),
            .PRODUCT_NAME = dataproperty(1),
            .PRODUCT_PRICE = dataproperty(2),
            .PRODUCT_STOCKS = dataproperty(3),
            .PRODUCT_IMAGE = dataproperty(4)
        }

        ' Initialize the datagrid so that we can use its reference from the callee
        _DATA_GRID = datagridview

        ' Bind the data source to the user interface
        Me.DataContext = _productData

        ' Fix the width and height of the Dialog
        Me.Width = parent.MinWidth - 100
        Me.Height = parent.MinHeight - 100
        _parent = parent
    End Sub

    Private Sub ClickEvents(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click, DeleteButton.Click
        ' Did user clicked the save button?
        If sender.Equals(SaveButton) Then
            Dim controls() As Object = {CodeTextBox, NameTextBox, PriceTextBox, StocksTextBox}
            If Not IsNotEmpty(controls) Then
                ' TODO CHECK IF THE INPUTS ARE CORRECT
                With _productData
                    ' Check if the update operation succeed
                    If _productAdapter.UpdateQueryProduct(.PRODUCT_CODE, .PRODUCT_NAME, .PRODUCT_STOCKS, .PRODUCT_PRICE, .PRODUCT_ID) <> 0 Then
                        HandyControl.Controls.MessageBox.Info("Product has been updated successfully.", "Update Success")
                        '_productAdapter.FillByProducts(_productTable)
                        _DATA_GRID.ItemsSource = Nothing                                            ' Refresh the data of datagrid
                        _DATA_GRID.ItemsSource = GetDataGridByProduct().DefaultView                                           ' Fill the data of the datagrid
                    Else
                        ' Display the error if the update operation did not succeed
                        HandyControl.Controls.MessageBox.Info("Failed to update the product.", "Update failed")
                    End If
                End With
                For Each control In controls
                    If String.IsNullOrEmpty(control.Text) Then
                        control.BorderBrush = Brushes.Gray
                    End If
                Next
            Else
                For Each control In controls
                    If String.IsNullOrEmpty(control.Text) Then
                        control.BorderBrush = Brushes.Red
                    End If
                Next
            End If
        ElseIf sender.Equals(DeleteButton) Then ' Is the delete button clicked?
            With _productData
                ' Check if the delete operation succeed
                If _productAdapter.DeleteQueryProduct(.PRODUCT_ID) <> 0 Then
                    HandyControl.Controls.MessageBox.Info("Product has been deleted successfully.", "Delete Success")
                    '_productAdapter.FillByProducts(_productTable)
                    _DATA_GRID.ItemsSource = Nothing                        ' Refresh the data in the stocks datagrid view
                    _DATA_GRID.ItemsSource = GetDataGridByProduct().DefaultView
                    ' Invoke the close after deleting
                    Dim peer As ButtonAutomationPeer = TryCast(UIElementAutomationPeer.CreatePeerForElement(Closebtn), ButtonAutomationPeer)
                    ' If the peer variable has found the button
                    If peer IsNot Nothing Then
                        ' We invoke the click so that it the dialog will close
                        Dim provider As IInvokeProvider = TryCast(peer.GetPattern(PatternInterface.Invoke), IInvokeProvider)
                        If provider IsNot Nothing Then
                            provider.Invoke()
                        End If
                    End If
                Else
                    ' Display the error if the delete operation did not succeed
                    HandyControl.Controls.MessageBox.Info("Failed to delete the product.", "Delete failed")
                End If
            End With
        End If
        TryCast(_parent.FindName("StocksTotalItems"), TextBlock).Text = _productAdapter.ScalarQueryProducts()
    End Sub
End Class
