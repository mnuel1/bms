Imports System.Data
Imports MySql.Data.MySqlClient

Public Class AccountantBookingControl
    Inherits UserControl

    Private dgvBookings As DataGridView
    Private dgvPayments As DataGridView
    Private customerPanel As TableLayoutPanel

    Private lblCustomerName, lblContact, lblEmail, lblAddress, lblCity, lblProvince As Label

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadBookings()
    End Sub

    Private Sub SetupLayout()
        ' Bookings Grid
        dgvBookings = New DataGridView With {
            .Dock = DockStyle.Top,
            .Height = 200,
            .ReadOnly = True,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False
        }
        AddHandler dgvBookings.CellClick, AddressOf dgvBookings_CellClick

        ' Customer Info Labels
        lblCustomerName = New Label()
        lblContact = New Label()
        lblEmail = New Label()
        lblAddress = New Label()
        lblCity = New Label()
        lblProvince = New Label()

        customerPanel = New TableLayoutPanel With {
            .Dock = DockStyle.Top,
            .Height = 120,
            .ColumnCount = 2,
            .RowCount = 6,
            .AutoSize = True
        }

        customerPanel.Controls.Add(New Label With {.Text = "Name:"}, 0, 0)
        customerPanel.Controls.Add(lblCustomerName, 1, 0)

        customerPanel.Controls.Add(New Label With {.Text = "Contact:"}, 0, 1)
        customerPanel.Controls.Add(lblContact, 1, 1)

        customerPanel.Controls.Add(New Label With {.Text = "Email:"}, 0, 2)
        customerPanel.Controls.Add(lblEmail, 1, 2)

        customerPanel.Controls.Add(New Label With {.Text = "Address:"}, 0, 3)
        customerPanel.Controls.Add(lblAddress, 1, 3)

        customerPanel.Controls.Add(New Label With {.Text = "City:"}, 0, 4)
        customerPanel.Controls.Add(lblCity, 1, 4)

        customerPanel.Controls.Add(New Label With {.Text = "Province:"}, 0, 5)
        customerPanel.Controls.Add(lblProvince, 1, 5)

        ' Payments Grid
        dgvPayments = New DataGridView With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False
        }

        ' Labels
        Dim lblBooking As New Label With {.Text = "Bookings", .Dock = DockStyle.Top, .Font = New Font("Segoe UI", 10, FontStyle.Bold), .Height = 25}
        Dim lblCustomer As New Label With {.Text = "Customer Information", .Dock = DockStyle.Top, .Font = New Font("Segoe UI", 10, FontStyle.Bold), .Height = 25}
        Dim lblPayment As New Label With {.Text = "Payments", .Dock = DockStyle.Top, .Font = New Font("Segoe UI", 10, FontStyle.Bold), .Height = 25}

        ' Layout
        Me.Controls.Add(dgvPayments)
        Me.Controls.Add(lblPayment)
        Me.Controls.Add(customerPanel)
        Me.Controls.Add(lblCustomer)
        Me.Controls.Add(dgvBookings)
        Me.Controls.Add(lblBooking)
    End Sub

    Private Sub LoadBookings()
        Dim query As String = "SELECT BookingID, CustomerID, EventID, ServiceID, BookingDate, BookedBy, BookingStatus, TotalAmount, PaymentStatus, Remarks FROM booking"
        Dim dt As DataTable = GetData(query)
        dgvBookings.DataSource = dt
    End Sub

    Private Sub LoadPayments(bookingID As Integer)
        Dim query As String = "SELECT PaymentID, PaymentDate, AmountPaid, PaymentMethod, ReferenceNumber, PaymentStatus, ProcessedBy, PaymentTime,
                               Balance, DiscountAmount, RefundedAmount, Remarks, ORNumber 
                               FROM payment WHERE BookingID = @BookingID"
        Dim parameters As New Dictionary(Of String, Object) From {
            {"@BookingID", bookingID}
        }
        Dim dt As DataTable = GetData(query, parameters)
        dgvPayments.DataSource = dt
    End Sub

    Private Sub LoadCustomer(customerID As Integer)
        Dim query As String = "SELECT FirstName, LastName, MiddleName, ContactNumber, Email, AddressLine, City, Province FROM customer WHERE CustomerID = @CustomerID"
        Dim parameters As New Dictionary(Of String, Object) From {
            {"@CustomerID", customerID}
        }

        Dim dt As DataTable = GetData(query, parameters)
        If dt.Rows.Count > 0 Then
            Dim row = dt.Rows(0)
            lblCustomerName.Text = $"{row("LastName")}, {row("FirstName")} {row("MiddleName")}"
            lblContact.Text = row("ContactNumber").ToString()
            lblEmail.Text = row("Email").ToString()
            lblAddress.Text = row("AddressLine").ToString()
            lblCity.Text = row("City").ToString()
            lblProvince.Text = row("Province").ToString()
        Else
            lblCustomerName.Text = ""
            lblContact.Text = ""
            lblEmail.Text = ""
            lblAddress.Text = ""
            lblCity.Text = ""
            lblProvince.Text = ""
        End If
    End Sub

    Private Sub dgvBookings_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvBookings.Rows(e.RowIndex)
            Dim bookingID As Integer = Convert.ToInt32(row.Cells("BookingID").Value)
            Dim customerID As Integer = Convert.ToInt32(row.Cells("CustomerID").Value)

            LoadPayments(bookingID)
            LoadCustomer(customerID)
        End If
    End Sub
End Class
