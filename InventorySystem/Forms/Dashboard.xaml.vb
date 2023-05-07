' This is for me

Imports System.Data
Imports System.Net.NetworkInformation
Imports System.Windows.Controls.Primitives
Imports HandyControl.Controls
Imports HandyControl.Data
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSet
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSetTableAdapters

Public Class Dashboard
    Private ReadOnly _orderAdapter As New ordersTableAdapter
    Private ReadOnly _categoryAdapter As New categoryTableAdapter
    Private ReadOnly _unitAdapter As New unitTableAdapter
    Private ReadOnly _productAdapter As New productTableAdapter
    Private ReadOnly _supplierAdapter As New supplierTableAdapter


    Private Sub Dashboard_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        DateTextBlock.Text = Date.Now.ToLongDateString
        WeeklySalesLabel.Text = "₱ " & _orderAdapter.ScalarQueryWeeklySales().ToString
        MonthlySalesLabel.Text = "₱ " & _orderAdapter.ScalarQueryMonthlySales().ToString
        YearlySalesLabel.Text = "₱ " & _orderAdapter.ScalarQueryYearlySales().ToString
        TransactionCountLabel.Text = _orderAdapter.ScalarQueryOrders()
        ProductCountLabel.Text = _productAdapter.ScalarQueryProducts()
        FormTitle.Text = "Dashboard".ToUpper
    End Sub


    Private Sub ProductHandler(parent As WrapPanel)
        ' Add a click handler for every product
        For i = 0 To parent.Children.Count - 1
            parent.Children.Item(i).AddHandler(ButtonBase.ClickEvent, New RoutedEventHandler(AddressOf AddRow))
        Next
    End Sub

    ' Triggered when we click the product cards
    Private Sub AddRow(sender As Object, e As EventArgs)
        Dim parent As Object = TryCast(sender, UserControl)
        PosDataGridView.Items.Add(New ProductDetails With {.PRODUCT_NAME = TryCast(parent.FindName("ProductName"), TextBlock).Text, .PRODUCT_PRICE = TryCast(parent.FindName("ProductPrice"), TextBlock).Text})
        PriceTotal.Text = Math.Round(Double.Parse(PriceTotal.Text) + Double.Parse(TryCast(parent.FindName("ProductPrice"), TextBlock).Text), 2)
    End Sub

    Private Sub ChangePanelEvents(sender As Object, e As EventArgs) Handles SalesButton.Click, StocksButton.Click,
            POSButton.Click, DashboardButton.Click, HelpButton.Click
        Dim panels() As Object = {SalesPanel, StocksPanel, POSPanel, HelpPanel, DashboardPanel}
        ' Hide all the panels before setting the visibility of the requested panel
        For Each panel As Object In panels
            If panel.IsVisible Then
                panel.Visibility = Visibility.Collapsed
            End If
        Next

        ' Determine which button triggered the click event so we can identify which panel should we open
        If sender.Equals(DashboardButton) Then
            DashboardPanel.Visibility = Visibility.Visible
            WeeklySalesLabel.Text = "₱ " & _orderAdapter.ScalarQueryWeeklySales().ToString
            MonthlySalesLabel.Text = "₱ " & _orderAdapter.ScalarQueryMonthlySales().ToString
            YearlySalesLabel.Text = "₱ " & _orderAdapter.ScalarQueryYearlySales().ToString
            FormTitle.Text = "Dashboard".ToUpper
        ElseIf sender.Equals(SalesButton) Then
            SalesPanel.Visibility = Visibility.Visible
            SalesDataGridView.ItemsSource = _orderAdapter.GetDataByOrders()
            FormTitle.Text = "Sales".ToUpper
        ElseIf sender.Equals(StocksButton) Then
            StocksPanel.Visibility = Visibility.Visible
            FormTitle.Text = "Stocks".ToUpper
            ' First Tab
            StocksDataGridView.ItemsSource = _productAdapter.GetDataByProducts()                ' Fill the stocks datagrid view with products
            StocksTotalItems.Text = _productAdapter.ScalarQueryProducts()
            ' Second Tab
            CategoryDataGridView.ItemsSource = _categoryAdapter.GetDataByDataGrid()
            UnitDataGridView.ItemsSource = _unitAdapter.GetDataByDataGrid()
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
        End If
    End Sub

    Private Sub TitleBarEvents(sender As Object, e As RoutedEventArgs) Handles SettingsButton.Click
        Dialog.Show(New SettingsDialog(Me))
    End Sub

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

    Private Sub SalesDataGridView_AutoGeneratingColumn(sender As Object, e As DataGridAutoGeneratingColumnEventArgs) Handles SalesDataGridView.AutoGeneratingColumn, StocksDataGridView.AutoGeneratingColumn,
            CategoryDataGridView.AutoGeneratingColumn, UnitDataGridView.AutoGeneratingColumn, SupplierDataGridView.AutoGeneratingColumn, CategoryDataGridView.AutoGeneratingColumn,
            UnitDataGridView.AutoGeneratingColumn
        ' Check if which header should we hide
        Dim columnsToHide() As String = {"id", "order_id", "order_date", "payment_id", "product_name", "ship_date", "user_id", "total_price", "item_count", "product_id", "unit_id", "image_path", "category_id", "discount_percent", "product_code", "category_id", "unit_in_stock", "unit_price", "category", "unit_name"}
        If columnsToHide.Contains(e.Column.Header.ToString()) Then
            e.Column.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Sub StocksDataGridView_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles StocksDataGridView.MouseLeftButtonUp
        Dim row As DataRowView = StocksDataGridView.SelectedItem
        'MsgBox(row.Row.Item(3))
        With row.Row
            ' A painful way of assigning values lol
            Dim product_id As String = .Item(0)
            Dim product_code As String = .Item(3)
            Dim product_name As String = .Item(4)
            Dim product_stock As String = .Item(5)
            Dim product_price As String = .Item(6)
            Dim product_image As String = .Item(8)
            Dialog.Show(New ProductDialog(Me, StocksDataGridView, product_id, product_code, product_name, product_stock, product_price, product_image))
        End With
    End Sub



    Private Sub ProfileButton(sender As Object, e As EventArgs) Handles AvatarButton.Click
        ' TODO Change this to actual data
        Dialog.Show(New ProfileDialog(Me, "John Doe", "/Resources/intel.jpg"))
    End Sub

    Private Sub StocksAddButton_Click(sender As Object, e As RoutedEventArgs) Handles StocksAddButton.Click
        Dialog.Show(New NewProductDialog(Me))
    End Sub

    Private Sub SalesDataGridView_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles SalesDataGridView.MouseLeftButtonUp

    End Sub

    Private Sub SupplierDataGridView_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles SupplierDataGridView.MouseLeftButtonUp
        With SupplierDataGridView.SelectedItem
            Dim data() As String = { .Item(0), .Item(1), .Item(2), .Item(3), .Item(4), .Item(5)}
            Dialog.Show(New AddSupplierDialog(Me, SupplierDataGridView, data))
        End With
    End Sub

    ' For adding category and unit
    Private Sub AddCategoryButton_Click(sender As Object, e As RoutedEventArgs) Handles AddCategoryUnitButton.Click
        If Not String.IsNullOrEmpty(CategoryTextBox.Text) And Not String.IsNullOrEmpty(UnitTextBox.Text) Then
            If _categoryAdapter.InsertQueryCategory(CategoryTextBox.Text) <> 0 AndAlso
            _unitAdapter.InsertQueryUnit(UnitTextBox.Text) <> 0 Then
                HandyControl.Controls.MessageBox.Info("Category and unit has been added successfully.", "Success!")
            Else
                HandyControl.Controls.MessageBox.Warning("Operation failed.", "Failed")
            End If
        ElseIf Not String.IsNullOrEmpty(CategoryTextBox.Text) Then
            If _categoryAdapter.InsertQueryCategory(CategoryTextBox.Text) <> 0 Then
                HandyControl.Controls.MessageBox.Info("Category has been added successfully.", "Success!")
            Else
                HandyControl.Controls.MessageBox.Warning("Operation failed.", "Failed")
            End If
        Else
            If _unitAdapter.InsertQueryUnit(UnitTextBox.Text) <> 0 Then
                HandyControl.Controls.MessageBox.Info("Unit has been added successfully.", "Success!")
            Else
                HandyControl.Controls.MessageBox.Warning("Operation failed.", "Failed")
            End If
        End If
        CategoryDataGridView.ItemsSource = _categoryAdapter.GetDataByDataGrid()
        UnitDataGridView.ItemsSource = _unitAdapter.GetDataByDataGrid()
    End Sub

    ' FOR SEARCHING PRODUCT
    Private Sub SearchItemTextBox_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles SearchItemTextBox.SearchStarted
        Dim query = SearchItemTextBox.Text.Insert(0, "%")
        StocksDataGridView.ItemsSource = _productAdapter.GetDataBySearch(SearchItemTextBox.Text)
    End Sub


    ' FOR SEARCHING SALES
    Private Sub SalesSearch_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles SalesSearch.SearchStarted
        Dim query = SalesSearch.Text.Insert(0, "%")
        SalesDataGridView.ItemsSource = _orderAdapter.GetDataBySearch(query.Insert(Len(query), "%"))

    End Sub

    ' FOR SEARCHING SUPPLIER
    Private Sub SearchSupplier_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles SearchSupplierTextBox.SearchStarted
        Dim query = SearchSupplierTextBox.Text.Insert(0, "%")
        SalesDataGridView.ItemsSource = _supplierAdapter.GetDataBySearch(query.Insert(Len(query), "%"))

    End Sub

    Private Sub SearchProductPOS_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles SearchProductPOS.SearchStarted
        PosProductContainer.Children.Clear()
        FillProductsBySearch(PosProductContainer, SearchProductPOS.Text)                                                   ' Refresh the products list from local database
        ProductHandler(PosProductContainer)
    End Sub


    ' FOR REFRESHING DATAGRID
    Private Sub RefreshButton_Click(sender As Object, e As RoutedEventArgs) Handles RefreshButton.Click
        SalesDataGridView.ItemsSource = _orderAdapter.GetDataByOrders()
    End Sub

    Private Sub SupplierAddButton_Click(sender As Object, e As RoutedEventArgs) Handles SupplierAddButton.Click
        Dialog.Show(New AddSupplierDialog(Me, SupplierDataGridView))
    End Sub

    Private Sub RefreshSuppliersButton_Click(sender As Object, e As RoutedEventArgs) Handles RefreshSuppliersButton.Click
        SupplierDataGridView.ItemsSource = _supplierAdapter.GetDataBySupplier()
    End Sub

    Private Sub RefreshProductsButton_Click(sender As Object, e As RoutedEventArgs) Handles RefreshProductsButton.Click
        StocksDataGridView.ItemsSource = _productAdapter.GetDataByProducts()
    End Sub

End Class
