Public Class Dashboard
    Private Sub ClickEvents(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click, RestoreDButton.Click,
        MinimizeButton.Click
        If sender.Equals(CloseButton) Then
            Me.Close()
        ElseIf sender.Equals(RestoreDButton) Then
            Dim rectArea As Rect = SystemParameters.WorkArea
            If Me.Width = rectArea.Width AndAlso Me.Height = rectArea.Height Then
                Me.Width = Me.MinWidth
                Me.Height = Me.MinHeight
                RestoreDIcon.Source = TryCast(FindResource("ic_maximizebit"), ImageSource)
            Else
                Me.Left = 0
                Me.Top = 0
                Me.Width = rectArea.Width
                Me.Height = rectArea.Height
                RestoreDIcon.Source = TryCast(FindResource("ic_restoredownbit"), ImageSource)
            End If
        ElseIf sender.Equals(MinimizeButton) Then
            Me.WindowState = WindowState.Minimized
        End If
    End Sub

    Private Sub Dashboard_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseLeftButtonDown
        DragMove()
    End Sub
End Class
