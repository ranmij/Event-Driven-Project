' This is for me

Option Strict On
Imports HandyControl.Controls

Public Class SignUpForm

    Private Sub Click_Events(sender As Object, e As RoutedEventArgs) Handles SignUpButton.Click, NextButton.Click, ConfirmAuthButton.Click
        If sender.Equals(NextButton) Then
            Dim controls() As Object = {FirstnameTextBox, LastnameTextBox, EmailTextBox, PhoneTextBox, GenderComboBox}
            ' Are the textboxes empty?
            If IsNotEmpty(controls) Then
                ' Is the email valid?
                If IsValidEmail(EmailTextBox.Text) Then
                    ' Is the phone number valid?
                    If IsValidPhone(PhoneTextBox.Text) Then
                        ' Is the phone number not taken?
                        If IsNotDuplicatePhone(PhoneTextBox.Text) Then
                            ' Is the first panel visible?
                            If firstpane.IsVisible Then
                                ' Clear the error and hide the first panel then second panel
                                ErrorLabel.Visibility = Visibility.Hidden
                                firstpane.Visibility = Visibility.Hidden
                                secondpane.Visibility = Visibility.Visible
                            End If
                        Else
                            ' Display taken phone number error
                            ErrorLabel.Visibility = Visibility.Visible
                            ErrorLabel.Text = "This number is already taken."
                        End If
                    Else
                        ' Display invalid phone error
                        ErrorLabel.Visibility = Visibility.Visible
                        ErrorLabel.Text = "Invalid phone."
                    End If
                Else
                    ' Display invalid email error
                    ErrorLabel.Visibility = Visibility.Visible
                    ErrorLabel.Text = "Invalid email."
                End If
            Else
                ' Display empty field error
                'TOOD A String literal
                ErrorLabel.Visibility = Visibility.Visible
                ErrorLabel.Text = "Plase fill out the empty fields."
            End If
        ElseIf sender.Equals(SignUpButton) Then
            ' Check if the username is valid and get the error result if there is any
            Dim validationResult() As Object = IsValidUserName(UsernameTextBox.Text)
            Dim controls() As Object = {UsernameTextBox, PasswordTextBox, ConfirmPasswordTextBox}
            ' Are the textboxes empty?
            If IsNotEmpty(controls) Then
                ' Is the username valid?
                If CBool(validationResult(0)) Then
                    ' Are the password and confirm password the same?
                    If PasswordTextBox.Text.ToLower = ConfirmPasswordTextBox.Text.ToLower Then

                        secondpane.Visibility = Visibility.Collapsed
                        thirdpane.Visibility = Visibility.Visible
                        GenerateAuth(PhoneTextBox.Text)
                    Else
                        ' Display the password error
                        ' TODO A string literal!
                        ErrorLabel2.Visibility = Visibility.Visible
                        ErrorLabel2.Text = "Password doesn't match!"
                    End If
                Else
                    ' Display the username error
                    ErrorLabel2.Visibility = Visibility.Visible
                    ErrorLabel2.Text = CStr(validationResult(1))
                End If
            Else
                ' Display the empty fields error
                ErrorLabel2.Visibility = Visibility.Visible
                ErrorLabel2.Text = "Please fill the empty fields."
            End If
        ElseIf sender.Equals(ConfirmAuthButton) Then
            If IsAuthVerified(PhoneTextBox.Text, AuthCodeTextBox.Text) Then
                'Peform the sign in operation
                Dim hasSignedIn As Boolean = SignIn(New UserModel With {
                                .first_name = FirstnameTextBox.Text,
                                .last_name = LastnameTextBox.Text,
                                .gender = GenderComboBox.Text.Chars(0),
                                .phone = PhoneTextBox.Text,
                                .email = EmailTextBox.Text,
                                .username = UsernameTextBox.Text,
                                .password = PasswordTextBox.Text
                        })
                ' has the user signed in?
                If hasSignedIn Then
                    ' Clear the errors and close this form and go back to the log in form
                    ErrorLabel2.Visibility = Visibility.Hidden
                    Dim login As New LoginForm()
                    login.Show()
                    DeleteAuth(PhoneTextBox.Text)
                    Me.Close()
                Else
                    ' Display the unknown error
                    ' TODO A string literal!
                    AuthErrorLabel.Visibility = Visibility.Visible
                    AuthErrorLabel.Text = "An error occured please try again."
                End If
            Else
                Dialog.Show(New AuthErrorDialog())
            End If
        End If
    End Sub

    Private Sub BackButton(sender As Object, e As EventArgs) Handles LoginBackButton.Click, SignUpBackButton.Click, SignUp1BackButton.Click
        If sender.Equals(LoginBackButton) Then
            Dim login As New LoginForm
            login.Show()
            Me.Close()
        ElseIf sender.Equals(SignUpBackButton) Then
            secondpane.Visibility = Visibility.Collapsed
            firstpane.Visibility = Visibility.Visible
        ElseIf sender.Equals(SignUp1BackButton) Then
            thirdpane.Visibility = Visibility.Collapsed
            secondpane.Visibility = Visibility.Visible
        End If
    End Sub

    ' To drag move the form
    Private Sub DragForm(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseLeftButtonDown
        DragMove()
    End Sub
End Class
