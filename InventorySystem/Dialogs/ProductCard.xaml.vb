Public Class ProductCard
    Public Sub New(imagePath As String, cardTitle As String, cardDesc As String)
        InitializeComponent()
        Dim propertyObject As New PropertyContainer With {
            .CARD_TITLE = cardTitle,
            .CARD_DESC = cardDesc,
            .IMAGE_PATH = imagePath
        }
        Me.DataContext = propertyObject
    End Sub
End Class


