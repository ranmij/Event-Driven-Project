' This is for me

Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports System.Data
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSetTableAdapters
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSet

Public Class ProductDialog
    Private _DATA_GRID As DataGrid = Nothing
    Private _productAdapter As New productTableAdapter
    Private _productData As ProductData
    Private _parent As Window

    Private ReadOnly unitDataTable As New unitDataTable
    Private ReadOnly categoryDataTable As New categoryDataTable

    Private ReadOnly unitTableAdapter As New unitTableAdapter
    Private ReadOnly categoryTableAdapter As New categoryTableAdapter
    Private _id As Integer

    Public Sub New(parent As Window, ByRef datagridview As DataGrid, product_id As String)
        InitializeComponent()
        ' Serves as data source to bind to the user interface Dialog
        Dim datarow As DataTable = ProductDataProperty(product_id)

        _productData = New ProductData With {
            .PRODUCT_ID = product_id,
            .PRODUCT_CODE = datarow.Rows(0).Item("product_code"),
            .PRODUCT_NAME = datarow.Rows(0).Item("product_name"),
            .PRODUCT_PRICE = datarow.Rows(0).Item("unit_price"),
            .PRODUCT_STOCKS = datarow.Rows(0).Item("unit_in_stock"),
            .PRODUCT_IMAGE = datarow.Rows(0).Item("image_path")
        }

        _id = product_id

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
            If IsNotEmpty(controls) Then
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

    Private Sub ProductDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        categoryTableAdapter.Fill(categoryDataTable)
        unitTableAdapter.Fill(unitDataTable)

        ProductCategoryComboBox.ItemsSource = categoryDataTable
        ProductCategoryComboBox.DisplayMemberPath = "category"
        ProductCategoryComboBox.SelectedValuePath = "id"

        ProductUnitComboBox.ItemsSource = unitDataTable
        ProductUnitComboBox.DisplayMemberPath = "unit_name"
        ProductUnitComboBox.SelectedValuePath = "id"

        Dim ids() As Integer = GetProductCategoryAndUnit(_id)

        ProductCategoryComboBox.SelectedValue = ids(0)
        ProductUnitComboBox.SelectedValue = ids(1)
    End Sub
End Class
