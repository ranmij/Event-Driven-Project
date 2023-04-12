Imports System.Text.RegularExpressions
Imports InventorySystem.InventorySystem.DataSets.UserDataSetTableAdapters
Imports Handy = HandyControl.Controls
Module Utils
    Private ReadOnly MAX_PHONE_LENGTH As Integer = &HB

    Public Function RandomAuthString() As String
        Return Nothing
    End Function

    Public Function IsValidUserName(username As String) As Object()
        Dim tableAdapter As New usersTableAdapter
        Dim isNotExisting As Boolean = tableAdapter.ScalarQueryDuplicateUsername(username) = 0
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

    Public Function IsValidEmail(email As String) As Boolean
        If Regex.IsMatch(email, "^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function IsNotEmpty(controls As Object()) As Boolean
        For Each control In controls
            If TryCast(control, Handy.PasswordBox) Is Nothing Then
                If String.IsNullOrEmpty(control.Text) OrElse String.IsNullOrWhiteSpace(control.Text) Then
                    Return False
                End If
            Else
                If String.IsNullOrEmpty(control.Password) OrElse String.IsNullOrWhiteSpace(control.Password) Then
                    Return False
                End If
            End If

        Next
        Return True
    End Function

    Public Function IsValidPhone(phone As String) As Boolean
        Console.WriteLine("ISMATCHED: " & Regex.IsMatch(phone, "([^0-9])+"))
        If Not Regex.IsMatch(phone, "([^0-9])+") AndAlso phone.Length >= MAX_PHONE_LENGTH Then
            Return True
        Else
            Return False
        End If
    End Function
End Module