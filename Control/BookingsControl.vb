Imports System.Data
Imports MySql.Data.MySqlClient

Public Class ComboItem
    Public Property Text As String
    Public Property Value As Object

    Public Sub New(text As String, value As Object)
        Me.Text = text
        Me.Value = value
    End Sub

    Public Overrides Function ToString() As String
        Return Text
    End Function
End Class


Public Class BookingsControl
    Inherits UserControl

    ' DataGridViews for Bookings, Events, and Services
    Private bookingsGrid, eventsGrid, servicesGrid As DataGridView
    Private txtAmount, txtRemarks, txtDiscount As TextBox
    Private cmbCustomerID, cmbEventID, cmbServiceID, cmbStatus, cmbPayment, cmbRefund As ComboBox
    Private dtpBookingDate, dtpEventDate, dtpTime As DateTimePicker

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadBookings()
        LoadEvents()
        LoadServices()
    End Sub

    Private Sub SetupLayout()
        ' Grid for Bookings
        bookingsGrid = New DataGridView With {
            .Name = "bookingsGrid",
            .Dock = DockStyle.Top,
            .Height = 250,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False
        }
        AddHandler bookingsGrid.CellClick, AddressOf bookingsGrid_CellClick

        ' Grid for Events
        eventsGrid = New DataGridView With {
            .Name = "eventsGrid",
            .Dock = DockStyle.Top,
            .Height = 200,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False
        }

        ' Grid for Services
        servicesGrid = New DataGridView With {
            .Name = "servicesGrid",
            .Dock = DockStyle.Top,
            .Height = 200,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False
        }

        ' Input Controls for Booking
        cmbCustomerID = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbEventID = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbServiceID = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList}

        ' Populate combo boxes
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
        Dim btnAdd As New Button With {.Text = "Add Booking"}
        AddHandler btnAdd.Click, AddressOf btnAdd_Click

        Dim btnUpdate As New Button With {.Text = "Update Booking"}
        AddHandler btnUpdate.Click, AddressOf btnUpdate_Click

        Dim btnDelete As New Button With {.Text = "Delete Booking"}
        AddHandler btnDelete.Click, AddressOf btnDelete_Click

        Dim btnClear As New Button With {.Text = "Clear Form"}
        AddHandler btnClear.Click, AddressOf btnClear_Click

        Dim btnAddEvent As New Button With {.Text = "Create Event"}
        AddHandler btnAddEvent.Click, AddressOf btnAddEvent_Click

        Dim btnAddService As New Button With {.Text = "Create Service"}
        AddHandler btnAddService.Click, AddressOf btnAddService_Click

        btnAddEvent.AutoSize = True
        btnAddService.AutoSize = True

        ' Layout
        Dim layout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 14,
            .Padding = New Padding(10)
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        layout.Controls.Add(New Label With {.Text = "Customer ID"}, 0, 0)
        layout.Controls.Add(cmbCustomerID, 1, 0)

        layout.Controls.Add(New Label With {.Text = "Event ID"}, 0, 1)
        layout.Controls.Add(cmbEventID, 1, 1)

        layout.Controls.Add(New Label With {.Text = "Service ID"}, 0, 2)
        layout.Controls.Add(cmbServiceID, 1, 2)

        layout.Controls.Add(New Label With {.Text = "Booking Date"}, 0, 4)
        layout.Controls.Add(dtpBookingDate, 1, 4)

        layout.Controls.Add(New Label With {.Text = "Event Date"}, 0, 5)
        layout.Controls.Add(dtpEventDate, 1, 5)

        layout.Controls.Add(New Label With {.Text = "Booking Time"}, 0, 6)
        layout.Controls.Add(dtpTime, 1, 6)

        layout.Controls.Add(New Label With {.Text = "Status"}, 0, 7)
        layout.Controls.Add(cmbStatus, 1, 7)

        layout.Controls.Add(New Label With {.Text = "Amount"}, 0, 8)
        layout.Controls.Add(txtAmount, 1, 8)

        layout.Controls.Add(New Label With {.Text = "Payment Status"}, 0, 9)
        layout.Controls.Add(cmbPayment, 1, 9)

        layout.Controls.Add(New Label With {.Text = "Remarks"}, 0, 10)
        layout.Controls.Add(txtRemarks, 1, 10)

        layout.Controls.Add(New Label With {.Text = "Discount Applied"}, 0, 11)
        layout.Controls.Add(txtDiscount, 1, 11)

        layout.Controls.Add(New Label With {.Text = "Refund Status"}, 0, 12)
        layout.Controls.Add(cmbRefund, 1, 12)

        ' Bottom Buttons
        Dim bottomPanel As New FlowLayoutPanel With {.FlowDirection = FlowDirection.LeftToRight, .Dock = DockStyle.Bottom}
        bottomPanel.Controls.AddRange({btnAdd, btnUpdate, btnDelete, btnClear})
        bottomPanel.Controls.AddRange({btnAddEvent, btnAddService})

        ' Add Controls to Form
        Me.Controls.Add(layout)
        Me.Controls.Add(bookingsGrid)
        Me.Controls.Add(eventsGrid)
        Me.Controls.Add(servicesGrid)
        Me.Controls.Add(bottomPanel)
    End Sub

    ' Load Bookings Data
    Private Sub LoadBookings()
        Dim query As String = "SELECT * FROM booking"
        Dim dt As DataTable = GetData(query)
        bookingsGrid.DataSource = dt
    End Sub

    ' Load Events Data
    Private Sub LoadEvents()
        Dim query As String = "SELECT * FROM event"
        Dim dt As DataTable = GetData(query)
        eventsGrid.DataSource = dt
    End Sub

    ' Load Services Data
    Private Sub LoadServices()
        Dim query As String = "SELECT * FROM service_availed"
        Dim dt As DataTable = GetData(query)
        servicesGrid.DataSource = dt
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
        If bookingsGrid.SelectedRows.Count = 0 Then
            MessageBox.Show("Select a booking to update.")
            Return
        End If

        Dim bookingID As Integer = Convert.ToInt32(bookingsGrid.SelectedRows(0).Cells("BookingID").Value)
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

    Private Sub btnDelete_Click(sender As Object, e As EventArgs)
        If bookingsGrid.SelectedRows.Count = 0 Then
            MessageBox.Show("Select a booking to delete.")
            Return
        End If

        Dim bookingID As Integer = Convert.ToInt32(bookingsGrid.SelectedRows(0).Cells("BookingID").Value)
        If MessageBox.Show("Are you sure?", "Delete", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Dim query As String = "DELETE FROM booking WHERE BookingID=@BookingID"
            Dim parameters As New Dictionary(Of String, Object) From {
                {"@BookingID", bookingID}
            }
            If ExecuteQuery(query, parameters) Then
                MessageBox.Show("Booking deleted.")
                LoadBookings()
                ClearFields()
            End If
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearFields()
    End Sub

    Private Sub bookingsGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = bookingsGrid.Rows(e.RowIndex)

            ' Assuming PopulateEventsDropdown and PopulateServicesDropdown already called
            cmbCustomerID.SelectedIndex = cmbCustomerID.FindStringExact(GetCustomerNameByID(row.Cells("CustomerID").Value.ToString()))
            cmbEventID.SelectedIndex = cmbEventID.FindStringExact(GetEventNameByID(row.Cells("EventID").Value.ToString()))
            cmbServiceID.SelectedIndex = cmbServiceID.FindStringExact(GetServiceNameByID(row.Cells("ServiceID").Value.ToString()))

            ' Other assignments                        
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

    Private Sub btnAddEvent_Click(sender As Object, e As EventArgs)
        Dim eventForm As New Form With {.Text = "New Event", .Size = New Size(400, 600)}
        Dim layout As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 12}
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60))

        ' Form fields
        Dim txtEventName As New TextBox()
        Dim cmbCustomer As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList}
        Dim cmbType As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbType.Items.AddRange(New String() {"Private", "Corporate"})
        cmbType.SelectedIndex = 0

        Dim dtpDate As New DateTimePicker() With {.Format = DateTimePickerFormat.Short}
        Dim dtpStart As New DateTimePicker() With {.Format = DateTimePickerFormat.Time, .ShowUpDown = True}
        Dim dtpEnd As New DateTimePicker() With {.Format = DateTimePickerFormat.Time, .ShowUpDown = True}
        Dim dtpSetup As New DateTimePicker() With {.Format = DateTimePickerFormat.Time, .ShowUpDown = True}
        Dim dtpCleanup As New DateTimePicker() With {.Format = DateTimePickerFormat.Time, .ShowUpDown = True}
        Dim cmbVenue As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbVenue.Items.AddRange(New String() {
            "Albay Astrodome",
            "Legazpi Convention Center",
            "Ibalong Centrum for Recreation",
            "Penaranda Park",
            "Cagsawa Ruins Park",
            "Lignon Hill Nature Park",
            "Pacific Mall Event Center",
            "Embarcadero de Legazpi",
            "Avenue Plaza Hotel",
            "CWC (Camsur Watersports Complex)",
            "Villa Caceres Hotel",
            "Biggs Diner Function Hall",
            "Naga City Civic Center",
            "Bicol University Gymnasium",
            "Jardin Real de Naga",
            "Ateneo de Naga University Gym",
            "Sorsogon Capitol Park",
            "Rizal Beach Resort",
            "Misibis Bay Resort",
            "Balay Cena Una",
            "Hotel Venezia",
            "Doña Mercedes Country Lodge"
        })

        cmbVenue.SelectedIndex = 0
        Dim txtGuests As New TextBox()
        Dim txtTheme As New TextBox()
        Dim txtRequests As New TextBox()


        ' Populate customers
        Dim customerQuery As String = "SELECT CustomerID, FirstName, LastName, MiddleName FROM customer"
        Dim dt As DataTable = GetData(customerQuery)
        For Each row As DataRow In dt.Rows
            Dim fullName As String = $"{row("FirstName")} {row("MiddleName")} {row("LastName")}".Trim()
            cmbCustomer.Items.Add(New ComboItem(fullName, row("CustomerID")))

        Next
        If cmbCustomer.Items.Count > 0 Then cmbCustomer.SelectedIndex = 0

        ' Add controls to layout
        layout.Controls.Add(New Label With {.Text = "Event Name"}, 0, 0)
        layout.Controls.Add(txtEventName, 1, 0)
        layout.Controls.Add(New Label With {.Text = "Customer"}, 0, 1)
        layout.Controls.Add(cmbCustomer, 1, 1)
        layout.Controls.Add(New Label With {.Text = "Event Type"}, 0, 2)
        layout.Controls.Add(cmbType, 1, 2)
        layout.Controls.Add(New Label With {.Text = "Date"}, 0, 3)
        layout.Controls.Add(dtpDate, 1, 3)
        layout.Controls.Add(New Label With {.Text = "Start Time"}, 0, 4)
        layout.Controls.Add(dtpStart, 1, 4)
        layout.Controls.Add(New Label With {.Text = "End Time"}, 0, 5)
        layout.Controls.Add(dtpEnd, 1, 5)
        layout.Controls.Add(New Label With {.Text = "Venue"}, 0, 6)
        layout.Controls.Add(cmbVenue, 1, 6)
        layout.Controls.Add(New Label With {.Text = "Guest Count"}, 0, 7)
        layout.Controls.Add(txtGuests, 1, 7)
        layout.Controls.Add(New Label With {.Text = "Theme"}, 0, 8)
        layout.Controls.Add(txtTheme, 1, 8)
        layout.Controls.Add(New Label With {.Text = "Special Requests"}, 0, 9)
        layout.Controls.Add(txtRequests, 1, 9)
        layout.Controls.Add(New Label With {.Text = "Setup Time"}, 0, 10)
        layout.Controls.Add(dtpSetup, 1, 10)
        layout.Controls.Add(New Label With {.Text = "Cleanup Time"}, 0, 11)
        layout.Controls.Add(dtpCleanup, 1, 11)

        ' Submit button
        Dim btnSubmit As New Button With {.Text = "Create"}
        AddHandler btnSubmit.Click, Sub()
                                        If cmbCustomer.SelectedItem Is Nothing Then
                                            MessageBox.Show("Please select a customer.")
                                            Return
                                        End If

                                        Dim selectedCustomer As ComboItem = CType(cmbCustomer.SelectedItem, ComboItem)
                                        Dim query = "INSERT INTO event (EventName, CustomerID, EventType, EventDate, StartTime, EndTime, VenueLocation, GuestCount, Theme, SpecialRequests, SetupTime, CleanupTime, Status, CreatedDate)
                     VALUES (@EventName, @CustomerID, @EventType, @EventDate, @StartTime, @EndTime, @VenueLocation, @GuestCount, @Theme, @SpecialRequests, @SetupTime, @CleanupTime, 'Upcoming', NOW())"

                                        Dim parameters As New Dictionary(Of String, Object) From {
            {"@EventName", txtEventName.Text},
            {"@CustomerID", selectedCustomer.Value},
            {"@EventType", cmbType.Text},
            {"@EventDate", dtpDate.Value},
            {"@StartTime", dtpStart.Value.ToString("HH:mm:ss")},
            {"@EndTime", dtpEnd.Value.ToString("HH:mm:ss")},
            {"@VenueLocation", cmbVenue.Text},
            {"@GuestCount", txtGuests.Text},
            {"@Theme", txtTheme.Text},
            {"@SpecialRequests", txtRequests.Text},
            {"@SetupTime", dtpSetup.Value.ToString("HH:mm:ss")},
            {"@CleanupTime", dtpCleanup.Value.ToString("HH:mm:ss")}
        }

                                        If ExecuteQuery(query, parameters) Then
                                            MessageBox.Show("Event created.")
                                            eventForm.Close()
                                        Else
                                            MessageBox.Show("Failed to create event.")
                                        End If
                                    End Sub

        layout.Controls.Add(btnSubmit, 1, 12)
        eventForm.Controls.Add(layout)
        eventForm.ShowDialog()
    End Sub


    Private Sub btnAddService_Click(sender As Object, e As EventArgs)
        Dim serviceForm As New Form With {.Text = "New Service", .Size = New Size(400, 500)}
        Dim layout As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 11}
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60))

        Dim txtName As New TextBox()
        Dim txtDesc As New TextBox()
        Dim cmbCat As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbCat.Items.AddRange(New String() {"Decoration", "Food", "Audio-Visual"})
        cmbCat.SelectedIndex = 0
        Dim txtPrice As New TextBox()
        Dim cmbUnit As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbUnit.Items.AddRange(New String() {"Per Hour", "Per Event", "Per Guest"})
        cmbUnit.SelectedIndex = 0
        Dim cmbAvail As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cmbAvail.Items.AddRange(New String() {"Available", "Unavailable"})
        cmbAvail.SelectedIndex = 0
        Dim txtSetupReq As New TextBox()
        Dim dtpDuration As New DateTimePicker() With {.Format = DateTimePickerFormat.Time, .ShowUpDown = True}
        Dim txtMinG As New TextBox()
        Dim txtMaxG As New TextBox()

        layout.Controls.Add(New Label With {.Text = "Service Name"}, 0, 0)
        layout.Controls.Add(txtName, 1, 0)
        layout.Controls.Add(New Label With {.Text = "Description"}, 0, 1)
        layout.Controls.Add(txtDesc, 1, 1)
        layout.Controls.Add(New Label With {.Text = "Category"}, 0, 2)
        layout.Controls.Add(cmbCat, 1, 2)
        layout.Controls.Add(New Label With {.Text = "Price"}, 0, 3)
        layout.Controls.Add(txtPrice, 1, 3)
        layout.Controls.Add(New Label With {.Text = "Unit"}, 0, 4)
        layout.Controls.Add(cmbUnit, 1, 4)
        layout.Controls.Add(New Label With {.Text = "Availability"}, 0, 5)
        layout.Controls.Add(cmbAvail, 1, 5)
        layout.Controls.Add(New Label With {.Text = "Setup Required"}, 0, 6)
        layout.Controls.Add(txtSetupReq, 1, 6)
        layout.Controls.Add(New Label With {.Text = "Duration Estimate"}, 0, 7)
        layout.Controls.Add(dtpDuration, 1, 7)
        layout.Controls.Add(New Label With {.Text = "Min Guests"}, 0, 8)
        layout.Controls.Add(txtMinG, 1, 8)
        layout.Controls.Add(New Label With {.Text = "Max Guests"}, 0, 9)
        layout.Controls.Add(txtMaxG, 1, 9)

        Dim btnSubmit As New Button With {.Text = "Create"}
        AddHandler btnSubmit.Click, Sub()
                                        Dim query = "INSERT INTO service_availed (ServiceName, Description, Category, Price, Unit, Availability, SetupRequired, DurationEstimate, MinGuest, MaxGuest, CreatedBy, CreatedDate, UpdatedDate, Status)
                     VALUES (@ServiceName, @Description, @Category, @Price, @Unit, @Availability, @SetupRequired, @DurationEstimate, @MinGuest, @MaxGuest, @CreatedBy, NOW(), NOW(), 'Active')"
                                        Dim parameters As New Dictionary(Of String, Object) From {
            {"@ServiceName", txtName.Text},
            {"@Description", txtDesc.Text},
            {"@Category", cmbCat.Text},
            {"@Price", txtPrice.Text},
            {"@Unit", cmbUnit.Text},
            {"@Availability", cmbAvail.Text},
            {"@SetupRequired", txtSetupReq.Text},
            {"@DurationEstimate", dtpDuration.Value.ToString("HH:mm:ss")},
            {"@MinGuest", txtMinG.Text},
            {"@MaxGuest", txtMaxG.Text},
            {"@CreatedBy", SessionInfo.LoggedInUserFullName}
        }

                                        If ExecuteQuery(query, parameters) Then
                                            MessageBox.Show("Service created.")
                                            serviceForm.Close()
                                        Else
                                            MessageBox.Show("Failed to create service.")
                                        End If
                                    End Sub

        layout.Controls.Add(btnSubmit, 1, 10)
        serviceForm.Controls.Add(layout)
        serviceForm.ShowDialog()
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
