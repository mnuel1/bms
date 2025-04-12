Public Class Login

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
    End Sub

    Private Sub loginBtn_Click(sender As Object, e As EventArgs) Handles loginBtn.Click
        Dim username As String = usernameTextBox.Text.Trim().ToLower()
        Dim password As String = passwordTextbox.Text

        Dim query As String = "SELECT * FROM user WHERE LOWER(Username) = @Username AND Password = @Password AND Status = 'Active'"
        Dim parameters As New Dictionary(Of String, Object) From {
            {"@Username", username},
            {"@Password", password}
        }

        Dim dt As DataTable = GetData(query, parameters)

        If dt.Rows.Count = 1 Then
            Dim userRow As DataRow = dt.Rows(0)

            ' Store session info
            SessionInfo.LoggedInUserID = Convert.ToInt32(userRow("UserID"))
            SessionInfo.LoggedInUserFullName = userRow("FullName").ToString()
            SessionInfo.LoggedInUserLevel = userRow("UserLevelName").ToString()

            ' Open dashboard based on user level
            Select Case SessionInfo.LoggedInUserLevel
                Case "Administrator"
                    AdminDashboard.Show()
                Case "Accountant/Clerk"
                    ClerkDashboard.Show()
                Case "Staff"
                    StaffDashboard.Show()
                Case Else
                    MessageBox.Show("Unknown user level.")
                    Return
            End Select

            Me.Hide()
        Else
            MessageBox.Show("Invalid username or password, or the account is inactive.")
        End If
    End Sub
End Class
