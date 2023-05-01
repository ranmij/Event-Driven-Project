Imports Firebase.Database
Imports Firebase.Database.Query
Imports BCrypt.Net.BCrypt
Imports System.Security.Policy
Imports System.Net.Http
Imports InventorySystem.InventorySystem.DataSets.UserDataSet

Module FirebaseUtil

    Public Class FirebaseCategory
        Public Property category_name As String
    End Class

    Public Class FirebaseUnit
        Public Property unit_name As String
    End Class

    Public Class FireBaseUserProp
        Public Property id As String
        Public Property first_name As String
        Public Property last_name As String
        Public Property gender As String
        Public Property phone As String
        Public Property email As String
        Public Property username As String
        Public Property password As String
        Public Property date_created As String
    End Class

    Public Class FireBaseProductProp
        Public Property product_code As String
        Public Property category As String
        Public Property unit As String
        Public Property product_name As String
        Public Property unit_in_stock As String
        Public Property unit_price As String
        Public Property image_path As String
    End Class

    Public Class Auth
        Public Property auth_code As String
        Public Property phone As String
        Public Property expire_time As String
    End Class


    Private client As New FirebaseClient(
        My.Settings.firebasePath,
        New FirebaseOptions() With {
            .AuthTokenAsyncFactory = Function() Task.FromResult(My.Settings.firebaseSecret)
        }
    )

    Public isExecuting As Boolean = False

#Region "UserFunctions"

    Public Async Function FirebaseSignUpAsync(userData As FireBaseUserProp) As Task(Of Boolean)
        userData.password = HashPassword(userData.password)
        Dim res = client.Child("users").Child(userData.username).PutAsync(userData)
        Await res
        If res.IsCompletedSuccessfully Then
            Return True
        End If
        Return False
    End Function

    Public Async Function FirebaseLogInAsync(username As String, password As String) As Task(Of Boolean)
        Dim res = Await client.Child("users").OrderByKey().EqualTo(username).OnceAsync(Of Dictionary(Of String, Object))
        If res.Count > 0 Then
            With res.ElementAt(0)
                Dim hashPass As String = .Object.Item("password")
                If Verify(password, hashPass) Then
                    My.Settings.UserID = CInt(.Object.Item("id"))
                    My.Settings.firebaseUsername = CStr(.Object.Item("username"))
                    My.Settings.Save()
                    Return True
                End If
            End With
        End If
        Return False
    End Function

    Public Async Sub FirebaseGenerateAuth(phone As String)
        Dim auth_code As String = New System.Random().Next(100000, 999999).ToString()
        Dim Authobj As New Auth() With {.auth_code = auth_code, .phone = phone, .expire_time = DateAndTime.Now().ToString}
        Try
            Dim res = client.Child("auth").Child(phone).PutAsync(Authobj)
            Dim clientHttp As New HttpClient()
            Dim response As HttpResponseMessage = Await clientHttp.GetAsync(String.Format("http://{0}:8080/?phone={1}&message=OTP CODE {2}", My.Settings.smsIP, phone, auth_code))
        Catch ex As Exception
            MsgBox("Unknown Error")
        End Try
    End Sub

    Public Async Function FirebaseIsAuthVerified(phone As String, auth_code As String) As Task(Of Boolean)
        Dim res = Await client.Child("auth").OrderByKey().EqualTo(phone).OnceAsync(Of Dictionary(Of String, Object))
        If res.Count > 0 Then
            With res.ElementAt(0)
                If auth_code = .Object.Item("auth_code") Then
                    Return True
                End If
            End With
        End If
        Return False
    End Function

    Public Async Sub FirebaseDeleteAuth(phone As String)
        Dim res = client.Child("auth").OrderByKey().EqualTo(phone).DeleteAsync()
        Await res
    End Sub
#End Region

#Region "ProductsFunctions"
    Public Async Function FirebaseAddProductAsync(productData As FireBaseProductProp) As Task(Of Boolean)
        Dim res = client.Child("products").Child(productData.product_name).PutAsync(productData)
        Await res
        If res.IsCompletedSuccessfully Then
            Return True
        End If
        Return False
    End Function

    Public Async Function FirebaseDeleteProductAsync(product_name As String) As Task(Of Boolean)
        Dim res = client.Child("products").Child(product_name).DeleteAsync()
        Await res
        If res.IsCompletedSuccessfully Then
            Return True
        End If
        Return False
    End Function

    Public Async Function FirebaseFillProducts(parentNode As WrapPanel) As Task(Of Object)
        isExecuting = True
        Dim productsData = Await client.Child("products").OnceAsync(Of Dictionary(Of String, Object))
        For Each product In productsData
            With product.Object
                Dim imagePath As String = .Item("image_path")
                Dim cardTitle As String = .Item("product_name")
                Dim cardPrice As String = CStr(.Item("unit_price"))
                parentNode.Children.Add(New ProductCard(imagePath, cardTitle, cardPrice))
            End With
        Next
        isExecuting = False
        Return Nothing
    End Function

    Public Async Function FirebaseProductCollectionAsync() As Task(Of PropertyContainer())
        Dim productsData = Await client.Child("products").OnceAsync(Of Dictionary(Of String, Object))
        Dim productCollection() As PropertyContainer = {}
        For Each product In productsData
            With product.Object
                Dim CARD_TITLE As String = .Item("product_name")
                Dim CARD_DESC As String = .Item("unit_price")
                Dim IMAGE_PATH As String = .Item("image_path")
                productCollection.Append(New PropertyContainer With {
                    .CARD_TITLE = CARD_TITLE,
                    .CARD_DESC = CARD_DESC,
                    .IMAGE_PATH = IMAGE_PATH
                })
            End With
        Next
        Return productCollection
    End Function

    Public Async Function FirebaseGetCategory() As Task(Of List(Of String))
        Dim categoryData = Await client.Child("category").OnceAsync(Of Dictionary(Of String, Object))
        Dim categoryCollection As New List(Of String)
        For Each product In categoryData
            With product.Object
                categoryCollection.Add(.Item("category_name").ToString().ToUpper())
            End With
        Next
        Return categoryCollection
    End Function

    Public Async Function FirebaseGetUnit() As Task(Of List(Of String))
        Dim unitData = Await client.Child("unit").OnceAsync(Of Dictionary(Of String, Object))
        Dim unitCollection As New List(Of String)
        For Each product In unitData
            With product.Object
                unitCollection.Add(.Item("unit_name").ToString().ToUpper())
            End With
        Next
        Return unitCollection
    End Function
#End Region

End Module
