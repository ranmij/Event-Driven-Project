Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSetTableAdapters

Public Class AddSupplierDialog
    Private ReadOnly _categoryAdapter As New categoryTableAdapter
    Private ReadOnly _supplierAdapter As New supplierTableAdapter
    Private _datacontext As SupplierProperty
    Private _DATAGRID As DataGrid
    Private _category As Integer = Nothing
    Private _id As Integer = Nothing

    Public Sub New(ByRef parent As Dashboard, dataGrid As DataGrid, Optional data() As String = Nothing)
        InitializeComponent()
        If data IsNot Nothing Then
            ButtonContainer.Children.Remove(SaveButton)
            _datacontext = New SupplierProperty With {
                .SUPPLIER_NAME = data(1),
                .SUPPLIER_ADDRESS = data(2),
                .SUPPLIER_CONTACT = data(3),
                .SUPPLIER_EMAIL = data(4),
                .SUPPLIER_CATEGORY = CInt(data(5))
            }
            Me.DataContext = _datacontext
            _category = CInt(data(5))
            _id = data(0)
        Else
            ButtonContainer.Children.Remove(UpdateButton)
            ButtonContainer.Children.Remove(DeleteButton)

        End If

        Me.Height = parent.MinHeight - 100
        Me.Width = parent.MinWidth - 500
        _DATAGRID = dataGrid
    End Sub

    Private Sub AddSupplierDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        SupplierCategoryComboBox.ItemsSource = _categoryAdapter.GetData()
        SupplierCategoryComboBox.DisplayMemberPath = "category"
        SupplierCategoryComboBox.SelectedValuePath = "id"
        If _category <> Nothing Then
            SupplierCategoryComboBox.SelectedValue = _category
        Else
            SupplierCategoryComboBox.SelectedIndex = 0
        End If
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click
        Dim controls() As Object = {SupplierNameTextBox, SupplierAddressTextBox, SupplierContactTextBox, SupplierCategoryComboBox, SupplierEmailTextBox}
        If Not IsNotEmpty(controls) Then
            For Each control In controls
                If String.IsNullOrEmpty(control.Text) Then
                    If TryCast(control, TextBox) IsNot Nothing Then
                        TryCast(control, TextBox).BorderBrush = Brushes.Red
                    Else
                        TryCast(control, ComboBox).BorderBrush = Brushes.Red
                    End If
                End If
            Next
        Else
            If IsValidPhone(SupplierContactTextBox.Text) Then
                For Each control In controls
                    If String.IsNullOrEmpty(control.Text) Then
                        If TryCast(control, TextBox) IsNot Nothing Then
                            TryCast(control, TextBox).BorderBrush = Brushes.Gray
                        Else
                            TryCast(control, ComboBox).BorderBrush = Brushes.Gray
                        End If
                    End If
                Next
                If IsValidEmail(SupplierEmailTextBox.Text) Then
                    For Each control In controls
                        If String.IsNullOrEmpty(control.Text) Then
                            If TryCast(control, TextBox) IsNot Nothing Then
                                TryCast(control, TextBox).BorderBrush = Brushes.Gray
                            Else
                                TryCast(control, ComboBox).BorderBrush = Brushes.Gray
                            End If
                        End If
                    Next
                    If _supplierAdapter.ScalarQuerySuplier(SupplierNameTextBox.Text, SupplierAddressTextBox.Text, SupplierContactTextBox.Text) = 0 Then
                        If _supplierAdapter.InsertQuerySupplier(SupplierNameTextBox.Text, SupplierAddressTextBox.Text, SupplierContactTextBox.Text, SupplierEmailTextBox.Text, SupplierCategoryComboBox.SelectedValue) <> 0 Then
                            HandyControl.Controls.MessageBox.Info("Supplier has been added successfully.", "Success!")
                            _DATAGRID.ItemsSource = _supplierAdapter.GetDataBySupplier()
                        Else
                            HandyControl.Controls.MessageBox.Info("Failed to add supplier", "Failed!")
                            _DATAGRID.ItemsSource = _supplierAdapter.GetDataBySupplier()
                        End If
                        For Each control In controls
                            If String.IsNullOrEmpty(control.Text) Then
                                If TryCast(control, TextBox) IsNot Nothing Then
                                    TryCast(control, TextBox).BorderBrush = Brushes.Gray
                                Else
                                    TryCast(control, ComboBox).BorderBrush = Brushes.Gray
                                End If
                            End If
                        Next
                    Else
                        HandyControl.Controls.MessageBox.Info("This supplier exists.", "Invalid Supplier Info")
                    End If
                Else

                    SupplierEmailTextBox.BorderBrush = Brushes.Red
                End If
            Else

                SupplierContactTextBox.BorderBrush = Brushes.Red
            End If
        End If
    End Sub

    Private Sub UpdateButton_Click(sender As Object, e As RoutedEventArgs) Handles UpdateButton.Click
        If _id <> Nothing Then
            If IsValidEmail(SupplierEmailTextBox.Text) Then
                If IsValidPhone(SupplierContactTextBox.Text) Then
                    If _supplierAdapter.ScalarQuerySuplier(SupplierNameTextBox.Text, SupplierAddressTextBox.Text, SupplierContactTextBox.Text) = 0 Then
                        If _supplierAdapter.UpdateQuerySupplier(SupplierNameTextBox.Text, SupplierAddressTextBox.Text, SupplierContactTextBox.Text, SupplierEmailTextBox.Text, SupplierCategoryComboBox.SelectedValue, _id) <> 0 Then
                            HandyControl.Controls.MessageBox.Info("Supplier has been updated successfully.", "Update Success")
                            _DATAGRID.ItemsSource = _supplierAdapter.GetDataBySupplier()
                        Else
                            HandyControl.Controls.MessageBox.Info("Failed to update supplier.", "Update Failed")
                        End If
                    Else
                        HandyControl.Controls.MessageBox.Info("This supplier exists.", "Invalid Supplier Info")
                    End If
                Else
                    SupplierContactTextBox.BorderBrush = Brushes.Red
                End If
            Else
                SupplierEmailTextBox.BorderBrush = Brushes.Red
            End If
        End If
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs) Handles DeleteButton.Click
        If _id <> Nothing Then
            If _supplierAdapter.DeleteQuerySupplier(_id) <> 0 Then
                HandyControl.Controls.MessageBox.Info("Supplier has been removed successfully.", "Delete Success")
                _DATAGRID.ItemsSource = _supplierAdapter.GetDataBySupplier()
                CloseForm()
            Else
                HandyControl.Controls.MessageBox.Info("Failed to remove supplier.", "Delete Failed")
            End If
        End If
    End Sub

    Private Sub CloseForm()
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
