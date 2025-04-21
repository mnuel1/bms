Imports System.Drawing.Drawing2D

Public Class ClerkDashboard
    Private Sub LoadContent(newContent As UserControl)
        SplitContainer1.Panel2.Controls.Clear()
        newContent.Dock = DockStyle.Fill
        SplitContainer1.Panel2.Controls.Add(newContent)
    End Sub

    Private Sub ClerkDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Apply form styling
            Me.BackColor = Color.FromArgb(245, 246, 245) ' Off-white
            SplitContainer1.Panel1.BackColor = Color.FromArgb(232, 236, 239) ' Slightly darker off-white
            SplitContainer1.Panel2.BackColor = Color.FromArgb(245, 246, 245)

            ' Style navigation buttons
            For Each btn In {bookingsNav, paymentNav, refundNav, reportsNav}
                btn.BackColor = Color.FromArgb(255, 218, 185) ' Soft peach
                btn.ForeColor = Color.FromArgb(51, 51, 51)
                btn.FlatStyle = FlatStyle.Flat
                btn.FlatAppearance.BorderSize = 0
                btn.Font = New Font("Comic Sans MS", 12, FontStyle.Bold)
                MakeButtonRounded(btn)
                ' Add hover effects
                AddHandler btn.MouseEnter, Sub() btn.BackColor = Color.FromArgb(255, 200, 155)
                AddHandler btn.MouseLeave, Sub() btn.BackColor = Color.FromArgb(255, 218, 185)
            Next

            ' Style logout button
            accountantLogout.BackColor = Color.FromArgb(244, 199, 195) ' Light coral
            accountantLogout.ForeColor = Color.FromArgb(51, 51, 51)
            accountantLogout.FlatStyle = FlatStyle.Flat
            accountantLogout.FlatAppearance.BorderSize = 0
            accountantLogout.Font = New Font("Comic Sans MS", 12, FontStyle.Bold)
            MakeButtonRounded(accountantLogout)
            AddHandler accountantLogout.MouseEnter, Sub() accountantLogout.BackColor = Color.FromArgb(232, 168, 162)
            AddHandler accountantLogout.MouseLeave, Sub() accountantLogout.BackColor = Color.FromArgb(244, 199, 195)

            ' Load default content
            LoadContent(New AccountantBookingControl())
        Catch ex As Exception
            MessageBox.Show("Error in Load: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub bookingsNav_Click(sender As Object, e As EventArgs) Handles bookingsNav.Click
        LoadContent(New AccountantBookingControl())
    End Sub

    Private Sub paymentNav_Click(sender As Object, e As EventArgs) Handles paymentNav.Click
        LoadContent(New AccountantPaymentControl())
    End Sub

    Private Sub refundNav_Click(sender As Object, e As EventArgs) Handles refundNav.Click
        LoadContent(New AccountantRefundControl())
    End Sub

    Private Sub reportsNav_Click(sender As Object, e As EventArgs) Handles reportsNav.Click
        LoadContent(New accountantReportControl())
    End Sub

    Private Sub accountantLogout_Click(sender As Object, e As EventArgs) Handles accountantLogout.Click
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