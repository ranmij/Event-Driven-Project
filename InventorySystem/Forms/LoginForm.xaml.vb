Imports System.Windows.Threading
Imports HandyControl.Controls
Imports HandyControl.Tools.Extension
Public Class LoginForm
    Private signForm As SignUpForm

    Private Sub Verified()
        Dim controls() As Object = {UsernameTextBox, PasswordTextBox}
        If IsNotEmpty(controls) Then
            If Login(UsernameTextBox.Text, PasswordTextBox.Password) Then
                Dialog.Show(New LoadingDialog())
                Dim timer As New DispatcherTimer()
                AddHandler timer.Tick, AddressOf CloseTick
                timer.Interval = New TimeSpan(0, 0, 2.5)
                timer.Start()
            Else
                ' TODO A string literal!
                ErrorLabel.Visibility = Visibility.Visible
                ErrorLabel.Text = "Incorrect username or password."
            End If
        Else
            ' TODO A string literal!
            ErrorLabel.Visibility = Visibility.Visible
            ErrorLabel.Text = "Please fill the empty fields."
        End If
    End Sub

    Private Sub ClickEvents(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click, LoginButton.Click, SignupButton.Click
        If sender.Equals(CloseButton) Then              ' CLOSE
            Me.Close()
        ElseIf sender.Equals(LoginButton) Then          ' LOG IN
            Verified()
        ElseIf sender.Equals(SignupButton) Then         ' SIGN UP
            signForm = New SignUpForm()
            signForm.Show()
            Me.Close()
        End If
    End Sub

    Private Sub CloseTick(sender As Object, e As EventArgs)
        TryCast(sender, DispatcherTimer).Stop()
        Dim dash As New Dashboard()
        dash.Show()
        Me.Close()

    End Sub
    Private Sub DragForm(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseLeftButtonDown
        DragMove()
    End Sub
End Class
