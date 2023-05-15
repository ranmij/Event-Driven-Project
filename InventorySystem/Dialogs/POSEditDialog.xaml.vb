Imports System.Data
Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider

Public Class POSEditDialog
    Private _collection As List(Of ProductDetails)
    Private _index As Integer
    Public Sub New(ByRef productProp As ProductDetails, ByRef collection As List(Of ProductDetails), index As Integer)

        InitializeComponent()
        DataContext = productProp
        _collection = collection
        _index = index
        Height = 400
        Width = 400
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click
        If IsNumeric(ProductQuantityTextBox.Text) Then
            Dim data As ProductDetails = _collection.Item(_index)
            Dim dataTable As DataTable = GetProductByName(data.PRODUCT_NAME)
            Dim stocks As Integer = Integer.Parse(dataTable.Rows(0).Item(5))
            If stocks - Integer.Parse(ProductQuantityTextBox.Text) >= 0 Then
                _collection.Item(_index).PRODUCT_QUANTITY = ProductQuantityTextBox.Text
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
                HandyControl.Controls.MessageBox.Info("Quantity exceeds the stocks.")
            End If
        Else
                ProductQuantityTextBox.BorderBrush = Brushes.Red
        End If

    End Sub
End Class
