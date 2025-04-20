Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D

Public Class ConnectSettingsForm
    Private Sub ConnectSettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Apply form styling
            Me.BackColor = Color.FromArgb(245, 246, 245) ' Off-white
            Me.FormBorderStyle = FormBorderStyle.FixedSingle
            Me.WindowState = FormWindowState.Maximized

            ' Center panel
            CenterPanel()

            ' Style textboxes
            For Each txt In {txtServer, txtUser, txtPassword, txtDatabase}
                txt.BackColor = Color.FromArgb(232, 246, 239) ' Soft mint
                txt.ForeColor = Color.FromArgb(51, 51, 51)
                txt.Font = New Font("Comic Sans MS", 12.0!)
                txt.BorderStyle = BorderStyle.None ' Remove default border
                MakeTextBoxRounded(txt)
            Next

            ' Style buttons
            btnSave.BackColor = Color.FromArgb(168, 230, 207) ' Mint green
            btnSave.ForeColor = Color.FromArgb(51, 51, 51)
            btnSave.FlatStyle = FlatStyle.Flat
            btnSave.FlatAppearance.BorderSize = 0
            btnSave.Font = New Font("Comic Sans MS", 12, FontStyle.Bold)

            btnCancel.BackColor = Color.FromArgb(200, 200, 200) ' Light gray
            btnCancel.ForeColor = Color.FromArgb(51, 51, 51)
            btnCancel.FlatStyle = FlatStyle.Flat
            btnCancel.FlatAppearance.BorderSize = 0
            btnCancel.Font = New Font("Comic Sans MS", 12, FontStyle.Bold)

            ' Add hover effects
            AddHandler btnSave.MouseEnter, Sub() btnSave.BackColor = Color.FromArgb(148, 210, 187)
            AddHandler btnSave.MouseLeave, Sub() btnSave.BackColor = Color.FromArgb(168, 230, 207)
            AddHandler btnCancel.MouseEnter, Sub() btnCancel.BackColor = Color.FromArgb(180, 180, 180)
            AddHandler btnCancel.MouseLeave, Sub() btnCancel.BackColor = Color.FromArgb(200, 200, 200)

            ' Make buttons rounded
            MakeButtonRounded(btnSave)
            MakeButtonRounded(btnCancel)

            ' Set default status
            lblStatus.Text = ""
        Catch ex As Exception
            MessageBox.Show("Error in Load: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Sub ConnectSettingsForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        CenterPanel()
    End Sub

    Private Sub CenterPanel()
        If pnlContainer IsNot Nothing Then
            Dim x As Integer = (Me.ClientSize.Width - pnlContainer.Width) \ 2
            Dim y As Integer = (Me.ClientSize.Height - pnlContainer.Height) \ 2
            pnlContainer.Location = New Point(Math.Max(x, 0), Math.Max(y, 0))
        End If
    End Sub

    Private Sub MakeButtonRounded(btn As Button)
        Dim path As New GraphicsPath()
        Dim radius As Integer = 20
        Dim rect As New Rectangle(0, 0, btn.Width, btn.Height)
        path.AddArc(rect.X, rect.Y, radius, radius, 180, 90)
        path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90)
        path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90)
        path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90)
        path.CloseFigure()
        btn.Region = New Region(path)
    End Sub

    Private Sub MakeTextBoxRounded(txt As TextBox)
        Dim path As New GraphicsPath()
        Dim radius As Integer = 10
        Dim rect As New Rectangle(0, 0, txt.Width, txt.Height)
        path.AddArc(rect.X, rect.Y, radius, radius, 180, 90)
        path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90)
        path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90)
        path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90)
        path.CloseFigure()
        txt.Region = New Region(path)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Dim server = txtServer.Text.Trim()
            Dim user = txtUser.Text.Trim()
            Dim pass = txtPassword.Text.Trim()
            Dim db = txtDatabase.Text.Trim()

            If String.IsNullOrEmpty(server) OrElse String.IsNullOrEmpty(user) OrElse String.IsNullOrEmpty(db) Then
                lblStatus.Text = "Please fill in all required fields."
                lblStatus.ForeColor = Color.FromArgb(255, 0, 0)
                Return
            End If

            Dim connStr = $"host={server};user={user};password={pass};database={db};"
            lblStatus.Text = "Connecting..."
            lblStatus.ForeColor = Color.FromArgb(51, 51, 51)
            Application.DoEvents()

            Using conn As New MySqlConnection(connStr)
                conn.Open()
                DatabaseHelper.connString = connStr
                lblStatus.Text = "Connected successfully!"
                lblStatus.ForeColor = Color.FromArgb(0, 128, 0)
                Me.DialogResult = DialogResult.OK ' Set for ShowDialog compatibility
                Dim loginForm As New Login()
                loginForm.Show()
                Me.Hide() ' Close instead of Hide to match Application.Run
            End Using
        Catch ex As Exception
            lblStatus.Text = "Connection failed: " & ex.Message
            lblStatus.ForeColor = Color.FromArgb(255, 0, 0)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class