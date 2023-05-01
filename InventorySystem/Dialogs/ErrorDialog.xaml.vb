Public Class ErrorDialog
    Public Sub New(errorMesage As String)
        InitializeComponent()
        Dim messageDataBinding As New ErrorMessage With {
            .ERROR_MESSAGE = errorMesage
        }
        Me.DataContext = messageDataBinding
    End Sub
End Class
