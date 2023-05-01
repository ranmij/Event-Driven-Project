' This is for me

Option Strict On
Imports System.Net.Http
Imports BCrypt.Net.BCrypt
Imports InventorySystem.InventorySystem.DataSets.UserDataSet
Imports InventorySystem.InventorySystem.DataSets.UserDataSetTableAdapters
Module UserModule
    ReadOnly tableAdapter As New usersTableAdapter
    ReadOnly authDataTable As New authDataTable
    ReadOnly authDataAdapter As New authTableAdapter
    Public Class LocalUserProp
        Public user_id As Integer
        Public first_name As String
        Public last_name As String
        Public gender As String
        Public phone As String
        Public email As String
        Public username As String
        Public password As String
    End Class

    Public Function IsAuthVerified(phone As String, auth_code As String) As Boolean
        authDataAdapter.FillByAuth(authDataTable, phone)
        If authDataTable.Rows.Count > 0 Then
            With authDataTable.Item(0)
                If auth_code = .auth_code Then
                    Return True
                End If
            End With
        End If
        Return False
    End Function

    Public Async Sub GenerateAuth(phone As String)
        Dim auth_code As String = New System.Random().Next(100000, 999999).ToString()
        authDataAdapter.InsertQueryAuth(phone, auth_code)
        Try
            Dim client As New HttpClient()
            Dim response As HttpResponseMessage = Await client.GetAsync(String.Format("http://{0}:8080/?phone={1}&message=OTP CODE {2}", My.Settings.smsIP, phone, auth_code))
        Catch ex As Exception
            MsgBox("Unknown Error")
        End Try
    End Sub

    Public Sub DeleteAuth(phone As String)
        authDataAdapter.DeleteQueryAuth(phone)
    End Sub

    Public Function ChangePassword(new_password As String, user_id As String) As Boolean
        ' TODO Implement Sign In
        Return False
    End Function

    Public Function ForgotPassword() As Boolean
        'TODO Implement Sign In
        Return True
    End Function

    Public Function Login(username As String, password As String) As Boolean
        ' TODO Implement Sign In
        Dim result As usersDataTable = tableAdapter.GetDataByUsername(username)
        If result.Count <> 0 Then
            With result.Item(0)
                Dim passwordHashed As String = .password.ToString()
                If Verify(password, passwordHashed) Then
                    My.Settings.UserID = CInt(.id)
                    My.Settings.Save()
                    Return True
                Else
                    Return False
                End If
            End With
        Else
            Return False
        End If

    End Function

    Public Function SignIn(userData As LocalUserProp) As Boolean
        With userData
            Dim hashedPassword As String = HashPassword(.password)

            ' Check if there's an existing admin in the system.
            If tableAdapter.ScalarQueryUsers() = 0 Then
                If tableAdapter.InsertQueryAdmin(1, .first_name, .last_name, .gender, .phone, .email, .username, hashedPassword) <> 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                If tableAdapter.InsertQueryUser(.first_name, .last_name, .gender, .phone, .email, .username, hashedPassword) <> 0 Then
                    Return True
                Else
                    Return False
                End If
            End If
        End With
    End Function

End Module

