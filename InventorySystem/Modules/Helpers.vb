Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports HandyControl.Controls

Module Helpers
    Private ReadOnly _connection As New SqlConnection(My.Settings.saisConnectionString)
    Private _sqlcommand As SqlCommand
    Private _sqladapter As SqlDataAdapter
    Private _datatable As DataTable

    ''' <summary>
    ''' Gets the searched product using the specified query
    ''' </summary>
    ''' <param name="query">A string to use in the where clause</param>
    ''' <returns>DataView that will fill the datagrid</returns>
    Public Function GetProductsBySearch(query As String) As DataView
        _sqlcommand = New SqlCommand("SELECT id,  product_code [Product Code],
                                             product_name [Product Name],
                                             unit_price Price,
                                             unit_in_stock Stocks,
                                             CONCAT(discount_percent, '%') [Discount Percent]
                                             FROM product WHERE (product_code LIKE @query OR product_name LIKE @query) AND unit_in_stock > 0", _connection)
        With _sqlcommand.Parameters
            .AddWithValue("@query", query)
        End With
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable.DefaultView
    End Function

    ''' <summary>
    ''' Use to fetch data from order table by using search.
    ''' </summary>
    ''' <param name="query">A regex pattern for search.</param>
    ''' <returns>A data view to fill the datagrid</returns>
    Public Function GetOrdersBySearch(query As String) As DataView
        _sqlcommand = New SqlCommand("SELECT CONCAT(u.first_name, ' ', u.last_name) Customer,
		                                    p.product_name Product,
                                            o.item_count Item,
		                                    o.total_price [Total Price],
		                                    CAST(o.order_date AS DATE) [Order Date]
		                                    FROM orders o
                                            JOIN product p ON o.product_id = p.id
                                            JOIN users u ON o.user_id = u.id
                                            WHERE CONCAT(u.first_name, ' ', u.last_name) LIKE @query OR p.product_name LIKE @query;", _connection)
        With _sqlcommand.Parameters
            .AddWithValue("@query", query)
        End With
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable.DefaultView
    End Function

    ''' <summary>
    ''' Use to fetch data from the supplier table by using search.
    ''' </summary>
    ''' <param name="query">A regex pattern for search.</param>
    ''' <returns>A data view to fill the datagrid</returns>
    Public Function GetSuppliersBySearch(query As String) As DataView
        _sqlcommand = New SqlCommand("SELECT s.id,
                                            s.supplier_name [Supplier Name],
                                            s.supplier_address [Address],
                                            s.contact Contact,
                                            s.email Email,
                                            UPPER(c.category) Category
                                            FROM supplier s
                                            JOIN category c ON s.category_id = c.id
                                            WHERE s.supplier_name LIKE @query OR s.supplier_address LIKE @query OR s.contact LIKE @query OR s.email LIKE @query;", _connection)
        With _sqlcommand.Parameters
            .AddWithValue("@query", query)
        End With
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable.DefaultView
    End Function

    ''' <summary>
    ''' Fills the datagrid view by product's data.
    ''' </summary>
    ''' <param name="datagrid">Product's datagrid view</param>
    Public Function GetDataGridByProduct() As DataTable
        _sqlcommand = New SqlCommand("SELECT id, product_code [Product Code],
                                        product_name [Product Name],
                                        unit_price Price,
                                        unit_in_stock Stocks,
                                        CONCAT(discount_percent, '%') [Discount Percent]
                                        FROM product
                                        WHERE (unit_in_stock > 0);", _connection)
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable
    End Function


    Public Function UpdateProductByCategory(id As String) As Boolean
        OpenConnection()
        _sqlcommand = New SqlCommand("UPDATE product
                                        SET category_id = (SELECT id FROM category WHERE category = 'None')
                                        WHERE category_id = @id;", _connection)
        _sqlcommand.Parameters.AddWithValue("@id", id)
        If _sqlcommand.ExecuteNonQuery() >= 0 Then
            Return True
        End If
        Return False
    End Function

    Public Function UpdateSupplierByCategory(id As String) As Boolean
        OpenConnection()
        _sqlcommand = New SqlCommand("UPDATE supplier
                                        SET category_id = (SELECT id FROM category WHERE category = 'None')
                                        WHERE category_id = @id;", _connection)
        _sqlcommand.Parameters.AddWithValue("@id", id)
        If _sqlcommand.ExecuteNonQuery() >= 0 Then
            Return True
        End If
        Return False
    End Function

    Public Function UpdateProductByUnit(id As String) As Boolean
        OpenConnection()
        _sqlcommand = New SqlCommand("UPDATE product
                                        SET category_id = (SELECT id FROM unit WHERE unit_name = 'None')
                                        WHERE unit_id = @id;", _connection)
        _sqlcommand.Parameters.AddWithValue("@id", id)
        If _sqlcommand.ExecuteNonQuery() >= 0 Then
            Return True
        End If
        Return False
    End Function

    Public Function GetOrderById(id As String) As DataTable
        _sqlcommand = New SqlCommand("SELECT CONCAT(CONCAT(UPPER(SUBSTRING(u.first_name,1, 1)), SUBSTRING(u.first_name, 2, LEN(u.first_name))),' ', CONCAT(UPPER(SUBSTRING(u.last_name,1, 1)), SUBSTRING(u.last_name, 2, LEN(u.last_name)))),
                                        p.product_code,
                                        p.product_name,
                                        p.unit_price,
                                        o.item_count, 
                                        o.order_date,
                                        o.total_price,
                                        u.image_path
                                        FROM orders AS o
                                        JOIN product p ON o.product_id = p.id
                                        JOIN users u ON o.user_id = u.id
                                        WHERE o.id = @id;", _connection)
        _sqlcommand.Parameters.AddWithValue("@id", id)
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable
    End Function

    ''' <summary>
    '''  Fetches the product data.
    ''' </summary>
    ''' <param name="id">id of the product.</param>
    ''' <returns>Returns a datatable cantaining the data of the product.</returns>
    Public Function ProductDataProperty(id As String) As DataTable
        _sqlcommand = New SqlCommand("SELECT product_code,
                                             product_name,
                                             unit_price,
                                             unit_in_stock,
                                             image_path,
                                             discount_percent
                                      FROM product
                                      WHERE id = @id;", _connection)
        _sqlcommand.Parameters.AddWithValue("@id", id)
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable
    End Function

    ''' <summary>
    ''' Use to fetch the data from products table
    ''' </summary>
    ''' <param name="query">A regex pattern to match the search.</param>
    ''' <returns>A data view to populate the data of card view.</returns>
    Private Function GetProductsByCardView(query As String) As DataView
        _sqlcommand = New SqlCommand("SELECT id
                                             FROM product WHERE product_name LIKE @query AND unit_in_stock > 0", _connection)
        With _sqlcommand.Parameters
            .AddWithValue("@query", query)
        End With
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable.DefaultView
    End Function

    'Public Sub FillDataGridBySupplier(dataGrid As DataGrid)
    '    _sqlcommand = New SqlCommand("SELECT s.id, supplier_name [Supplier Name], supplier_address [Address], contact Contact, email Email, c.category Category FROM supplier s JOIN category c ON s.category_id = c.id", _connection)
    '    _sqladapter = New SqlDataAdapter(_sqlcommand)
    '    _datatable = New DataTable
    '    _sqladapter.Fill(_datatable)
    '    dataGrid.ItemsSource = _datatable.DefaultView
    'End Sub

    ''' <summary>
    ''' Use to fetch data from products to fill a WrapPanel
    ''' </summary>
    ''' <param name="parentNode">Parent container of generated child.</param>
    ''' <param name="query">A regex pattern for search query.</param>
    Public Sub FillWrapPanelBySearch(ByRef parentNode As WrapPanel, query As String)
        Dim productsDataTable As DataView = GetProductsByCardView(query)
        'Debug.WriteLine(productsDataTable.Count)
        For i = 0 To productsDataTable.Count - 1
            With productsDataTable.Item(i)
                parentNode.Children.Add(New ProductCard(.Item(0)))
            End With
        Next
    End Sub

    ''' <summary>
    ''' Fetches the data of the current user
    ''' </summary>
    ''' <returns>A datatable containing all the data of the user</returns>
    Public Function GetCurrentUser() As DataTable
        _sqlcommand = New SqlCommand("SELECT CONCAT(CONCAT(UPPER(SUBSTRING(first_name,1, 1)), SUBSTRING(first_name, 2, LEN(first_name))),' ', CONCAT(UPPER(SUBSTRING(last_name,1, 1)), SUBSTRING(last_name, 2, LEN(last_name)))), gender, phone, email FROM users WHERE id = @id", _connection)
        _sqlcommand.Parameters.AddWithValue("@id", My.Settings.UserID)
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable
    End Function

    ''' <summary>
    '''  Check if the user exists.
    ''' </summary>
    ''' <param name="query">A string that contains the email or username of the user.</param>
    ''' <returns>True if the account exits otherwise false.</returns>
    Public Function IsAccountExists(query As String) As Boolean
        OpenConnection()
        _sqlcommand = New SqlCommand("SELECT COUNT(*) FROM users WHERE email = @query OR username = @query", _connection)
        With _sqlcommand.Parameters
            .AddWithValue("@query", query)
        End With
        If _sqlcommand.ExecuteScalar() <> 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Gets the phone number of the user for code authentication
    ''' </summary>
    ''' <param name="query">A string that contains a username or email of the user</param>
    ''' <returns>A datatable containing the number of the result user</returns>
    Public Function GetUserByQuery(query As String) As DataTable
        _sqlcommand = New SqlCommand("SELECT phone FROM users WHERE email = @query OR username = @query", _connection)
        _sqlcommand.Parameters.AddWithValue("@query", query)
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable
    End Function

    ''' <summary>
    ''' Checks if the phone number exists in the authentication code
    ''' </summary>
    ''' <param name="phone">A string that contains the phone number of the user</param>
    ''' <returns>True if the auth exists otherwise false</returns>
    Public Function IsAuthExists(phone As String) As Boolean
        OpenConnection()
        _sqlcommand = New SqlCommand("SELECT COUNT(*) FROM auth WHERE phone = @phone", _connection)
        _sqlcommand.Parameters.AddWithValue("@phone", phone)
        If _sqlcommand.ExecuteScalar() > 0 Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Gets the auth code of the existing phone's auth code then update the expire time
    ''' </summary>
    ''' <param name="phone">A string that contains the phone number of the user</param>
    ''' <returns>A string containing the auth code</returns>
    Public Function GetAuthByPhone(phone As String) As String
        Dim auth As String
        _sqlcommand = New SqlCommand("SELECT auth_code FROM auth WHERE phone = @phone", _connection)
        _sqlcommand.Parameters.AddWithValue("@phone", phone)
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)


        auth = _datatable.Rows(0).Item(0)

        OpenConnection()
        _sqlcommand = New SqlCommand("UPDATE auth SET expire_time = CURRENT_TIMESTAMP WHERE phone = @phone", _connection)
        _sqlcommand.Parameters.AddWithValue("@phone", phone)
        _sqlcommand.ExecuteNonQuery()
        Return auth
    End Function

    ''' <summary>
    ''' Opens the sql connection
    ''' </summary>
    Public Sub OpenConnection()
        If _connection.State = ConnectionState.Closed Then
            _connection.Open()
        End If
    End Sub


    Public Function GetDataByProducts() As DataTable
        _sqlcommand = New SqlCommand("SELECT CONCAT(u.first_name, ' ', u.last_name) Customer,
                                                p.product_name Product,
                                                o.item_count Item,
                                                o.total_price [Total Price],
                                                CAST(o.order_date AS DATE) [Order Date]
                                        FROM orders o
                                        JOIN product p ON o.product_id = p.id
                                        JOIN users u ON o.user_id = u.id;", _connection)
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable
    End Function

    Public Function GetProductByID(id As String) As DataTable
        _sqlcommand = New SqlCommand("SELECT product_name, unit_price, image_path FROM product WHERE id = @id", _connection)
        _sqlcommand.Parameters.AddWithValue("@id", id)
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable
    End Function

    Public Sub CheckProduct()
        _sqlcommand = New SqlCommand("SELECT product_name FROM product WHERE unit_in_stock < 5", _connection)
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)

        For i = 0 To _datatable.Rows.Count - 1
            Growl.InfoGlobal(_datatable.Rows(i).Item(0) & " is running out of stock.")
        Next
    End Sub

    Public Function GetProductByName(productName As String) As DataTable
        _sqlcommand = New SqlCommand("SELECT * FROM product WHERE product_name = @name", _connection)
        _sqlcommand.Parameters.AddWithValue("@name", productName)
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable
    End Function

    Public Function UpdateProductProperties(name As String, quantity As Integer) As Boolean

        Dim stocks As Integer = GetStocksCount(name, quantity)(0)

        If Not stocks < 0 Then
            OpenConnection()
            _sqlcommand = New SqlCommand("UPDATE product SET unit_in_stock = @stocks_left WHERE product_name = @name", _connection)
            _sqlcommand.Parameters.AddWithValue("@name", name)
            _sqlcommand.Parameters.AddWithValue("@stocks_left", stocks)
            If _sqlcommand.ExecuteNonQuery() <> 0 Then
                OpenConnection()
                _sqlcommand = New SqlCommand("SELECT id, unit_price FROM product WHERE product_name = @name", _connection)
                _sqlcommand.Parameters.AddWithValue("@name", name)
                _sqladapter = New SqlDataAdapter(_sqlcommand)
                _datatable = New DataTable
                _sqladapter.Fill(_datatable)


                Dim pid As Integer = _datatable.Rows(0).Item(0)
                Dim product_price As Double = Double.Parse(_datatable.Rows(0).Item(1))
                _sqlcommand = New SqlCommand("INSERT INTO orders VALUES (@pid, @uid, CURRENT_TIMESTAMP, @total_price, @quantity)", _connection)
                _sqlcommand.Parameters.AddWithValue("@pid", pid)
                _sqlcommand.Parameters.AddWithValue("@uid", My.Settings.UserID)
                _sqlcommand.Parameters.AddWithValue("@total_price", product_price * quantity)
                _sqlcommand.Parameters.AddWithValue("@quantity", quantity)
                If _sqlcommand.ExecuteNonQuery() <> 0 Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Public Function GetStocksCount(productName As String, quantity As Integer) As Object()
        _sqlcommand = New SqlCommand("SELECT unit_in_stock, product_name FROM product WHERE product_name = @name", _connection)
        _sqlcommand.Parameters.AddWithValue("@name", productName)
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)

        Dim stocks As Integer = Integer.Parse(_datatable.Rows(0).Item(0)) - quantity
        Return {stocks, _datatable.Rows(0).Item(1)}
    End Function

    Public Function IsStocksEnough(dataGrid As DataGrid, ByRef postCart As List(Of ProductDetails)) As Object()

        For Each row As ProductDetails In dataGrid.Items
            Dim result() As Object = GetStocksCount(row.PRODUCT_NAME, row.PRODUCT_QUANTITY)
            If result(0) < 0 Then
                Return {False, result(1)}
            End If
        Next
        Return {True}
    End Function

    Public Function BuyProducts(dataGrid As DataGrid, ByRef postCart As List(Of ProductDetails)) As Boolean
        Dim isFailed = False
        For Each row As ProductDetails In dataGrid.Items
            If Not UpdateProductProperties(row.PRODUCT_NAME, row.PRODUCT_QUANTITY) Then
                isFailed = True
                Exit For
            End If
        Next
        postCart.Clear()
        dataGrid.ItemsSource = postCart
        Return isFailed
    End Function

    Public Function GetDataByMonthly() As DataTable
        _sqlcommand = New SqlCommand("SELECT o.id, CONCAT(u.first_name, ' ', u.last_name) Cashier,
		            p.product_name Product ,
                                                 o.item_count Item,
		            o.total_price [Total Price],
		            CAST(o.order_date AS DATE) [Order Date]
		            FROM orders o
            JOIN product p ON o.product_id = p.id
            JOIN users u ON o.user_id = u.id  WHERE MONTH(order_date) = MONTH(GETDATE())", _connection)
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable
    End Function

    Public Function GetDataByYearly() As DataTable
        _sqlcommand = New SqlCommand("SELECT o.id, CONCAT(u.first_name, ' ', u.last_name) Cashier,
		            p.product_name Product ,
                                                 o.item_count Item,
		            o.total_price [Total Price],
		            CAST(o.order_date AS DATE) [Order Date]
		            FROM orders o
            JOIN product p ON o.product_id = p.id
            JOIN users u ON o.user_id = u.id  WHERE YEAR(order_date) = YEAR(GETDATE())", _connection)
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable
    End Function

    Public Function GetDataBywWeekly() As DataTable
        _sqlcommand = New SqlCommand("SELECT o.id, CONCAT(u.first_name, ' ', u.last_name) Cashier,
		            p.product_name Product ,
                                                 o.item_count Item,
		            o.total_price [Total Price],
		            SUBSTRING(CAST(o.order_date AS VARCHAR), 1, 12) [Order Date]
		            FROM orders o
            JOIN product p ON o.product_id = p.id
            JOIN users u ON o.user_id = u.id  WHERE CAST(order_date AS DATE) BETWEEN DATEADD(DAY, 1 - DATEPART(DW, GETDATE()), CAST(GETDATE() AS DATE)) AND DATEADD(DAY, 6, DATEADD(DAY, 1 - DATEPART(DW, GETDATE()), CAST(GETDATE() AS DATE)))", _connection)
        _sqladapter = New SqlDataAdapter(_sqlcommand)
        _datatable = New DataTable
        _sqladapter.Fill(_datatable)
        Return _datatable
    End Function

End Module
