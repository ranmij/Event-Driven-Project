' This is for me
Option Strict On
Imports System.IO
Imports System.Text.RegularExpressions
Imports HandyControl.Controls
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSet
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSetTableAdapters
Imports InventorySystem.InventorySystem.DataSets.UserDataSetTableAdapters

Public Class ProductDetails
    Public Property PRODUCT_ID As String
    Public Property PRODUCT_NAME As String
    Public Property PRODUCT_PRICE As String
    Public Property PRODUCT_QUANTITY As Integer
End Class
Module Utils
    ReadOnly tableAdapter As New usersTableAdapter
    Private ReadOnly MAX_PHONE_LENGTH As Integer = &HB

    ' Use as a data source context for the POS datagrid view


    ' Use as a data source context for the Stocks datagrid view
    Public Class ProductData
        Public Property PRODUCT_ID As String
        Public Property PRODUCT_CODE As String
        Public Property PRODUCT_NAME As String
        Public Property PRODUCT_STOCKS As String
        Public Property PRODUCT_PRICE As String
        Public Property PRODUCT_IMAGE As String
    End Class

    ' Use as a datasource context for the Product Card
    Public Class PropertyContainer
        Public Property CARD_TITLE As String
        Public Property CARD_DESC As String
        Public Property IMAGE_PATH As String
    End Class

    Public Class UserData
        Public Property USER_PROFILE As String
        Public Property USER_NAME As String
    End Class

    Public Class ErrorMessage
        Public Property ERROR_MESSAGE As String
    End Class

    Public Class SupplierProperty
        Public Property ID As Integer
        Public Property SUPPLIER_NAME As String
        Public Property SUPPLIER_ADDRESS As String
        Public Property SUPPLIER_CONTACT As String
        Public Property SUPPLIER_EMAIL As String
        Public Property SUPPLIER_CATEGORY As Int16
    End Class

    ' Check wether the phone number already exists
    Public Function IsNotDuplicatePhone(phone As String) As Boolean
        If tableAdapter.ScalarQueryDuplicatePhone(phone) > 0 Then
            Return False
        End If
        Return True
    End Function

    ' Check if the username is valid
    Public Function IsValidUserName(username As String) As Object()
        Dim tableAdapter As New usersTableAdapter
        Dim isNotExisting As Boolean = CBool(tableAdapter.ScalarQueryDuplicateUsername(username) = 0)
        Dim isValid As Boolean = Not Regex.IsMatch(username, "[^a-zA-Z0-9_]+")
        If isNotExisting Then
            If isValid Then
                Return {True}
            Else
                Return {False, "Invalid username!"}
            End If
        Else
            Return {False, "Username exists!"}
        End If
    End Function

    ' Check if the email is valid
    Public Function IsValidEmail(email As String) As Boolean
        ' This pattern means begin with word followed by @ followed by word and . and a word again
        If Regex.IsMatch(email, "^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") Then
            Return True
        Else
            Return False
        End If
    End Function

    ' Check if the controls are empty
    Public Function IsNotEmpty(controls As Object()) As Boolean
        For Each control In controls
            If TryCast(control, TextBox) IsNot Nothing Then
                Dim controlB As TextBox = TryCast(control, TextBox)
                If String.IsNullOrEmpty(controlB.Text) OrElse String.IsNullOrWhiteSpace(controlB.Text) Then
                    Return False
                End If
            ElseIf TryCast(control, ComboBox) IsNot Nothing Then
                Dim controlB As ComboBox = TryCast(control, ComboBox)
                If String.IsNullOrEmpty(controlB.Text) OrElse String.IsNullOrWhiteSpace(controlB.Text) Then
                    Return False
                End If
            ElseIf TryCast(control, PasswordBox) IsNot Nothing Then
                Dim controlB As PasswordBox = TryCast(control, PasswordBox)
                If String.IsNullOrEmpty(controlB.Password) OrElse String.IsNullOrWhiteSpace(controlB.Password) Then
                    Return False
                End If
            End If

        Next
        Return True
    End Function

    ' Check if the phone number is valid
    Public Function IsValidPhone(phone As String) As Boolean
        Console.WriteLine("ISMATCHED: " & Regex.IsMatch(phone, "([^0-9])+"))
        ' This pattern means all the characters that are not number can also be written ([^\d])+
        If Not Regex.IsMatch(phone, "([^0-9])+") AndAlso phone.Length >= MAX_PHONE_LENGTH Then
            Return True
        Else
            Return False
        End If
    End Function

    ' To fill the products in the scroll view
    Public Sub FillProducts(ByRef parentNode As WrapPanel)
        Dim productsDataTable As New productDataTable
        Dim productsTableAdapter As New productTableAdapter
        productsTableAdapter.Fill(productsDataTable)
        For i = 0 To productsDataTable.Rows.Count - 1
            With productsDataTable.Item(i)
                parentNode.Children.Add(New ProductCard(.id.ToString))
            End With
        Next
    End Sub

    Public Function SaveImage(filePath As String) As String
        ' Random name for the file
        Dim randomFileName As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Profiles\" & Path.GetRandomFileName() & If(filePath.EndsWith(".png"), ".png", ".jpg")

        ' Copies the file to the designated directory
        If Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Profiles") Then
            If File.Exists(filePath) AndAlso filePath IsNot Nothing Then
                File.Copy(filePath, randomFileName)
                Return randomFileName
            End If
        Else
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Profiles")
            If File.Exists(filePath) AndAlso Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Profiles") AndAlso filePath IsNot Nothing Then
                File.Copy(filePath, randomFileName)
                Return randomFileName
            End If
        End If
        Return Nothing
    End Function
End Module