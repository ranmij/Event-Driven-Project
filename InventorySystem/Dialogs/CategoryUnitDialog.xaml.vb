Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSetTableAdapters

Public Class CategoryUnitDialog
    Private ReadOnly _HEIGHT As Integer = 200
    Private ReadOnly _WIDTH As Integer = 400
    Private ReadOnly _CATEGORY_ADAPTER As New categoryTableAdapter
    Private ReadOnly _UNIT_ADAPTER As New unitTableAdapter
    Private _ID As Integer = Nothing
    Private _IS_CATEGORY As Boolean
    Private _DATA_GRID As DataGrid

    Public Sub New(isCategory As Boolean, ByRef datagrid As DataGrid, Optional isSave As Boolean = Nothing, Optional id As Integer = Nothing)

        InitializeComponent()
        If isSave = Nothing Then
            If isCategory Then
                CategoryNameTextBox.Text = _CATEGORY_ADAPTER.GetDataById(id).Item(0).Item(1)
                ControlContainer.Children.RemoveAt(1)
            Else
                UnitNameTextBox.Text = _UNIT_ADAPTER.GetDataById(id).Item(0).Item(1)
                ControlContainer.Children.RemoveAt(0)
            End If
            ButtonContainer.Children.Remove(SaveButton)
        Else
            If isCategory Then
                'CategoryNameTextBox.Text = _CATEGORY_ADAPTER.GetDataById(id).Item(0).Item(1)
                ControlContainer.Children.RemoveAt(1)
            Else
                'UnitNameTextBox.Text = _UNIT_ADAPTER.GetDataById(id).Item(0).Item(1)
                ControlContainer.Children.RemoveAt(0)
            End If
            ButtonContainer.Children.Remove(DeleteButton)
            ButtonContainer.Children.Remove(UpdateButton)
        End If


        _ID = id
        _DATA_GRID = datagrid
        _IS_CATEGORY = isCategory
        Me.Height = _HEIGHT
        Me.Width = _WIDTH
    End Sub

    Private Sub UpdateButton_Click(sender As Object, e As RoutedEventArgs) Handles UpdateButton.Click
        If _IS_CATEGORY Then
            If Not String.IsNullOrEmpty(CategoryNameTextBox.Text) Then
                If _CATEGORY_ADAPTER.ScalarQueryCategory(CategoryNameTextBox.Text) = 0 Then
                    If _CATEGORY_ADAPTER.UpdateQueryCategory(CategoryNameTextBox.Text, _ID) <> 0 Then
                        HandyControl.Controls.MessageBox.Info("Category has been updated.", "Update Success")
                        _DATA_GRID.ItemsSource = _CATEGORY_ADAPTER.GetDataByDataGrid()
                        CategoryNameTextBox.BorderBrush = Brushes.Gray
                    End If
                Else
                    CategoryNameTextBox.BorderBrush = Brushes.Red
                    HandyControl.Controls.MessageBox.Info("This category already exists", "Invalid Category Name")
                End If
            Else
                CategoryNameTextBox.BorderBrush = Brushes.Red
            End If
        Else
            If Not String.IsNullOrEmpty(UnitNameTextBox.Text) Then
                If _UNIT_ADAPTER.ScalarQueryUnit(UnitNameTextBox.Text) = 0 Then
                    If _UNIT_ADAPTER.UpdateQueryUnit(UnitNameTextBox.Text, _ID) <> 0 Then
                        HandyControl.Controls.MessageBox.Info("Unit has been updated.", "Update Success")
                        _DATA_GRID.ItemsSource = _UNIT_ADAPTER.GetDataByDataGrid()
                        UnitNameTextBox.BorderBrush = Brushes.Gray
                    End If
                Else
                    UnitNameTextBox.BorderBrush = Brushes.Red
                    HandyControl.Controls.MessageBox.Info("This unit already exists", "Invalid Unit Name")
                End If
            Else
                UnitNameTextBox.BorderBrush = Brushes.Red
            End If
        End If
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs) Handles DeleteButton.Click
        If _ID <> Nothing Then
            If _IS_CATEGORY Then
                If UpdateProductByCategory(_ID) Then
                    If UpdateSupplierByCategory(_ID) Then
                        If _CATEGORY_ADAPTER.DeleteQueryCategory(_ID) <> 0 Then
                            _DATA_GRID.ItemsSource = _CATEGORY_ADAPTER.GetDataByDataGrid()
                            HandyControl.Controls.MessageBox.Info("Category has been deleted.", "Delete Success")
                        Else
                            HandyControl.Controls.MessageBox.Info("Unable to delete category.", "Delete Failed")
                        End If
                    End If
                End If
            Else
                If UpdateProductByUnit(_ID) Then
                    If _UNIT_ADAPTER.DeleteQueryUnit(_ID) <> 0 Then
                        _DATA_GRID.ItemsSource = _UNIT_ADAPTER.GetDataByDataGrid()
                        HandyControl.Controls.MessageBox.Info("Unit has been deleted.", "Delete Success")
                    Else
                        HandyControl.Controls.MessageBox.Info("Unable to delete unit.", "Delete Failed")
                    End If
                End If
            End If
        End If
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

    Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click
        If _IS_CATEGORY Then
            If Not String.IsNullOrEmpty(CategoryNameTextBox.Text) Then
                If _CATEGORY_ADAPTER.ScalarQueryCategory(CategoryNameTextBox.Text) = 0 Then
                    If _CATEGORY_ADAPTER.InsertQueryCategory(CategoryNameTextBox.Text) <> 0 Then
                        HandyControl.Controls.MessageBox.Info("Category added successfully", "Success")
                        _DATA_GRID.ItemsSource = _CATEGORY_ADAPTER.GetDataByDataGrid()
                        CategoryNameTextBox.BorderBrush = Brushes.Gray
                    End If
                Else
                    CategoryNameTextBox.BorderBrush = Brushes.Red
                    HandyControl.Controls.MessageBox.Info("This category already exists", "Invalid Category Name")
                End If
            Else
                CategoryNameTextBox.BorderBrush = Brushes.Red
            End If
        Else
            If Not String.IsNullOrEmpty(UnitNameTextBox.Text) Then
                If _UNIT_ADAPTER.ScalarQueryUnit(UnitNameTextBox.Text) = 0 Then
                    If _UNIT_ADAPTER.InsertQueryUnit(UnitNameTextBox.Text) <> 0 Then
                        HandyControl.Controls.MessageBox.Info("Unit added successfully", "Success")
                        _DATA_GRID.ItemsSource = _UNIT_ADAPTER.GetDataByDataGrid()
                        UnitNameTextBox.BorderBrush = Brushes.Gray
                    End If
                Else
                    UnitNameTextBox.BorderBrush = Brushes.Red
                    HandyControl.Controls.MessageBox.Info("This unit already exists", "Invalid Unit Name")
                End If
            Else
                UnitNameTextBox.BorderBrush = Brushes.Red
            End If
        End If
    End Sub
End Class
