Imports MySql.Data.MySqlClient

Public Class ConnectSettingsForm
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim server = txtServer.Text.Trim()
        Dim user = txtUser.Text.Trim()
        Dim pass = txtPassword.Text.Trim()
        Dim db = txtDatabase.Text.Trim()

        Dim connStr = $"host={server};user={user};password={pass};database={db};"

        Try
            Using conn As New MySqlConnection(connStr)
                conn.Open()
                DatabaseHelper.connString = connStr ' ✅ Set the connection globally
                Me.DialogResult = DialogResult.OK
                Me.Hide()
                Dim loginForm As New Login()
                loginForm.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show(connStr & ex.Message)
        End Try
    End Sub


End Class