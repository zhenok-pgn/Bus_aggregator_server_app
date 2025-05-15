namespace App.Core.Enums
{
    public enum BookingStatus
    {
        RefundRequested, // requested by the buyer
        RefundApproved, // approved by the bus company
        RefundRejected, // rejected by the bus company
        Reserved, // reserved for the buyer while waiting for payment
        ReserveCancelled, // expired or cancelled by the buyer
        Paid, // paid by the buyer
        Confirmed, // on the bus
        Completed // completed the trip
    }
}
