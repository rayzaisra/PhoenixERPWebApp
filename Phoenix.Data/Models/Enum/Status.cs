using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models
{
    public static class StatusTransaction
    {
        public const int Draft = 0;
        public const int CurrentApproval = 1;
        public const int WaitingApproval = 2;
        public const int Approved = 3;
        public const int Rejected = 4;
        public const int Revised = 5; //tidak di pakai untuk proses approval.

        public static string StatusName(int? value)
        {
            string result = "";
            switch (value)
            {
                case CurrentApproval:
                    result = "Current Approval";
                    break;
                case Draft:
                    result = "Open";
                    break;
                case WaitingApproval:
                    result = "Waiting Approval";
                    break;
                case Rejected:
                    result = "Rejected";
                    break;
                case Approved:
                    result = "Approved";
                    break;
                case Revised:
                    result = "Revised";
                    break;
                default:
                    result = "Open";
                    break;
            }
            return result;
        }
    }

    public static class StatusTransactionName
    {
        public const string Draft = "Open";
        public const string OnProcess = "On Process";
        public const string CurrentApproval = "Current Approval";
        public const string WaitingApproval = "Waiting Approval";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
        public const string Revised = "Revised";
        public const string Close = "Close";
        public const string Completed = "Completed";
        public const string Publish = "Closed";
    }
    #region Hris
    //public static class TerminationStatus
    //{
    //    public const int Draft = 0;
    //    public const int WaitingApproval = 1;
    //    public const int Process = 2;
    //    public const int Rejected = 3;
    //    public const int Approved = 4;

    //    public static string StatusName(int? value)
    //    {
    //        string result = "";
    //        switch (value)
    //        {
    //            case Draft:
    //                result = "Draft";
    //                break;
    //            case WaitingApproval:
    //                result = "Waiting Approval";
    //                break;
    //            case Process:
    //                result = "Process";
    //                break;
    //            case Rejected:
    //                result = "Rejected";
    //                break;
    //            case Approved:
    //                result = "Approved";
    //                break;
    //            default:
    //                result = "Draft";
    //                break;
    //        }
    //        return result;
    //    }

    //    public static string StatusHCName(int? value)
    //    {
    //        string result = "";
    //        switch (value)
    //        {
    //            case Process:
    //                result = "Process";
    //                break;
    //            case Approved:
    //                result = "Terminate";
    //                break;
    //            default:
    //                result = "Process";
    //                break;
    //        }
    //        return result;
    //    }
    //}

    public static class TerminationInterviewStatus
    {
        public const int Waiting = 1;
        public const int Approved = 2;

        public static string StatusName(int? value)
        {
            string result = "";
            switch (value)
            {
                case Waiting:
                    result = "Waiting";
                    break;
                case Approved:
                    result = "Approved";
                    break;
                default:
                    result = "Waiting";
                    break;
            }
            return result;
        }

        public static string StatusHCName(int? value)
        {
            string result = "";
            switch (value)
            {
                case Waiting:
                    result = "Waiting";
                    break;
                case Approved:
                    result = "Submitted";
                    break;
                default:
                    result = "Waiting";
                    break;
            }
            return result;
        }
    }

    public static class TerminationExitChecklistStatus
    {
        public const int OnProgress = 1;
        public const int Done = 2;
        public const int Unfinished = 3;

        public static string StatusName(int? value)
        {
            string result = "";
            switch (value)
            {
                case OnProgress://jika sudah ada yg melakukan exit checlist (udh ada yg di isi)
                    result = "OnProgress";
                    break;
                case Done://sudah melakukan checklist semua data
                    result = "Done";
                    break;
                case Unfinished://belumn melakukan proses apa2(tidak ada yg di checklist)
                    result = "Unfinished";
                    break;
                default:
                    result = "OnProgress";
                    break;
            }
            return result;
        }
    }

    public static class EmployeeServiceRequestStatus
    {
        public const int Submit = 0;
        public const int Requested = 1;
        public const int Prosess = 2;
        public const int Procurement = 3;
        public const int Fullfillment = 4;
        public const int Closed = 5;

        public static string StatusName(int? value)
        {
            string result = "";
            switch (value)
            {
                case Submit:
                    result = "Submit";
                    break;
                case Requested:
                    result = "Requested";
                    break;
                case Prosess:
                    result = "On Prosess";
                    break;
                case Procurement:
                    result = "On Procurement";
                    break;
                case Fullfillment:
                    result = "Fullfillment";
                    break;
                case Closed:
                    result = "Closed";
                    break;
                default:
                    result = "Submit";
                    break;
            }
            return result;
        }
    }

    public static class LoanStatus
    {
        public const int NotPaid = 0;
        public const int Paid = 1;

        public static string StatusName(int? value)
        {
            string result = "";
            switch (value)
            {
                case NotPaid:
                    result = "Not Paid";
                    break;
                case Paid:
                    result = "Paid";
                    break;
                default:
                    result = "Not Paid";
                    break;
            }
            return result;
        }
    }

    public static class EmployeeStatus
    {
        public const string Probation = "1066362184578409984";
        public const string Contract = "1066372119832694784";
        public const string Tetap = "1066372149285097472";
        public const string Outsource = "1066372185578409984";
    }

    #endregion

    #region Media

    public static class MediaStatusDesc
    {
        public static string StatusDescMedia(int? value)
        {
            string result = "";
            switch (value)
            {
                case StatusTransaction.CurrentApproval:
                    result = "Current Approval";
                    break;
                case StatusTransaction.Draft:
                    result = "Open";
                    break;
                case StatusTransaction.WaitingApproval:
                    result = "Waiting Approval";
                    break;
                case StatusTransaction.Rejected:
                    result = "Rejected";
                    break;
                case StatusTransaction.Approved:
                    result = "Approved";
                    break;
                case 5:
                    result = "Finish";
                    break;
                case 6:
                    result = "Published";
                    break;
                default:
                    result = "Open";
                    break;
            }
            return result;
        }

    }
    public static class MediaPlanStatusName
    {
        public const string Draft = "91823981293123";
        public const string NeedApproval = "71283781728312";
        public const string Approved = "92839283928932";
    }

    public static class MediaOrderStatusName
    {
        public const string Draft = "32323981293123";
        public const string WaitingApproval = "22223781728312";
        public const string Approved = "88839283928932";
        public const string Rejected = "f9fa6b57-9762-46b0-b3b6-8971c9ad5cc4";
    }

    #endregion

    #region Finance  
    public static class PurchaseOrderStatus
    {
        public const string Draft = "Open";
        public const string WaitingApproval = "Waiting Approval";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
        //public const string CanceledRequest = "Cancelled request";
        public const string Cancelled = "Cancelled";
        //public const string ClosedRequest = "Closed request";
        public const string Closed = "Closed";

        public static string StatusName(string value)
        {
            string result = "";
            switch (value)
            {
                case Rejected:
                    result = "Revised";
                    break;
                case WaitingApproval:
                    result = "Request Approval";
                    break;
                case Approved:
                    result = "Publish";
                    break;
                default:
                    result = value;
                    break;
            }
            return result;
        }
    }

    public class TAPStatus
    {
        public const string HeaderAccountId = "1";
        public const string DetailsAccountId = "1";
    }
    public class InvoiceReceivedStatus
    {
        //public const string Open = "1";
        //public const string ApproveFrontDesk = "2";
        //public const string ApproveAdmin = "3";
        //public const string Rejected = "4";
        //public const string TAP = "5";
        //public const string JournalConfirm = "6";
        //public const string TaxVerivication = "7";
        //public const string AP = "8";

        public const string Open = "1";
        public const string Revised = "2";
        public const string Rejected = "3";
        public const string TAP = "4";
        public const string TAXTAP = "5";
        public const string OpenAP = "6";
        public const string Posted = "7";
        public const string Cancelled = "8";

        public static string StatusName(string value)
        {
            string result = "";
            switch (value)
            {
                case Open:
                    result = "Open";
                    break;
                case Revised:
                    result = "Revised";
                    break;
                case Rejected:
                    result = "Rejected";
                    break;
                case TAP:
                    result = "TAP";
                    break;
                case TAXTAP:
                    result = "TAXTAP";
                    break;
                case OpenAP:
                    result = "OpenAP";
                    break;
                case Posted:
                    result = "Posted";
                    break;
                case Cancelled:
                    result = "Cancelled";
                    break;
                default:
                    result = "Open";
                    break;
            }
            return result;
        }
    }

    public class InvoiceClientStatus
    {
        public const string Open = "1";
        public const string WaitingApproval = "2";
        public const string Approved = "3";
        public const string Rejected = "4";
        //public const string Balance = "5";
        //public const string Closed = "6";

        public static string StatusName(string value)
        {
            string result = "";
            switch (value)
            {
                case Open:
                    result = "Open";
                    break;
                case WaitingApproval:
                    result = "Waiting Approval";
                    break;
                case Rejected:
                    result = "Rejected";
                    break;
                case Approved:
                    result = "Approved";
                    break;
                //case Balance:
                //    result = "Balance";
                //    break;
                //case Closed:
                //    result = "Closed";
                //    break;
                default:
                    result = "Open";
                    break;
            }
            return result;
        }
    }


    public class ARStatus
    {
        public const string Open = "Open";
        public const string Posted = "Posted";
    }

    public class InvoiceDeliveryNoteStatus
    {
        public const string Open = "Open";
        public const string Closed = "Closed";
    }

    public static class PettyCashStatus
    {
        public const int Open = 0;
        public const int WAITING_APPROVAL = 1;
        public const int Approved = 2;
        public const int REVISE = 3;
        //public const int Settle = 5;
        //public const int Complete = 6;

        public static string StatusName(int? value)
        {
            string result = "";
            switch (value)
            {
                case Open:
                    result = "Open";
                    break;
                case WAITING_APPROVAL:
                    result = "Waiting Approval";
                    break;
                case Approved:
                    result = "Complete";
                    break;
                case REVISE:
                    result = "Need Revise";
                    break;
                //case Settle:
                //    result = "Settle";
                //    break;
                //case Complete:
                //    result = "Complete";
                //    break;
                default:
                    result = "Open";
                    break;
            }
            return result;
        }
    }

    public static class PettyGroupName
    {
        public const string GroupSettle = "pettycash_holder";
        public const string GroupJournal = "Admin Staff";
    }

    public class PaymentRequestStatus
    {
        public const string Open = "1";
        public const string WaitingApproval = "2";
        //public const string Approved = "3";
        public const string Reject = "4";
        public const string PayBalance = "5";
        public const string Complete = "6";
        public const string Reconsiliation = "7";


        public static string GetStatus(string value)
        {
            string result = "";
            switch (value)
            {
                case Open:
                    result = "Open";
                    break;
                case WaitingApproval:
                    result = "Waiting Approval";
                    break;
                //case Approved:
                //    result = "Approved";
                //    break;
                case Reject:
                    result = "Revised";
                    break;
                case PayBalance:
                    result = "PayBalance";
                    break;
                case Complete:
                    result = "Complete";
                    break;
                case Reconsiliation:
                    result = "Reconsiliation";
                    break;
                default:
                    result = "Open";
                    break;
            }
            return result;
        }

    }

    public class ReconcileStatus
    {
        public const string Reconciled = "Reconciled";
        public const string Unreconciled = "Unreconciled";
    }
    #endregion


    #region PM
    public static class PCAStatus
    {
        public const int Open = 0;
        public const int RFQ = 1;
        public const int CA = 2;
        public const int Close = 3;
        public const int PO = 4;
        public const int CAS = 5;
        public const int Pay = 6;

        public static string StatusName(int value)
        {
            string result = "";
            switch (value)
            {
                case Open:
                    result = "OPEN";
                    break;
                case RFQ:
                    result = "RFQ";
                    break;
                case CA:
                    result = "Cash Advance";
                    break;
                case Close:
                    result = "Closed";
                    break;
                case PO:
                    result = "Purchase Order";
                    break;
                case CAS:
                    result = "Cash Advance Settlement";
                    break;
                case Pay:
                    result = "Payment Request";
                    break;
            }
            return result;
        }
    }

    public static class PCEStatus
    {
        public const string Approved = "Approved";
        public const string Revise = "Revise";
    }
    #endregion
}
