Public Class StaffDashboard

    Private Sub LoadContent(newContent As UserControl)
        SplitContainer1.Panel2.Controls.Clear()
        newContent.Dock = DockStyle.Fill
        SplitContainer1.Panel2.Controls.Add(newContent)
    End Sub

    Private Sub StaffDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadContent(New StaffCustomerControl()) ' Load Booking as default
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

    Private Sub logout_Click(sender As Object, e As EventArgs) Handles staffLogout.Click
        Me.Hide()
        Login.Show()
    End Sub
End Class

