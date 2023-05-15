Imports System.Data
Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider

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

    Private Sub OkButton_Click(sender As Object, e As RoutedEventArgs) Handles OkButton.Click
        Dim peer As ButtonAutomationPeer = TryCast(UIElementAutomationPeer.CreatePeerForElement(Closebtn), ButtonAutomationPeer)
        ' If the peer variable has found the button
        If peer IsNot Nothing Then
            ' We invoke the click so that it the dialog will close
            Dim provider As IInvokeProvider = TryCast(peer.GetPattern(PatternInterface.Invoke), IInvokeProvider)
            If provider IsNot Nothing Then
                provider.Invoke()
            End If
        End If
    End Sub
End Class
