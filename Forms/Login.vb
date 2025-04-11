Public Class Login

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Prevent resizing of the form
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False   ' Disable the maximize button
        Me.MinimizeBox = False   ' Disable the minimize button

    End Sub

    Private Sub loginBtn_Click(sender As Object, e As EventArgs) Handles loginBtn.Click
        Dim username As String = usernameTextBox.Text.Trim().ToLower()
        Dim password As String = passwordTextbox.Text

        Select Case username
            Case "admin"
                If password = "admin123" Then
                    AdminDashboard.Show()
                    Me.Hide()
                Else
                    MessageBox.Show("Wrong password for Admin.")
                End If

            Case "staff"
                If password = "staff123" Then
                    StaffDashboard.Show()
                    Me.Hide()
                Else
                    MessageBox.Show("Wrong password for Staff.")
                End If

            Case "clerk"
                If password = "clerk123" Then
                    ClerkDashboard.Show()
                    Me.Hide()
                Else
                    MessageBox.Show("Wrong password for Clerk.")
                End If

            Case Else
                MessageBox.Show("Invalid username.")
        End Select
    End Sub
End Class
