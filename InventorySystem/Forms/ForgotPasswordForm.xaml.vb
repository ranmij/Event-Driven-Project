Public Class ForgotPasswordForm
    Private _phone As String
    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click
        Me.Close()
    End Sub

    Private Sub ConfirmCodeButton_Click(sender As Object, e As RoutedEventArgs) Handles ConfirmCodeButton.Click
        If IsAuthVerified(_phone, CodeTextBox.Text) Then
            ConfirmContainer.Visibility = Visibility.Collapsed
            ChangeContainer.Visibility = Visibility.Visible
            CodeTextBox.BorderBrush = Brushes.Gray
        Else
            CodeTextBox.BorderBrush = Brushes.Red
        End If
    End Sub

    Private Sub ChangePasswordButton_Click(sender As Object, e As RoutedEventArgs) Handles ChangePasswordButton.Click
        If NewPasswordTextBox.Text = ConfirmPasswordTextBox.Text Then
            Dim loginF As New LoginForm
            loginF.Show()
            Me.Close()
        Else
            HandyControl.Controls.MessageBox.Info("Password doesn't match.")
        End If

    End Sub

    Private Sub FindButton_Click(sender As Object, e As RoutedEventArgs) Handles FindButton.Click
        If IsAccountExists(UserIDTextBox.Text) Then
            FindContainer.Visibility = Visibility.Collapsed
            ConfirmContainer.Visibility = Visibility.Visible
            _phone = GetUserByQuery(UserIDTextBox.Text).Rows.Item(0).Item(0)
            GenerateAuth(_phone)
            UserIDTextBox.BorderBrush = Brushes.Gray
        Else
            HandyControl.Controls.MessageBox.Info("Username or Email does not exists.")
            UserIDTextBox.BorderBrush = Brushes.Red
        End If
    End Sub
End Class
