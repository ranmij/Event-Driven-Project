Public Class SignUpForm

    Private Sub Click_Events(sender As Object, e As RoutedEventArgs) Handles SignUpButton.Click, NextButton.Click
        If sender.Equals(NextButton) Then
            Dim controls() As Object = {FirstnameTextBox, LastnameTextBox, EmailTextBox, PhoneTextBox, GenderComboBox}
            If IsNotEmpty(controls) Then
                If IsValidEmail(EmailTextBox.Text) Then
                    If IsValidPhone(PhoneTextBox.Text) Then
                        If firstpane.IsVisible Then
                            ErrorLabel.Visibility = Visibility.Hidden
                            firstpane.Visibility = Visibility.Hidden
                            secondpane.Visibility = Visibility.Visible
                        End If
                    Else
                        ErrorLabel.Visibility = Visibility.Visible
                        ErrorLabel.Text = "Invalid phone."
                    End If
                Else
                    ErrorLabel.Visibility = Visibility.Visible
                    ErrorLabel.Text = "Invalid email."
                End If
            Else
                'TOOD A String literal
                ErrorLabel.Visibility = Visibility.Visible
                ErrorLabel.Text = "Plase fill out the empty fields."
            End If
        ElseIf sender.Equals(SignUpButton) Then
            Dim validationResult() As Object = IsValidUserName(UsernameTextBox.Text)
            Dim controls() As Object = {UsernameTextBox, PasswordTextBox, ConfirmPasswordTextBox}
            If IsNotEmpty(controls) Then
                If CBool(validationResult(0)) Then
                    If PasswordTextBox.Text.ToLower = ConfirmPasswordTextBox.Text.ToLower Then
                        Dim hasSignedIn As Boolean = SignIn(New UserModel With {
                                .first_name = FirstnameTextBox.Text,
                                .last_name = LastnameTextBox.Text,
                                .gender = GenderComboBox.Text.Chars(0),
                                .phone = PhoneTextBox.Text,
                                .email = EmailTextBox.Text,
                                .username = UsernameTextBox.Text,
                                .password = PasswordTextBox.Text
                        })
                        If hasSignedIn Then
                            ErrorLabel2.Visibility = Visibility.Hidden
                            Dim login As New LoginForm()
                            login.Show()
                            Me.Close()
                        Else
                            ' TODO A string literal!
                            ErrorLabel2.Visibility = Visibility.Visible
                            ErrorLabel2.Text = "An error occured please try again."
                        End If
                    Else
                        ' TODO A string literal!
                        ErrorLabel2.Visibility = Visibility.Visible
                        ErrorLabel2.Text = "Password doesn't match!"
                    End If
                Else
                    ErrorLabel2.Visibility = Visibility.Visible
                    ErrorLabel2.Text = CStr(validationResult(1))
                End If
            Else
                ErrorLabel2.Visibility = Visibility.Visible
                ErrorLabel2.Text = "Please fill the empty fields."
            End If
        End If
    End Sub

    Private Sub DragForm(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseLeftButtonDown
        DragMove()
    End Sub
End Class
