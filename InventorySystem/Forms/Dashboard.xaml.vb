' This is for me

Imports System.Data
Imports System.Windows.Controls.Primitives
Imports System.Windows.Interop
Imports HandyControl.Controls
Imports HandyControl.Data
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSetTableAdapters

Public Class Dashboard
    Private ReadOnly _orderAdapter As New ordersTableAdapter
    Private ReadOnly _categoryAdapter As New categoryTableAdapter
    Private ReadOnly _unitAdapter As New unitTableAdapter
    Private ReadOnly _productAdapter As New productTableAdapter
    Private ReadOnly _supplierAdapter As New supplierTableAdapter
    Private ReadOnly _poslist As New List(Of ProductDetails)

    Private weeklySales As String = Double.Parse(If(_orderAdapter.ScalarQueryWeeklySales() Is Nothing, 0, _orderAdapter.ScalarQueryWeeklySales().ToString))
    Private monthlySales As String = Double.Parse(If(_orderAdapter.ScalarQueryMonthlySales() Is Nothing, 0, _orderAdapter.ScalarQueryMonthlySales().ToString))
    Private yearlySales As String = Double.Parse(If(_orderAdapter.ScalarQueryYearlySales() Is Nothing, 0, _orderAdapter.ScalarQueryYearlySales().ToString))
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        AddHandler CategoryUnitComboBox.SelectionChanged, AddressOf CategoryUnitComboBox_SelectionChanged
    End Sub

    Private Sub Dashboard_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded



        DateTextBlock.Text = Date.Now.ToLongDateString
        WeeklySalesLabel.Text = "₱ " & weeklySales
        MonthlySalesLabel.Text = "₱ " & monthlySales
        YearlySalesLabel.Text = "₱ " & yearlySales
        TransactionCountLabel.Text = _orderAdapter.ScalarQueryOrders()
        ProductCountLabel.Text = _productAdapter.ScalarQueryProducts()
        FormTitle.Text = "Dashboard".ToUpper
        If Not My.Settings.isAdmin Then
            BottomContainer.Children.Remove(DashboardPanel)
            BottomContainer.Children.Remove(LogsPanel)
            PanelControls.Children.Remove(DashboardButton)
            PanelControls.Children.Remove(LogsButton)
            TextTransitionPanel.Children.Remove(LogsTransitionText)
            TextTransitionPanel.Children.Remove(DashboardTransitionText)
        End If
        PosDataGridView.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden
        CheckProduct()
    End Sub

    Private Sub ProductHandler(parent As WrapPanel)
        ' Add a click handler for every product
        For i = 0 To parent.Children.Count - 1
            parent.Children.Item(i).AddHandler(ButtonBase.ClickEvent, New RoutedEventHandler(AddressOf AddRow))
        Next
    End Sub

    ' Triggered when we click the product cards
    Private Sub AddRow(sender As Object, e As EventArgs)
        PosDataGridView.ItemsSource = Nothing
        Dim parent As Object = TryCast(sender, UserControl)
        If _poslist.Count <= 0 Then
            _poslist.Add(New ProductDetails With {.PRODUCT_NAME = TryCast(parent.FindName("ProductName"), TextBlock).Text, .PRODUCT_PRICE = TryCast(parent.FindName("ProductPrice"), TextBlock).Text, .PRODUCT_QUANTITY = 1})
            'PosDataGridView.Items.Add(New ProductDetails With {.PRODUCT_NAME = TryCast(parent.FindName("ProductName"), TextBlock).Text, .PRODUCT_PRICE = TryCast(parent.FindName("ProductPrice"), TextBlock).Text, .PRODUCT_QUANTITY = 1})
        Else
            Dim exists As Boolean = False
            For i = 0 To _poslist.Count - 1
                Dim productName As String = TryCast(parent.FindName("ProductName"), TextBlock).Text
                If _poslist(i).PRODUCT_NAME = productName Then
                    _poslist(i).PRODUCT_QUANTITY = Integer.Parse(_poslist(i).PRODUCT_QUANTITY) + 1
                    exists = True
                    Exit For
                End If
            Next

            If Not exists Then
                _poslist.Add(New ProductDetails With {.PRODUCT_NAME = TryCast(parent.FindName("ProductName"), TextBlock).Text, .PRODUCT_PRICE = TryCast(parent.FindName("ProductPrice"), TextBlock).Text, .PRODUCT_QUANTITY = 1})
            End If
        End If
        PosDataGridView.ItemsSource = _poslist

        ' TODO FIX THIS
        'PriceTotal.Text = Math.Round(Double.Parse(PriceTotal.Text) + Double.Parse(TryCast(parent.FindName("ProductPrice"), TextBlock).Text), 2)
    End Sub
    Public Sub EditRowPOS(sender As Object, e As RoutedEventArgs)
        Dim vis = TryCast(sender, Visual)

        While vis IsNot Nothing
            If TypeOf vis Is DataGridRow Then
                Dim row = DirectCast(vis, DataGridRow).Item()
                If TypeOf row Is ProductDetails Then
                    row = DirectCast(row, ProductDetails)
                    Dim index As Integer = _poslist.IndexOf(row)
                    Dialog.Show(New POSEditDialog(row, _poslist, index))
                End If
                Exit While
            End If

            vis = TryCast(VisualTreeHelper.GetParent(vis), Visual)
        End While
    End Sub

    ' Click event for Delete Action in POS DataGridView
    Public Sub DeleteRowPOS(sender As Object, e As RoutedEventArgs)
        ' Cast the sender of the action as visual
        Dim vis = TryCast(sender, Visual)

        ' Get the row parent starting from Button->Cells->Row
        While vis IsNot Nothing
            If TypeOf vis Is DataGridRow Then
                ' Cast the visual to datagridrow to access it's content
                Dim row = DirectCast(vis, DataGridRow).Item()

                ' Check if we are already in the itemsource data row
                If TypeOf row Is ProductDetails Then

                    ' Remove the row to the ItemSouce, this is an object so the List Object will take care of the reference
                    _poslist.Remove(row)

                    ' Change the DataSource of the POS DataGridView
                    ' Then reassign the data to the DataGrid
                    PosDataGridView.ItemsSource = Nothing
                    PosDataGridView.ItemsSource = _poslist
                End If

                ' End of searching for the DataGridRow Element
                Exit While
            End If

            ' Change it till we get to the DataGridRow parent
            vis = TryCast(VisualTreeHelper.GetParent(vis), Visual)
        End While
    End Sub



    Private Sub ChangePanelEvents(sender As Object, e As EventArgs) Handles SalesButton.Click, StocksButton.Click,
            POSButton.Click, DashboardButton.Click, HelpButton.Click, LogsButton.Click
        Dim panels() As Object = {SalesPanel, StocksPanel, POSPanel, HelpPanel, DashboardPanel, LogsPanel}
        ' Hide all the panels before setting the visibility of the requested panel
        For Each panel As Object In panels
            If panel.IsVisible Then
                panel.Visibility = Visibility.Collapsed
            End If
        Next

        ' Determine which button triggered the click event so we can identify which panel should we open
        If sender.Equals(DashboardButton) Then
            ' Change the panel's visibility
            DashboardPanel.Visibility = Visibility.Visible

            ' Assign all the values to the dashboard
            WeeklySalesLabel.Text = "₱ " & weeklySales
            MonthlySalesLabel.Text = "₱ " & monthlySales
            YearlySalesLabel.Text = "₱ " & yearlySales

            ' Change the title of the panel
            FormTitle.Text = "Dashboard".ToUpper

        ElseIf sender.Equals(SalesButton) Then
            SalesPanel.Visibility = Visibility.Visible
            SalesDataGridView.ItemsSource = _orderAdapter.GetDataByOrders()
            FormTitle.Text = "Sales".ToUpper
        ElseIf sender.Equals(StocksButton) Then
            StocksPanel.Visibility = Visibility.Visible
            FormTitle.Text = "Stocks".ToUpper
            ' First Tab
            StocksDataGridView.ItemsSource = GetDataGridByProduct().DefaultView
            StocksTotalItems.Text = _productAdapter.ScalarQueryProducts()
            ' Second Tab
            If CategoryUnitComboBox.SelectedIndex = 0 Then
                CategoryUnitDataGridView.ItemsSource = _categoryAdapter.GetDataByDataGrid()
            Else
                CategoryUnitDataGridView.ItemsSource = _unitAdapter.GetDataByDataGrid()
            End If
            'Third Tab
            SupplierDataGridView.ItemsSource = _supplierAdapter.GetDataBySupplier()
        ElseIf sender.Equals(POSButton) Then
            POSPanel.Visibility = Visibility.Visible
            PosProductContainer.Children.Clear()
            FillProducts(PosProductContainer)                                                   ' Refresh the products list from local database
            ProductHandler(PosProductContainer)                                                     ' To add click handler for every card.
            FormTitle.Text = "POS"
        ElseIf sender.Equals(HelpButton) Then
            HelpPanel.Visibility = Visibility.Visible
            FormTitle.Text = "Help".ToUpper
        ElseIf sender.Equals(LogsButton) Then
            LogsPanel.Visibility = Visibility.Visible
            FormTitle.Text = "Logs".ToUpper
            LogsDataGridView.ItemsSource = GetLogs().DefaultView
        End If
    End Sub

    ' Click event to dispaly the settings
    Private Sub TitleBarEvents(sender As Object, e As RoutedEventArgs) Handles SettingsButton.Click
        Dialog.Show(New SettingsDialog(Me))
    End Sub

    ' Use for the title bar click events, minimize, maximize, and close
    Private Sub ClickEvents(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click, RestoreDButton.Click,
        MinimizeButton.Click
        If sender.Equals(CloseButton) Then
            Me.Close()                                                                          ' Close the window
        ElseIf sender.Equals(RestoreDButton) Then
            Dim rectArea As Rect = SystemParameters.WorkArea
            ' Is the window in maximized state?
            If Me.Width = rectArea.Width AndAlso Me.Height = rectArea.Height Then
                ' If so then go back to the initial height and width
                Me.Width = Me.MinWidth
                Me.Height = Me.MinHeight
                RestoreDIcon.Source = TryCast(FindResource("ic_maximizebit"), ImageSource)      ' Change the icon of restore down
            Else
                ' Position the window at the leftmost and topmost
                Me.Left = 0
                Me.Top = 0
                Me.Width = rectArea.Width
                Me.Height = rectArea.Height
                RestoreDIcon.Source = TryCast(FindResource("ic_restoredownbit"), ImageSource)   ' Change the icon of the restore down
            End If
        ElseIf sender.Equals(MinimizeButton) Then
            Me.WindowState = WindowState.Minimized                                              ' Minimize the window
        End If
    End Sub

    Private Sub Dashboard_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseLeftButtonDown
        DragMove()
    End Sub

    Private Sub SalesDataGridView_AutoGeneratingColumn(sender As Object, e As DataGridAutoGeneratingColumnEventArgs) Handles SalesDataGridView.AutoGeneratingColumn, StocksDataGridView.AutoGeneratingColumn, SupplierDataGridView.AutoGeneratingColumn,
            CategoryUnitDataGridView.AutoGeneratingColumn
        ' Check if which header should we hide
        Dim columnsToHide() As String = {"id", "unit_name", "category", "category_id"}
        If columnsToHide.Contains(e.Column.Header.ToString()) Then
            e.Column.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Sub StocksDataGridView_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles StocksDataGridView.MouseLeftButtonUp
        Dim row As DataRowView = StocksDataGridView.SelectedItem
        With row.Row
            Dim product_id As String = .Item(0)
            Dialog.Show(New ProductDialog(Me, StocksDataGridView, product_id))
        End With
    End Sub



    Private Sub ProfileButton(sender As Object, e As EventArgs) Handles AvatarButton.Click
        Dialog.Show(New ProfileDialog(Me))
    End Sub

    Private Sub StocksAddButton_Click(sender As Object, e As RoutedEventArgs) Handles StocksAddButton.Click
        Dialog.Show(New NewProductDialog(Me, StocksDataGridView))
    End Sub

    Private Sub SalesDataGridView_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles SalesDataGridView.MouseLeftButtonUp
        Dialog.Show(New PurchaseDetailsDialog(Me, SalesDataGridView.SelectedItem.Item(0)))
    End Sub

    Private Sub SupplierDataGridView_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles SupplierDataGridView.MouseLeftButtonUp
        With SupplierDataGridView.SelectedItem
            Dim data() As String = { .Item(0), .Item(1), .Item(2), .Item(3), .Item(4), .Item(5)}
            Dialog.Show(New AddSupplierDialog(Me, SupplierDataGridView, data))
        End With
    End Sub

    Private Sub SupplierAddButton_Click(sender As Object, e As RoutedEventArgs) Handles SupplierAddButton.Click
        Dialog.Show(New AddSupplierDialog(Me, SupplierDataGridView))
    End Sub

#Region "Search Control"
    Private Sub SearchItemTextBox_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles SearchItemTextBox.SearchStarted
        StocksDataGridView.ItemsSource = GetProductsBySearch("%" & SearchItemTextBox.Text & "%")
        StocksTotalItems.Text = StocksDataGridView.Items.Count
    End Sub

    Private Sub SalesSearch_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles SalesSearch.SearchStarted
        SalesDataGridView.ItemsSource = GetOrdersBySearch("%" & SalesSearch.Text & "%")

    End Sub

    Private Sub SearchSupplier_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles SearchSupplierTextBox.SearchStarted
        SupplierDataGridView.ItemsSource = GetSuppliersBySearch("%" & SearchSupplierTextBox.Text & "%")
    End Sub

    Private Sub SearchProductPOS_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles SearchProductPOS.SearchStarted
        PosProductContainer.Children.Clear()
        FillWrapPanelBySearch(PosProductContainer, "%" & SearchProductPOS.Text & "%")
        ProductHandler(PosProductContainer)
    End Sub
#End Region

#Region "Refresh Datagrid"
    Private Sub RefreshButton_Click(sender As Object, e As RoutedEventArgs) Handles RefreshButton.Click
        SalesDataGridView.ItemsSource = _orderAdapter.GetDataByOrders()
    End Sub

    Private Sub RefreshSuppliersButton_Click(sender As Object, e As RoutedEventArgs) Handles RefreshSuppliersButton.Click
        SupplierDataGridView.ItemsSource = _supplierAdapter.GetDataBySupplier()
    End Sub

    Private Sub RefreshProductsButton_Click(sender As Object, e As RoutedEventArgs) Handles RefreshProductsButton.Click
        StocksDataGridView.ItemsSource = GetDataGridByProduct().DefaultView
    End Sub
#End Region

#Region "Unit And Category"
    Private Sub UnitDataGridView_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles CategoryUnitDataGridView.MouseLeftButtonUp
        If CategoryUnitComboBox.SelectedIndex = 0 Then
            Dialog.Show(New CategoryUnitDialog(isCategory:=True, sender, id:=CategoryUnitDataGridView.SelectedItem.item(0)))
        Else
            Dialog.Show(New CategoryUnitDialog(isCategory:=False, sender, id:=CategoryUnitDataGridView.SelectedItem.item(0)))
        End If
    End Sub
#End Region
    Private Sub CategoryUnitComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If CategoryUnitComboBox.SelectedIndex = 0 Then
            CategoryUnitDataGridView.ItemsSource = _categoryAdapter.GetDataByDataGrid()
        Else
            CategoryUnitDataGridView.ItemsSource = _unitAdapter.GetDataByDataGrid()
        End If
    End Sub

    Private Sub AddCategoryUnitProductsButton_Click(sender As Object, e As RoutedEventArgs) Handles AddCategoryUnitProductsButton.Click
        Dialog.Show(New CategoryUnitDialog(isCategory:=If(CategoryUnitComboBox.SelectedIndex = 0, True, False), CategoryUnitDataGridView, isSave:=True))
    End Sub

    Private Sub BuyButton_DataContextChanged(sender As Object, e As RoutedEventArgs) Handles BuyButton.Click
        If _poslist.Count > 0 Then
            Dim result() As Object = IsStocksEnough(PosDataGridView, _poslist)
            If result(0) Then
                If Not BuyProducts(PosDataGridView, _poslist) Then
                    HandyControl.Controls.MessageBox.Info("Purchased Successfully.", "Success")
                Else
                    HandyControl.Controls.MessageBox.Info("Purchased Failed.", "Failed")
                End If
            Else
                HandyControl.Controls.MessageBox.Info("Product quantity of " & result(1) & " exceeds the stocks.", "Invaid Quantity")
            End If
        Else
            HandyControl.Controls.MessageBox.Info("Please add a product.", "Empty List")
        End If
    End Sub

    Private Sub DashboardDailyColBox_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles DashboardDailyColBox.MouseLeftButtonDown
        Dialog.Show(New DashboardDialog(Me, "Weekly Sales Report", GetDataBywWeekly()))
    End Sub

    Private Sub StackPanel_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Dialog.Show(New DashboardDialog(Me, "Monthly Sales Report", GetDataByMonthly()))
    End Sub

    Private Sub StackPanel_MouseLeftButtonDown_1(sender As Object, e As MouseButtonEventArgs)
        Dialog.Show(New DashboardDialog(Me, "Annual Sales Report", GetDataByYearly()))
    End Sub

    Private Sub StackPanel_MouseLeftButtonDown_2(sender As Object, e As MouseButtonEventArgs)
        Dialog.Show(New DashboardDialog(Me, "Products Report", GetAllProducts()))
    End Sub

    Private Sub StackPanel_MouseLeftButtonDown_3(sender As Object, e As MouseButtonEventArgs)
        Dialog.Show(New DashboardDialog(Me, "Transactions Report", _orderAdapter.GetDataByOrders()))
    End Sub
End Class
