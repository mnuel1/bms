Public Class AdminDashboard
    Private Sub LoadContent(newContent As UserControl)
        SplitContainer1.Panel2.Controls.Clear()
        newContent.Dock = DockStyle.Fill
        SplitContainer1.Panel2.Controls.Add(newContent)
    End Sub

    Private Sub AdminDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadContent(New UsersControl()) ' Load UsersControl on startup
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
        ' Optional: confirm logout and return to login form
        Me.Hide()
        Login.Show()
    End Sub

    Private Sub SplitContainer1_Panel2_Paint(sender As Object, e As PaintEventArgs) Handles SplitContainer1.Panel2.Paint

    End Sub
End Class