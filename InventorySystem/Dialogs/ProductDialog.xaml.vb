' This is for me

Imports System.Net.NetworkInformation
Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSet
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSetTableAdapters

Public Class ProductDialog
    Private _DATA_GRID As DataGrid = Nothing
    Private _productAdapter As New productTableAdapter
    Private _productTable As New productDataTable
    Private _productData As ProductData

    ' I know that this constructor has too many parameters, but I have to use this for now, very unhealthy.
    Public Sub New(parent As Window, ByRef datagridview As DataGrid, product_id As String, product_code As String, product_name As String, product_stock As String, product_price As String, image_path As String)
        InitializeComponent()
        ' Serves as data source to bind to the user interface Dialog
        _productData = New ProductData With {
            .PRODUCT_ID = product_id,
            .PRODUCT_CODE = product_code,
            .PRODUCT_NAME = product_name,
            .PRODUCT_PRICE = product_price,
            .PRODUCT_STOCKS = product_stock,
            .PRODUCT_IMAGE = image_path
        }

        ' Initialize the datagrid so that we can use its reference from the callee
        _DATA_GRID = datagridview

        ' Bind the data source to the user interface
        Me.DataContext = _productData

        ' Fix the width and height of the Dialog
        Me.Width = parent.MinWidth - 100
        Me.Height = parent.MinHeight - 100
    End Sub

    Private Async Sub ClickEvents(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click, DeleteButton.Click
        ' Did user clicked the save button?
        If sender.Equals(SaveButton) Then
            ' Is the firebase feature enable and is internet available?
            If My.Settings.firebaseEnable AndAlso NetworkInterface.GetIsNetworkAvailable() Then
                Dim productData As New FireBaseProductProp With {
                    .product_code = _productData.PRODUCT_CODE,
                    .product_name = _productData.PRODUCT_NAME,
                    .unit_in_stock = _productData.PRODUCT_STOCKS,
                    .unit_price = _productData.PRODUCT_PRICE,
                    .image_path = "/Resources/intel.jpg"
                }
                ' Add means overiting the product data
                If Await FirebaseAddProductAsync(productData) Then
                    HandyControl.Controls.MessageBox.Info("Product has been updated successfully.", "Update Success")
                    _DATA_GRID.ItemsSource = Nothing
                    _DATA_GRID.ItemsSource = Await FirebaseProductCollectionAsync()
                Else
                    ' Display the error if the update operation did not succeed
                    HandyControl.Controls.MessageBox.Info("Failed to update the product.", "Update failed")
                End If
            Else
                With _productData
                    ' Check if the update operation succeed
                    If _productAdapter.UpdateQueryProduct(.PRODUCT_CODE, .PRODUCT_NAME, .PRODUCT_STOCKS, .PRODUCT_PRICE, .PRODUCT_ID) <> 0 Then
                        HandyControl.Controls.MessageBox.Info("Product has been updated successfully.", "Update Success")
                        _productAdapter.FillByProducts(_productTable)
                        _DATA_GRID.ItemsSource = Nothing                        ' Refresh the data of datagrid
                        _DATA_GRID.ItemsSource = _productTable                  ' Fill the data of the datagrid
                    Else
                        ' Display the error if the update operation did not succeed
                        HandyControl.Controls.MessageBox.Info("Failed to update the product.", "Update failed")
                    End If
                End With
            End If
        ElseIf sender.Equals(DeleteButton) Then ' Is the delete button clicked?
            ' Is the firebase feature enable and is internet available?
            If My.Settings.firebaseEnable AndAlso NetworkInterface.GetIsNetworkAvailable() Then
                If Await FirebaseDeleteProductAsync(_productData.PRODUCT_NAME) Then
                    HandyControl.Controls.MessageBox.Info("Product has been deleted successfully.", "Delete Success")
                    _DATA_GRID.ItemsSource = Nothing
                    _DATA_GRID.ItemsSource = Await FirebaseProductCollectionAsync()
                    ' Invoke the close after deleting
                    Dim peer As ButtonAutomationPeer = TryCast(UIElementAutomationPeer.CreatePeerForElement(Closebtn), ButtonAutomationPeer)
                    ' If the peer variable has found the button
                    If peer IsNot Nothing Then
                        ' We invoke the click  to close the dialog
                        Dim provider As IInvokeProvider = TryCast(peer.GetPattern(PatternInterface.Invoke), IInvokeProvider)
                        If provider IsNot Nothing Then
                            provider.Invoke()
                        End If
                    End If
                Else
                    ' Display the error if the delete operation did not succeed
                    HandyControl.Controls.MessageBox.Info("Failed to delete the product.", "Delete failed")
                End If

            Else ' We will delete using the local database
                With _productData
                    ' Check if the delete operation succeed
                    If _productAdapter.DeleteQueryProduct(.PRODUCT_ID) <> 0 Then
                        HandyControl.Controls.MessageBox.Info("Product has been deleted successfully.", "Delete Success")
                        _productAdapter.FillByProducts(_productTable)
                        _DATA_GRID.ItemsSource = Nothing                        ' Refresh the data in the stocks datagrid view
                        _DATA_GRID.ItemsSource = _productTable
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
        End If
    End Sub
End Class
