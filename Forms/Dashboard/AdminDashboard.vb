Imports System.Drawing.Drawing2D

Public Class AdminDashboard
    Private Sub LoadContent(newContent As UserControl)
        SplitContainer1.Panel2.Controls.Clear()
        newContent.Dock = DockStyle.Fill
        SplitContainer1.Panel2.Controls.Add(newContent)
    End Sub

    Private Sub AdminDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Apply form styling
            Me.BackColor = Color.FromArgb(245, 246, 245) ' Off-white
            SplitContainer1.Panel1.BackColor = Color.FromArgb(232, 236, 239) ' Slightly darker off-white
            SplitContainer1.Panel2.BackColor = Color.FromArgb(245, 246, 245)

            ' Style navigation buttons
            For Each btn In {usersNav, bookingsNav, reportsNav, customersNav}
                btn.BackColor = Color.FromArgb(168, 230, 207) ' Mint green
                btn.ForeColor = Color.FromArgb(51, 51, 51)
                btn.FlatStyle = FlatStyle.Flat
                btn.FlatAppearance.BorderSize = 0
                btn.Font = New Font("Comic Sans MS", 12, FontStyle.Bold)
                MakeButtonRounded(btn)
                ' Add hover effects
                AddHandler btn.MouseEnter, Sub() btn.BackColor = Color.FromArgb(148, 210, 187)
                AddHandler btn.MouseLeave, Sub() btn.BackColor = Color.FromArgb(168, 230, 207)
            Next

            ' Style logout button
            adminLogout.BackColor = Color.FromArgb(200, 200, 200) ' Light gray
            adminLogout.ForeColor = Color.FromArgb(51, 51, 51)
            adminLogout.FlatStyle = FlatStyle.Flat
            adminLogout.FlatAppearance.BorderSize = 0
            adminLogout.Font = New Font("Comic Sans MS", 12, FontStyle.Bold)
            MakeButtonRounded(adminLogout)
            AddHandler adminLogout.MouseEnter, Sub() adminLogout.BackColor = Color.FromArgb(180, 180, 180)
            AddHandler adminLogout.MouseLeave, Sub() adminLogout.BackColor = Color.FromArgb(200, 200, 200)

            ' Load default content
            LoadContent(New UsersControl())
        Catch ex As Exception
            MessageBox.Show("Error in Load: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub usersNav_Click(sender As Object, e As EventArgs) Handles usersNav.Click
        LoadContent(New UsersControl())
    End Sub

    Private Sub bookingsNav_Click(sender As Object, e As EventArgs) Handles bookingsNav.Click
        LoadContent(New BookingsControl())
    End Sub

    Private Sub reportsNav_Click(sender As Object, e As EventArgs) Handles reportsNav.Click
        LoadContent(New ReportsControl())
    End Sub

    Private Sub customersNav_Click(sender As Object, e As EventArgs) Handles customersNav.Click
        LoadContent(New CustomersControl())
    End Sub

    Private Sub adminLogout_Click(sender As Object, e As EventArgs) Handles adminLogout.Click
        If MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Me.Hide()
            Dim loginForm As New Login()
            loginForm.Show()
        End If
    End Sub

    Private Sub MakeButtonRounded(btn As Button)
        Try
            Dim path As New GraphicsPath()
            Dim radius As Integer = 20
            Dim rect As New Rectangle(0, 0, btn.Width, btn.Height)
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90)
            path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90)
            path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90)
            path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90)
            path.CloseFigure()
            btn.Region = New Region(path)
        Catch
            ' Silent fail to avoid crashing
        End Try
    End Sub
End Class