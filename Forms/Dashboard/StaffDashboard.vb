Imports System.Drawing.Drawing2D

Public Class StaffDashboard
    Private Sub LoadContent(newContent As UserControl)
        SplitContainer1.Panel2.Controls.Clear()
        newContent.Dock = DockStyle.Fill
        SplitContainer1.Panel2.Controls.Add(newContent)
    End Sub

    Private Sub StaffDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Apply form styling
            Me.BackColor = Color.FromArgb(245, 246, 245) ' Off-white
            SplitContainer1.Panel1.BackColor = Color.FromArgb(232, 236, 239) ' Slightly darker off-white
            SplitContainer1.Panel2.BackColor = Color.FromArgb(245, 246, 245)

            ' Style navigation buttons
            For Each btn In {dashboardNav, customersNav, bookingNav, venueNav}
                btn.BackColor = Color.FromArgb(179, 229, 252) ' Baby blue
                btn.ForeColor = Color.FromArgb(51, 51, 51)
                btn.FlatStyle = FlatStyle.Flat
                btn.FlatAppearance.BorderSize = 0
                btn.Font = New Font("Comic Sans MS", 12, FontStyle.Bold)
                MakeButtonRounded(btn)
                ' Add hover effects
                AddHandler btn.MouseEnter, Sub() btn.BackColor = Color.FromArgb(135, 206, 235)
                AddHandler btn.MouseLeave, Sub() btn.BackColor = Color.FromArgb(179, 229, 252)
            Next

            ' Style logout button
            staffLogout.BackColor = Color.FromArgb(230, 230, 250) ' Light lavender
            staffLogout.ForeColor = Color.FromArgb(51, 51, 51)
            staffLogout.FlatStyle = FlatStyle.Flat
            staffLogout.FlatAppearance.BorderSize = 0
            staffLogout.Font = New Font("Comic Sans MS", 12, FontStyle.Bold)
            MakeButtonRounded(staffLogout)
            AddHandler staffLogout.MouseEnter, Sub() staffLogout.BackColor = Color.FromArgb(216, 191, 216)
            AddHandler staffLogout.MouseLeave, Sub() staffLogout.BackColor = Color.FromArgb(230, 230, 250)

            ' Load default content
            LoadContent(New StaffDashboardControl())
        Catch ex As Exception
            MessageBox.Show("Error in Load: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dashboardNav_Click(sender As Object, e As EventArgs) Handles dashboardNav.Click
        LoadContent(New StaffDashboardControl())
    End Sub

    Private Sub customersNav_Click(sender As Object, e As EventArgs) Handles customersNav.Click
        LoadContent(New StaffCustomerControl())
    End Sub

    Private Sub bookingNav_Click(sender As Object, e As EventArgs) Handles bookingNav.Click
        LoadContent(New StaffBookingControl())
    End Sub

    Private Sub venueNav_Click(sender As Object, e As EventArgs) Handles venueNav.Click
        LoadContent(New StaffVenueControl())
    End Sub

    Private Sub staffLogout_Click(sender As Object, e As EventArgs) Handles staffLogout.Click
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