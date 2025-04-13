Imports System.Data
Imports MySql.Data.MySqlClient


Public Class StaffBookingControl
    Inherits UserControl

    Private dgvBookings As DataGridView
    Private txtAmount, txtRemarks, txtDiscount As TextBox
    Private cmbCustomerID, cmbEventID, cmbServiceID, cmbStatus, cmbPayment, cmbRefund As ComboBox
    Private dtpBookingDate, dtpEventDate, dtpTime As DateTimePicker

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadBookings()
    End Sub

    Private Sub SetupLayout()
        ' Booking Grid
        dgvBookings = New DataGridView With {
            .Dock = DockStyle.Top,
            .Height = 250,
            .ReadOnly = True,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False
        }
        AddHandler dgvBookings.CellClick, AddressOf dgvBookings_CellClick

        ' Input Controls
        cmbCustomerID = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}

        cmbEventID = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbServiceID = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}

        ' Populate the combo boxes with data from the database
        PopulateCustomerDropdown()
        PopulateEventsDropdown()
        PopulateServicesDropdown()

        txtAmount = New TextBox()
        txtRemarks = New TextBox()
        txtDiscount = New TextBox()

        cmbStatus = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbStatus.Items.AddRange({"Pending", "Confirmed", "Cancelled"})

        cmbPayment = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbPayment.Items.AddRange({"Paid", "Unpaid", "Partially Paid"})

        cmbRefund = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbRefund.Items.AddRange({"Refunded", "Not Refunded"})


        dtpBookingDate = New DateTimePicker()
        dtpEventDate = New DateTimePicker()
        dtpTime = New DateTimePicker With {
            .Format = DateTimePickerFormat.Time,
            .ShowUpDown = True
        }

        ' Buttons
        Dim btnAdd As New Button With {.Text = "Create Booking"}
        AddHandler btnAdd.Click, AddressOf btnAdd_Click

        Dim btnUpdate As New Button With {.Text = "Update Booking"}
        AddHandler btnUpdate.Click, AddressOf btnUpdate_Click

        Dim btnCancel As New Button With {.Text = "Cancel Booking"}
        AddHandler btnCancel.Click, AddressOf btnCancel_Click

        Dim btnClear As New Button With {.Text = "Clear Form"}
        AddHandler btnClear.Click, AddressOf btnClear_Click

        ' Layout
        Dim formLayout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 9
        }

        formLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        formLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        formLayout.Controls.Add(New Label With {.Text = "Customer ID"}, 0, 0)
        formLayout.Controls.Add(cmbCustomerID, 1, 0)

        formLayout.Controls.Add(New Label With {.Text = "Event ID"}, 0, 1)
        formLayout.Controls.Add(cmbEventID, 1, 1)

        formLayout.Controls.Add(New Label With {.Text = "Service ID"}, 0, 2)
        formLayout.Controls.Add(cmbServiceID, 1, 2)

        formLayout.Controls.Add(New Label With {.Text = "Booking Date"}, 0, 4)
        formLayout.Controls.Add(dtpBookingDate, 1, 4)

        formLayout.Controls.Add(New Label With {.Text = "Event Date"}, 0, 5)
        formLayout.Controls.Add(dtpEventDate, 1, 5)

        formLayout.Controls.Add(New Label With {.Text = "Booking Time"}, 0, 6)
        formLayout.Controls.Add(dtpTime, 1, 6)

        formLayout.Controls.Add(New Label With {.Text = "Status"}, 0, 7)
        formLayout.Controls.Add(cmbStatus, 1, 7)

        formLayout.Controls.Add(New Label With {.Text = "Amount"}, 0, 8)
        formLayout.Controls.Add(txtAmount, 1, 8)

        formLayout.Controls.Add(New Label With {.Text = "Payment Status"}, 0, 9)
        formLayout.Controls.Add(cmbPayment, 1, 9)

        formLayout.Controls.Add(New Label With {.Text = "Remarks"}, 0, 10)
        formLayout.Controls.Add(txtRemarks, 1, 10)

        formLayout.Controls.Add(New Label With {.Text = "Discount Applied"}, 0, 11)
        formLayout.Controls.Add(txtDiscount, 1, 11)

        formLayout.Controls.Add(New Label With {.Text = "Refund Status"}, 0, 12)
        formLayout.Controls.Add(cmbRefund, 1, 12)

        ' Buttons
        Dim buttonPanel As New FlowLayoutPanel With {.Dock = DockStyle.Bottom}
        buttonPanel.Controls.AddRange({btnAdd, btnUpdate, btnCancel, btnClear})

        ' Add to control
        Me.Controls.Add(buttonPanel)
        Me.Controls.Add(formLayout)
        Me.Controls.Add(dgvBookings)
    End Sub

    Private Sub LoadBookings()
        Dim query As String = "SELECT * FROM booking"
        Dim dt As DataTable = GetData(query)
        dgvBookings.DataSource = dt
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        Dim query As String = "INSERT INTO booking (CustomerID, EventID, ServiceID, BookingDate, BookedBy, BookingStatus, BookingTime, EventDate, TotalAmount, PaymentStatus, Remarks, DiscountApplied, RefundStatus, CreatedAt)
                           VALUES (@CustomerID, @EventID, @ServiceID, @BookingDate, @BookedBy, @BookingStatus, @BookingTime, @EventDate, @TotalAmount, @PaymentStatus, @Remarks, @DiscountApplied, @RefundStatus, NOW())"

        Dim selectedCustomerID As Object = DirectCast(cmbCustomerID.SelectedItem, ComboItem).Value
        Dim selectedEventID As Object = DirectCast(cmbEventID.SelectedItem, ComboItem).Value
        Dim selectedServiceID As Object = DirectCast(cmbServiceID.SelectedItem, ComboItem).Value

        Dim parameters As New Dictionary(Of String, Object) From {
        {"@CustomerID", selectedCustomerID},
        {"@EventID", selectedEventID},
        {"@ServiceID", selectedServiceID},
        {"@BookingDate", dtpBookingDate.Value},
        {"@BookedBy", SessionInfo.LoggedInUserFullName},
        {"@BookingStatus", cmbStatus.Text},
        {"@BookingTime", dtpTime.Value.ToString("HH:mm:ss")},
        {"@EventDate", dtpEventDate.Value},
        {"@TotalAmount", txtAmount.Text},
        {"@PaymentStatus", cmbPayment.Text},
        {"@Remarks", txtRemarks.Text},
        {"@DiscountApplied", txtDiscount.Text},
        {"@RefundStatus", cmbRefund.Text}
    }

        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("Booking added.")
            LoadBookings()
            ClearFields()
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs)
        If dgvBookings.SelectedRows.Count = 0 Then
            MessageBox.Show("Select a booking to update.")
            Return
        End If

        Dim bookingID As Integer = Convert.ToInt32(dgvBookings.SelectedRows(0).Cells("BookingID").Value)
        Dim query As String = "UPDATE booking SET CustomerID=@CustomerID, EventID=@EventID, ServiceID=@ServiceID, BookingDate=@BookingDate,
                                BookingStatus=@BookingStatus, BookingTime=@BookingTime, EventDate=@EventDate,
                                TotalAmount=@TotalAmount, PaymentStatus=@PaymentStatus, Remarks=@Remarks,
                                DiscountApplied=@DiscountApplied, RefundStatus=@RefundStatus
                                WHERE BookingID=@BookingID"
        Dim parameters As New Dictionary(Of String, Object) From {
            {"@BookingID", bookingID},
            {"@CustomerID", CType(cmbCustomerID.SelectedItem, Object).Value},
            {"@EventID", CType(cmbEventID.SelectedItem, Object).Value},
            {"@ServiceID", CType(cmbServiceID.SelectedItem, Object).Value},
            {"@BookingDate", dtpBookingDate.Value},
            {"@BookingStatus", cmbStatus.Text},
            {"@BookingTime", dtpTime.Value.ToString("HH:mm:ss")},
            {"@EventDate", dtpEventDate.Value},
            {"@TotalAmount", txtAmount.Text},
            {"@PaymentStatus", cmbPayment.Text},
            {"@Remarks", txtRemarks.Text},
            {"@DiscountApplied", txtDiscount.Text},
            {"@RefundStatus", cmbRefund.Text}
        }
        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("Booking updated.")
            LoadBookings()
            ClearFields()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs)
        If dgvBookings.SelectedRows.Count = 0 Then
            MessageBox.Show("Select a booking to cancel.")
            Return
        End If
        Dim bookingID As Integer = dgvBookings.SelectedRows(0).Cells("BookingID").Value
        Dim query As String = "UPDATE booking SET BookingStatus = 'Cancelled' WHERE BookingID = @BookingID"
        Dim parameters As New Dictionary(Of String, Object) From {
            {"@BookingID", bookingID}
        }
        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("Booking cancelled.")
            LoadBookings()
            ClearFields()
        End If
    End Sub

    Private Sub StaffBookingControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearFields()
    End Sub

    Private Sub dgvBookings_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row = dgvBookings.Rows(e.RowIndex)
            cmbCustomerID.SelectedIndex = cmbCustomerID.FindStringExact(GetCustomerNameByID(row.Cells("CustomerID").Value.ToString()))
            cmbEventID.SelectedIndex = cmbEventID.FindStringExact(GetEventNameByID(row.Cells("EventID").Value.ToString()))
            cmbServiceID.SelectedIndex = cmbServiceID.FindStringExact(GetServiceNameByID(row.Cells("ServiceID").Value.ToString()))
            dtpTime.Value = DateTime.ParseExact(row.Cells("BookingTime").Value.ToString(), "HH:mm:ss", Nothing)
            txtAmount.Text = row.Cells("TotalAmount").Value.ToString()
            txtRemarks.Text = row.Cells("Remarks").Value.ToString()
            txtDiscount.Text = row.Cells("DiscountApplied").Value.ToString()
            cmbRefund.Text = row.Cells("RefundStatus").Value.ToString()
            cmbStatus.Text = row.Cells("BookingStatus").Value.ToString()
            cmbPayment.Text = row.Cells("PaymentStatus").Value.ToString()
            dtpBookingDate.Value = Convert.ToDateTime(row.Cells("BookingDate").Value)
            dtpEventDate.Value = Convert.ToDateTime(row.Cells("EventDate").Value)
        End If
    End Sub

    Private Sub PopulateCustomerDropdown()
        Dim query As String = "SELECT CustomerID, FirstName, LastName, MiddleName FROM customer WHERE Status = 'Active'"
        Dim dt As DataTable = GetData(query)
        cmbCustomerID.Items.Clear()
        For Each row As DataRow In dt.Rows
            Dim fullName As String = $"{row("FirstName")} {row("MiddleName")} {row("LastName")}"
            cmbCustomerID.Items.Add(New ComboItem(fullName.Trim(), row("CustomerID")))
        Next
        If cmbCustomerID.Items.Count > 0 Then
            cmbCustomerID.SelectedIndex = 0
        End If
    End Sub

    Private Sub PopulateEventsDropdown()
        Dim query As String = "SELECT EventID, EventName FROM event WHERE Status = 'Upcoming'"
        Dim dt As DataTable = GetData(query)
        cmbEventID.Items.Clear()
        For Each row As DataRow In dt.Rows
            cmbEventID.Items.Add(New ComboItem(row("EventName").ToString(), row("EventID")))
        Next
        If cmbEventID.Items.Count > 0 Then
            cmbEventID.SelectedIndex = 0
        End If
    End Sub

    Private Sub PopulateServicesDropdown()
        Dim query As String = "SELECT ServiceID, ServiceName FROM service_availed WHERE Status = 'Active'"
        Dim dt As DataTable = GetData(query)
        cmbServiceID.Items.Clear()
        For Each row As DataRow In dt.Rows
            cmbServiceID.Items.Add(New ComboItem(row("ServiceName").ToString(), row("ServiceID")))
        Next
        If cmbServiceID.Items.Count > 0 Then
            cmbServiceID.SelectedIndex = 0
        End If
    End Sub

    Private Function GetCustomerNameByID(CustomerID As String) As String
        For Each item As ComboItem In cmbCustomerID.Items
            If item.Value.ToString() = CustomerID Then
                Return item.Text
            End If
        Next
        Return ""
    End Function

    Private Function GetEventNameByID(eventID As String) As String
        For Each item As ComboItem In cmbEventID.Items
            If item.Value.ToString() = eventID Then
                Return item.Text
            End If
        Next
        Return ""
    End Function

    Private Function GetServiceNameByID(serviceID As String) As String
        For Each item As ComboItem In cmbServiceID.Items
            If item.Value.ToString() = serviceID Then
                Return item.Text
            End If
        Next
        Return ""
    End Function



    Private Sub ClearFields()
        cmbCustomerID.SelectedIndex = -1
        cmbEventID.SelectedIndex = -1
        cmbServiceID.SelectedIndex = -1
        dtpTime.Value = DateTime.Now
        txtAmount.Clear()
        txtRemarks.Clear()
        txtDiscount.Clear()
        cmbRefund.SelectedIndex = -1
        cmbStatus.SelectedIndex = -1
        cmbPayment.SelectedIndex = -1
        dtpBookingDate.Value = DateTime.Now
        dtpEventDate.Value = DateTime.Now
    End Sub
End Class
